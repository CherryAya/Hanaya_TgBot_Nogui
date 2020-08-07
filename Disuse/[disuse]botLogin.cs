using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Native.Tool.IniConfig;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Native.Tool.IniConfig.Linq;

namespace Hanaya_TgBot_Nogui
{
    public class botLogin
    {
        public string BotLogin(string token)
        {
            string Rtn = null;
            try
            {
                //SSL/TLS连接
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                //建立Bot客户端
                var botClient = new TelegramBotClient(token);
                //获取Bot资料
                var bot_info = botClient.GetMeAsync();
                Console.WriteLine($"=====================\n" +
                    $"Well done.Login success." +
                    $"\nAccount:{bot_info.Result.Id}\n" +
                    $"Name:{bot_info.Result.FirstName}" +
                    $"\n=====================\n");
                //写入Config
                string path = Directory.GetCurrentDirectory() + "\\botInfo.ini";
                FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
                if (File.Exists(path) == false)
                {
                    File.Create(path);
                    fs.Close();
                }
                else
                {
                    fs.Close();
                }
                IniConfig ini = new IniConfig(path);
                ini.Load();
                ini.Clear();
                ini.Object.Add(new ISection("BotAccount"));
                ini.Object["BotAccount"]["token"] = token;
                ini.Object["BotAccount"]["ID"] = bot_info.Result.Id;
                ini.Object["BotAccount"]["Name"] = bot_info.Result.FirstName;
                ini.Save();
                Rtn = "Login success.";
            }
            catch(Exception ex)
            {
                Rtn = ex.Message;
            }
            return Rtn;
        }
    }
}
