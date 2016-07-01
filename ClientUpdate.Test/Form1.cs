using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using UpdateHelper.Http;

namespace ClientUpdate.Test
{
    public partial class Form1 : Form
    {
        private long readSize = 0;
        private long prevSize = 0;
        private long fileSize = 0;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

        }

        protected override void OnClosing(CancelEventArgs e)
{
    MessageBox.Show("我点击了关闭按钮");
    e.Cancel = true;

}

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text =
                HttpHelper.HttpGet(System.Configuration.ConfigurationManager.AppSettings["UpdateCheckUrl"]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HttpWebRequest request =
                (HttpWebRequest)
                    WebRequest.Create("http://patch3.800vod.com/2013/ALI213-Game.CommonRedist.V2-ALI213.rar");
            richTextBox1.Text = request.GetResponse().Headers["Content-Disposition"].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            new Thread(new ThreadStart(DownloadFile)).Start();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpHelper.HttpGet(ConfigurationManager.AppSettings["UpdateCheckUrl"]));//加载更新检测文件
            string version = xmlDoc.SelectSingleNode("//Version").InnerText;//获取服务器上的版本号
            if (version != ConfigurationManager.AppSettings["Version"])//如果服务器上的版本号与本地不相等
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    //启动更新程序
                    Process.Start("ClientUpdate.exe", ConfigurationManager.AppSettings["UpdateCheckUrl"]);
                    //关闭沟通平台
                    Application.Exit();
                }));
                
            }
        }



        /// <summary>
        /// 解压功能(解压压缩文件到指定目录)
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件</param>
        /// <param name="ZipedFolder">指定解压目标目录</param>
        public static void UnZip(string FileToUpZip, string ZipedFolder, string Password)
        {
            if (!File.Exists(FileToUpZip))
            {
                return;
            }

            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }

            ZipInputStream s = null;
            ZipEntry theEntry = null;

            string fileName;
            FileStream streamWriter = null;
            try
            {
                s = new ZipInputStream(File.OpenRead(FileToUpZip));
                s.Password = Password;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        ///判断文件路径是否是文件夹
                        if (fileName.EndsWith("/") || fileName.EndsWith("//"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        streamWriter = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (s != null)
                {
                    s.Close();
                    s = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        }
  


private void DownloadFile()
        {
            //string url = "http://patch3.800vod.com/2013/ALI213-Game.CommonRedist.V2-ALI213.rar";
            string url = "http://oa.xpdjk.com/Debug.rar";
            long count = 0;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                //向服务器请求，获得服务器的回应数据流
                HttpWebResponse webResponse = (HttpWebResponse)myRequest.GetResponse();
                fileSize = webResponse.ContentLength;
                progressBar1.Maximum = int.Parse((webResponse.ContentLength / 102400).ToString());
                label2.Text = "文件大小:" + (webResponse.ContentLength / 1048576.0).ToString("F") + "MB";
                Stream myStream = webResponse.GetResponseStream();
                string fileName = url.Split('/')[url.Split('/').Length - 1];
                //string fileName = webResponse.Headers["Content-disposition"];
                label1.Text = "文件名:" + fileName;
                FileStream fs = new FileStream(fileName, FileMode.Create);
                byte[] bufferSize = new byte[102400];
                int temp = 0;
                while ((temp = myStream.Read(bufferSize, 0, bufferSize.Length)) > 0)
                {
                    fs.Write(bufferSize, 0, temp);
                    readSize += temp;
                    if (progressBar1.Value < progressBar1.Maximum)
                    {
                        progressBar1.Value = int.Parse((readSize / 102400).ToString());                        
                    }
                    label4.Text = "已下载：" + (readSize/1048576.0).ToString("F")+"MB";
                    fs.Flush();
                }
                timer1.Stop();
                label3.Text = "";
                fs.Close();
                myStream.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            long temp = readSize;
            double sudu = (temp - prevSize) / 1024.0;
            if (sudu > 1024)
            {
                label3.Text = (sudu/1024.0).ToString("F") + "MB/S";
            }
            else
            {
                label3.Text = sudu.ToString("F") + "KB/S";
            }
            double time = (fileSize - readSize) / sudu / 1024.0;
            if (time != Double.NaN)
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)time );
                label5.Text = "剩余时间:" + ts.Hours + "小时" + ts.Minutes + "分" + ts.Seconds + "秒"; 
            }
            
            prevSize = temp;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UnZip(@"D:\vs2010proj\ClientUpdate\ClientUpdate.Test\bin\Debug\Debug.rar", @"D:\vs2010proj\ClientUpdate\ClientUpdate.Test\bin\Debug\Debug","");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RunBat(Application.StartupPath + @"\cmd.bat");
            MessageBox.Show("s");
        }

        private void RunBat(string batPath)
        {
            Process pro = new Process();

            FileInfo file = new FileInfo(batPath);
            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            pro.StartInfo.FileName = batPath;
            pro.StartInfo.CreateNoWindow = false;
            pro.Start();
            pro.WaitForExit();
        }

    }
}
