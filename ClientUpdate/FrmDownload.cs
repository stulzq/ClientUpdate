using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ClientUpdate.Model;
using ClientUpdate.Util;

namespace ClientUpdate
{
    /// <summary>
    /// 第二部 下载更新包
    /// </summary>
    public partial class FrmDownload : Form
    {
        private long FILESZE = 0; //更新包大小
        private long DOWNLOADSIZE = 0; //已下载大小
        private long PREVOUSSIZE = 0; //上一次统计速度的大小
        private int BUFFERSIZE = 102400; //缓冲区大小

        private delegate void UpdateUI(string speed, string download, string time); //更新ui

        private delegate void UpdatePgBar(int value); //更新进度条

        private Thread downThread; //下载线程

        public FrmDownload()
        {
            InitializeComponent();
            pgbarDownload.Value = 0;
            pgbarDownload.Minimum = 0;
        }

        private void FrmDownload_Load(object sender, EventArgs e)
        {
            ProcessFormClose.DisableCloseButton(this.Handle); //禁止使用关闭按钮
            if (!Directory.Exists("Update")) //判断更新包存放目录是否存在 不存在则创建
            {
                Directory.CreateDirectory("Update");
            }
            timerSpeed.Start();
            //下载
            downThread = new Thread(new ThreadStart(DownloadFile));
            downThread.Start();
        }

        private void FrmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; //禁止使用alt+f4关闭
        }

        private void DownloadFile()
        {
            HttpWebRequest myRequest = (HttpWebRequest) WebRequest.Create(UpdateModel.FileUrl);
            //向服务器请求，获得服务器的回应数据流
            HttpWebResponse webResponse = (HttpWebResponse) myRequest.GetResponse();
            //获得文件大小
            FILESZE = webResponse.ContentLength;
            //设置进度条最大值
            this.Invoke((UpdatePgBar) delegate(int value) { pgbarDownload.Maximum = (int) (FILESZE/1024); }, 0);
            //设置文件大小
            this.Invoke(
                (UpdateUI)delegate(string fsize, string download, string time) { lbFileSize.Text = "文件大小："+fsize + " MB"; },
                new string[] {(FILESZE/1048576.0).ToString("F"), "", ""});

            Stream myStream = webResponse.GetResponseStream();

            UpdateModel.UpdateFilePath = "Update/update_" + DateTime.Now.ToString("yyyy-MM-dd") + ".zip";

            FileStream fs = new FileStream(UpdateModel.UpdateFilePath, FileMode.Create);
            byte[] bufferByte = new byte[BUFFERSIZE];
            int oneRead = 0;
            while ((oneRead = myStream.Read(bufferByte, 0, bufferByte.Length)) > 0)
            {
                fs.Write(bufferByte, 0, oneRead);
                DOWNLOADSIZE += oneRead;
                this.Invoke((UpdatePgBar) delegate(int value)
                {
                    if (pgbarDownload.Value < pgbarDownload.Maximum)
                    {
                        pgbarDownload.Value = value;
                    }
                }, int.Parse((DOWNLOADSIZE/1024).ToString()));

                fs.Flush();
            }

            fs.Close();
            myStream.Close();
            //下载完成 进行下一步 解压
            this.Invoke((UpdateUI) delegate(string a, string b, string c)
            {
                FrmUnZip frmUnZip=new FrmUnZip();
                frmUnZip.Show();
                this.Hide();
                downThread.Abort();
            },new string[]{"","",""});

        }

        private void timerSpeed_Tick(object sender, EventArgs e)
        {
            //已下载大小
            long currentSize = DOWNLOADSIZE;
            //速度 kb/s
            double speed = (currentSize - PREVOUSSIZE)/1024.0;
            //已下载大小 mb
            double downloadSize = currentSize/1048576.0;

            if (DOWNLOADSIZE == FILESZE)
            {
                timerSpeed.Stop();
            }
            //换算剩余时间
            double overTime = (FILESZE - currentSize)/(speed*1024);
            overTime = (overTime == Double.NaN ? 0.0 : overTime);
            TimeSpan ts = new TimeSpan(0, 0, (int) overTime);
            string strTime = ts.Hours + "小时" + ts.Minutes + "分" + ts.Seconds + "秒";

            this.Invoke((UpdateUI) delegate(string currentSpeed, string download, string time)
            {
                lbSpeed.Text = "下载速度：" + currentSpeed;
                lbDlSize.Text = "已下载：" + download;
                lbTime.Text = "剩余时间：" + time;
            }, new string[]
            {
                speed >= 1024.0 ? (speed/1024.0).ToString("F") + " MB/S" : speed.ToString("F") + " KB/S",
                downloadSize.ToString("F") + " MB",
                strTime
            });
            PREVOUSSIZE = currentSize;
            if (DOWNLOADSIZE == FILESZE)
            {
                this.Invoke((UpdateUI) delegate(string currentSpeed, string download, string time)
                {
                    lbSpeed.Text = "下载速度：" + currentSpeed;
                    lbDlSize.Text = "已下载：" + download;
                    lbTime.Text = "剩余时间：" + time;
                }, new string[]
                {
                    "0 KB/S",
                    downloadSize.ToString("F") + " MB",
                    "0秒"
                });
                timerSpeed.Stop();
            }
        }
    }
}
