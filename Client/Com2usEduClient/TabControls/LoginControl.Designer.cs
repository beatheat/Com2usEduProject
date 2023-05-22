namespace Com2usEduClient
{
    partial class LoginControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbNotice = new System.Windows.Forms.Label();
            this.tbNotice = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCreateAccount = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbID = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbNotice
            // 
            this.lbNotice.AutoSize = true;
            this.lbNotice.Location = new System.Drawing.Point(567, 23);
            this.lbNotice.Name = "lbNotice";
            this.lbNotice.Size = new System.Drawing.Size(69, 20);
            this.lbNotice.TabIndex = 15;
            this.lbNotice.Text = "공지사항";
            // 
            // tbNotice
            // 
            this.tbNotice.Location = new System.Drawing.Point(567, 59);
            this.tbNotice.Multiline = true;
            this.tbNotice.Name = "tbNotice";
            this.tbNotice.Size = new System.Drawing.Size(410, 246);
            this.tbNotice.TabIndex = 14;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(16, 161);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(215, 29);
            this.btnLogin.TabIndex = 13;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.Location = new System.Drawing.Point(16, 126);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new System.Drawing.Size(215, 29);
            this.btnCreateAccount.TabIndex = 12;
            this.btnCreateAccount.Text = "CreateAccount";
            this.btnCreateAccount.UseVisualStyleBackColor = true;
            this.btnCreateAccount.Click += new System.EventHandler(this.btnCreateAccount_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(106, 72);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(125, 27);
            this.tbPassword.TabIndex = 11;
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(16, 79);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(69, 20);
            this.lbPassword.TabIndex = 10;
            this.lbPassword.Text = "비밀번호";
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.Location = new System.Drawing.Point(16, 35);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(54, 20);
            this.lbID.TabIndex = 9;
            this.lbID.Text = "아이디";
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(106, 32);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(125, 27);
            this.tbID.TabIndex = 8;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbNotice);
            this.Controls.Add(this.tbNotice);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnCreateAccount);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.tbID);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(998, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lbNotice;
        private TextBox tbNotice;
        private Button btnLogin;
        private Button btnCreateAccount;
        private TextBox tbPassword;
        private Label lbPassword;
        private Label lbID;
        private TextBox tbID;
    }
}
