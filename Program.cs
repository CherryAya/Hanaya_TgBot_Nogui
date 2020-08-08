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
                Console.WriteLine("欢迎使用Hanaya_TgBot的Nogui版本");
                Console.WriteLine("获取更新或支持请前往Github");
                Console.WriteLine("如果是国内用户，请确保代理有被启用并有效");
                Console.WriteLine("请保证登录阶段网络延迟在可接受范围内");
                Console.WriteLine("否则程序有闪退可能.遇到Bug请去发issue");
                Console.WriteLine("GitHub:https://github.com/Tgbotdev/Hanaya_TgBot_Nogui");
                Console.WriteLine("================");

                //初始化检测:Initialization 使用Json ini弃用
                string firstpath = Directory.GetCurrentDirectory() + "\\firstCheck.ini";
                if (!File.Exists(firstpath))
                {
                    Console.WriteLine("[信息]应用开始初始化");
                    File.Create(firstpath);
                    File.Create(Directory.GetCurrentDirectory() + "\\config.ini");

                    Console.WriteLine("[信息]完成初始化,请重启应用");
                    Console.WriteLine("按任意键退出");
                    Console.ReadKey();
                    Environment.Exit(0);
                    //IniConfig ini = new IniConfig(Directory.GetCurrentDirectory() + "\\config.ini");
                    //ini.Load();
                    //ini.Object.Add(new ISection("function"));
                    //ini.Object["function"]["use"] = "0";
                    //ini.Save();
                }

                //Token输入
                Console.Write("Token:");
                string token = Console.ReadLine();

                //Token储存 临时文件使用txt 带加密 
                string tokenPath = Directory.GetCurrentDirectory() + "\\tokenSave.txt";
                tokenProtect.Encryption encryption = new tokenProtect.Encryption();
                string EncryptionText = encryption.Encrypt(token);
                if (EncryptionText == "Error")
                {
                    Console.WriteLine("[错误]token加密未成功,取消本地写入token并退出,请重启程序");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("[信息]token加密成功,已写入本地");
                    File.WriteAllText(tokenPath, EncryptionText);
                }


                //该方法弃用
                ////传参至botLogin,返回loginResult,并输出
                //botLogin login = new botLogin();
                //string resultLoginStr = login.BotLogin(token);
                //Console.WriteLine(resultLoginStr);

                //SSL/TLS连接
                Console.WriteLine("[信息]载入SSL/TLS安全传输协议");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                //建立Bot客户端
                Console.WriteLine("[信息]搭建Bot客户端 导入token");
                var botClient = new TelegramBotClient(token);

                //获取Bot资料
                Console.WriteLine("[信息]开始获取Bot资料\n");
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
                Console.WriteLine("[信息]Bot信息已被储存到本地");

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
                Console.WriteLine("开始接收消息并根据配置文件处理消息");
                Console.WriteLine("==================");

                //检查功能启用 使用ini储存
                string configPath = Directory.GetCurrentDirectory() + "\\config.ini";
                IniConfig iniConfig = new IniConfig(configPath);
                //if (!File.Exists(configPath))
                //{
                //    File.Create(configPath);
                //    iniConfig.Load();
                //    iniConfig.Object.Add(new ISection("function"));
                //    iniConfig.Object["function"]["use"] = "0";
                //    iniConfig.Save();
                //    Console.WriteLine("[信息]配置文件丢失,重新创建文件,默认功能为'0'");
                //}
                iniConfig.Load();
                if (iniConfig.Object["function"]["use"] == "0")
                {
                    iniConfig.Save();
                    Console.WriteLine("[信息]载入0号功能:复读机");
                    //复读机
                    //接受消息开始
                    botClient.OnMessage += BotClient_Respeak;
                    botClient.StartReceiving();
                    Console.WriteLine("消息处理开始,按任意键终止\n");
                    Console.ReadKey();
                    botClient.StopReceiving();
                }
                else if (iniConfig.Object["function"]["use"] == "1")
                {
                    iniConfig.Save();
                    Console.WriteLine("[信息]载入1号功能:BiliBIli信息获取");
                    //BiliBili信息获取
                    //接受消息开始
                    botClient.OnMessage += BotClient_InfoGet_bili;
                    botClient.StartReceiving();
                    Console.WriteLine("消息处理开始,按任意键终止\n");
                    Console.ReadKey();
                    botClient.StopReceiving();
                }
                else
                {
                    iniConfig.Save();
                    Console.WriteLine("没有找到配置文件值相对应功能，应用退出");
                }
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


       //复读机
       //不直接提供token
       //static ITelegramBotClient botClient;
       //异步直接tm飞向finally***
       //采用同步，否则直接finally
       //这边骂一句example,nmd纯属误导人
        static void BotClient_Respeak(object sender, MessageEventArgs e)
        {
            try
            {
                //建立Bot客户端
                //写到这才想起json没储存token...
                //var botInfo = File.ReadAllText(Directory.GetCurrentDirectory() + "\\botInfo.json");
                //var Json = JsonConvert.DeserializeObject<dynamic>(botInfo);
                string token = File.ReadAllText(Directory.GetCurrentDirectory()+"\\tokenSave.txt");
                tokenProtect.Decryption decryption = new tokenProtect.Decryption();
                string DecryptionText = decryption.Decrypt(token); //解密
                if (DecryptionText == "Error")
                {
                    Console.WriteLine("[错误]token解密未成功,将导致空指针异常,本次信息可能返回控制台但不被处理");
                    DecryptionText = null;
                }
                TelegramBotClient botClient = new TelegramBotClient(DecryptionText);

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

        static void BotClient_InfoGet_bili(object sender, MessageEventArgs e)
        {
            try
            {
                //Json
                string Json, av, datetime, tidstr, copyrightstr;
                string bvid, aid, title, pic, videos, tid, tname, copyright, pubdate, desc, duration;//data.
                string mid, name, face;//data.owner.
                string view, danmaku, reply, favorite, coin, share, like;//data.stat.
                int[] pages;//data.pages.
                string cid, page, part, duration_page;//if_data.pages=Int[],data.pages.[0/1/2]
                int[] staff;//data.staff.
                string mid_staff, title_staff, name_staff, face_staff, official, follower;//if_data.staff=Int[],data.staff.[0/1/2]
                //开发中
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n------------------------\n" + ex.ToString() + "\n------------------------\n");
            }
        }


    }
}
