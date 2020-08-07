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
        static async Task Main(string[] args)
        {
            try
            {
                //Tips
                Console.WriteLine("================");
                Console.WriteLine("如果是国内用户，请确保代理有被启用并有效");
                Console.WriteLine("请保证登录阶段网络延迟在可接受范围内");
                Console.WriteLine("否则程序有闪退可能.遇到Bug请去发issue");
                Console.WriteLine("GitHub:https://github.com/Tgbotdev/Hanaya_TgBot_Nogui");
                Console.WriteLine("================");

                //Token输入
                Console.Write("Token:");
                string token = Console.ReadLine();

                //Token储存
                string tokenPath = Directory.GetCurrentDirectory() + "\\tokenSave.txt";
                File.WriteAllText(tokenPath,token);

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
                botClient.OnMessage += BotClient_OnMessage;
                botClient.StartReceiving();
                Console.WriteLine("消息处理开始,按任意键终止\n");
                Console.ReadKey();
                botClient.StopReceiving();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n------------------------\n" + ex.ToString() + "\n------------------------\n");
            }
            finally
            {
                //删除储存的token
                File.Delete(Directory.GetCurrentDirectory()+"\\tokenSave.txt");
                //结束
                Console.WriteLine("按任意键退出\n");
                Console.ReadKey();
            }
        }

       //不直接提供token
       //static ITelegramBotClient botClient;
       //异步直接tm飞向finally***
       //采用同步，否则直接finally
       //这边骂一句example,nmd纯属误导人
       //采用异步请再118的static后加上async,135头部加上await
        static void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                //建立Bot客户端
                //写到这才想起json没储存token...
                //var botInfo = File.ReadAllText(Directory.GetCurrentDirectory() + "\\botInfo.json");
                //var Json = JsonConvert.DeserializeObject<dynamic>(botInfo);
                string token = File.ReadAllText(Directory.GetCurrentDirectory()+"\\tokenSave.txt");
                TelegramBotClient botClient = new TelegramBotClient(token);

                //判断消息不为空
                if (e.Message.Text != null)
                {
                    //控制台输出
                    Console.WriteLine($"[信息]: 来自{e.Message.Chat.Id},消息:{e.Message.Text}.");
                    Console.WriteLine($"[处理]: 对消息来源{e.Message.Chat.Id}发送消息: 你发送了{e.Message.Text}.");
                   
                    //消息发送
                    botClient.SendTextMessageAsync(
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
