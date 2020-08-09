using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

//Json工具
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//ini工具
//using Native.Tool.IniConfig;
//using Native.Tool.IniConfig.Linq;

//Telegram.Bot SDK
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

//Socks5 Proxy
using MihaZupan;

namespace Hanaya_TgBot_Nogui
{
    public class Program
    {
        public static ITelegramBotClient botClient;
        static async Task Main(string[] args)
        {
            try
            {
                //Tips
                Console.WriteLine("================");
                Console.WriteLine("欢迎使用Hanaya_TgBot的Nogui版本");
                Console.WriteLine("获取更新或支持请前往Github");
                Console.WriteLine("如果是国内用户 请确保代理有被启用并有效");
                Console.WriteLine("需要使用Http/Socks5代理请去填写\\Proxy.json 不支持使用MTProto");
                Console.WriteLine("如果代理文件未生成 请执行一遍程序 会自动生成");
                Console.WriteLine("请保证登录阶段网络延迟在可接受范围内");
                Console.WriteLine("GitHub:https://github.com/Tgbotdev/Hanaya_TgBot_Nogui");
                Console.WriteLine("================");

                //废弃初始化
                ////初始化检测:Initialization 直接储存json 弃用ini
                //string TgBotJsonPath = Directory.GetCurrentDirectory() + "\\config\\TgBot.json";
                //if (!File.Exists(TgBotJsonPath))
                //{
                //    Console.WriteLine("[信息]应用开始初始化");
                //    File.Create(TgBotJsonPath);
                //    var TgBotJson = "[{\"Initialization\":\"Initialized\",\"function\":\"0\"}]";
                //    File.WriteAllText(TgBotJsonPath, TgBotJson);
                //    Console.WriteLine("[信息]完成初始化,请重启应用");
                //    Console.WriteLine("按任意键退出");
                //    Console.ReadKey();
                //    Environment.Exit(0);
                //    //IniConfig ini = new IniConfig(Directory.GetCurrentDirectory() + "\\config.ini");
                //    //ini.Load();
                //    //ini.Object.Add(new ISection("function"));
                //    //ini.Object["function"]["use"] = "0";
                //    //ini.Save();
                //}
                //var InitializationJson = File.ReadAllText(TgBotJsonPath);
                //var Initialized = JsonConvert.DeserializeObject<dynamic>(InitializationJson);
                //if (!Initialized.Initialization == "Initialized")
                //{
                //    Console.WriteLine("[信息]检测到需要初始化(配置项不为Initialized)");
                //    var TgBotJson = "[{\"Initialization\":\"Initialized\",\"function\":\"0\"}]";
                //    File.WriteAllText(TgBotJsonPath,TgBotJson);
                //    Console.WriteLine("[信息]完成初始化,请重启应用");
                //    Console.WriteLine("按任意键退出");
                //    Console.ReadKey();
                //    Environment.Exit(0);
                //}

                //Token输入
                Console.Write("Token:");
                string token = Console.ReadLine();

                ////Token储存 临时文件使用txt 带加密 取消使用
                //string tokenPath = Directory.GetCurrentDirectory() + "\\tokenSave.txt";
                //tokenProtect.Encryption encryption = new tokenProtect.Encryption();
                //string EncryptionText = encryption.Encrypt(token);
                //if (EncryptionText == "Error")
                //{
                //    Console.WriteLine("[错误]token加密未成功,取消本地写入token并退出,请重启程序");
                //    Environment.Exit(0);
                //}
                //else
                //{
                //    Console.WriteLine("[信息]token加密成功,已写入本地");
                //    System.IO.File.WriteAllText(tokenPath, EncryptionText);
                //}


                //该方法弃用
                ////传参至botLogin,返回loginResult,并输出
                //botLogin login = new botLogin();
                //string resultLoginStr = login.BotLogin(token);
                //Console.WriteLine(resultLoginStr);

                //SSL/TLS连接
                Console.WriteLine("[信息]载入SSL/TLS安全传输协议");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                //检测代理配置文件 并建立Bot客户端
                string ProxyConfig = Directory.GetCurrentDirectory() + "\\Proxy.json";
                if (System.IO.File.Exists(ProxyConfig) == false)
                {
                    Console.WriteLine("[代理]配置文件不存在 创建默认配置文件");
                    System.IO.File.WriteAllText(ProxyConfig, "{\"Mode\":\"None\",\"Socks5\":{\"ServerAddress\":\"\",\"ServerPort\":\"\",\"Username\":\"\",\"Password\":\"\"},\"Http\":{\"ServerAddress\":\"\",\"ServerPort\":\"\",\"Username\":\"\",\"Password\":\"\"}}");
                }
                var ProxyJson = System.IO.File.ReadAllText(ProxyConfig);
                var Proxy = JsonConvert.DeserializeObject<dynamic>(ProxyJson);
                string ProxyMode = Proxy.Mode;
                if (ProxyMode == "None")
                {
                    Console.WriteLine("[代理]代理设置为None(不使用代理)");
                    Console.WriteLine("[信息]搭建Bot客户端 导入token");
                    botClient = new TelegramBotClient(token);
                }
                else if (ProxyMode == "Http")
                {
                    string ServerAddress, Username, Password;
                    int ServerPort;
                    ServerAddress = Proxy.Http.ServerAddress;
                    ServerPort = Proxy.Http.ServerPort;
                    Username = Proxy.Http.Username;
                    Password = Proxy.Http.Password;
                    var proxy = new WebProxy(ServerAddress, ServerPort)
                    {
                        Credentials = new NetworkCredential(Username, Password)
                    };
                    Console.WriteLine("[代理]使用代理 模式'Http-Proxy'");
                    Console.WriteLine($"[代理]准备连接至{ServerAddress}:{ServerPort}");
                    Console.WriteLine($"[代理]用户名{Username} 密码{Password}");
                    Console.WriteLine("[信息]搭建Bot客户端 导入token");
                    botClient = new TelegramBotClient(token,proxy);
                }
                else if (ProxyMode == "Socks5")
                {
                    string ServerAddress, Username, Password;
                    int ServerPort;
                    ServerAddress = Proxy.Socks5.ServerAddress;
                    ServerPort = Proxy.Socks5.ServerPort;
                    Username = Proxy.Socks5.Username;
                    Password = Proxy.Socks5.Password;
                    var proxy = new HttpToSocks5Proxy(ServerAddress, ServerPort, Username, Password);
                    proxy.ResolveHostnamesLocally = true;
                    Console.WriteLine("[代理]使用代理 模式'Socks5-Proxy'");
                    Console.WriteLine($"[代理]准备连接至{ServerAddress}:{ServerPort}");
                    Console.WriteLine($"[代理]用户名{Username} 密码{Password}");
                    Console.WriteLine("[信息]搭建Bot客户端 导入token");
                    botClient = new TelegramBotClient(token,proxy);
                }

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
                await Task.Run(() => System.IO.File.WriteAllText(path, Json));
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
                Console.WriteLine("开始接收消息并根据所选择功能处理消息");
                Console.WriteLine("==================");

                //取消检测 改用输入
                //检查功能启用 使用ini储存
                //string configPath = Directory.GetCurrentDirectory() + "\\config\\TgBot.json";
                //var funcjson = File.ReadAllText(configPath);
                //var func = JsonConvert.DeserializeObject<dynamic>(funcjson);
                //IniConfig iniConfig = new IniConfig(configPath);
                //if (!File.Exists(configPath))
                //{
                //    File.Create(configPath);
                //    iniConfig.Load();
                //    iniConfig.Object.Add(new ISection("function"));
                //    iniConfig.Object["function"]["use"] = "0";
                //    iniConfig.Save();
                //    Console.WriteLine("[信息]配置文件丢失,重新创建文件,默认功能为'0'");
                //}
                //iniConfig.Load();

                //选择功能
                Console.WriteLine("[信息]请选择启用的功能");
                Console.WriteLine("[功能]1 复读机 (Respeak)");
                Console.WriteLine("[功能]2 BiliBili信息获取 (InfoGet_fromBili)");
                Console.WriteLine("[功能]3 一言 (Hitokoto)");
                Console.Write(">");
                string func = Console.ReadLine();
                if (func == "1")
                {
                    //iniConfig.Save();
                    Console.WriteLine("[信息]载入1号功能:复读机");
                    //复读机
                    //接受消息开始
                    botClient.OnMessage += BotClient_Respeak;
                    botClient.StartReceiving();
                    Console.WriteLine("[提示]消息处理开始,按任意键终止\n");
                    Console.ReadKey();
                    botClient.StopReceiving();
                }
                else if (func == "2")
                {
                    //iniConfig.Save();
                    Console.WriteLine("[信息]载入2号功能:BiliBIli信息获取");
                    //BiliBili信息获取
                    //接受消息开始
                    botClient.OnMessage += BotClient_InfoGet_bili;
                    botClient.StartReceiving();
                    Console.WriteLine("[处理]: 对消息来源一律处理 -> BiliBili_InfoGet. (不匹配消息会被忽略)");
                    Console.WriteLine("[提示]消息处理开始,按任意键终止\n");
                    Console.ReadKey();
                    botClient.StopReceiving();
                }else if (func == "3")
                {
                    //一言
                    Console.WriteLine("[信息]载入3号功能:一言");
                    //接受消息开始
                    botClient.OnMessage += BotClient_Hitokoto;
                    botClient.StartReceiving();
                    Console.WriteLine("[处理]: 对消息来源一律处理 -> Hitokoto. (不匹配消息会被忽略)");
                    Console.WriteLine("[提示]消息处理开始,按任意键终止\n");
                    Console.ReadKey();
                    botClient.StopReceiving();
                }
                else
                {
                    //iniConfig.Save();
                    Console.WriteLine("没有找到配置文件值相对应功能，应用退出");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n------------------------\n" + ex.ToString() + "\n------------------------\n");
            }
            finally
            {
                //检测储存的token
                if (System.IO.File.Exists(Directory.GetCurrentDirectory() + "\\tokenSave.txt"))
                {
                    //删除储存的token
                    System.IO.File.Delete(Directory.GetCurrentDirectory() + "\\tokenSave.txt");
                }
                //结束
                Console.WriteLine("\n按任意键退出\n");
                Console.ReadKey();
            }
        }

       //复读机
       //异步直接tm飞向finally***
       //采用同步，否则直接finally
       //这边骂一句example,nmd纯属误导人
        static void BotClient_Respeak(object sender, MessageEventArgs e)
        {
            try
            {
                //建立Bot客户端 废弃 使用全局Bot客户端
                //写到这才想起json没储存token...
                //var botInfo = File.ReadAllText(Directory.GetCurrentDirectory() + "\\botInfo.json");
                //var Json = JsonConvert.DeserializeObject<dynamic>(botInfo);
                //string token = System.IO.File.ReadAllText(Directory.GetCurrentDirectory()+"\\tokenSave.txt");
                //tokenProtect.Decryption decryption = new tokenProtect.Decryption();
                //string DecryptionText = decryption.Decrypt(token); //解密
                //if (DecryptionText == "Error")
                //{
                //    Console.WriteLine("[错误]token解密未成功,将导致空指针异常,本次信息可能返回控制台但不被处理");
                //    DecryptionText = null;
                //}
                //botClient = new TelegramBotClient(DecryptionText);

                //判断消息不为空
                if (e.Message.Text != null)
                {
                    //控制台输出
                    Console.WriteLine($"[信息]: 来自{e.Message.Chat.Id},消息:{e.Message.Text}.");
                    Console.WriteLine($"[处理]: 对消息来源{e.Message.Chat.Id}发送消息: 你发送了{e.Message.Text}.");
                   
                    //消息发送
                    botClient.SendTextMessageAsync(
                         e.Message.Chat,
                         "你发送了:\n" + e.Message.Text
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
                //Json -> 变量
                string Json, av, datetime, tidstr, copyrightstr;
                string bvid, aid, title, pic, videos, tid, tname, copyright, pubdate, desc, duration;//data.
                string mid, name, face;//data.owner.
                string view, danmaku, reply, favorite, coin, share, like;//data.stat.

                ////建立Bot客户端 废弃 使用全局Bot客户端
                //string token = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "\\tokenSave.txt");
                //tokenProtect.Decryption decryption = new tokenProtect.Decryption();
                //string DecryptionText = decryption.Decrypt(token); //解密
                //if (DecryptionText == "Error")
                //{
                //    Console.WriteLine("[错误]token解密未成功,将导致空指针异常,本次信息可能返回控制台但不被处理");
                //    DecryptionText = null;
                //}
                //TelegramBotClient botClient = new TelegramBotClient(DecryptionText);

                //判断消息不为空
                if (e.Message.Text != null)
                {
                    //控制台输出
                    Console.WriteLine($"[信息]: 来自{e.Message.Chat.Id},消息:{e.Message.Text}.");

                    string Msg = e.Message.Text;
                    if (Msg.Length >= 2)
                    {
                        if (Msg.Substring(0, 2) == "av")
                        {
                            //HttpGet_Json
                            BiliBili_HttpGet_AV _HttpGet = new BiliBili_HttpGet_AV();
                            Json = _HttpGet.HttpGet(aid = Msg.Substring(2, Msg.Length - 2));
                            var _Jsonobj = JsonConvert.DeserializeObject<dynamic>(Json);
                            if (_Jsonobj.code == 0)
                            {
                                //Json -> 变量
                                //data.
                                pic = _Jsonobj.data.pic;
                                bvid = _Jsonobj.data.bvid;  //bv号
                                av = "av" + _Jsonobj.data.aid; //av号
                                title = _Jsonobj.data.title;  //标题
                                videos = _Jsonobj.data.videos;  //分P数量
                                tid = _Jsonobj.data.tid;  //主分区
                                tname = _Jsonobj.data.tname;  //子分区
                                copyright = _Jsonobj.data.copyright;  //版权信息
                                pubdate = _Jsonobj.data.pubdate;  //投稿时间(时间戳
                                desc = _Jsonobj.data.desc;  //简介
                                duration = _Jsonobj.data.duration;  //视频持续时长(所有分P
                                //data.owner.
                                mid = _Jsonobj.data.owner.mid;  //up主UID
                                name = _Jsonobj.data.owner.name;  //up主昵称
                                face = _Jsonobj.data.owner.face;  //up主头像地址
                                //data.stat.
                                view = _Jsonobj.data.stat.view;  //观看数量
                                danmaku = _Jsonobj.data.stat.danmaku;  //弹幕数量
                                reply = _Jsonobj.data.stat.reply;  //评论数量
                                favorite = _Jsonobj.data.stat.favorite;  //收藏数量
                                coin = _Jsonobj.data.stat.coin;  //投币数量
                                share = _Jsonobj.data.stat.share;  //分享数量
                                like = _Jsonobj.data.stat.like;  //获赞数量

                                //时间戳转换
                                Pubdate_Convert pubdate_Convert = new Pubdate_Convert();
                                datetime = pubdate_Convert.Pubdate(pubdate);

                                //主分区转换
                                Tid_Judge tid_judge = new Tid_Judge();
                                tidstr = tid_judge.Tid(Convert.ToInt32(tid));

                                //版权信息判断
                                Copyright_Judge copyright_judge = new Copyright_Judge();
                                copyrightstr = copyright_judge.Copyright(Convert.ToInt32(copyright));

                                //消息发送
                                botClient.SendTextMessageAsync(
                                e.Message.Chat,
                                title + "[共" + videos + "P]" + "\n" + "bv号:" + bvid + "\n" + "av号:" + av + "\n" + "UP主:" + name 
                                  + "\nUP主UID:" + mid + "\n" + "-----------------\n" +"分区:"+ tidstr + ":" + tname + "\n" + "点赞:" + like + "  投币:" 
                                  + coin + "\n收藏:" + favorite + "  观看:" + view + "\n弹幕:" + danmaku + "  评论:" + reply + "\n分享:" + share +
                                  "\n-----------------\n" + "简介:" + desc);

                                //图片发送
                                botClient.SendPhotoAsync(e.Message.Chat,pic,"视频封面",ParseMode.Html);
                            }
                            else
                            {
                                //发送错误详情
                                botClient.SendTextMessageAsync(e.Message.Chat,"错误:\n" + "Code:" + _Jsonobj.code + "\n" + _Jsonobj.Message + "\n" + "错误码:\n400为请求错误\n404为找不到稿件\n62002为稿件不可见");
                            }
                        }
                        else if (Msg.Substring(0, 2) == "AV")
                        {
                            //HttpGet_Json
                            BiliBili_HttpGet_AV _HttpGet = new BiliBili_HttpGet_AV();
                            Json = _HttpGet.HttpGet(aid = Msg.Substring(2, Msg.Length - 2));
                            var _Jsonobj = JsonConvert.DeserializeObject<dynamic>(Json);
                            if (_Jsonobj.code == 0)
                            {
                                //Json -> 变量
                                //data.
                                pic = _Jsonobj.data.pic;
                                bvid = _Jsonobj.data.bvid;  //bv号
                                av = "av" + _Jsonobj.data.aid; //av号
                                title = _Jsonobj.data.title;  //标题
                                videos = _Jsonobj.data.videos;  //分P数量
                                tid = _Jsonobj.data.tid;  //主分区
                                tname = _Jsonobj.data.tname;  //子分区
                                copyright = _Jsonobj.data.copyright;  //版权信息
                                pubdate = _Jsonobj.data.pubdate;  //投稿时间(时间戳
                                desc = _Jsonobj.data.desc;  //简介
                                duration = _Jsonobj.data.duration;  //视频持续时长(所有分P
                                //data.owner.
                                mid = _Jsonobj.data.owner.mid;  //up主UID
                                name = _Jsonobj.data.owner.name;  //up主昵称
                                face = _Jsonobj.data.owner.face;  //up主头像地址
                                //data.stat.
                                view = _Jsonobj.data.stat.view;  //观看数量
                                danmaku = _Jsonobj.data.stat.danmaku;  //弹幕数量
                                reply = _Jsonobj.data.stat.reply;  //评论数量
                                favorite = _Jsonobj.data.stat.favorite;  //收藏数量
                                coin = _Jsonobj.data.stat.coin;  //投币数量
                                share = _Jsonobj.data.stat.share;  //分享数量
                                like = _Jsonobj.data.stat.like;  //获赞数量

                                //时间戳转换
                                Pubdate_Convert pubdate_Convert = new Pubdate_Convert();
                                datetime = pubdate_Convert.Pubdate(pubdate);

                                //主分区转换
                                Tid_Judge tid_judge = new Tid_Judge();
                                tidstr = tid_judge.Tid(Convert.ToInt32(tid));

                                //版权信息判断
                                Copyright_Judge copyright_judge = new Copyright_Judge();
                                copyrightstr = copyright_judge.Copyright(Convert.ToInt32(copyright));

                                //消息发送
                                botClient.SendTextMessageAsync(
                                e.Message.Chat,
                                title + "[共" + videos + "P]" + "\n" + "bv号:" + bvid + "\n" + "av号:" + av + "\n" + "UP主:" + name
                                  + "\nUP主UID:" + mid + "\n" + "-----------------\n" + "分区:" + tidstr + ":" + tname + "\n" + "点赞:" + like + "  投币:"
                                  + coin + "\n收藏:" + favorite + "  观看:" + view + "\n弹幕:" + danmaku + "  评论:" + reply + "\n分享:" + share +
                                  "\n-----------------\n" + "简介:" + desc);

                                //图片发送
                                botClient.SendPhotoAsync(e.Message.Chat, pic, "视频封面", ParseMode.Html);
                            }
                            else
                            {
                                //发送错误详情
                                botClient.SendTextMessageAsync(e.Message.Chat, "错误:\n" + "Code:" + _Jsonobj.code + "\n" + _Jsonobj.Message + "\n" + "错误码:\n400为请求错误\n404为找不到稿件\n62002为稿件不可见");
                            }
                        }
                        else if (Msg.Substring(0, 2) == "BV")
                        {
                            //HttpGet_Json
                            BiliBili_HttpGet_BV _HttpGet = new BiliBili_HttpGet_BV();
                            Json = _HttpGet.HttpGet(aid = Msg.Substring(2, Msg.Length - 2));
                            var _Jsonobj = JsonConvert.DeserializeObject<dynamic>(Json);
                            if (_Jsonobj.code == 0)
                            {
                                //Json -> 变量
                                //data.
                                pic = _Jsonobj.data.pic;
                                bvid = _Jsonobj.data.bvid;  //bv号
                                av = "av" + _Jsonobj.data.aid; //av号
                                title = _Jsonobj.data.title;  //标题
                                videos = _Jsonobj.data.videos;  //分P数量
                                tid = _Jsonobj.data.tid;  //主分区
                                tname = _Jsonobj.data.tname;  //子分区
                                copyright = _Jsonobj.data.copyright;  //版权信息
                                pubdate = _Jsonobj.data.pubdate;  //投稿时间(时间戳
                                desc = _Jsonobj.data.desc;  //简介
                                duration = _Jsonobj.data.duration;  //视频持续时长(所有分P
                                //data.owner.
                                mid = _Jsonobj.data.owner.mid;  //up主UID
                                name = _Jsonobj.data.owner.name;  //up主昵称
                                face = _Jsonobj.data.owner.face;  //up主头像地址
                                //data.stat.
                                view = _Jsonobj.data.stat.view;  //观看数量
                                danmaku = _Jsonobj.data.stat.danmaku;  //弹幕数量
                                reply = _Jsonobj.data.stat.reply;  //评论数量
                                favorite = _Jsonobj.data.stat.favorite;  //收藏数量
                                coin = _Jsonobj.data.stat.coin;  //投币数量
                                share = _Jsonobj.data.stat.share;  //分享数量
                                like = _Jsonobj.data.stat.like;  //获赞数量

                                //时间戳转换
                                Pubdate_Convert pubdate_Convert = new Pubdate_Convert();
                                datetime = pubdate_Convert.Pubdate(pubdate);

                                //主分区转换
                                Tid_Judge tid_judge = new Tid_Judge();
                                tidstr = tid_judge.Tid(Convert.ToInt32(tid));

                                //版权信息判断
                                Copyright_Judge copyright_judge = new Copyright_Judge();
                                copyrightstr = copyright_judge.Copyright(Convert.ToInt32(copyright));

                                //消息发送
                                botClient.SendTextMessageAsync(
                                e.Message.Chat,
                                title + "[共" + videos + "P]" + "\n" + "bv号:" + bvid + "\n" + "av号:" + av + "\n" + "UP主:" + name
                                  + "\nUP主UID:" + mid + "\n" + "-----------------\n" + "分区:" + tidstr + ":" + tname + "\n" + "点赞:" + like + "  投币:"
                                  + coin + "\n收藏:" + favorite + "  观看:" + view + "\n弹幕:" + danmaku + "  评论:" + reply + "\n分享:" + share +
                                  "\n-----------------\n" + "简介:" + desc);

                                //图片发送
                                botClient.SendPhotoAsync(e.Message.Chat, pic, "视频封面", ParseMode.Html);
                            }
                            else
                            {
                                //发送错误详情
                                botClient.SendTextMessageAsync(e.Message.Chat, "错误:\n" + "Code:" + _Jsonobj.code + "\n" + _Jsonobj.Message + "\n" + "错误码:\n400为请求错误\n404为找不到稿件\n62002为稿件不可见");
                            }
                        }
                        else if (Msg.Substring(0, 2) == "bv")
                        {
                            //HttpGet_Json
                            BiliBili_HttpGet_BV _HttpGet = new BiliBili_HttpGet_BV();
                            Json = _HttpGet.HttpGet(aid = Msg.Substring(2, Msg.Length - 2));
                            var _Jsonobj = JsonConvert.DeserializeObject<dynamic>(Json);
                            if (_Jsonobj.code == 0)
                            {
                                //Json -> 变量
                                //data.
                                pic = _Jsonobj.data.pic;
                                bvid = _Jsonobj.data.bvid;  //bv号
                                av = "av" + _Jsonobj.data.aid; //av号
                                title = _Jsonobj.data.title;  //标题
                                videos = _Jsonobj.data.videos;  //分P数量
                                tid = _Jsonobj.data.tid;  //主分区
                                tname = _Jsonobj.data.tname;  //子分区
                                copyright = _Jsonobj.data.copyright;  //版权信息
                                pubdate = _Jsonobj.data.pubdate;  //投稿时间(时间戳
                                desc = _Jsonobj.data.desc;  //简介
                                duration = _Jsonobj.data.duration;  //视频持续时长(所有分P
                                //data.owner.
                                mid = _Jsonobj.data.owner.mid;  //up主UID
                                name = _Jsonobj.data.owner.name;  //up主昵称
                                face = _Jsonobj.data.owner.face;  //up主头像地址
                                //data.stat.
                                view = _Jsonobj.data.stat.view;  //观看数量
                                danmaku = _Jsonobj.data.stat.danmaku;  //弹幕数量
                                reply = _Jsonobj.data.stat.reply;  //评论数量
                                favorite = _Jsonobj.data.stat.favorite;  //收藏数量
                                coin = _Jsonobj.data.stat.coin;  //投币数量
                                share = _Jsonobj.data.stat.share;  //分享数量
                                like = _Jsonobj.data.stat.like;  //获赞数量

                                //时间戳转换
                                Pubdate_Convert pubdate_Convert = new Pubdate_Convert();
                                datetime = pubdate_Convert.Pubdate(pubdate);

                                //主分区转换
                                Tid_Judge tid_judge = new Tid_Judge();
                                tidstr = tid_judge.Tid(Convert.ToInt32(tid));

                                //版权信息判断
                                Copyright_Judge copyright_judge = new Copyright_Judge();
                                copyrightstr = copyright_judge.Copyright(Convert.ToInt32(copyright));

                                //消息发送
                                botClient.SendTextMessageAsync(
                                e.Message.Chat,
                                title + "[共" + videos + "P]" + "\n" + "bv号:" + bvid + "\n" + "av号:" + av + "\n" + "UP主:" + name
                                  + "\nUP主UID:" + mid + "\n" + "-----------------\n" + "分区:" + tidstr + ":" + tname + "\n" + "点赞:" + like + "  投币:"
                                  + coin + "\n收藏:" + favorite + "  观看:" + view + "\n弹幕:" + danmaku + "  评论:" + reply + "\n分享:" + share +
                                  "\n-----------------\n" + "简介:" + desc);

                                //图片发送
                                botClient.SendPhotoAsync(e.Message.Chat, pic, "视频封面", ParseMode.Html);
                            }
                            else
                            {
                                //发送错误详情
                                botClient.SendTextMessageAsync(e.Message.Chat, "错误:\n" + "Code:" + _Jsonobj.code + "\n" + _Jsonobj.Message + "\n" + "错误码:\n400为请求错误\n404为找不到稿件\n62002为稿件不可见");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n------------------------\n" + ex.ToString() + "\n------------------------\n");
            }
        }

        static void BotClient_Hitokoto(object sender,MessageEventArgs e)
        {
            try
            {
                //判断不为空
                if (e.Message.Text != null)
                {
                    //控制台输出
                    Console.WriteLine($"[信息]: 来自{e.Message.Chat.Id},消息:{e.Message.Text}.");
                    //判断关键词
                    string Msg = e.Message.Text;
                    if (Msg.Length >= 2)
                    {
                        if (Msg.Length ==2 && Msg.Substring(0, 2) == "一言")
                        {
                            //HttpGet
                            Hitokoto_HttpGet _HttpGet = new Hitokoto_HttpGet();
                            string Rtn = _HttpGet.HttpGet();
                            var Json = JsonConvert.DeserializeObject<dynamic>(Rtn);
                            string Hitokoto = Json.msg;
                            botClient.SendTextMessageAsync(e.Message.Chat, Hitokoto);
                        }
                        else if (Msg.Length == 2 && Msg.Substring(0, 2) == "动漫")
                        {
                            Anime_HttpGet _HttpGet = new Anime_HttpGet();
                            string Rtn = _HttpGet.HttpGet();
                            var Json = JsonConvert.DeserializeObject<dynamic>(Rtn);
                            string Anime = Json.msg;
                            botClient.SendTextMessageAsync(e.Message.Chat, Anime);
                        }
                        else if (Msg.Length == 2 && Msg.Substring(0, 2) == "表情")
                        {
                            Emoji_HttpGet _HttpGet = new Emoji_HttpGet();
                            string Rtn = _HttpGet.HttpGet();
                            var Json = JsonConvert.DeserializeObject<dynamic>(Rtn);
                            string Emoji = Json.msg;
                            botClient.SendTextMessageAsync(e.Message.Chat, Emoji);
                        }
                        else if (Msg.Length == 2 && Msg.Substring(0, 2) == "帮助")
                        {
                            string Help = "一言-帮助:\n" +
                                "发送下列命令可获得返回\n" +
                                "一言/动漫/表情/为什么/小提示\n" +
                                "来源接口:tosks.com";
                            botClient.SendTextMessageAsync(e.Message.Chat, Help);
                        }
                        else if (Msg.Length == 3 && Msg.Substring(0, 3) == "为什么")
                        {
                            Whys_HttpGet _HttpGet = new Whys_HttpGet();
                            string Rtn = _HttpGet.HttpGet();
                            var Json = JsonConvert.DeserializeObject<dynamic>(Rtn);
                            string Whys = Json.msg;
                            botClient.SendTextMessageAsync(e.Message.Chat, Whys);
                        }
                        else if (Msg.Length == 3 && Msg.Substring(0, 3) == "小提示")
                        {
                            Tips_HttpGet _HttpGet = new Tips_HttpGet();
                            string Rtn = _HttpGet.HttpGet();
                            var Json = JsonConvert.DeserializeObject<dynamic>(Rtn);
                            string Tips = Json.msg;
                            botClient.SendTextMessageAsync(e.Message.Chat, Tips);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n------------------------\n" + ex.ToString() + "\n------------------------\n");
            }
        }
    }
}
