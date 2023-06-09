﻿using Com2usEduClient.Game;
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
using Com2usEduClient.ReqRes;

namespace Com2usEduClient.TabControls
{
    public partial class ShopControl : UserControl
    {
        public ShopControl()
        {
            InitializeComponent();
        }
        private async void btnBuyShopItem1_Click(object sender, EventArgs e)
        {
            await BuyInAppItem(1);
        }

        private async void btnBuyShopItem2_Click(object sender, EventArgs e)
        {
            await BuyInAppItem(2);
        }

        private async void btnBuyShopItem3_Click(object sender, EventArgs e)
        {
            await BuyInAppItem(3);
        }


        private async Task BuyInAppItem(int shopCode)
        {
            var request = new ReceiveInAppPurchaseItemRequest
            {
                BillToken = HttpRequest.PlayerId * 1000 + shopCode,
                ShopCode = shopCode
            };

            var response = await HttpRequest.PostAuth<ReceiveAttendanceRewardResponse>("ReceiveInAppPurchaseItem", request);
            if (response == null) return;

            MessageBox.Show("구매완료! 우편함에서 아이템을 확인하세요");
            
        }

    }
}
