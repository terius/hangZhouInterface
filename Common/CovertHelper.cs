using System;

namespace Common
{
    public class ConvertHelper
    {
        public static string ToDateTimeStr(object ob, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (ob == null)
            {
                return "";
            }
            //   DateTime dtTemp = DateTime.Now;
            if (DateTime.TryParse(ob.ToString(), out DateTime dtTemp))
            {
                return dtTemp.ToString(format);
            }
            return "";
        }
    }
}
