﻿using System.Text;
using System.IO;
using System.Net;

namespace Hanaya_TgBot_Nogui
{
    public class Picture_HttpGet
    {
        public string HttpGet()
        {
            string PicUrl, ApiUrl = "https://api.tosks.com/acgpicture/?type=text";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);
            request.Proxy = null;
            request.KeepAlive = false;
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:78.0) Gecko/20100101 Firefox/78.0";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            PicUrl = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            return PicUrl;
        }
    }
}
