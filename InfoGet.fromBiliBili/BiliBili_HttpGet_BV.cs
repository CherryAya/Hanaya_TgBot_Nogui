using System.IO;
using System.Net;
using System.Text;

namespace Hanaya_TgBot_Nogui
{
    public class BiliBili_HttpGet_BV
    {
        public string HttpGet(string bvid)
        {
            string Json, Url = "http://api.bilibili.com/x/web-interface/view?bvid=";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + bvid);
            request.Proxy = null;
            request.KeepAlive = false;
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:78.0) Gecko/20100101 Firefox/78.0";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            Json = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            return Json;
        }
    }
}
