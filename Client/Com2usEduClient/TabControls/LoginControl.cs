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

namespace Com2usEduClient
{
    public partial class LoginControl : UserControl
    {

        private TextBox tbPlayerId;
        private TextBox tbAccountId;
        private TextBox tbAuthToken;
        private Action showTabs;
        public string Nickname { get; private set; } = "";

        public LoginControl(TextBox tbPlayerId, TextBox tbAccountId, TextBox tbAuthToken, Action showTabs)
        {
            InitializeComponent();
            this.tbPlayerId = tbPlayerId;
            this.tbAccountId = tbAccountId;
            this.tbAuthToken = tbAuthToken;
            this.showTabs = showTabs;
        }


        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            var jsonBody = new CreateAccountRequest
            {
                LoginId = tbID.Text,
                Password = tbPassword.Text,
            };
            var response = await HttpRequest.Post("CreateAccount", JsonSerializer.Serialize(jsonBody, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(HttpRequest.encoderSettings),
                WriteIndented = true,
            }));

            if (response == null) return;
            var createAccountResponse = JsonSerializer.Deserialize<CreateAccountResponse>(response);
            if(createAccountResponse.Result == ErrorCode.None)
                MessageBox.Show("계정이 생성되었습니다.");
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
            var response = await HttpRequest.Post("Login", JsonSerializer.Serialize(jsonBody, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(HttpRequest.encoderSettings),
                WriteIndented = true,
            }));
            if (response == null)
            {
                return;
            }

            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(response);

            tbNotice.Text = loginResponse.Notice;

            HttpRequest.SetAuth(loginResponse.AccountId, loginResponse.PlayerId, loginResponse.AuthToken);


            tbPlayerId.Text = loginResponse.PlayerId.ToString();
            tbAccountId.Text = loginResponse.AccountId.ToString();
            tbAuthToken.Text = loginResponse.AuthToken;

            Nickname = tbID.Text;

            showTabs();
        }
    }
}
