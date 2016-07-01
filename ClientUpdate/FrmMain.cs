using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ClientUpdate.Model;
using UpdateHelper.Http;

namespace ClientUpdate
{
    public partial class FrmMain : Form
    {
        private delegate string RequestUpdateInfo(string url);//请求更新信息委托

        private delegate void UpdateUI(string text);//更新ui

        private RequestUpdateInfo request;
        private string GetUpdateUrl;

        public FrmMain(string getUpdateUrl)
        {
            this.GetUpdateUrl = getUpdateUrl;
            InitializeComponent();
            txtDescription.Text = "更新信息读取中，请稍后。。。";
            btnOpenBrowser.Enabled = btnUpdate.Enabled = false;
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            request=new RequestUpdateInfo(HttpHelper.HttpGet);
            request.BeginInvoke(GetUpdateUrl, ShowUpdate , null);
        }

        public void ShowUpdate(IAsyncResult result)
        {
            string res = request.EndInvoke(result);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(res);
            UpdateModel.Version = xmlDocument.SelectSingleNode("//Version").InnerText;
            UpdateModel.ContentUrl = xmlDocument.SelectSingleNode("//ContentUrl").InnerText;
            UpdateModel.Content = xmlDocument.SelectSingleNode("//Content").InnerText;
            UpdateModel.FileUrl = xmlDocument.SelectSingleNode("//FileUrl").InnerText;
            UpdateModel.Start = xmlDocument.SelectSingleNode("//Start").InnerText;
            UpdateModel.Delete = xmlDocument.SelectSingleNode("//Delete").InnerText;
            UpdateModel.ScriptUrl = xmlDocument.SelectSingleNode("//ScriptUrl").InnerText;
            UpdateModel.ScriptKey = xmlDocument.SelectSingleNode("//ScriptKey").InnerText;
            this.Invoke((UpdateUI)delegate(string text)
            {
                txtDescription.Text = text;
                btnOpenBrowser.Enabled = btnUpdate.Enabled = true;
            },UpdateModel.Content);
        }

        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            Process.Start(UpdateModel.ContentUrl);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmDownload frmDownload=new FrmDownload();
            this.Hide();
            frmDownload.Show();
        }
    }
}
