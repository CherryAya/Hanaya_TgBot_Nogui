using System;

namespace Hanaya_TgBot_Nogui
{
    public class Pubdate_Convert
    {
        public string Pubdate(string pubdate)
        {
            Int64 begtime = Convert.ToInt64(pubdate) * 10000000;
            DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0);
            long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
            long time_tricks = tricks_1970 + begtime;//日志日期刻度
            DateTime dt = new DateTime(time_tricks);//转化为DateTime
            return dt.ToString();
        }
    }
}
