namespace Com2usEduClient.TabControls
{
    partial class ChatControl
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
            this.components = new System.ComponentModel.Container();
            this.tbChatViewer = new System.Windows.Forms.TextBox();
            this.btnChat = new System.Windows.Forms.Button();
            this.tbUserChat = new System.Windows.Forms.TextBox();
            this.cbChannel = new System.Windows.Forms.ComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tbChatViewer
            // 
            this.tbChatViewer.Location = new System.Drawing.Point(3, 5);
            this.tbChatViewer.Multiline = true;
            this.tbChatViewer.Name = "tbChatViewer";
            this.tbChatViewer.ReadOnly = true;
            this.tbChatViewer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbChatViewer.Size = new System.Drawing.Size(986, 288);
            this.tbChatViewer.TabIndex = 5;
            // 
            // btnChat
            // 
            this.btnChat.Location = new System.Drawing.Point(817, 299);
            this.btnChat.Name = "btnChat";
            this.btnChat.Size = new System.Drawing.Size(94, 29);
            this.btnChat.TabIndex = 4;
            this.btnChat.Text = "▶";
            this.btnChat.UseVisualStyleBackColor = true;
            this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
            // 
            // tbUserChat
            // 
            this.tbUserChat.Location = new System.Drawing.Point(3, 299);
            this.tbUserChat.Name = "tbUserChat";
            this.tbUserChat.Size = new System.Drawing.Size(808, 27);
            this.tbUserChat.TabIndex = 3;
            this.tbUserChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUserChat_KeyDown);
            // 
            // cbChannel
            // 
            this.cbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannel.FormattingEnabled = true;
            this.cbChannel.Location = new System.Drawing.Point(917, 300);
            this.cbChannel.Name = "cbChannel";
            this.cbChannel.Size = new System.Drawing.Size(72, 28);
            this.cbChannel.TabIndex = 6;
            this.cbChannel.SelectedIndexChanged += new System.EventHandler(this.cbChannel_SelectedIndexChanged);
            this.cbChannel.SelectionChangeCommitted += new System.EventHandler(this.cbChannel_SelectionChangeCommitted);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // ChatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbChannel);
            this.Controls.Add(this.tbChatViewer);
            this.Controls.Add(this.btnChat);
            this.Controls.Add(this.tbUserChat);
            this.Name = "ChatControl";
            this.Size = new System.Drawing.Size(998, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tbChatViewer;
        private Button btnChat;
        private TextBox tbUserChat;
        private ComboBox cbChannel;
        private System.Windows.Forms.Timer timer;
    }
}
