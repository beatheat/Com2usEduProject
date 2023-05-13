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

        private async void btnAttend_Click(object sender, EventArgs e)
        {
            var request = new ReceiveAttendanceRewardRequest();

            var response = await HttpRequest.PostAuth("ReceiveAttendanceReward", request);
            if (response == null) return;

            var receiveAttendanceRewardResponse = JsonSerializer.Deserialize<ReceiveAttendanceRewardResponse>(response);

            if (receiveAttendanceRewardResponse.Result == ErrorCode.None)
            {
                MessageBox.Show("출석완료! 우편함에서 보상을 확인하세요");
            }
        }
    }
}
