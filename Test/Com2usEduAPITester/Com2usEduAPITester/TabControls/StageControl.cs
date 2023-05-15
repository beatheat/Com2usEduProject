using Com2usEduAPITester.Model;
using Com2usEduAPITester.Game;
using Com2usEduAPITester.ReqRes;
using Com2usEduAPITester.ReqRes.Stage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Com2usEduAPITester.TabControls
{
    public partial class StageControl : UserControl
    {
        private List<int> _accessibleStages = new List<int>();
        private int _selectedStageCode = 0;

        public StageControl()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            LoadPlayerStageInfo();
        }

        private async void LoadPlayerStageInfo()
        {
            var request = new LoadPlayerStageInfoRequest();
            var response = await HttpRequest.PostAuth("LoadPlayerStageInfo", request);
            if (response == null) return;
            var loadPlayerStageInfoResponse = JsonSerializer.Deserialize<LoadPlayerStageInfoResponse>(response);
            var maxStageCode = loadPlayerStageInfoResponse.MaxStageCode;
            var completedStages = loadPlayerStageInfoResponse.CompletedStages.ToList();
            _accessibleStages = loadPlayerStageInfoResponse.AccessibleStages.ToList();

            lbxStageList.Items.Clear();

            for(int i=1;i<=maxStageCode;i++)
            {
                var text = "";
                if (!_accessibleStages.Contains(i))
                    text += "🔒";
                text += "스테이지 " + i;
                if (completedStages.Contains(i))
                    text += " : Clear";
                text += "\r\n";
                lbxStageList.Items.Add(text);
            }
        }

        private void lbxStageList_DoubleClick(object sender, EventArgs e)
        {
            tbStageLog.Text = "";
            _selectedStageCode = 0;
            if(lbxStageList.SelectedItem.ToString().Substring(0,2) == "🔒")
            {
                return;
            }
            tbStageLog.Text = lbxStageList.SelectedItem.ToString();
            _selectedStageCode = lbxStageList.SelectedIndex + 1;
        }

        private async void btnAutoGame_Click(object sender, EventArgs e)
        {
            if (_selectedStageCode == 0) return;
            btnAutoGame.Enabled = false;

            var enterStageRequest = new EnterStageRequest
            {
                StageCode = _selectedStageCode
            };
            var response = await HttpRequest.PostAuth("EnterStage", enterStageRequest);
            if (response == null)
            {
                btnAutoGame.Enabled = true;
                return;
            }
            var enterStageResponse = JsonSerializer.Deserialize<EnterStageResponse>(response);
           

            var (itemString, expString) = await Game(enterStageResponse.StageItems, enterStageResponse.StageNpcs);

            var completeStageRequest = new CompleteStageRequest();
            response = await HttpRequest.PostAuth("CompleteStage", enterStageRequest);
            if (response == null)
            {
                btnAutoGame.Enabled = true;
                return;
            }
            var completeStageResponse = JsonSerializer.Deserialize<CompleteStageResponse>(response);

            string message = completeStageResponse.IsStageCleared ? "스테이지 성공\r\n" : "스테이지 실패\r\n";
            if(completeStageResponse.IsStageCleared)
            {
                message += "--획득 경험치--\r\n";
                message += expString + "\r\n";
                message += "--획득 아이템--\r\n";
                message += itemString;
            }

            MessageBox.Show(message);

            btnAutoGame.Enabled = true;

            LoadPlayerStageInfo();
        } 

        private async Task<(string,string)> Game(IList<StageItem> stageItems, IList<StageNpc> stageNpcs)
        {
            var itemString = "";
            var npcString = "";


            var itemTask = Task.Run(async () =>
            {
                var farmItems = GetRandomStageItems(stageItems);

                foreach (var farmItem in farmItems)
                {
                    var request = new FarmStageItemRequest
                    {
                        ItemCode = farmItem.ItemCode,
                        ItemCount = farmItem.ItemCount
                    };
                    var response = await HttpRequest.PostAuth("FarmStageItem", request);
                    if (response == null)
                    {
                        MessageBox.Show(farmItem.ItemCode + ", " + farmItem.ItemCount);
                        return;
                    }
                    lock (this)
                    {
                        tbStageLog.AppendText(MasterData.ItemName[farmItem.ItemCode] + "를 " + farmItem.ItemCount + "개 획득!\r\n");
                    }
                    itemString += MasterData.ItemName[farmItem.ItemCode] + " : " + farmItem.ItemCount + "개\r\n";
                    await Task.Delay(100);
                }
            });

            var npcTask = Task.Run(async () =>
            {
                var farmNpcs = GetRandomStageNpcs(stageNpcs);
                int exp = 0;
                foreach (var farmNpc in farmNpcs)
                {
                    for (int i = 0; i < farmNpc.Count; i++)
                    {
                        var request = new FarmStageNpcRequest
                        {
                            NpcCode = farmNpc.Code
                        };
                        var response = await HttpRequest.PostAuth("FarmStageNpc", request);
                        if (response == null)
                            return;
                        lock (this)
                        {
                            tbStageLog.AppendText(farmNpc.Code + "를 처치했습니다! 경험치(" + farmNpc.Exp + ") 획득! \r\n");
                        }
                        exp += farmNpc.Exp;
                        await Task.Delay(50);
                    }
                }
                npcString = "경험치 : " + exp;
            });

            await itemTask;
            await npcTask;

            return (itemString, npcString);

        }

        private List<ItemBundle> GetRandomStageItems(IList<StageItem> stageItems)
        {
            List<ItemBundle> result = new List<ItemBundle>();
            Random random = new Random();
            foreach (var item in stageItems)
            {
                if (random.Next(100) < 75)
                {
                    var itemBundle = new ItemBundle();
                    itemBundle.ItemCode = item.ItemCode;
                    itemBundle.ItemCount = random.Next(item.MaxItemCount) + 1;
                    result.Add(itemBundle);
                }
            }
            return result;
        }


        private List<StageNpc> GetRandomStageNpcs(IList<StageNpc> stageNpcs)
        {

            Random random = new Random();

            if (random.Next(100) < 75)
            {
                return stageNpcs.ToList();
            }

            List<StageNpc> result = new List<StageNpc>();

            foreach (var stageNpc in stageNpcs)
            {
                if (random.Next(100) < 75)
                {
                    var selectedNpc = new StageNpc();
                    selectedNpc.Count = random.Next(stageNpc.Count)+1;
                    result.Add(selectedNpc);
                }
            }
            return result;
        }

    }
}
