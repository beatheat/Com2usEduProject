namespace Com2usEduClient
{
    partial class MainForm
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
            this.tabAPI = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
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
            this.tbServerURL.Text = "http://localhost:18989";
            this.tbServerURL.TextChanged += new System.EventHandler(this.tbServerURL_TextChanged);
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
            // tabAPI
            // 
            this.tabAPI.Location = new System.Drawing.Point(12, 76);
            this.tabAPI.Name = "tabAPI";
            this.tabAPI.SelectedIndex = 0;
            this.tabAPI.Size = new System.Drawing.Size(1006, 363);
            this.tabAPI.TabIndex = 0;
            this.tabAPI.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabAPI_Selected);
            // 
            // MainForm
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
            this.Name = "MainForm";
            this.Text = "Com2usEduClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox tbRequest;
        private Label lbRequest;
        private Label lbResponse;
        private TextBox tbResponse;
        private TextBox tbServerURL;
        private Label lbServerURL;
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
        private TabControl tabAPI;
    }
}