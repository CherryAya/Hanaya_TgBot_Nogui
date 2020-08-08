using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Hanaya_TgBot_Nogui.tokenProtect
{
    public class Decryption
    {
        static string encryptKey = "rcnb";
        public string Decrypt(string str)
        {
            //开始解密字符串
            try
            {
                //密钥
                byte[] key = Encoding.Unicode.GetBytes(encryptKey);
                //待解密字符串
                byte[] data = Convert.FromBase64String(str);
                //加密、解密对象
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
                //内存流对象
                MemoryStream MStream = new MemoryStream();
                //用内存流实例化解密流对象
                CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
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
                //返回解密后的字符串
                return Encoding.Unicode.GetString(temp);
            }
            catch
            {
                string err = "Error";
                return err;
            }
        }
    }
}
