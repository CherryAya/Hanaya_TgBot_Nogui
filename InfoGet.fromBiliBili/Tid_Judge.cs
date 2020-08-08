using System;

namespace Hanaya_TgBot_Nogui
{
    public class Tid_Judge
    {
        public string Tid(int tid)
        {
            string str = null;
            if (tid == 0 || tid == 3)
            {
                str = "音乐选集";
            }
            else if (tid == 1)
            {
                str = "短片·手书·配音";
            }
            else if (tid == 4)
            {
                str = "GMV";
            }
            else if (tid == 5)
            {
                str = "综艺";
            }
            else if (tid == 11)
            {
                str = "电视剧相关";
            }
            else if (tid == 13)
            {
                str = "官方延伸";
            }
            else if (tid == 17)
            {
                str = "实况解说";
            }
            else if (tid == 19)
            {
                str = "Mugen";
            }
            else if (tid == 20)
            {
                str = "宅舞";
            }
            else if (tid == 22)
            {
                str = "鬼畜调教";
            }
            else if (tid == 23)
            {
                str = "电影相关";
            }
            else if (tid == 21)
            {
                str = "日常";
            }
            else if (tid == 24)
            {
                str = "MAD·AMV";
            }
            else if (tid == 26)
            {
                str = "音MAD";
            }
            else if (tid == 27)
            {
                str = "动画综合";
            }
            else if (tid == 29)
            {
                str = "三次元音乐";
            }
            else if (tid == 30 || tid == 58)
            {
                str = "Vocaloid中文曲";
            }
            else if (tid == 31)
            {
                str = "翻唱";
            }
            else if (tid == 36)
            {
                str = "演讲·公开课";
            }
            else if (tid == 39)
            {
                str = "公开课";
            }
            else if (tid == 40)
            {
                str = "技术宅";
            }
            else if (tid == 43)
            {
                str = "舞蹈MMD";
            }
            else if (tid == 44 || tid == 25)
            {
                str = "剧情MMD";
            }
            else if (tid == 45)
            {
                str = "原创模型";
            }
            else if (tid == 46)
            {
                str = "MMD·3D";
            }
            else if (tid == 48)
            {
                str = "原创动画";
            }
            else if (tid == 49)
            {
                str = "ACG配音";
            }
            else if (tid == 50 || tid == 47)
            {
                str = "手书";
            }
            else if (tid == 51)
            {
                str = "资讯";
            }
            else if (tid == 53)
            {
                str = "动画综合";
            }
            else if (tid == 54)
            {
                str = "OP/ED/OST";
            }
            else if (tid == 56)
            {
                str = "Vocaloid";
            }
            else if (tid == 57)
            {
                str = "UTAU";
            }
            else if (tid == 59)
            {
                str = "演奏";
            }
            else if (tid == 61)
            {
                str = "预告资讯";
            }
            else if (tid == 63)
            {
                str = "实况解说";
            }
            else if (tid == 64)
            {
                str = "游戏杂谈";
            }
            else if (tid == 66)
            {
                str = "游戏集锦";
            }
            else if (tid == 67)
            {
                str = "单机联机";
            }
            else if (tid == 72)
            {
                str = "运动";
            }
            else if (tid == 74)
            {
                str = "日常";
            }
            else if (tid == 81 || tid == 76)
            {
                str = "料理制作";
            }
            else if (tid == 82)
            {
                str = "电影相关";
            }
            else if (tid == 98)
            {
                str = "机械";
            }
            else if (tid == 104)
            {
                str = "公开课";
            }
            else if (tid == 105 || tid == 122)
            {
                str = "演示";
            }
            else if (tid == 107)
            {
                str = "科技人文";
            }
            else if (tid == 108 || tid == 124)
            {
                str = "趣味短片";
            }
            else if (tid == 119)
            {
                str = "鬼畜调教";
            }
            else if (tid == 121)
            {
                str = "GMV";
            }
            else if (tid == 126)
            {
                str = "人力Vocaloid";
            }
            else if (tid == 128 || tid == 73)
            {
                str = "影视剪辑";
            }
            else if (tid == 129)
            {
                str = "宅舞";
            }
            else if (tid == 130)
            {
                str = "音乐选集";
            }
            else if (tid == 136)
            {
                str = "音游";
            }
            else if (tid == 138)
            {
                str = "搞笑";
            }
            else if (tid == 152)
            {
                str = "官方延伸";
            }
            else if (tid == 153)
            {
                str = "国产动画";
            }
            else if (tid == 154)
            {
                str = "三次元舞蹈";
            }
            else if (tid == 160)
            {
                str = "美食圈";
            }
            else if (tid == 162)
            {
                str = "绘画";
            }
            else
            {
                str = "其他分区";
            }
            return str;
        }
    }
}
