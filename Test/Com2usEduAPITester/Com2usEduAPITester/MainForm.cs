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
using Com2usEduAPITester.TabControls;

namespace Com2usEduAPITester
{
    public partial class MainForm : Form
    {

        List<PlayerItem> enforceItemList = new();

        public MainForm()
        {

            InitializeComponent();

            TabPage loginPage = new TabPage();
            loginPage.Text = "로그인";
            loginPage.Controls.Add(new LoginControl(tbPlayerId, tbAccountId, tbAuthToken));
            tabAPI.TabPages.Add(loginPage);

            TabPage mailboxPage = new TabPage();
            mailboxPage.Text = "우편함";
            mailboxPage.Controls.Add(new MailboxControl());
            tabAPI.TabPages.Add(mailboxPage);

            TabPage playerInfoPage = new TabPage();
            playerInfoPage.Text = "플레이어 정보";
            playerInfoPage.Controls.Add(new PlayerInfoControl());
            tabAPI.TabPages.Add(playerInfoPage);


            TabPage attendancePage = new TabPage();
            attendancePage.Text = "출석부";
            attendancePage.Controls.Add(new AttendanceControl());
            tabAPI.TabPages.Add(attendancePage);

            TabPage shopPage = new TabPage();
            shopPage.Text = "상점";
            shopPage.Controls.Add(new ShopControl());
            tabAPI.TabPages.Add(shopPage);

            TabPage enforcePage = new TabPage();
            enforcePage.Text = "강화";
            enforcePage.Controls.Add(new EnforceControl());
            tabAPI.TabPages.Add(enforcePage);

            TabPage chatPage = new TabPage();
            chatPage.Text = "채팅";
            chatPage.Controls.Add(new ChatControl());
            tabAPI.TabPages.Add(chatPage);
        }


        private void tabAPI_Selected(object sender, TabControlEventArgs e)
        {
            switch (tabAPI.SelectedTab.Name)
            {
                case "tabMail":
                    break;
                case "tabPlayerInfo":
                    break;
                case "tabEnforce":
                    break;
                case "tabChat":
                    break;
            }
        }
    }
}