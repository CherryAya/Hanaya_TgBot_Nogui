using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System;

namespace Hanaya_TgBot_Nogui
{
    public class illustrations_api
    {
        public string FirstResult(string keyword)
        {
            string Rtn, Pximg, PicUrl, Url = "https://api.pixivic.com/illustrations?keyword=" + keyword + "&page=1&pageSize=1";
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
            Rtn = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            var Json = JsonConvert.DeserializeObject<dynamic>(Rtn);
            Pximg = Json.data[0].imageUrls[0].original;
            PicUrl = "https://i.pixiv.cat/" + Pximg.Substring(20, Pximg.Length - 20);
            return PicUrl;
        }

        public string ListResult(string keyword, string pageSize)
        {
            string Rtn, Pximg, imageUrls = null, Url = "https://api.pixivic.com/illustrations?keyword=" + keyword + "&page=1&pageSize=" + pageSize;
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
            Rtn = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            var Json = JsonConvert.DeserializeObject<dynamic>(Rtn);
            for (int i = 0; i <= Convert.ToInt32(pageSize) - 1; i++)
            {
                Pximg = Json.data[i].imageUrls[0].original;
                imageUrls = imageUrls + (i + 1) + ". " + "https://i.pixiv.cat/" + Pximg.Substring(20, Pximg.Length - 20) + "\n";
            }
            return imageUrls;
        }
    }
}
