using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hanaya_TgBot_Nogui
{
    public class ranks_api
    {
        public string WeekRank()
        {
            string data  = DateTime.Now.ToString("yyyy-MM-dd");
            string page = "1";
            string mode = "week";
            string pageSize = "10";
            string Url = "https://api.pixivic.com/ranks?page=1&date="+data+"&mode="+mode+"&page="+page+"&pageSize="+pageSize;
            string Json;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
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
