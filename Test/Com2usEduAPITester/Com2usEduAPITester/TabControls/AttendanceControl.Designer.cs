namespace Com2usEduAPITester.TabControls
{
    partial class AttendanceControl
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
            this.btnAttend = new System.Windows.Forms.Button();
            this.lbxAttendanceReward = new System.Windows.Forms.ListBox();
            this.lbLastAttendanceDateIndicator = new System.Windows.Forms.Label();
            this.lbLastAttendanceDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAttend
            // 
            this.btnAttend.Location = new System.Drawing.Point(886, 14);
            this.btnAttend.Name = "btnAttend";
            this.btnAttend.Size = new System.Drawing.Size(94, 29);
            this.btnAttend.TabIndex = 1;
            this.btnAttend.Text = "출석하기";
            this.btnAttend.UseVisualStyleBackColor = true;
            this.btnAttend.Click += new System.EventHandler(this.btnAttend_Click);
            // 
            // lbxAttendanceReward
            // 
            this.lbxAttendanceReward.FormattingEnabled = true;
            this.lbxAttendanceReward.ItemHeight = 20;
            this.lbxAttendanceReward.Location = new System.Drawing.Point(17, 14);
            this.lbxAttendanceReward.Name = "lbxAttendanceReward";
            this.lbxAttendanceReward.Size = new System.Drawing.Size(847, 304);
            this.lbxAttendanceReward.TabIndex = 2;
            // 
            // lbLastAttendanceDateIndicator
            // 
            this.lbLastAttendanceDateIndicator.AutoSize = true;
            this.lbLastAttendanceDateIndicator.Location = new System.Drawing.Point(876, 70);
            this.lbLastAttendanceDateIndicator.Name = "lbLastAttendanceDateIndicator";
            this.lbLastAttendanceDateIndicator.Size = new System.Drawing.Size(104, 20);
            this.lbLastAttendanceDateIndicator.TabIndex = 3;
            this.lbLastAttendanceDateIndicator.Text = "마지막 출석일";
            // 
            // lbLastAttendanceDate
            // 
            this.lbLastAttendanceDate.AutoSize = true;
            this.lbLastAttendanceDate.Location = new System.Drawing.Point(876, 102);
            this.lbLastAttendanceDate.Name = "lbLastAttendanceDate";
            this.lbLastAttendanceDate.Size = new System.Drawing.Size(15, 20);
            this.lbLastAttendanceDate.TabIndex = 4;
            this.lbLastAttendanceDate.Text = "-";
            // 
            // AttendanceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbLastAttendanceDate);
            this.Controls.Add(this.lbLastAttendanceDateIndicator);
            this.Controls.Add(this.lbxAttendanceReward);
            this.Controls.Add(this.btnAttend);
            this.Name = "AttendanceControl";
            this.Size = new System.Drawing.Size(998, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnAttend;
        private ListBox lbxAttendanceReward;
        private Label lbLastAttendanceDateIndicator;
        private Label lbLastAttendanceDate;
    }
}
