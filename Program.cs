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
using Newtonsoft.Json.Linq;
using Telegram.Bot.Args;

namespace Hanaya_TgBot_Nogui
{
    public class Program
    {
        static ITelegramBotClient botClient;
        static async Task Main(string[] args)
        {
            try
            {
                //Tips
                Console.WriteLine("================");
                Console.WriteLine("如果是国内用户，请确保代理有被启用并有效");
                Console.WriteLine("GitHub:https://github.com/Tgbotdev/Hanaya_TgBot_Nogui");
                Console.WriteLine("================");

                //Token输入
                Console.Write("Token:");
                string token = Console.ReadLine();

                //该方法弃用
                ////传参至botLogin,返回loginResult,并输出
                //botLogin login = new botLogin();
                //string resultLoginStr = login.BotLogin(token);
                //Console.WriteLine(resultLoginStr);

                //SSL/TLS连接
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                //建立Bot客户端
                var botClient = new TelegramBotClient(token);

                //获取Bot资料
                var bot_info = await botClient.GetMeAsync();
                var Json = JsonConvert.SerializeObject(bot_info);
                Console.WriteLine($"=====================\n" +
                    $"Well done.Login success." +
                    $"\nAccount:{bot_info.Id}\n" +
                    $"Name:{bot_info.FirstName}" +
                    $"\n=====================\n");

                //写入botInfo.json
                string path = Directory.GetCurrentDirectory() + "\\botInfo.json";
                await Task.Run(() => File.WriteAllText(path, Json));

                //写入Config,废弃,改用Json,见上
                //string path = Directory.GetCurrentDirectory() + "\\botInfo.ini";
                //FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
                //if (!File.Exists(path))
                //{
                //    File.Create(path);
                //    fs.Close();
                //}
                //else
                //{
                //    fs.Close();
                //}
                //IniConfig ini = new IniConfig(path);
                //ini.Load();
                //ini.Clear();
                //ini.Object.Add(new ISection("BotAccount"));
                //ini.Object["BotAccount"]["token"] = token;
                //ini.Object["BotAccount"]["ID"] = bot_info.Result.Id;
                //ini.Object["BotAccount"]["Name"] = bot_info.Result.FirstName;
                //ini.Save();

                //Tips
                Console.Clear();
                Console.WriteLine("==================");
                Console.WriteLine("Congratulations! 登入成功");
                Console.WriteLine("开始执行预定初始任务");
                Console.WriteLine("==================");
                //接受消息开始
                botClient.StartReceiving();
                botClient.OnMessage += BotClient_OnMessage;
                Console.WriteLine("\n\n\n应用结束处理消息,按任意键退出\n");
                Console.ReadKey();
                botClient.StopReceiving();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n------------------------\n" + ex.ToString() + "\n------------------------\n");
            }
            finally
            {
                Console.WriteLine("按任意键退出\n");
                Console.ReadKey();
            }
        }

        static async void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                //判断消息不为空
                if (e.Message.Text != null)
                {
                    //控制台输出
                    Console.WriteLine($"[Info]: 来自{e.Message.Chat.Id},消息:{e.Message.Text}.");
                    //消息发送
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "你发送了:\n" + e.Message.Text
                        );
                }
            }catch(Exception ex)
            {
                Console.WriteLine("\n------------------------\n" + ex.ToString() + "\n------------------------\n");
            }
        }
    }
}
