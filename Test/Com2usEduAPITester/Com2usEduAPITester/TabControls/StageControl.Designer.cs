namespace Com2usEduAPITester.TabControls
{
    partial class StageControl
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
            this.lbxStageList = new System.Windows.Forms.ListBox();
            this.lbStageList = new System.Windows.Forms.Label();
            this.btnAutoGame = new System.Windows.Forms.Button();
            this.tbStageLog = new System.Windows.Forms.TextBox();
            this.lbStageContent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbxStageList
            // 
            this.lbxStageList.FormattingEnabled = true;
            this.lbxStageList.ItemHeight = 20;
            this.lbxStageList.Location = new System.Drawing.Point(17, 64);
            this.lbxStageList.Name = "lbxStageList";
            this.lbxStageList.Size = new System.Drawing.Size(309, 204);
            this.lbxStageList.TabIndex = 0;
            this.lbxStageList.DoubleClick += new System.EventHandler(this.lbxStageList_DoubleClick);
            // 
            // lbStageList
            // 
            this.lbStageList.AutoSize = true;
            this.lbStageList.Location = new System.Drawing.Point(17, 23);
            this.lbStageList.Name = "lbStageList";
            this.lbStageList.Size = new System.Drawing.Size(69, 20);
            this.lbStageList.TabIndex = 1;
            this.lbStageList.Text = "스테이지";
            // 
            // btnAutoGame
            // 
            this.btnAutoGame.Location = new System.Drawing.Point(816, 282);
            this.btnAutoGame.Name = "btnAutoGame";
            this.btnAutoGame.Size = new System.Drawing.Size(156, 29);
            this.btnAutoGame.TabIndex = 2;
            this.btnAutoGame.Text = "자동사냥 시작";
            this.btnAutoGame.UseVisualStyleBackColor = true;
            this.btnAutoGame.Click += new System.EventHandler(this.btnAutoGame_Click);
            // 
            // tbStageLog
            // 
            this.tbStageLog.Location = new System.Drawing.Point(342, 64);
            this.tbStageLog.Multiline = true;
            this.tbStageLog.Name = "tbStageLog";
            this.tbStageLog.ReadOnly = true;
            this.tbStageLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbStageLog.Size = new System.Drawing.Size(630, 209);
            this.tbStageLog.TabIndex = 3;
            // 
            // lbStageContent
            // 
            this.lbStageContent.AutoSize = true;
            this.lbStageContent.Location = new System.Drawing.Point(342, 23);
            this.lbStageContent.Name = "lbStageContent";
            this.lbStageContent.Size = new System.Drawing.Size(139, 20);
            this.lbStageContent.TabIndex = 5;
            this.lbStageContent.Text = "스테이지 진행 로그";
            // 
            // StageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbStageContent);
            this.Controls.Add(this.tbStageLog);
            this.Controls.Add(this.btnAutoGame);
            this.Controls.Add(this.lbStageList);
            this.Controls.Add(this.lbxStageList);
            this.Name = "StageControl";
            this.Size = new System.Drawing.Size(998, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox lbxStageList;
        private Label lbStageList;
        private Button btnAutoGame;
        private TextBox tbStageLog;
        private Label lbStageContent;
    }
}
