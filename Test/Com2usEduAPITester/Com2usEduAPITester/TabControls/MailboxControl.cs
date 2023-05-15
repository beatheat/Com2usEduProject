using Com2usEduAPITester.Game;
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

namespace Com2usEduAPITester
{
    public partial class MailboxControl : UserControl
    {

        int _maxMailBoxPageNo = 0;
        int _currentMailBoxPageNo = 1;

        public MailboxControl()
        {
            InitializeComponent();
        }

        public async void RefreshData()
        {
            _currentMailBoxPageNo = 1;
            await LoadMailBox(_currentMailBoxPageNo);
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            _currentMailBoxPageNo = 1;
            await LoadMailBox(_currentMailBoxPageNo);
        }

        private async void btnMailBoxRight_Click(object sender, EventArgs e)
        {
            if (_currentMailBoxPageNo >= _maxMailBoxPageNo)
                return;
            _currentMailBoxPageNo++;
            await LoadMailBox(_currentMailBoxPageNo);
        }
        private async void btnMailBoxLeft_Click(object sender, EventArgs e)
        {
            if (_currentMailBoxPageNo <= 1)
                return;
            _currentMailBoxPageNo--;
            await LoadMailBox(_currentMailBoxPageNo);
        }

        private async void btnReceiveMailItem_Click(object sender, EventArgs e)
        {
            var mailBoxRow = tbMailDetail.Text.Split("\r\n")[0];
            var mailId = int.Parse(mailBoxRow.Split(":")[0]);

            var request = new ReceiveMailItemRequest
            {
                MailId = mailId
            };

            var response = await HttpRequest.PostAuth("ReceiveMailItem", request);

            if (response == null)
                return;

            var receiveMailItemResponse = JsonSerializer.Deserialize<ReceiveMailItemResponse>(response);
            if (receiveMailItemResponse.Result == ErrorCode.None)
            {
                MessageBox.Show("수령완료");
            }
        }

        private async void lbxMailBox_DoubleClick(object sender, EventArgs e)
        {
            if (lbxMailBox.SelectedItem == null)
                return;

            var mailBoxRow = lbxMailBox.SelectedItem.ToString();
            var mailId = int.Parse(mailBoxRow.Split(":")[0]);

            var request = new LoadMailRequest
            {
                MailId = mailId
            };
            var response = await HttpRequest.PostAuth("LoadMail", request);

            if (response == null)
                return;

            var loadMailResponse = JsonSerializer.Deserialize<LoadMailResponse>(response);

            var mail = loadMailResponse.Mail;

            tbMailDetail.Text = mailId + "\r\n";
            tbMailDetail.Text += "제목 : " + mail.Name + "\r\n";
            tbMailDetail.Text += "발신일 : " + mail.TransmissionDate + "\r\n";
            tbMailDetail.Text += "유효기간 : " + mail.ExpireDate + "\r\n";
            tbMailDetail.Text += "내용 : " + mail.Content + "\r\n";
            tbMailDetail.Text += "아이템 수신 여부 : " + mail.IsItemReceived + "\r\n";
            tbMailDetail.Text += "아이템 : ";
            foreach (var item in mail.GetItemList())
            {
                if (item.ItemCode != -1)
                    tbMailDetail.Text += $"(이름: {MasterData.ItemName[item.ItemCode]}, 수량: {item.ItemCount}), ";
            }
        }

        private async Task LoadMailBox(int pageNo)
        {
            var request = new LoadMailListRequest
            {
                PageNo = pageNo
            };
            var response = await HttpRequest.PostAuth("LoadMailList", request);
            if (response == null)
            {
                return;
            }

            var mailListResponse = JsonSerializer.Deserialize<LoadMailListResponse>(response);

            _maxMailBoxPageNo = mailListResponse.TotalPageCount;
            lbMailBoxPageNo.Text = pageNo + " / " + mailListResponse.TotalPageCount.ToString();

            lbxMailBox.Items.Clear();
            foreach (var mail in mailListResponse.MailList)
            {
                var item = "";
                item += mail.Id + ":\t";
                item += mail.Name + "\t";
                item += mail.TransmissionDate + "\t";
                item += mail.IsItemReceived ? "아이템O" : "아이템X";

                lbxMailBox.Items.Add(item);
            }
        }
    }
}
