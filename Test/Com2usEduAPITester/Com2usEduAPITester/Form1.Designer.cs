namespace Com2usEduAPITester
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabAPI = new System.Windows.Forms.TabControl();
            this.tabLogin = new System.Windows.Forms.TabPage();
            this.lbNotice = new System.Windows.Forms.Label();
            this.tbNotice = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCreateAccount = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbID = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.tabMail = new System.Windows.Forms.TabPage();
            this.btnReceiveMailItem = new System.Windows.Forms.Button();
            this.btnMailBoxLeft = new System.Windows.Forms.Button();
            this.lbMailBoxPageNo = new System.Windows.Forms.Label();
            this.btnMailBoxRight = new System.Windows.Forms.Button();
            this.tbMailDetail = new System.Windows.Forms.TextBox();
            this.lbMailDetal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbxMailBox = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabPlayerInfo = new System.Windows.Forms.TabPage();
            this.btnPlayerInfoLoad = new System.Windows.Forms.Button();
            this.tbPlayerItemInfo = new System.Windows.Forms.TextBox();
            this.tbPlayerInfo = new System.Windows.Forms.TextBox();
            this.lbPlayerItemInfo = new System.Windows.Forms.Label();
            this.lbPlayerInfo = new System.Windows.Forms.Label();
            this.tabAttendance = new System.Windows.Forms.TabPage();
            this.btnAttend = new System.Windows.Forms.Button();
            this.tabShop = new System.Windows.Forms.TabPage();
            this.lbShopItem3 = new System.Windows.Forms.Label();
            this.tbShopItem3 = new System.Windows.Forms.TextBox();
            this.btnBuyShopItem3 = new System.Windows.Forms.Button();
            this.lbShopItem2 = new System.Windows.Forms.Label();
            this.tbShopItem2 = new System.Windows.Forms.TextBox();
            this.btnBuyShopItem2 = new System.Windows.Forms.Button();
            this.lbShopItem1 = new System.Windows.Forms.Label();
            this.tbShopItem1 = new System.Windows.Forms.TextBox();
            this.btnBuyShopItem1 = new System.Windows.Forms.Button();
            this.tabEnforce = new System.Windows.Forms.TabPage();
            this.lbEnforceDetail = new System.Windows.Forms.Label();
            this.btnEnforce = new System.Windows.Forms.Button();
            this.lbEnforceArrow = new System.Windows.Forms.Label();
            this.lbEnforceResult = new System.Windows.Forms.Label();
            this.tbEnforceResult = new System.Windows.Forms.TextBox();
            this.lbEnforceTarget = new System.Windows.Forms.Label();
            this.tbEnforceDetail = new System.Windows.Forms.TextBox();
            this.lbxEnforcePlayerItem = new System.Windows.Forms.ListBox();
            this.lbEnforceList = new System.Windows.Forms.Label();
            this.tbRequest = new System.Windows.Forms.TextBox();
            this.lbRequest = new System.Windows.Forms.Label();
            this.lbResponse = new System.Windows.Forms.Label();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.tbServerURL = new System.Windows.Forms.TextBox();
            this.lbServerURL = new System.Windows.Forms.Label();
            this.lbAccountId = new System.Windows.Forms.Label();
            this.lbAuth = new System.Windows.Forms.Label();
            this.tbAccountId = new System.Windows.Forms.TextBox();
            this.lbPlayerId = new System.Windows.Forms.Label();
            this.tbPlayerId = new System.Windows.Forms.TextBox();
            this.tbAuthToken = new System.Windows.Forms.TextBox();
            this.lbMasterVersion = new System.Windows.Forms.Label();
            this.tbMasterDataVersion = new System.Windows.Forms.TextBox();
            this.lbClientVersion = new System.Windows.Forms.Label();
            this.tbClientVersion = new System.Windows.Forms.TextBox();
            this.tabAPI.SuspendLayout();
            this.tabLogin.SuspendLayout();
            this.tabMail.SuspendLayout();
            this.tabPlayerInfo.SuspendLayout();
            this.tabAttendance.SuspendLayout();
            this.tabShop.SuspendLayout();
            this.tabEnforce.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabAPI
            // 
            this.tabAPI.Controls.Add(this.tabLogin);
            this.tabAPI.Controls.Add(this.tabMail);
            this.tabAPI.Controls.Add(this.tabPlayerInfo);
            this.tabAPI.Controls.Add(this.tabAttendance);
            this.tabAPI.Controls.Add(this.tabShop);
            this.tabAPI.Controls.Add(this.tabEnforce);
            this.tabAPI.Location = new System.Drawing.Point(12, 76);
            this.tabAPI.Name = "tabAPI";
            this.tabAPI.SelectedIndex = 0;
            this.tabAPI.Size = new System.Drawing.Size(1006, 363);
            this.tabAPI.TabIndex = 0;
            this.tabAPI.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabAPI_Selected);
            // 
            // tabLogin
            // 
            this.tabLogin.Controls.Add(this.lbNotice);
            this.tabLogin.Controls.Add(this.tbNotice);
            this.tabLogin.Controls.Add(this.btnLogin);
            this.tabLogin.Controls.Add(this.btnCreateAccount);
            this.tabLogin.Controls.Add(this.tbPassword);
            this.tabLogin.Controls.Add(this.lbPassword);
            this.tabLogin.Controls.Add(this.lbID);
            this.tabLogin.Controls.Add(this.tbID);
            this.tabLogin.Location = new System.Drawing.Point(4, 29);
            this.tabLogin.Name = "tabLogin";
            this.tabLogin.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogin.Size = new System.Drawing.Size(998, 330);
            this.tabLogin.TabIndex = 0;
            this.tabLogin.Text = "로그인";
            this.tabLogin.UseVisualStyleBackColor = true;
            // 
            // lbNotice
            // 
            this.lbNotice.AutoSize = true;
            this.lbNotice.Location = new System.Drawing.Point(564, 15);
            this.lbNotice.Name = "lbNotice";
            this.lbNotice.Size = new System.Drawing.Size(69, 20);
            this.lbNotice.TabIndex = 7;
            this.lbNotice.Text = "공지사항";
            // 
            // tbNotice
            // 
            this.tbNotice.Location = new System.Drawing.Point(564, 51);
            this.tbNotice.Multiline = true;
            this.tbNotice.Name = "tbNotice";
            this.tbNotice.Size = new System.Drawing.Size(410, 246);
            this.tbNotice.TabIndex = 6;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(13, 153);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(215, 29);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.Location = new System.Drawing.Point(13, 118);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new System.Drawing.Size(215, 29);
            this.btnCreateAccount.TabIndex = 4;
            this.btnCreateAccount.Text = "CreateAccount";
            this.btnCreateAccount.UseVisualStyleBackColor = true;
            this.btnCreateAccount.Click += new System.EventHandler(this.btnCreateAccount_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(103, 64);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(125, 27);
            this.tbPassword.TabIndex = 3;
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(13, 71);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(69, 20);
            this.lbPassword.TabIndex = 2;
            this.lbPassword.Text = "비밀번호";
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.Location = new System.Drawing.Point(13, 27);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(54, 20);
            this.lbID.TabIndex = 1;
            this.lbID.Text = "아이디";
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(103, 24);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(125, 27);
            this.tbID.TabIndex = 0;
            // 
            // tabMail
            // 
            this.tabMail.Controls.Add(this.btnReceiveMailItem);
            this.tabMail.Controls.Add(this.btnMailBoxLeft);
            this.tabMail.Controls.Add(this.lbMailBoxPageNo);
            this.tabMail.Controls.Add(this.btnMailBoxRight);
            this.tabMail.Controls.Add(this.tbMailDetail);
            this.tabMail.Controls.Add(this.lbMailDetal);
            this.tabMail.Controls.Add(this.label1);
            this.tabMail.Controls.Add(this.lbxMailBox);
            this.tabMail.Controls.Add(this.btnRefresh);
            this.tabMail.Location = new System.Drawing.Point(4, 29);
            this.tabMail.Name = "tabMail";
            this.tabMail.Padding = new System.Windows.Forms.Padding(3);
            this.tabMail.Size = new System.Drawing.Size(998, 330);
            this.tabMail.TabIndex = 1;
            this.tabMail.Text = "우편함";
            this.tabMail.UseVisualStyleBackColor = true;
            // 
            // btnReceiveMailItem
            // 
            this.btnReceiveMailItem.Location = new System.Drawing.Point(866, 283);
            this.btnReceiveMailItem.Name = "btnReceiveMailItem";
            this.btnReceiveMailItem.Size = new System.Drawing.Size(104, 29);
            this.btnReceiveMailItem.TabIndex = 13;
            this.btnReceiveMailItem.Text = "아이템 수령";
            this.btnReceiveMailItem.UseVisualStyleBackColor = true;
            this.btnReceiveMailItem.Click += new System.EventHandler(this.btnReceiveMailItem_Click);
            // 
            // btnMailBoxLeft
            // 
            this.btnMailBoxLeft.Location = new System.Drawing.Point(104, 283);
            this.btnMailBoxLeft.Name = "btnMailBoxLeft";
            this.btnMailBoxLeft.Size = new System.Drawing.Size(94, 29);
            this.btnMailBoxLeft.TabIndex = 12;
            this.btnMailBoxLeft.Text = "<";
            this.btnMailBoxLeft.UseVisualStyleBackColor = true;
            this.btnMailBoxLeft.Click += new System.EventHandler(this.btnMailBoxLeft_Click);
            // 
            // lbMailBoxPageNo
            // 
            this.lbMailBoxPageNo.AutoSize = true;
            this.lbMailBoxPageNo.Location = new System.Drawing.Point(236, 287);
            this.lbMailBoxPageNo.Name = "lbMailBoxPageNo";
            this.lbMailBoxPageNo.Size = new System.Drawing.Size(41, 20);
            this.lbMailBoxPageNo.TabIndex = 11;
            this.lbMailBoxPageNo.Text = "0 / 0";
            // 
            // btnMailBoxRight
            // 
            this.btnMailBoxRight.Location = new System.Drawing.Point(323, 283);
            this.btnMailBoxRight.Name = "btnMailBoxRight";
            this.btnMailBoxRight.Size = new System.Drawing.Size(94, 29);
            this.btnMailBoxRight.TabIndex = 10;
            this.btnMailBoxRight.Text = ">";
            this.btnMailBoxRight.UseVisualStyleBackColor = true;
            this.btnMailBoxRight.Click += new System.EventHandler(this.btnMailBoxRight_Click);
            // 
            // tbMailDetail
            // 
            this.tbMailDetail.Location = new System.Drawing.Point(510, 53);
            this.tbMailDetail.Multiline = true;
            this.tbMailDetail.Name = "tbMailDetail";
            this.tbMailDetail.ReadOnly = true;
            this.tbMailDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMailDetail.Size = new System.Drawing.Size(460, 224);
            this.tbMailDetail.TabIndex = 7;
            // 
            // lbMailDetal
            // 
            this.lbMailDetal.AutoSize = true;
            this.lbMailDetal.Location = new System.Drawing.Point(514, 19);
            this.lbMailDetal.Name = "lbMailDetal";
            this.lbMailDetal.Size = new System.Drawing.Size(69, 20);
            this.lbMailDetal.TabIndex = 9;
            this.lbMailDetal.Text = "우편상세";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "우편함";
            // 
            // lbxMailBox
            // 
            this.lbxMailBox.FormattingEnabled = true;
            this.lbxMailBox.ItemHeight = 20;
            this.lbxMailBox.Location = new System.Drawing.Point(21, 53);
            this.lbxMailBox.Name = "lbxMailBox";
            this.lbxMailBox.Size = new System.Drawing.Size(464, 224);
            this.lbxMailBox.TabIndex = 1;
            this.lbxMailBox.DoubleClick += new System.EventHandler(this.lbxMailBox_DoubleClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(391, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(94, 29);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "새로고침";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tabPlayerInfo
            // 
            this.tabPlayerInfo.Controls.Add(this.btnPlayerInfoLoad);
            this.tabPlayerInfo.Controls.Add(this.tbPlayerItemInfo);
            this.tabPlayerInfo.Controls.Add(this.tbPlayerInfo);
            this.tabPlayerInfo.Controls.Add(this.lbPlayerItemInfo);
            this.tabPlayerInfo.Controls.Add(this.lbPlayerInfo);
            this.tabPlayerInfo.Location = new System.Drawing.Point(4, 29);
            this.tabPlayerInfo.Name = "tabPlayerInfo";
            this.tabPlayerInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPlayerInfo.Size = new System.Drawing.Size(998, 330);
            this.tabPlayerInfo.TabIndex = 2;
            this.tabPlayerInfo.Text = "플레이어 정보";
            this.tabPlayerInfo.UseVisualStyleBackColor = true;
            // 
            // btnPlayerInfoLoad
            // 
            this.btnPlayerInfoLoad.Location = new System.Drawing.Point(898, 4);
            this.btnPlayerInfoLoad.Name = "btnPlayerInfoLoad";
            this.btnPlayerInfoLoad.Size = new System.Drawing.Size(94, 29);
            this.btnPlayerInfoLoad.TabIndex = 4;
            this.btnPlayerInfoLoad.Text = "로드";
            this.btnPlayerInfoLoad.UseVisualStyleBackColor = true;
            this.btnPlayerInfoLoad.Click += new System.EventHandler(this.btnPlayerInfoLoad_Click);
            // 
            // tbPlayerItemInfo
            // 
            this.tbPlayerItemInfo.Location = new System.Drawing.Point(472, 36);
            this.tbPlayerItemInfo.Multiline = true;
            this.tbPlayerItemInfo.Name = "tbPlayerItemInfo";
            this.tbPlayerItemInfo.ReadOnly = true;
            this.tbPlayerItemInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbPlayerItemInfo.Size = new System.Drawing.Size(520, 291);
            this.tbPlayerItemInfo.TabIndex = 3;
            // 
            // tbPlayerInfo
            // 
            this.tbPlayerInfo.Location = new System.Drawing.Point(6, 36);
            this.tbPlayerInfo.Multiline = true;
            this.tbPlayerInfo.Name = "tbPlayerInfo";
            this.tbPlayerInfo.ReadOnly = true;
            this.tbPlayerInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tbPlayerInfo.Size = new System.Drawing.Size(447, 291);
            this.tbPlayerInfo.TabIndex = 2;
            // 
            // lbPlayerItemInfo
            // 
            this.lbPlayerItemInfo.AutoSize = true;
            this.lbPlayerItemInfo.Location = new System.Drawing.Point(472, 13);
            this.lbPlayerItemInfo.Name = "lbPlayerItemInfo";
            this.lbPlayerItemInfo.Size = new System.Drawing.Size(119, 20);
            this.lbPlayerItemInfo.TabIndex = 1;
            this.lbPlayerItemInfo.Text = "플레이어 아이템";
            // 
            // lbPlayerInfo
            // 
            this.lbPlayerInfo.AutoSize = true;
            this.lbPlayerInfo.Location = new System.Drawing.Point(6, 13);
            this.lbPlayerInfo.Name = "lbPlayerInfo";
            this.lbPlayerInfo.Size = new System.Drawing.Size(104, 20);
            this.lbPlayerInfo.TabIndex = 0;
            this.lbPlayerInfo.Text = "플레이어 정보";
            // 
            // tabAttendance
            // 
            this.tabAttendance.Controls.Add(this.btnAttend);
            this.tabAttendance.Location = new System.Drawing.Point(4, 29);
            this.tabAttendance.Name = "tabAttendance";
            this.tabAttendance.Padding = new System.Windows.Forms.Padding(3);
            this.tabAttendance.Size = new System.Drawing.Size(998, 330);
            this.tabAttendance.TabIndex = 3;
            this.tabAttendance.Text = "출석부";
            this.tabAttendance.UseVisualStyleBackColor = true;
            // 
            // btnAttend
            // 
            this.btnAttend.Location = new System.Drawing.Point(560, 6);
            this.btnAttend.Name = "btnAttend";
            this.btnAttend.Size = new System.Drawing.Size(94, 29);
            this.btnAttend.TabIndex = 0;
            this.btnAttend.Text = "출석하기";
            this.btnAttend.UseVisualStyleBackColor = true;
            this.btnAttend.Click += new System.EventHandler(this.btnAttend_Click);
            // 
            // tabShop
            // 
            this.tabShop.Controls.Add(this.lbShopItem3);
            this.tabShop.Controls.Add(this.tbShopItem3);
            this.tabShop.Controls.Add(this.btnBuyShopItem3);
            this.tabShop.Controls.Add(this.lbShopItem2);
            this.tabShop.Controls.Add(this.tbShopItem2);
            this.tabShop.Controls.Add(this.btnBuyShopItem2);
            this.tabShop.Controls.Add(this.lbShopItem1);
            this.tabShop.Controls.Add(this.tbShopItem1);
            this.tabShop.Controls.Add(this.btnBuyShopItem1);
            this.tabShop.Location = new System.Drawing.Point(4, 29);
            this.tabShop.Name = "tabShop";
            this.tabShop.Padding = new System.Windows.Forms.Padding(3);
            this.tabShop.Size = new System.Drawing.Size(998, 330);
            this.tabShop.TabIndex = 4;
            this.tabShop.Text = "상점";
            this.tabShop.UseVisualStyleBackColor = true;
            // 
            // lbShopItem3
            // 
            this.lbShopItem3.AutoSize = true;
            this.lbShopItem3.Location = new System.Drawing.Point(788, 36);
            this.lbShopItem3.Name = "lbShopItem3";
            this.lbShopItem3.Size = new System.Drawing.Size(84, 20);
            this.lbShopItem3.TabIndex = 23;
            this.lbShopItem3.Text = "중급자세트";
            // 
            // tbShopItem3
            // 
            this.tbShopItem3.Location = new System.Drawing.Point(729, 80);
            this.tbShopItem3.Multiline = true;
            this.tbShopItem3.Name = "tbShopItem3";
            this.tbShopItem3.ReadOnly = true;
            this.tbShopItem3.Size = new System.Drawing.Size(206, 173);
            this.tbShopItem3.TabIndex = 22;
            this.tbShopItem3.Text = "돈 2000\r\n작은 칼 1\r\n나무 방패 1\r\n보통 모자 1";
            // 
            // btnBuyShopItem3
            // 
            this.btnBuyShopItem3.Location = new System.Drawing.Point(778, 274);
            this.btnBuyShopItem3.Name = "btnBuyShopItem3";
            this.btnBuyShopItem3.Size = new System.Drawing.Size(94, 29);
            this.btnBuyShopItem3.TabIndex = 21;
            this.btnBuyShopItem3.Text = "구매";
            this.btnBuyShopItem3.UseVisualStyleBackColor = true;
            this.btnBuyShopItem3.Click += new System.EventHandler(this.btnBuyShopItem3_Click);
            // 
            // lbShopItem2
            // 
            this.lbShopItem2.AutoSize = true;
            this.lbShopItem2.Location = new System.Drawing.Point(445, 36);
            this.lbShopItem2.Name = "lbShopItem2";
            this.lbShopItem2.Size = new System.Drawing.Size(92, 20);
            this.lbShopItem2.TabIndex = 20;
            this.lbShopItem2.Text = "초보자세트2";
            // 
            // tbShopItem2
            // 
            this.tbShopItem2.Location = new System.Drawing.Point(386, 80);
            this.tbShopItem2.Multiline = true;
            this.tbShopItem2.Name = "tbShopItem2";
            this.tbShopItem2.ReadOnly = true;
            this.tbShopItem2.Size = new System.Drawing.Size(206, 173);
            this.tbShopItem2.TabIndex = 19;
            this.tbShopItem2.Text = "나무방패 1\r\n보통모자 1\r\n포션 10";
            // 
            // btnBuyShopItem2
            // 
            this.btnBuyShopItem2.Location = new System.Drawing.Point(435, 274);
            this.btnBuyShopItem2.Name = "btnBuyShopItem2";
            this.btnBuyShopItem2.Size = new System.Drawing.Size(94, 29);
            this.btnBuyShopItem2.TabIndex = 18;
            this.btnBuyShopItem2.Text = "구매";
            this.btnBuyShopItem2.UseVisualStyleBackColor = true;
            this.btnBuyShopItem2.Click += new System.EventHandler(this.btnBuyShopItem2_Click);
            // 
            // lbShopItem1
            // 
            this.lbShopItem1.AutoSize = true;
            this.lbShopItem1.Location = new System.Drawing.Point(99, 36);
            this.lbShopItem1.Name = "lbShopItem1";
            this.lbShopItem1.Size = new System.Drawing.Size(84, 20);
            this.lbShopItem1.TabIndex = 17;
            this.lbShopItem1.Text = "초보자세트";
            // 
            // tbShopItem1
            // 
            this.tbShopItem1.Location = new System.Drawing.Point(40, 80);
            this.tbShopItem1.Multiline = true;
            this.tbShopItem1.Name = "tbShopItem1";
            this.tbShopItem1.ReadOnly = true;
            this.tbShopItem1.Size = new System.Drawing.Size(206, 173);
            this.tbShopItem1.TabIndex = 1;
            this.tbShopItem1.Text = "돈 1000원\r\n작은 칼 1개\r\n도금 칼 1개";
            // 
            // btnBuyShopItem1
            // 
            this.btnBuyShopItem1.Location = new System.Drawing.Point(89, 274);
            this.btnBuyShopItem1.Name = "btnBuyShopItem1";
            this.btnBuyShopItem1.Size = new System.Drawing.Size(94, 29);
            this.btnBuyShopItem1.TabIndex = 0;
            this.btnBuyShopItem1.Text = "구매";
            this.btnBuyShopItem1.UseVisualStyleBackColor = true;
            this.btnBuyShopItem1.Click += new System.EventHandler(this.btnBuyShopItem1_Click);
            // 
            // tabEnforce
            // 
            this.tabEnforce.Controls.Add(this.lbEnforceDetail);
            this.tabEnforce.Controls.Add(this.btnEnforce);
            this.tabEnforce.Controls.Add(this.lbEnforceArrow);
            this.tabEnforce.Controls.Add(this.lbEnforceResult);
            this.tabEnforce.Controls.Add(this.tbEnforceResult);
            this.tabEnforce.Controls.Add(this.lbEnforceTarget);
            this.tabEnforce.Controls.Add(this.tbEnforceDetail);
            this.tabEnforce.Controls.Add(this.lbxEnforcePlayerItem);
            this.tabEnforce.Controls.Add(this.lbEnforceList);
            this.tabEnforce.Location = new System.Drawing.Point(4, 29);
            this.tabEnforce.Name = "tabEnforce";
            this.tabEnforce.Padding = new System.Windows.Forms.Padding(3);
            this.tabEnforce.Size = new System.Drawing.Size(998, 330);
            this.tabEnforce.TabIndex = 5;
            this.tabEnforce.Text = "강화";
            this.tabEnforce.UseVisualStyleBackColor = true;
            // 
            // lbEnforceDetail
            // 
            this.lbEnforceDetail.AutoSize = true;
            this.lbEnforceDetail.Location = new System.Drawing.Point(365, 255);
            this.lbEnforceDetail.Name = "lbEnforceDetail";
            this.lbEnforceDetail.Size = new System.Drawing.Size(179, 40);
            this.lbEnforceDetail.TabIndex = 12;
            this.lbEnforceDetail.Text = "강화확률 : 30%\r\n강화 실패 시 아이템 파괴\r\n";
            // 
            // btnEnforce
            // 
            this.btnEnforce.Location = new System.Drawing.Point(567, 235);
            this.btnEnforce.Name = "btnEnforce";
            this.btnEnforce.Size = new System.Drawing.Size(180, 82);
            this.btnEnforce.TabIndex = 11;
            this.btnEnforce.Text = "강화";
            this.btnEnforce.UseVisualStyleBackColor = true;
            this.btnEnforce.Click += new System.EventHandler(this.btnEnforce_Click);
            // 
            // lbEnforceArrow
            // 
            this.lbEnforceArrow.AutoSize = true;
            this.lbEnforceArrow.Location = new System.Drawing.Point(603, 108);
            this.lbEnforceArrow.Name = "lbEnforceArrow";
            this.lbEnforceArrow.Size = new System.Drawing.Size(108, 20);
            this.lbEnforceArrow.TabIndex = 10;
            this.lbEnforceArrow.Text = "=======>>";
            // 
            // lbEnforceResult
            // 
            this.lbEnforceResult.AutoSize = true;
            this.lbEnforceResult.Location = new System.Drawing.Point(731, 31);
            this.lbEnforceResult.Name = "lbEnforceResult";
            this.lbEnforceResult.Size = new System.Drawing.Size(74, 20);
            this.lbEnforceResult.TabIndex = 9;
            this.lbEnforceResult.Text = "강화 결과";
            // 
            // tbEnforceResult
            // 
            this.tbEnforceResult.Location = new System.Drawing.Point(731, 55);
            this.tbEnforceResult.Multiline = true;
            this.tbEnforceResult.Name = "tbEnforceResult";
            this.tbEnforceResult.ReadOnly = true;
            this.tbEnforceResult.Size = new System.Drawing.Size(144, 174);
            this.tbEnforceResult.TabIndex = 8;
            // 
            // lbEnforceTarget
            // 
            this.lbEnforceTarget.AutoSize = true;
            this.lbEnforceTarget.Location = new System.Drawing.Point(438, 31);
            this.lbEnforceTarget.Name = "lbEnforceTarget";
            this.lbEnforceTarget.Size = new System.Drawing.Size(74, 20);
            this.lbEnforceTarget.TabIndex = 7;
            this.lbEnforceTarget.Text = "강화 대상";
            // 
            // tbEnforceDetail
            // 
            this.tbEnforceDetail.Location = new System.Drawing.Point(438, 55);
            this.tbEnforceDetail.Multiline = true;
            this.tbEnforceDetail.Name = "tbEnforceDetail";
            this.tbEnforceDetail.ReadOnly = true;
            this.tbEnforceDetail.Size = new System.Drawing.Size(144, 174);
            this.tbEnforceDetail.TabIndex = 6;
            // 
            // lbxEnforcePlayerItem
            // 
            this.lbxEnforcePlayerItem.FormattingEnabled = true;
            this.lbxEnforcePlayerItem.ItemHeight = 20;
            this.lbxEnforcePlayerItem.Location = new System.Drawing.Point(6, 33);
            this.lbxEnforcePlayerItem.Name = "lbxEnforcePlayerItem";
            this.lbxEnforcePlayerItem.Size = new System.Drawing.Size(339, 284);
            this.lbxEnforcePlayerItem.TabIndex = 5;
            this.lbxEnforcePlayerItem.DoubleClick += new System.EventHandler(this.lbxEnforcePlayerItem_DoubleClick);
            // 
            // lbEnforceList
            // 
            this.lbEnforceList.AutoSize = true;
            this.lbEnforceList.Location = new System.Drawing.Point(3, 10);
            this.lbEnforceList.Name = "lbEnforceList";
            this.lbEnforceList.Size = new System.Drawing.Size(119, 20);
            this.lbEnforceList.TabIndex = 4;
            this.lbEnforceList.Text = "플레이어 아이템";
            // 
            // tbRequest
            // 
            this.tbRequest.Location = new System.Drawing.Point(16, 465);
            this.tbRequest.Multiline = true;
            this.tbRequest.Name = "tbRequest";
            this.tbRequest.ReadOnly = true;
            this.tbRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbRequest.Size = new System.Drawing.Size(485, 241);
            this.tbRequest.TabIndex = 1;
            // 
            // lbRequest
            // 
            this.lbRequest.AutoSize = true;
            this.lbRequest.Location = new System.Drawing.Point(16, 442);
            this.lbRequest.Name = "lbRequest";
            this.lbRequest.Size = new System.Drawing.Size(63, 20);
            this.lbRequest.TabIndex = 1;
            this.lbRequest.Text = "Request";
            // 
            // lbResponse
            // 
            this.lbResponse.AutoSize = true;
            this.lbResponse.Location = new System.Drawing.Point(526, 442);
            this.lbResponse.Name = "lbResponse";
            this.lbResponse.Size = new System.Drawing.Size(73, 20);
            this.lbResponse.TabIndex = 2;
            this.lbResponse.Text = "Response";
            // 
            // tbResponse
            // 
            this.tbResponse.Location = new System.Drawing.Point(526, 465);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.ReadOnly = true;
            this.tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResponse.Size = new System.Drawing.Size(485, 241);
            this.tbResponse.TabIndex = 3;
            // 
            // tbServerURL
            // 
            this.tbServerURL.Location = new System.Drawing.Point(97, 12);
            this.tbServerURL.Name = "tbServerURL";
            this.tbServerURL.Size = new System.Drawing.Size(235, 27);
            this.tbServerURL.TabIndex = 6;
            this.tbServerURL.Text = "http://localhost:11500";
            // 
            // lbServerURL
            // 
            this.lbServerURL.AutoSize = true;
            this.lbServerURL.Location = new System.Drawing.Point(16, 15);
            this.lbServerURL.Name = "lbServerURL";
            this.lbServerURL.Size = new System.Drawing.Size(66, 20);
            this.lbServerURL.TabIndex = 6;
            this.lbServerURL.Text = "서버URL";
            // 
            // lbAccountId
            // 
            this.lbAccountId.AutoSize = true;
            this.lbAccountId.Location = new System.Drawing.Point(345, 15);
            this.lbAccountId.Name = "lbAccountId";
            this.lbAccountId.Size = new System.Drawing.Size(78, 20);
            this.lbAccountId.TabIndex = 7;
            this.lbAccountId.Text = "AccountId";
            // 
            // lbAuth
            // 
            this.lbAuth.AutoSize = true;
            this.lbAuth.Location = new System.Drawing.Point(745, 15);
            this.lbAuth.Name = "lbAuth";
            this.lbAuth.Size = new System.Drawing.Size(84, 20);
            this.lbAuth.TabIndex = 8;
            this.lbAuth.Text = "AuthToken";
            // 
            // tbAccountId
            // 
            this.tbAccountId.Location = new System.Drawing.Point(429, 12);
            this.tbAccountId.Name = "tbAccountId";
            this.tbAccountId.ReadOnly = true;
            this.tbAccountId.Size = new System.Drawing.Size(109, 27);
            this.tbAccountId.TabIndex = 9;
            // 
            // lbPlayerId
            // 
            this.lbPlayerId.AutoSize = true;
            this.lbPlayerId.Location = new System.Drawing.Point(545, 15);
            this.lbPlayerId.Name = "lbPlayerId";
            this.lbPlayerId.Size = new System.Drawing.Size(63, 20);
            this.lbPlayerId.TabIndex = 10;
            this.lbPlayerId.Text = "PlayerId";
            // 
            // tbPlayerId
            // 
            this.tbPlayerId.Location = new System.Drawing.Point(617, 12);
            this.tbPlayerId.Name = "tbPlayerId";
            this.tbPlayerId.ReadOnly = true;
            this.tbPlayerId.Size = new System.Drawing.Size(122, 27);
            this.tbPlayerId.TabIndex = 11;
            // 
            // tbAuthToken
            // 
            this.tbAuthToken.Location = new System.Drawing.Point(835, 12);
            this.tbAuthToken.Name = "tbAuthToken";
            this.tbAuthToken.ReadOnly = true;
            this.tbAuthToken.Size = new System.Drawing.Size(183, 27);
            this.tbAuthToken.TabIndex = 12;
            // 
            // lbMasterVersion
            // 
            this.lbMasterVersion.AutoSize = true;
            this.lbMasterVersion.Location = new System.Drawing.Point(345, 53);
            this.lbMasterVersion.Name = "lbMasterVersion";
            this.lbMasterVersion.Size = new System.Drawing.Size(138, 20);
            this.lbMasterVersion.TabIndex = 13;
            this.lbMasterVersion.Text = "MasterDataVersion";
            // 
            // tbMasterDataVersion
            // 
            this.tbMasterDataVersion.Location = new System.Drawing.Point(489, 50);
            this.tbMasterDataVersion.Name = "tbMasterDataVersion";
            this.tbMasterDataVersion.ReadOnly = true;
            this.tbMasterDataVersion.Size = new System.Drawing.Size(109, 27);
            this.tbMasterDataVersion.TabIndex = 14;
            this.tbMasterDataVersion.Text = "1.0.0";
            // 
            // lbClientVersion
            // 
            this.lbClientVersion.AutoSize = true;
            this.lbClientVersion.Location = new System.Drawing.Point(617, 53);
            this.lbClientVersion.Name = "lbClientVersion";
            this.lbClientVersion.Size = new System.Drawing.Size(100, 20);
            this.lbClientVersion.TabIndex = 15;
            this.lbClientVersion.Text = "ClientVersion";
            // 
            // tbClientVersion
            // 
            this.tbClientVersion.Location = new System.Drawing.Point(723, 50);
            this.tbClientVersion.Name = "tbClientVersion";
            this.tbClientVersion.ReadOnly = true;
            this.tbClientVersion.Size = new System.Drawing.Size(109, 27);
            this.tbClientVersion.TabIndex = 16;
            this.tbClientVersion.Text = "1.0.0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 718);
            this.Controls.Add(this.tbClientVersion);
            this.Controls.Add(this.lbClientVersion);
            this.Controls.Add(this.tbMasterDataVersion);
            this.Controls.Add(this.lbMasterVersion);
            this.Controls.Add(this.tbAuthToken);
            this.Controls.Add(this.tbPlayerId);
            this.Controls.Add(this.lbPlayerId);
            this.Controls.Add(this.tbAccountId);
            this.Controls.Add(this.lbAuth);
            this.Controls.Add(this.lbAccountId);
            this.Controls.Add(this.lbServerURL);
            this.Controls.Add(this.tbServerURL);
            this.Controls.Add(this.lbResponse);
            this.Controls.Add(this.tbResponse);
            this.Controls.Add(this.lbRequest);
            this.Controls.Add(this.tbRequest);
            this.Controls.Add(this.tabAPI);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabAPI.ResumeLayout(false);
            this.tabLogin.ResumeLayout(false);
            this.tabLogin.PerformLayout();
            this.tabMail.ResumeLayout(false);
            this.tabMail.PerformLayout();
            this.tabPlayerInfo.ResumeLayout(false);
            this.tabPlayerInfo.PerformLayout();
            this.tabAttendance.ResumeLayout(false);
            this.tabShop.ResumeLayout(false);
            this.tabShop.PerformLayout();
            this.tabEnforce.ResumeLayout(false);
            this.tabEnforce.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabControl tabAPI;
        private TabPage tabLogin;
        private Button btnLogin;
        private Button btnCreateAccount;
        private TextBox tbPassword;
        private Label lbPassword;
        private Label lbID;
        private TextBox tbID;
        private TabPage tabMail;
        private TextBox tbRequest;
        private Label lbRequest;
        private Label lbResponse;
        private TextBox tbResponse;
        private TextBox tbServerURL;
        private Label lbServerURL;
        private Label lbNotice;
        private TextBox tbNotice;
        private ListBox lbxMailBox;
        private Button btnRefresh;
        private TextBox tbMailDetail;
        private Label lbMailDetal;
        private Label label1;
        private Button btnMailBoxLeft;
        private Label lbMailBoxPageNo;
        private Button btnMailBoxRight;
        private Button btnReceiveMailItem;
        private Label lbAccountId;
        private Label lbAuth;
        private TextBox tbAccountId;
        private Label lbPlayerId;
        private TextBox tbPlayerId;
        private TextBox tbAuthToken;
        private Label lbMasterVersion;
        private TextBox tbMasterDataVersion;
        private Label lbClientVersion;
        private TextBox tbClientVersion;
        private TabPage tabPlayerInfo;
        private TextBox tbPlayerInfo;
        private Label lbPlayerItemInfo;
        private Label lbPlayerInfo;
        private TextBox tbPlayerItemInfo;
        private Button btnPlayerInfoLoad;
        private TabPage tabAttendance;
        private Button btnAttend;
        private TabPage tabShop;
        private Label lbShopItem3;
        private TextBox tbShopItem3;
        private Button btnBuyShopItem3;
        private Label lbShopItem2;
        private TextBox tbShopItem2;
        private Button btnBuyShopItem2;
        private Label lbShopItem1;
        private TextBox tbShopItem1;
        private Button btnBuyShopItem1;
        private TabPage tabEnforce;
        private Label lbEnforceDetail;
        private Button btnEnforce;
        private Label lbEnforceArrow;
        private Label lbEnforceResult;
        private TextBox tbEnforceResult;
        private Label lbEnforceTarget;
        private TextBox tbEnforceDetail;
        private ListBox lbxEnforcePlayerItem;
        private Label lbEnforceList;
    }
}