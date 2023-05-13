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

        LoginControl _loginControl;
        MailboxControl _mailboxContorl;
        PlayerInfoControl _playerInfoControl;
        AttendanceControl _attendanceControl;
        ShopControl _shopControl;
        EnforceControl _enforceControl;
        ChatControl _chatContorl;

        public MainForm()
        {

            InitializeComponent();

            HttpRequest.Init(tbMasterDataVersion.Text, tbClientVersion.Text, tbRequest, tbResponse);
            HttpRequest.SetServerURL(tbServerURL.Text);

            TabPage loginPage = new TabPage();
            _loginControl = new LoginControl(tbPlayerId, tbAccountId, tbAuthToken, ShowTabs);
            loginPage.Text = "로그인";
            loginPage.Controls.Add(_loginControl);
            tabAPI.TabPages.Add(loginPage);

        }

        private void ShowTabs()
        {
            for (int i = tabAPI.Controls.Count - 1; i > 1; i--)
                tabAPI.Controls.RemoveAt(i);

            TabPage mailboxPage = new TabPage();
            _mailboxContorl = new MailboxControl();
            mailboxPage.Text = "우편함";
            mailboxPage.Controls.Add(_mailboxContorl);
            tabAPI.TabPages.Add(mailboxPage);

            TabPage playerInfoPage = new TabPage();
            _playerInfoControl = new PlayerInfoControl();
            playerInfoPage.Text = "플레이어 정보";
            playerInfoPage.Controls.Add(_playerInfoControl);
            tabAPI.TabPages.Add(playerInfoPage);

            TabPage attendancePage = new TabPage();
            _attendanceControl = new AttendanceControl();
            attendancePage.Text = "출석부";
            attendancePage.Controls.Add(_attendanceControl);
            tabAPI.TabPages.Add(attendancePage);

            TabPage shopPage = new TabPage();
            _shopControl = new ShopControl();
            shopPage.Text = "상점";
            shopPage.Controls.Add(_shopControl);
            tabAPI.TabPages.Add(shopPage);

            TabPage enforcePage = new TabPage();
            _enforceControl = new EnforceControl();
            enforcePage.Text = "강화";
            enforcePage.Controls.Add(_enforceControl);
            tabAPI.TabPages.Add(enforcePage);

            TabPage chatPage = new TabPage();
            _chatContorl = new ChatControl(_loginControl.Nickname);
            chatPage.Text = "채팅";
            chatPage.Controls.Add(_chatContorl);
            tabAPI.TabPages.Add(chatPage);
        }


        private void tabAPI_Selected(object sender, TabControlEventArgs e)
        {
            bool chat = false;
            switch (tabAPI.SelectedTab.Text)
            {
                case "우편함":
                    _mailboxContorl.RefreshData();
                    break;
                case "플레이어 정보":
                    _playerInfoControl.RefreshData();
                    break;
                case "강화":
                    _enforceControl.RefreshData();
                    break;
                case "채팅":
                    _chatContorl.RefreshData();
                    chat = true;
                    break;
            }
            if (chat == false) _chatContorl.StopTimer();
        }

        private void tbServerURL_TextChanged(object sender, EventArgs e)
        {
            HttpRequest.SetServerURL(tbServerURL.Text);
        }
    }
}