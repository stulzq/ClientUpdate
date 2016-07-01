using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ClientUpdate.Model;
using ClientUpdate.Util;
using ICSharpCode.SharpZipLib.Zip;

namespace ClientUpdate
{
    /// <summary>
    /// 第三步 解压
    /// </summary>
    public partial class FrmUnZip : Form
    {
        private delegate void AddLogDelegate(string text);
        private delegate void UpdatePgBar(int value);

        private Thread UnZipThread;//解压线程

        public FrmUnZip()
        {
            InitializeComponent();
            pgBarUnZip.Value = 0;
            pgBarUnZip.Minimum = 0;
        }

        private void FrmUnZip_Load(object sender, System.EventArgs e)
        {
            ProcessFormClose.DisableCloseButton(this.Handle); //禁止使用关闭按钮

            UnZipThread=new Thread(new ThreadStart(UnZip));
            UnZipThread.Start();
        }

        private void FrmUnZip_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; //禁止alt+f4关闭
        }

        /// <summary>
        /// 解压功能(解压压缩文件到指定目录)
        /// </summary>
        public void UnZip()
        {
            string FileToUpZip = UpdateModel.UpdateFilePath;
            string ZipedFolder = Application.StartupPath;
            string Password = "";
            //如果文件不存在就return
            if (!File.Exists(FileToUpZip))
            {
                return;
            }
            //如果目录不创建则创建
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
                FileStream fs = File.OpenRead(FileToUpZip);
                this.Invoke((UpdatePgBar)delegate(int value) { pgBarUnZip.Maximum = value; }, (int)fs.Length / 1024);
                s = new ZipInputStream(fs);
                s.Password = Password;
                long count = 0;//计算已解压的文件总大小
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        ///判断文件路径是否是文件夹
                        if (fileName.EndsWith("/") || fileName.EndsWith("//"))
                        {
                            Directory.CreateDirectory(fileName);
                            this.Invoke((AddLogDelegate)delegate(string text) { AddLog(text); }, "正在创建文件夹：" + theEntry.Name);
                            continue;
                        }
                        this.Invoke((AddLogDelegate)delegate(string text) { AddLog(text); }, "正在解压文件：" + theEntry.Name);
                        streamWriter = File.Create(fileName);
                        int size = 102400;
                        byte[] data = new byte[102400];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            count += size;
                            this.Invoke((UpdatePgBar)delegate(int value)
                            {
                                if (value < pgBarUnZip.Maximum)
                                {
                                    pgBarUnZip.Value = value;
                                }
                            }, (Int32)count / 1024);
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
            //解压完成 进行下一步 退出更新程序或者执行脚本
            if (!string.IsNullOrEmpty(UpdateModel.ScriptUrl))
            {
                this.Invoke((UpdatePgBar) delegate(int value)
                {
                    FrmCmd frmCmd = new FrmCmd();
                    frmCmd.Show();
                    this.Hide();
                },0);
                
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo("Update");
                directoryInfo.Delete(true);
                Process.Start(UpdateModel.Start + ".exe");
                Environment.Exit(0);
            }
        }


        private void AddLog(string text)
        {
            txtLog.AppendText(text+"\n");
        }
    }
}
