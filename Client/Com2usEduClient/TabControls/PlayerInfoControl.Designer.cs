namespace Com2usEduClient.TabControls
{
    partial class PlayerInfoControl
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
            this.btnPlayerInfoLoad = new System.Windows.Forms.Button();
            this.tbPlayerItemInfo = new System.Windows.Forms.TextBox();
            this.tbPlayerInfo = new System.Windows.Forms.TextBox();
            this.lbPlayerItemInfo = new System.Windows.Forms.Label();
            this.lbPlayerInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnPlayerInfoLoad
            // 
            this.btnPlayerInfoLoad.Location = new System.Drawing.Point(898, 4);
            this.btnPlayerInfoLoad.Name = "btnPlayerInfoLoad";
            this.btnPlayerInfoLoad.Size = new System.Drawing.Size(94, 29);
            this.btnPlayerInfoLoad.TabIndex = 9;
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
            this.tbPlayerItemInfo.TabIndex = 8;
            // 
            // tbPlayerInfo
            // 
            this.tbPlayerInfo.Location = new System.Drawing.Point(6, 36);
            this.tbPlayerInfo.Multiline = true;
            this.tbPlayerInfo.Name = "tbPlayerInfo";
            this.tbPlayerInfo.ReadOnly = true;
            this.tbPlayerInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tbPlayerInfo.Size = new System.Drawing.Size(447, 291);
            this.tbPlayerInfo.TabIndex = 7;
            // 
            // lbPlayerItemInfo
            // 
            this.lbPlayerItemInfo.AutoSize = true;
            this.lbPlayerItemInfo.Location = new System.Drawing.Point(472, 13);
            this.lbPlayerItemInfo.Name = "lbPlayerItemInfo";
            this.lbPlayerItemInfo.Size = new System.Drawing.Size(119, 20);
            this.lbPlayerItemInfo.TabIndex = 6;
            this.lbPlayerItemInfo.Text = "플레이어 아이템";
            // 
            // lbPlayerInfo
            // 
            this.lbPlayerInfo.AutoSize = true;
            this.lbPlayerInfo.Location = new System.Drawing.Point(6, 13);
            this.lbPlayerInfo.Name = "lbPlayerInfo";
            this.lbPlayerInfo.Size = new System.Drawing.Size(104, 20);
            this.lbPlayerInfo.TabIndex = 5;
            this.lbPlayerInfo.Text = "플레이어 정보";
            // 
            // PlayerInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPlayerInfoLoad);
            this.Controls.Add(this.tbPlayerItemInfo);
            this.Controls.Add(this.tbPlayerInfo);
            this.Controls.Add(this.lbPlayerItemInfo);
            this.Controls.Add(this.lbPlayerInfo);
            this.Name = "PlayerInfoControl";
            this.Size = new System.Drawing.Size(998, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnPlayerInfoLoad;
        private TextBox tbPlayerItemInfo;
        private TextBox tbPlayerInfo;
        private Label lbPlayerItemInfo;
        private Label lbPlayerInfo;
    }
}
