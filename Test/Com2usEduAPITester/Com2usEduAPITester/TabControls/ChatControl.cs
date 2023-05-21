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
using Com2usEduAPITester.Game;

namespace Com2usEduAPITester.TabControls
{
    public partial class ChatControl : UserControl
    {
        private string nickname = "";
        private long lastChatIndex = -1;

        private int currentLobbyNumber = 0;
        public ChatControl(string nickname)
        {
            InitializeComponent();
            for (int i = 1; i <= 100; i++)
                cbChannel.Items.Add(i);
            this.nickname = nickname;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
            timer.Stop();
        }

        public void RefreshData()
        {
            EnterLobby();
            timer.Enabled = true;
            timer.Start();
        }
        public void StopTimer()
        {
            timer.Stop();
        }

        private async void EnterLobby()
        {
            EnterChatLobbyRequest request;
            request = new EnterChatLobbyRequest();

            var response = await HttpRequest.PostAuth<EnterChatLobbyResponse>("EnterChatLobby", request);

            if (response == null) return;


            cbChannel.SelectedIndex = response.LobbyNumber - 1;
            currentLobbyNumber = response.LobbyNumber;

            tbChatViewer.Text = "";
            foreach (var chat in response.ChatHistory)
            {
                ShowChat(chat);
            }
            if (response.ChatHistory.Count != 0)
                lastChatIndex = response.ChatHistory.Last().Index;
            else
                lastChatIndex = -1;

        }

        private async Task ExitLobby()
        {
            ExitChatLobbyRequest request;
            request = new ExitChatLobbyRequest();
            
            var response = await HttpRequest.PostAuth<ExitChatLobbyResponse>("ExitChatLobby", request);

            if (response == null) return;
        }
        
        private async void ChangeLobby()
        {
            EnterChatLobbyRequest request;

            request = new EnterChatLobbyRequest
            {
                LobbyNumber = currentLobbyNumber
            };

            var response = await HttpRequest.PostAuth<EnterChatLobbyResponse>("EnterChatLobby", request);

            if (response == null) return;


            tbChatViewer.Text = "";
            foreach (var chat in response.ChatHistory)
            {
                ShowChat(chat);
            }
            if (response.ChatHistory.Count != 0)
                lastChatIndex = response.ChatHistory.Last().Index;
            else
                lastChatIndex = -1;
        }


        private void ShowChat(Chat chat)
        {
            tbChatViewer.AppendText(chat.PlayerNickname + " : " + chat.Content + "\r\n");
        }

        private async Task WriteChat()
        {
            WriteChatRequest request;

            request = new WriteChatRequest
            {
                LobbyNumber = currentLobbyNumber,
                PlayerNickName = nickname,
                Chat = tbUserChat.Text
            };
            
            tbUserChat.Text = "";
            var response = await HttpRequest.PostAuth<WriteChatResponse>("WriteChat", request);
            ReadChat();
        }


        private async void tbUserChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            await WriteChat();
        }

        private async void btnChat_Click(object sender, EventArgs e)
        {
            await WriteChat();
        }

        private void cbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ReadChat();
        }

        private async void ReadChat()
        {
            ReadChatRequest request;

            request = new ReadChatRequest
            {
                LobbyNumber = currentLobbyNumber,
                LastChatIndex = lastChatIndex
            };
            
            var response = await HttpRequest.PostAuth<ReadChatResponse>("ReadChat", request);

            if (response == null) return;

            foreach (var chat in response.Chats)
            {
                ShowChat(chat);
            }
            if (response.Chats.Count != 0)
                lastChatIndex = response.Chats.Last().Index;
        }

        private async void cbChannel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            await ExitLobby();
            currentLobbyNumber = cbChannel.SelectedIndex + 1;
            ChangeLobby();
        }
    }
}
