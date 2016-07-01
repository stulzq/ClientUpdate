using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ClientUpdate.Model;
using ClientUpdate.Util;
using UpdateHelper.Http;

namespace ClientUpdate
{
    public partial class FrmCmd : Form
    {
        private delegate void UpdateUI(string text);
        public FrmCmd()
        {
            InitializeComponent();
            btnExit.Visible = false;
        }

        private void FrmCmd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void FrmCmd_Load(object sender, EventArgs e)
        {
            ProcessFormClose.DisableCloseButton(this.Handle); //禁止使用关闭按钮
            txtLog.AppendText("正在执行脚本。。。\n");
            new Thread(new ThreadStart(Work)).Start();
            
        }

        private void Work()
        {
            Thread.Sleep(1500);
            try
            {
                RunBat(UpdateModel.ScriptUrl);
                DirectoryInfo directoryInfo=new DirectoryInfo("Update");
                directoryInfo.Delete(true);
                File.Delete(UpdateModel.ScriptUrl);
                Process.Start(UpdateModel.Start+".exe");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {

                this.Invoke((UpdateUI)delegate(string text) { txtLog.AppendText("脚本在执行过程中出现错误，未能完成更新，请联系技术人员！\n错误详细信息：\n" + text); btnExit.Visible = true; ; }, ex.Message);
                DirectoryInfo directoryInfo = new DirectoryInfo("Update");
                directoryInfo.Delete(true);
                File.Delete(UpdateModel.ScriptUrl);
            }
            

        }

        private void RunBat(string batPath)
        {
            try
            {
                Process pro = new Process();

                FileInfo file = new FileInfo(batPath);
                pro.StartInfo.WorkingDirectory = file.Directory.FullName;
                pro.StartInfo.FileName = batPath;
                pro.StartInfo.CreateNoWindow = false;
                pro.Start();
                pro.WaitForExit();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }
    }
}
