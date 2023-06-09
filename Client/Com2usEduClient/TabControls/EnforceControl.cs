﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Com2usEduClient.Game;
using Com2usEduClient.ReqRes;


namespace Com2usEduClient.TabControls
{
    public partial class EnforceControl : UserControl
    {
        List<PlayerItem> _enforceItemList = new();

        public EnforceControl()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            LoadEnforceItemList();
        }

        private async void btnEnforce_Click(object sender, EventArgs e)
        {
            string itemIdString = tbEnforceDetail.Text.Split("\r\n")[0];
            if (!int.TryParse(itemIdString, out var itemId))
                return;

            var request = new EnforcePlayerItemRequest
            {
                PlayerItemId = itemId
            };

            var response = await HttpRequest.PostAuthWithErrorCode<EnforcePlayerItemResponse>("EnforcePlayerItem", request);

            if (response == null) return;


            if (response.Result != ErrorCode.None)
            {
                MessageBox.Show("에러!");
            }
            else
            {
                var enforceState = response.EnforceState;
                if (enforceState == EnforceState.Success)
                {
                    var item = response.EnforcedItem;

                    tbEnforceResult.Text += item.Id + "\r\n";
                    tbEnforceResult.Text += $"이름: {MasterData.ItemName[item.ItemCode]}\r\n";
                    tbEnforceResult.Text += $"수량: {item.Count}\r\n";
                    tbEnforceResult.Text += $"공격력: {item.Attack}\r\n";
                    tbEnforceResult.Text += $"방어력: {item.Defence}\r\n";
                    tbEnforceResult.Text += $"마법력: {item.Magic}\r\n";
                    tbEnforceResult.Text += $"강화횟수: {item.EnhanceCount}\r\n";

                    var itemIndex = _enforceItemList.FindIndex(x => x.Id == itemId);
                    _enforceItemList[itemIndex] = item;
                }
                else if (enforceState == EnforceState.Fail)
                {
                    MessageBox.Show("강화실패");
                    LoadEnforceItemList();
                }
                else
                {
                    MessageBox.Show("강화불가");
                }
            }
        }


        private async void LoadEnforceItemList()
        {
            var request = new LoadPlayerItemRequest();

            var response = await HttpRequest.PostAuth<LoadPlayerItemResponse>("LoadPlayerItem", request);
            if (response == null) return;

            var playerItems = response.PlayerItems;

            tbEnforceDetail.Text = "";
            tbEnforceResult.Text = "";
            lbxEnforcePlayerItem.Items.Clear();
            _enforceItemList.Clear();
            foreach (var item in playerItems)
            {
                //돈 or 포션
                if (item.ItemCode == 1 || item.ItemCode == 6)
                    continue;
                _enforceItemList.Add(item);
                lbxEnforcePlayerItem.Items.Add($"{item.Id}: " + MasterData.ItemName[item.ItemCode]);
            }
        }

        private void lbxEnforcePlayerItem_DoubleClick(object sender, EventArgs e)
        {
            tbEnforceResult.Text = "";
            int selectedIndex = lbxEnforcePlayerItem.SelectedIndex;

            var item = _enforceItemList[selectedIndex];

            tbEnforceDetail.Text = item.Id + "\r\n";
            tbEnforceDetail.Text += $"이름: {MasterData.ItemName[item.ItemCode]}\r\n";
            tbEnforceDetail.Text += $"수량: {item.Count}\r\n";
            tbEnforceDetail.Text += $"공격력: {item.Attack}\r\n";
            tbEnforceDetail.Text += $"방어력: {item.Defence}\r\n";
            tbEnforceDetail.Text += $"마법력: {item.Magic}\r\n";
            tbEnforceDetail.Text += $"강화횟수: {item.EnhanceCount}\r\n";
        }
    }
}
