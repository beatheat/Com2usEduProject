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

namespace Com2usEduAPITester.TabControls
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

            var response = await HttpRequest.PostAuth("ReceiveInAppPurchaseItem", request);
            if (response == null) return;

            var receiveInAppPurchaseItemResponse = JsonSerializer.Deserialize<ReceiveInAppPurchaseItemResponse>(response);

            if (receiveInAppPurchaseItemResponse.Result == ErrorCode.None)
            {
                MessageBox.Show("구매완료! 우편함에서 아이템을 확인하세요");
            }
        }

    }
}
