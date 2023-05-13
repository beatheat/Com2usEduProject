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
        public ChatControl(string nickname)
        {
            InitializeComponent();
            for (int i = 1; i <= 100; i++)
                cbChannel.Items.Add(i);
            cbChannel.SelectedIndex = new Random().Next(1, 100);
            this.nickname = nickname;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
            timer.Stop();
        }

        public void RefreshData()
        {
            LoadChatHistory();
            timer.Enabled = true;
            timer.Start();
        }
        public void StopTimer()
        {
            timer.Stop();
        }

        private async void LoadChatHistory()
        {
            var request = new EnterChatLobbyRequest
            {
                LobbyNumber = int.Parse(cbChannel.SelectedItem.ToString())
            };
            var response = await HttpRequest.PostAuth("EnterChatLobby", request);

            if (response == null) return;

            var enterChatLobbyReponse = JsonSerializer.Deserialize<EnterChatLobbyResponse>(response);

            tbChatViewer.Text = "";
            foreach(var chat in enterChatLobbyReponse.ChatHistory)
            {
                ShowChat(chat);
            }
            if (enterChatLobbyReponse.ChatHistory.Count != 0)
                lastChatIndex = enterChatLobbyReponse.ChatHistory.Last().Index;
            else
                lastChatIndex = -1;
        }


        private void ShowChat(Chat chat)
        {
            string line = "";
            line += chat.PlayerNickname + " : " + chat.Content;
            tbChatViewer.AppendText(line + "\r\n");
        }

        private async Task WriteChat()
        {

            var request = new WriteChatRequest
            {
                LobbyNumber = int.Parse(cbChannel.SelectedItem.ToString()),
                PlayerNickName = nickname,
                Chat = tbUserChat.Text
            };
            tbUserChat.Text = "";
            var response = await HttpRequest.PostAuth("WriteChat", request);
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
            var request = new ReadChatRequest
            {
                LobbyNumber = int.Parse(cbChannel.SelectedItem.ToString()),
                LastChatIndex = lastChatIndex
            };
            var response = await HttpRequest.PostAuth("ReadChat", request);

            if (response == null) return;

            var readChatResponse = JsonSerializer.Deserialize<ReadChatResponse>(response);

            foreach (var chat in readChatResponse.Chats)
            {
                ShowChat(chat);
            }
            if (readChatResponse.Chats.Count != 0)
                lastChatIndex = readChatResponse.Chats.Last().Index;
        }

        private void cbChannel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadChatHistory();
        }
    }
}
