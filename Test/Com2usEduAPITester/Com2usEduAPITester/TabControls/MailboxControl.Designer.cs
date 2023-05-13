namespace Com2usEduAPITester
{
    partial class MailboxControl
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
            this.btnReceiveMailItem = new System.Windows.Forms.Button();
            this.btnMailBoxLeft = new System.Windows.Forms.Button();
            this.lbMailBoxPageNo = new System.Windows.Forms.Label();
            this.btnMailBoxRight = new System.Windows.Forms.Button();
            this.tbMailDetail = new System.Windows.Forms.TextBox();
            this.lbMailDetal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbxMailBox = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReceiveMailItem
            // 
            this.btnReceiveMailItem.Location = new System.Drawing.Point(870, 285);
            this.btnReceiveMailItem.Name = "btnReceiveMailItem";
            this.btnReceiveMailItem.Size = new System.Drawing.Size(104, 29);
            this.btnReceiveMailItem.TabIndex = 22;
            this.btnReceiveMailItem.Text = "아이템 수령";
            this.btnReceiveMailItem.UseVisualStyleBackColor = true;
            this.btnReceiveMailItem.Click += new System.EventHandler(this.btnReceiveMailItem_Click);
            // 
            // btnMailBoxLeft
            // 
            this.btnMailBoxLeft.Location = new System.Drawing.Point(108, 285);
            this.btnMailBoxLeft.Name = "btnMailBoxLeft";
            this.btnMailBoxLeft.Size = new System.Drawing.Size(94, 29);
            this.btnMailBoxLeft.TabIndex = 21;
            this.btnMailBoxLeft.Text = "<";
            this.btnMailBoxLeft.UseVisualStyleBackColor = true;
            this.btnMailBoxLeft.Click += new System.EventHandler(this.btnMailBoxLeft_Click);
            // 
            // lbMailBoxPageNo
            // 
            this.lbMailBoxPageNo.AutoSize = true;
            this.lbMailBoxPageNo.Location = new System.Drawing.Point(240, 289);
            this.lbMailBoxPageNo.Name = "lbMailBoxPageNo";
            this.lbMailBoxPageNo.Size = new System.Drawing.Size(41, 20);
            this.lbMailBoxPageNo.TabIndex = 20;
            this.lbMailBoxPageNo.Text = "0 / 0";
            // 
            // btnMailBoxRight
            // 
            this.btnMailBoxRight.Location = new System.Drawing.Point(327, 285);
            this.btnMailBoxRight.Name = "btnMailBoxRight";
            this.btnMailBoxRight.Size = new System.Drawing.Size(94, 29);
            this.btnMailBoxRight.TabIndex = 19;
            this.btnMailBoxRight.Text = ">";
            this.btnMailBoxRight.UseVisualStyleBackColor = true;
            this.btnMailBoxRight.Click += new System.EventHandler(this.btnMailBoxRight_Click);
            // 
            // tbMailDetail
            // 
            this.tbMailDetail.Location = new System.Drawing.Point(514, 55);
            this.tbMailDetail.Multiline = true;
            this.tbMailDetail.Name = "tbMailDetail";
            this.tbMailDetail.ReadOnly = true;
            this.tbMailDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMailDetail.Size = new System.Drawing.Size(460, 224);
            this.tbMailDetail.TabIndex = 16;
            // 
            // lbMailDetal
            // 
            this.lbMailDetal.AutoSize = true;
            this.lbMailDetal.Location = new System.Drawing.Point(518, 21);
            this.lbMailDetal.Name = "lbMailDetal";
            this.lbMailDetal.Size = new System.Drawing.Size(69, 20);
            this.lbMailDetal.TabIndex = 18;
            this.lbMailDetal.Text = "우편상세";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "우편함";
            // 
            // lbxMailBox
            // 
            this.lbxMailBox.FormattingEnabled = true;
            this.lbxMailBox.ItemHeight = 20;
            this.lbxMailBox.Location = new System.Drawing.Point(25, 55);
            this.lbxMailBox.Name = "lbxMailBox";
            this.lbxMailBox.Size = new System.Drawing.Size(464, 224);
            this.lbxMailBox.TabIndex = 15;
            this.lbxMailBox.DoubleClick += new System.EventHandler(this.lbxMailBox_DoubleClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(395, 17);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(94, 29);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "새로고침";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // MailboxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnReceiveMailItem);
            this.Controls.Add(this.btnMailBoxLeft);
            this.Controls.Add(this.lbMailBoxPageNo);
            this.Controls.Add(this.btnMailBoxRight);
            this.Controls.Add(this.tbMailDetail);
            this.Controls.Add(this.lbMailDetal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbxMailBox);
            this.Controls.Add(this.btnRefresh);
            this.Name = "MailboxControl";
            this.Size = new System.Drawing.Size(998, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnReceiveMailItem;
        private Button btnMailBoxLeft;
        private Label lbMailBoxPageNo;
        private Button btnMailBoxRight;
        private TextBox tbMailDetail;
        private Label lbMailDetal;
        private Label label1;
        private ListBox lbxMailBox;
        private Button btnRefresh;
    }
}
