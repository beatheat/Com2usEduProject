using Com2usEduAPITester.ReqRes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Com2usEduAPITester.TabControls
{
    public partial class AttendanceControl : UserControl
    {
        public AttendanceControl()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            LoadAttendanceInfo();
        }

        private async void LoadAttendanceInfo()
        {
            var request = new LoadAttendanceInfoRequest();

            var response = await HttpRequest.PostAuth<LoadAttendanceInfoResponse>("LoadAttendanceInfo", request);
            if (response == null) return;

            var attendanceInfo = response.AttendanceInfo;

            lbLastAttendanceDate.Text = attendanceInfo.LastAttendanceDate.ToString("yyyy-MM-dd");
            lbxAttendanceReward.Items.Clear();
            for (int i=0;i<30;i++)
            {
                var rewardItem = MasterData.attendanceRewards[i];
                var rewardString = (i+1) +  "일차 보상  \t" + 
                    MasterData.ItemName[rewardItem.ItemCode] + " : " + rewardItem.ItemCount;
                if (i < attendanceInfo.ContinuousAttendanceDays)
                    rewardString += " [수령완료] ";
                if (i == attendanceInfo.ContinuousAttendanceDays && attendanceInfo.LastAttendanceDate != DateTime.Today)
                    rewardString += " [오늘의 보상] ";
                lbxAttendanceReward.Items.Add(rewardString);
            }
        }

        private async void btnAttend_Click(object sender, EventArgs e)
        {
            var request = new ReceiveAttendanceRewardRequest();

            var response = await HttpRequest.PostAuthWithErrorCode<ReceiveAttendanceRewardResponse>("ReceiveAttendanceReward", request);
            if (response == null) return;


            LoadAttendanceInfo();

            if (response.Result == ErrorCode.None)
            {
                MessageBox.Show("출석완료! 우편함에서 보상을 확인하세요");
            }
            else if(response.Result == ErrorCode.ReceiveAttendanceRewardAlready)
            {
                MessageBox.Show("이미 보상을 수령했습니다!");
            }
        }
    }
}
