using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hanaya_TgBot_Nogui.tokenProtect
{
    public class Encryption
    {
        static string encryptKey = "rcnb";
        public string Encrypt(string str)
        {
            //开始加密字符串
            try
            {
                //密钥
                byte[] key = Encoding.Unicode.GetBytes(encryptKey);
                //待加密字符串
                byte[] data = Encoding.Unicode.GetBytes(str);
                //加密、解密对象
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
                //内存流对象
                MemoryStream MStream = new MemoryStream();
                //用内存流实例化加密流对象
                CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
                //向加密流中写入数据
                CStream.Write(data, 0, data.Length);
                //将数据压入基础流
                CStream.FlushFinalBlock();
                //从内存流中获取字节序列
                byte[] temp = MStream.ToArray();
                //关闭加密流
                CStream.Close();
                //关闭内存流
                MStream.Close();
                //返回加密后的字符串
                return Convert.ToBase64String(temp);
            }
            catch
            {
                string err = "Error";
                return err;
            }
        }
    }
}
