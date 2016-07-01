
using System;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;

namespace UpdateHelper.Http
{
    /// <summary>
    /// Http操作类
    /// Author：李志强
    /// Time：2016年2月22日10:34:33
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// GET请求获取结果
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;  
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

        /// <summary>
        /// 下载小文件
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="savePath">保存文件绝对路径</param>
        public static void DownloadFile(string url, string savePath)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                FileStream fs=new FileStream(savePath,FileMode.Create);

                byte[] bufferBytes=new byte[1024];

                int read = 0;

                while ((read = myResponseStream.Read(bufferBytes, 0, bufferBytes.Length)) > 0)
                {
                    fs.Write(bufferBytes,0,read);
                    fs.Flush();
                }


                fs.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

    }
}