using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Financial.CommonLib
{
    /// <summary>
    /// 数据转换
    /// </summary>
    public class ConvertHelper
    {
        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str, string separator)
        {
            string[] result = null;
            try
            {
                result = Regex.Split(str, separator, RegexOptions.IgnoreCase);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">数组</param>
        /// <param name="speater">分隔符</param>
        /// <returns>字符串</returns>
        public static string GetArrayStr(IList<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                    continue;
                }
                sb.Append(list[i]);
                sb.Append(speater);
            }
            return sb.ToString();
        }

        /// <summary>
        /// object型转换为bool型(缺省值为false)
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ObjToBool(object expression)
        {
            return ObjToBool(expression, false);
        }

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ObjToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                return StrToBool(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null && expression.Length > 0)
            {
                switch (expression.ToLower())
                {
                    case "true":
                        return true;
                    default:
                        return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将字符串转换为Int16类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的short类型结果</returns>
        public static short StrToShort(string expression, short defValue)
        {
            if (expression != null && expression.Length > 0)
            {
                try
                {
                    return Convert.ToInt16(expression);
                }
                catch
                {
                    return defValue;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int16类型(缺省值为0)
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的short类型结果</returns>
        public static short ObjToShort(object expression)
        {
            return ObjToShort(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int16类型(缺省值为0)
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的short类型结果</returns>
        public static short ObjToShort(object expression, short defValue)
        {
            if (expression != null && expression != DBNull.Value)
            {
                string temp = expression.ToString();
                if (temp.Length > 0)
                {
                    try
                    {
                        return Convert.ToInt16(temp);
                    }
                    catch
                    {
                        return defValue;
                    }
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Long类型(缺省值为0)
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的Long类型结果</returns>
        public static short ObjToLong(object expression)
        {
            return ObjToLong(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Long类型(缺省值为0)
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Long类型结果</returns>
        public static short ObjToLong(object expression, short defValue)
        {
            if (expression != null && expression != DBNull.Value)
            {
                string temp = expression.ToString();
                if (temp.Length > 0)
                {
                    try
                    {
                        return Convert.ToInt16(temp);
                    }
                    catch
                    {
                        return defValue;
                    }
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型(缺省值为0)
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression)
        {
            if (expression == null)
            {
                return 0;
            }
            return ObjToInt(expression.ToString(), 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                return StrToInt(expression.ToString(), defValue);
            }
            return 0;
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (expression != null && expression.Length > 0)
            {
                try
                {
                    return Convert.ToInt32(expression);
                }
                catch
                {
                    return defValue;
                }
            }
            return defValue;
        }

        /// <summary>
        /// Object型转换为float型(缺省值为0)
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object expression)
        {
            return ObjToFloat(expression.ToString(), 0);
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object expression, float defValue)
        {
            if (expression != null)
            {
                return StrToFloat(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string expression, float defValue)
        {
            if (expression != null && expression.Length > 0)
            {
                try
                {
                    return float.Parse(expression);
                }
                catch
                {
                    return defValue;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为double型(缺省值为0)
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的double类型结果</returns>
        public static double ObjToDouble(object expression)
        {
            if (expression == null)
            {
                return 0;
            }
            return ObjToDouble(expression.ToString(), 0);
        }

        /// <summary>
        /// 将对象转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的double类型结果</returns>
        public static double ObjToDouble(object expression, double defValue)
        {
            if (expression != null)
            {
                return StrToDouble(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为Double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static double StrToDouble(string expression, double defValue)
        {
            if (expression != null && expression.Length > 0)
            {
                try
                {
                    return Convert.ToDouble(expression);
                }
                catch
                {
                    return defValue;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型(缺省值为DateTime.Now)
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime ObjectToDateTime(object expression)
        {
            if (expression == null)
            {
                return DateTime.Now;
            }
            return ObjToDateTime(expression.ToString(), DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime ObjToDateTime(object expression)
        {
            if (expression == null)
            {
                return DateTime.Now;
            }
            return ObjToDateTime(expression.ToString(), DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime ObjToDateTime(object expression, DateTime defValue)
        {
            if (expression == null)
            {
                return defValue;
            }
            return StrToDateTime(expression.ToString(), defValue);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (str != null && str.Length > 0)
            {
                DateTime temp = DateTime.MinValue;
                if (DateTime.TryParse(str, out temp))
                {
                    return temp;
                }
                string farmat = "dd/MM/yy HH:mm:ss";
                if (DateTime.TryParseExact(str, farmat, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out temp))
                {
                    return temp;
                }
                return defValue;
            }
            return defValue;
        }

        /// <summary>  
        /// 将DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>Unix时间戳格式</returns>  
        public static long GetTimeStampByDateTime(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long result = Convert.ToInt64((time - startTime).TotalSeconds);
            return result;
        }

        /// <summary>
        /// 将对象转换为字符串(缺省值为"")
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的string类型结果</returns>
        public static string ObjectToStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString().Trim();
        }

        /// <summary>
        /// 将秒转化时间格式(HH:mm:ss)
        /// </summary>
        /// <param name="second">秒</param>
        /// <returns>转化后的时间</returns>
        public static string SecondToTime(int second)
        {
            int hour = second / 60 / 24;
            int minute = (second - hour * 24 * 60) / 60;
            second = second - hour * 24 * 60 - minute * 60;
            StringBuilder result = new StringBuilder();
            if (hour > 0)
            {
                result.Append(string.Format("{0}{1}:", hour > 9 ? "" : "0", hour));
            }
            result.Append(string.Format("{0}{1}:", minute > 9 ? "" : "0", minute));
            result.Append(string.Format("{0}{1}", second > 9 ? "" : "0", second));
            return result.ToString();
        }

        /// <summary>
        /// 数字转为大写(万以内)
        /// </summary>
        /// <param name="isJiaoFen"></param>
        /// <returns>大写数字</returns>
        public static string NumToCH(int num)
        {
            string[] list = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            if (num < 10)
            {
                return list[num];
            }
            int[] number = new int[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };
            string[] unit = new string[] { "", "十", "百", "千", "万", "十万", "百万", "千万", "亿" };
            int temp = 0;
            int j = 0;
            StringBuilder result = new StringBuilder();
            for (int i = unit.Length - 1; i > -1; i--)
            {
                if (num < 10)
                {
                    result.Append(list[num]);
                    break;
                }
                temp = num % number[i];
                if (temp < number[i])
                {
                    continue;
                }
                if (temp == 0)
                {
                    result.Append(list[num / number[i]] + unit[i]);
                    num -= num / number[i] * number[i];
                    continue;
                }
                if (temp > 0)
                {
                    for (j = 0; j < 10; j++)
                    {
                        if (j == temp)
                        {
                            result.Append(list[j]);
                            break;
                        }
                    }
                    result.Append(unit[i]);
                    num -= num / number[i] * number[i];
                }
            }
            string r = result.ToString();
            if (r.LastIndexOf("零") == r.Length - 1)
            {
                r.Remove(r.Length - 1, 1);
            }
            if (r.IndexOf("一十") == 0)
            {
                r.Remove(0, 1);
            }
            return r;
        }
    }
}
