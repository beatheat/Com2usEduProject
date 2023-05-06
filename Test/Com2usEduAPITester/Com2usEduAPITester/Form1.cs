using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Com2usEduAPITester.ReqRes;
using Com2usEduAPITester.Game;
using Com2usEduAPITester.MasterData;
using static Com2usEduAPITester.MasterData.MasterData;

namespace Com2usEduAPITester
{
    public partial class Form1 : Form
    {
        static readonly HttpClient httpClient = new HttpClient();
        static readonly TextEncoderSettings encoderSettings = new();

        int accountId = -1;
        string authToken = "";
        int playerId = -1;


        int _currentMailBoxPageNo = 1;
        int _maxMailBoxPageNo = -1;


        List<PlayerItem> enforceItemList = new();

        public Form1()
        {
            InitializeComponent();
            encoderSettings.AllowRange(UnicodeRanges.All);
        }

        private class CommonResponse
        {
            public ErrorCode Result { get; set; }
        }

        private async Task<string?> RequestPostAuth(string apiName, object jsonBody)
        {

            var jsonString = JsonSerializer.Serialize(jsonBody);

            var jsonObject = JsonNode.Parse(jsonString);

            jsonObject["MasterDataVersion"] = tbMasterDataVersion.Text;
            jsonObject["ClientVersion"] = tbClientVersion.Text;
            jsonObject["AccountId"] = accountId;
            jsonObject["AuthToken"] = authToken;
            jsonObject["PlayerId"] = playerId;

            jsonString = jsonObject.ToJsonString(new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(encoderSettings),
                WriteIndented = true,
            });

            return await RequestPost(apiName, jsonString);
        }

        private async Task<string?> RequestPost(string apiName, string jsonString)
        {
            var targetURL = tbServerURL.Text + "/" + apiName;

            tbResponse.Text = "";
            tbRequest.Text = "";
            tbRequest.Text += "POST " + targetURL + "\r\n";
            tbRequest.Text += "Content-Type: application/json\r\n";
            tbRequest.Text += jsonString;

            using (var response = await httpClient.PostAsync(tbServerURL.Text + "/" + apiName, new StringContent(jsonString, Encoding.UTF8, "application/json")))
            {
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var prettyJsonString = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(responseString), new JsonSerializerOptions { WriteIndented = true });

                    tbResponse.Text = prettyJsonString;

                    var commonResponse = JsonSerializer.Deserialize<CommonResponse>(responseString);
                    if (commonResponse.Result != ErrorCode.None)
                    {
                        tbResponse.Text = "ErrorCode : " + commonResponse.Result.ToString();
                        return null;
                    }

                    return responseString;
                }
                else
                {
                    tbResponse.Text = response.StatusCode.ToString();
                    return null;
                }
            }

        }

        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            var jsonBody = new CreateAccountRequest
            {
                LoginId = tbID.Text,
                Password = tbPassword.Text,
            };
            var response = await RequestPost("CreateAccount", JsonSerializer.Serialize(jsonBody, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(encoderSettings),
                WriteIndented = true,
            }));

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var jsonBody = new
            {
                MasterDataVersion = "1.0.0",
                ClientVersion = "1.0.0",
                LoginId = tbID.Text,
                Password = tbPassword.Text,
            };
            var response = await RequestPost("Login", JsonSerializer.Serialize(jsonBody, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(encoderSettings),
                WriteIndented = true,
            }));
            if (response == null)
            {
                return;
            }

            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(response);

            playerId = loginResponse.Player.Id;
            authToken = loginResponse.AuthToken;
            accountId = loginResponse.AccountId;
            tbNotice.Text = loginResponse.Notice;

            tbPlayerId.Text = playerId.ToString();
            tbAccountId.Text = accountId.ToString();
            tbAuthToken.Text = authToken;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            _currentMailBoxPageNo = 1;
            LoadMailBox(_currentMailBoxPageNo);
        }

        private async void btnMailBoxRight_Click(object sender, EventArgs e)
        {
            if (_currentMailBoxPageNo >= _maxMailBoxPageNo)
                return;
            _currentMailBoxPageNo++;
            LoadMailBox(_currentMailBoxPageNo);
        }
        private void btnMailBoxLeft_Click(object sender, EventArgs e)
        {
            if (_currentMailBoxPageNo <= 1)
                return;
            _currentMailBoxPageNo--;
            LoadMailBox(_currentMailBoxPageNo);
        }



        private async void LoadMailBox(int pageNo)
        {
            var request = new LoadMailListRequest
            {
                PageNo = pageNo
            };
            var response = await RequestPostAuth("LoadMailList", request);
            if (response == null)
            {
                return;
            }

            var mailListReponse = JsonSerializer.Deserialize<LoadMailListResponse>(response);

            _maxMailBoxPageNo = mailListReponse.TotalPageCount;
            lbMailBoxPageNo.Text = pageNo + " / " + mailListReponse.TotalPageCount.ToString();

            lbxMailBox.Items.Clear();
            foreach (var mail in mailListReponse.MailList)
            {
                var item = "";
                item += mail.Id + ":\t";
                item += mail.Name + "\t";
                item += mail.TransmissionDate + "\t";
                item += mail.IsItemReceived ? "아이템O" : "아이템X";

                lbxMailBox.Items.Add(item);
            }
        }

        private async void lbxMailBox_DoubleClick(object sender, EventArgs e)
        {
            if (lbxMailBox.SelectedItem == null)
                return;

            var mailBoxRow = lbxMailBox.SelectedItem.ToString();
            var mailId = int.Parse(mailBoxRow.Split(":")[0]);

            var request = new LoadMailRequest
            {
                MailId = mailId
            };
            var response = await RequestPostAuth("LoadMail", request);

            if (response == null)
                return;

            var loadMailResponse = JsonSerializer.Deserialize<LoadMailResponse>(response);

            var mail = loadMailResponse.Mail;
            var mailItems = loadMailResponse.MailItems;

            tbMailDetail.Text = mailId + "\r\n";
            tbMailDetail.Text += "제목 : " + mail.Name + "\r\n";
            tbMailDetail.Text += "발신일 : " + mail.TransmissionDate + "\r\n";
            tbMailDetail.Text += "유효기간 : " + mail.ExpireDate + "\r\n";
            tbMailDetail.Text += "내용 : " + mail.Content + "\r\n";
            tbMailDetail.Text += "아이템 수신 여부 : " + mail.IsItemReceived + "\r\n";
            tbMailDetail.Text += "아이템 : ";
            foreach (var item in mailItems)
            {
                tbMailDetail.Text += $"(이름: {ItemName[item.ItemCode]}, 수량: {item.ItemCount}), ";
            }
        }

        private async void btnReceiveMailItem_Click(object sender, EventArgs e)
        {
            var mailBoxRow = tbMailDetail.Text.Split("\r\n")[0];
            var mailId = int.Parse(mailBoxRow.Split(":")[0]);

            var request = new ReceiveMailItemRequest
            {
                MailId = mailId
            };

            var response = await RequestPostAuth("ReceiveMailItem", request);

            if (response == null)
                return;

            var receiveMailItemResponse = JsonSerializer.Deserialize<ReceiveMailItemResponse>(response);
            if (receiveMailItemResponse.Result == ErrorCode.None)
            {
                MessageBox.Show("수령완료");
            }
        }

        private async void btnPlayerInfoLoad_Click(object sender, EventArgs e)
        {
            await LoadPlayerInfo();
        }

        private async Task LoadPlayerInfo()
        {
            var loadPlayerRequest = new LoadPlayerRequest
            {
                PlayerId = playerId
            };

            var response = await RequestPostAuth("LoadPlayer", loadPlayerRequest);
            if (response == null) return;

            var loadPlayerResponse = JsonSerializer.Deserialize<LoadPlayerResponse>(response);
            var player = loadPlayerResponse.Player;

            tbPlayerInfo.Text = "";
            tbPlayerInfo.Text += "소지금 : " + player.Money + "\r\n";
            tbPlayerInfo.Text += "최종출석일 : " + player.LastAttendanceDate + "\r\n";
            tbPlayerInfo.Text += "연속출석일 : " + player.ContinuousAttendanceDays + "\r\n";

            var loadPlayerItemRequest = new LoadPlayerItemRequest();

            response = await RequestPostAuth("LoadPlayerItem", loadPlayerItemRequest);
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

        private async void btnAttend_Click(object sender, EventArgs e)
        {
            var request = new ReceiveAttendanceRewardRequest();

            var response = await RequestPostAuth("ReceiveAttendanceReward", request);
            if (response == null) return;

            var receiveAttendanceRewardResponse = JsonSerializer.Deserialize<ReceiveAttendanceRewardResponse>(response);

            if (receiveAttendanceRewardResponse.Result == ErrorCode.None)
            {
                MessageBox.Show("출석완료! 우편함에서 보상을 확인하세요");
            }
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
                PlayerId = playerId,
                BillToken = playerId *1000 + shopCode,
                ShopCode = shopCode
            };

            var response = await RequestPostAuth("ReceiveInAppPurchaseItem", request);
            if (response == null) return;

            var receiveInAppPurchaseItemResponse = JsonSerializer.Deserialize<ReceiveInAppPurchaseItemResponse>(response);

            if (receiveInAppPurchaseItemResponse.Result == ErrorCode.None)
            {
                MessageBox.Show("구매완료! 우편함에서 아이템을 확인하세요");
            }
        }




        private async void LoadEnforceItemList()
        {
            var request = new LoadPlayerItemRequest();

            var response = await RequestPostAuth("LoadPlayerItem", request);
            if (response == null) return;

            var loadPlayerItemResponse = JsonSerializer.Deserialize<LoadPlayerItemResponse>(response);
            var playerItems = loadPlayerItemResponse.PlayerItems;

            tbEnforceDetail.Text = "";
            tbEnforceResult.Text = "";
            lbxEnforcePlayerItem.Items.Clear();
            enforceItemList.Clear();
            foreach (var item in playerItems)
            {
                enforceItemList.Add(item);
                lbxEnforcePlayerItem.Items.Add($"{item.Id}: " + ItemName[item.ItemCode]);
            }
        }


        private void tabAPI_Selected(object sender, TabControlEventArgs e)
        {
            switch (tabAPI.SelectedTab.Name)
            {
                case "tabMail":
                    _currentMailBoxPageNo = 1;
                    LoadMailBox(_currentMailBoxPageNo);
                    break;
                case "tabPlayerInfo":
                    LoadPlayerInfo();
                    break;
                case "tabEnforce":
                    LoadEnforceItemList();
                    break;

            }
        }

        private void lbxEnforcePlayerItem_DoubleClick(object sender, EventArgs e)
        {
            if (lbxEnforcePlayerItem.SelectedItem == null)
                return;

            int itemId = int.Parse(lbxEnforcePlayerItem.SelectedItem.ToString().Split(":")[0]);

            var item = enforceItemList.Find(x => x.Id == itemId);

            tbEnforceDetail.Text = "";
            tbEnforceDetail.Text += item.Id + "\r\n";
            tbEnforceDetail.Text += $"이름: {ItemName[item.ItemCode]}\r\n";
            tbEnforceDetail.Text += $"수량: {item.Count}\r\n";
            tbEnforceDetail.Text += $"공격력: {item.Attack}\r\n";
            tbEnforceDetail.Text += $"방어력: {item.Defence}\r\n";
            tbEnforceDetail.Text += $"마법력: {item.Magic}\r\n";
            tbEnforceDetail.Text += $"강화횟수: {item.EnhanceCount}\r\n";
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

            var response = await RequestPostAuth("EnforcePlayerItem", request);

            if (response == null) return;

            var enforcePlayerItemResponse = JsonSerializer.Deserialize<EnforcePlayerItemResponse>(response);
            
            if(enforcePlayerItemResponse.Result != ErrorCode.None)
            {
                MessageBox.Show("에러!");
            }
            else
            {
                var enforceState = enforcePlayerItemResponse.EnforceState;
                if(enforceState == EnforceState.Success)
                {
                    var item = enforcePlayerItemResponse.EnforcedItem;

                    tbEnforceResult.Text += item.Id + "\r\n";
                    tbEnforceResult.Text += $"이름: {ItemName[item.ItemCode]}\r\n";
                    tbEnforceResult.Text += $"수량: {item.Count}\r\n";
                    tbEnforceResult.Text += $"공격력: {item.Attack}\r\n";
                    tbEnforceResult.Text += $"방어력: {item.Defence}\r\n";
                    tbEnforceResult.Text += $"마법력: {item.Magic}\r\n";
                    tbEnforceResult.Text += $"강화횟수: {item.EnhanceCount}\r\n";
                }
                else if(enforceState == EnforceState.Fail)
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
    }
}