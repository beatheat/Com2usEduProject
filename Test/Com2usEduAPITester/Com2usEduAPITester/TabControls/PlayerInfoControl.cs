using Com2usEduAPITester.Game;
using Com2usEduAPITester.ReqRes;
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
using static Com2usEduAPITester.MasterData;

namespace Com2usEduAPITester.TabControls
{
    public partial class PlayerInfoControl : UserControl
    {
        public PlayerInfoControl()
        {
            InitializeComponent();
        }


        private async void btnPlayerInfoLoad_Click(object sender, EventArgs e)
        {
            await LoadPlayerInfo();
        }

        public async void RefreshData()
        {
            await LoadPlayerInfo();
        }

        private async Task LoadPlayerInfo()
        {
            var loadPlayerRequest = new LoadPlayerRequest();

            var response = await HttpRequest.PostAuth("LoadPlayer", loadPlayerRequest);
            if (response == null) return;

            var loadPlayerResponse = JsonSerializer.Deserialize<LoadPlayerResponse>(response);
            var player = loadPlayerResponse.Player;

            tbPlayerInfo.Text = "";

            tbPlayerInfo.Text += "닉네임 : " + player.Nickname + "\r\n";
            tbPlayerInfo.Text += "소지금 : " + player.Money + "\r\n";
            tbPlayerInfo.Text += "레벨 : " + player.Level + "\r\n";
            tbPlayerInfo.Text += "경험치 : " + player.Exp + "\r\n";
            tbPlayerInfo.Text += "체력 : " + player.HP + "\r\n";
            tbPlayerInfo.Text += "공격력 : " + player.Attack + "\r\n";
            tbPlayerInfo.Text += "방어력 : " + player.Defence + "\r\n";
            tbPlayerInfo.Text += "마법력 : " + player.Magic + "\r\n";

            var loadPlayerItemRequest = new LoadPlayerItemRequest();

            response = await HttpRequest.PostAuth("LoadPlayerItem", loadPlayerItemRequest);
            if (response == null) return;

            var loadPlayerItemResponse = JsonSerializer.Deserialize<LoadPlayerItemResponse>(response);
            var playerItems = loadPlayerItemResponse.PlayerItems;

            tbPlayerItemInfo.Text = "";
            foreach (var item in playerItems)
            {
                tbPlayerItemInfo.Text += $"이름: {ItemName[item.ItemCode]}\r\n";
                tbPlayerItemInfo.Text += $"수량: {item.Count}\r\n";
                tbPlayerItemInfo.Text += $"공격력: {item.Attack}\r\n";
                tbPlayerItemInfo.Text += $"방어력: {item.Defence}\r\n";
                tbPlayerItemInfo.Text += $"마법력: {item.Magic}\r\n";
                tbPlayerItemInfo.Text += $"강화횟수: {item.EnhanceCount}\r\n";

                tbPlayerItemInfo.Text += "--------------------\r\n";
            }
        }
    }
}
