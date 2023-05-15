namespace Com2usEduAPITester.TabControls
{
    partial class EnforceControl
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
            this.lbEnforceDetail = new System.Windows.Forms.Label();
            this.btnEnforce = new System.Windows.Forms.Button();
            this.lbEnforceArrow = new System.Windows.Forms.Label();
            this.lbEnforceResult = new System.Windows.Forms.Label();
            this.tbEnforceResult = new System.Windows.Forms.TextBox();
            this.lbEnforceTarget = new System.Windows.Forms.Label();
            this.tbEnforceDetail = new System.Windows.Forms.TextBox();
            this.lbxEnforcePlayerItem = new System.Windows.Forms.ListBox();
            this.lbEnforceList = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbEnforceDetail
            // 
            this.lbEnforceDetail.AutoSize = true;
            this.lbEnforceDetail.Location = new System.Drawing.Point(378, 253);
            this.lbEnforceDetail.Name = "lbEnforceDetail";
            this.lbEnforceDetail.Size = new System.Drawing.Size(179, 40);
            this.lbEnforceDetail.TabIndex = 21;
            this.lbEnforceDetail.Text = "강화확률 : 30%\r\n강화 실패 시 아이템 파괴\r\n";
            // 
            // btnEnforce
            // 
            this.btnEnforce.Location = new System.Drawing.Point(580, 233);
            this.btnEnforce.Name = "btnEnforce";
            this.btnEnforce.Size = new System.Drawing.Size(180, 82);
            this.btnEnforce.TabIndex = 20;
            this.btnEnforce.Text = "강화";
            this.btnEnforce.UseVisualStyleBackColor = true;
            this.btnEnforce.Click += new System.EventHandler(this.btnEnforce_Click);
            // 
            // lbEnforceArrow
            // 
            this.lbEnforceArrow.AutoSize = true;
            this.lbEnforceArrow.Location = new System.Drawing.Point(616, 106);
            this.lbEnforceArrow.Name = "lbEnforceArrow";
            this.lbEnforceArrow.Size = new System.Drawing.Size(108, 20);
            this.lbEnforceArrow.TabIndex = 19;
            this.lbEnforceArrow.Text = "=======>>";
            // 
            // lbEnforceResult
            // 
            this.lbEnforceResult.AutoSize = true;
            this.lbEnforceResult.Location = new System.Drawing.Point(744, 29);
            this.lbEnforceResult.Name = "lbEnforceResult";
            this.lbEnforceResult.Size = new System.Drawing.Size(74, 20);
            this.lbEnforceResult.TabIndex = 18;
            this.lbEnforceResult.Text = "강화 결과";
            // 
            // tbEnforceResult
            // 
            this.tbEnforceResult.Location = new System.Drawing.Point(744, 53);
            this.tbEnforceResult.Multiline = true;
            this.tbEnforceResult.Name = "tbEnforceResult";
            this.tbEnforceResult.ReadOnly = true;
            this.tbEnforceResult.Size = new System.Drawing.Size(144, 174);
            this.tbEnforceResult.TabIndex = 17;
            // 
            // lbEnforceTarget
            // 
            this.lbEnforceTarget.AutoSize = true;
            this.lbEnforceTarget.Location = new System.Drawing.Point(451, 29);
            this.lbEnforceTarget.Name = "lbEnforceTarget";
            this.lbEnforceTarget.Size = new System.Drawing.Size(74, 20);
            this.lbEnforceTarget.TabIndex = 16;
            this.lbEnforceTarget.Text = "강화 대상";
            // 
            // tbEnforceDetail
            // 
            this.tbEnforceDetail.Location = new System.Drawing.Point(451, 53);
            this.tbEnforceDetail.Multiline = true;
            this.tbEnforceDetail.Name = "tbEnforceDetail";
            this.tbEnforceDetail.ReadOnly = true;
            this.tbEnforceDetail.Size = new System.Drawing.Size(144, 174);
            this.tbEnforceDetail.TabIndex = 15;
            // 
            // lbxEnforcePlayerItem
            // 
            this.lbxEnforcePlayerItem.FormattingEnabled = true;
            this.lbxEnforcePlayerItem.ItemHeight = 20;
            this.lbxEnforcePlayerItem.Location = new System.Drawing.Point(19, 31);
            this.lbxEnforcePlayerItem.Name = "lbxEnforcePlayerItem";
            this.lbxEnforcePlayerItem.Size = new System.Drawing.Size(339, 284);
            this.lbxEnforcePlayerItem.TabIndex = 14;
            this.lbxEnforcePlayerItem.DoubleClick += new System.EventHandler(this.lbxEnforcePlayerItem_DoubleClick);
            // 
            // lbEnforceList
            // 
            this.lbEnforceList.AutoSize = true;
            this.lbEnforceList.Location = new System.Drawing.Point(16, 8);
            this.lbEnforceList.Name = "lbEnforceList";
            this.lbEnforceList.Size = new System.Drawing.Size(119, 20);
            this.lbEnforceList.TabIndex = 13;
            this.lbEnforceList.Text = "플레이어 아이템";
            // 
            // EnforceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbEnforceDetail);
            this.Controls.Add(this.btnEnforce);
            this.Controls.Add(this.lbEnforceArrow);
            this.Controls.Add(this.lbEnforceResult);
            this.Controls.Add(this.tbEnforceResult);
            this.Controls.Add(this.lbEnforceTarget);
            this.Controls.Add(this.tbEnforceDetail);
            this.Controls.Add(this.lbxEnforcePlayerItem);
            this.Controls.Add(this.lbEnforceList);
            this.Name = "EnforceControl";
            this.Size = new System.Drawing.Size(998, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
