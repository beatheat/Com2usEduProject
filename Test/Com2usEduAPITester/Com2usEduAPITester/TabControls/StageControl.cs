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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;


namespace Com2usEduAPITester.TabControls
{
    public partial class StageControl : UserControl
    {
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
            var response = await HttpRequest.PostAuth<LoadPlayerStageInfoResponse>("LoadPlayerStageInfo", request);
            if (response == null) return;
            var maxStageCode = response.MaxStageCode;   

            var completedStages = response.ClearStageCodes;
            var accessibleStages = response.AccessibleStageCodes;

            lbxStageList.Items.Clear();


            for(int i=1;i<=maxStageCode;i++)
            {
                var text = "";
                if (!accessibleStages.Contains(i))
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
            var responseEnter = await HttpRequest.PostAuth<EnterStageResponse>("EnterStage", enterStageRequest);
            if (responseEnter == null)
            {
                btnAutoGame.Enabled = true;
                return;
            }

            var (itemString, expString) = await Game(responseEnter.StageItems, responseEnter.StageNpcs);

            var completeStageRequest = new CompleteStageRequest();
            var responseComplete = await HttpRequest.PostAuth<CompleteStageResponse>("CompleteStage", enterStageRequest);
            if (responseComplete == null)
            {
                btnAutoGame.Enabled = true;
                return;
            }

            string message = responseComplete.IsStageCleared ? "스테이지 성공\r\n" : "스테이지 실패\r\n";
            if(responseComplete.IsStageCleared)
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

            var farmItems = GetRandomStageItems(stageItems);
            var farmNpcs = GetRandomStageNpcs(stageNpcs);

            List<object> farmObjects = new List<object>();

            var random = new Random();
            
            int farmItemIndex = 0;
            int farmNpcIndex = 0;

            int exp = 0;
            
            while(farmItemIndex < farmItems.Count || farmNpcIndex < farmNpcs.Count)
            {
                if (random.Next(2) == 0 && farmItemIndex < farmItems.Count)
                {
                    var farmItem = farmItems[farmItemIndex];
                    var request = new FarmStageItemRequest
                    {
                        ItemCode = farmItem.ItemCode,
                        ItemCount = farmItem.ItemCount
                    };
                    var response = await HttpRequest.PostAuth<FarmStageItemResponse>("FarmStageItem", request);
                    if (response == null)
                    {
                        break;
                    }
    
                    tbStageLog.AppendText(MasterData.ItemName[farmItem.ItemCode] + "를 " + farmItem.ItemCount + "개 획득!\r\n");
                   
                    itemString += MasterData.ItemName[farmItem.ItemCode] + " : " + farmItem.ItemCount + "개\r\n";
                    farmItemIndex++;
                }
                else if(farmNpcIndex < farmNpcs.Count)
                {
                    var farmNpc = farmNpcs[farmNpcIndex];

                    var request = new FarmStageNpcRequest
                    {
                        NpcCode = farmNpc.Code
                    };
                    var response = await HttpRequest.PostAuth<FarmStageItemResponse>("FarmStageNpc", request);
                    if (response == null)
                        break;
      
                    tbStageLog.AppendText(farmNpc.Code + "를 처치했습니다! 경험치(" + farmNpc.Exp + ") 획득! \r\n");
                    exp += farmNpc.Exp;

                    farmNpcIndex++;
                    await Task.Delay(50);

                }

            }
            npcString += "경험치 : " + exp + "\r\n";  
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
            List<StageNpc> result = new List<StageNpc>();


            if (random.Next(100) < 75)
            {
                foreach (var stageNpc in stageNpcs)
                {
                    for (int i = 0; i < stageNpc.Count; i++)
                    {
                        result.Add(stageNpc);
                    }
                }
                return result;
            }


            foreach (var stageNpc in stageNpcs)
            {
                for (int i = 0; i < stageNpc.Count; i++)
                {
                    if (random.Next(100) < 75)
                    {
                        result.Add(stageNpc);
                    }
                }
            }
            return result;
        }

    }
}
