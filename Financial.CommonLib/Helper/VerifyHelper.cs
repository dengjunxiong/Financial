using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Financial.CommonLib
{
    /// <summary>
    /// 数据验证
    /// </summary>
    public class VerifyHelper
    {
        /// <summary>
        /// 验证对象是否为Int32类型的数字
        /// </summary>
        /// <param name="obj">要验证的对象</param>
        /// <returns>是否为Int32类型的数字</returns>
        public static bool IsNumeric(object obj)
        {
            if (obj != null)
            {
                return IsNumeric(obj.ToString());
            }
            return false;
        }

        /// <summary>
        /// 判断字符串是否为Int32类型的数字
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为Int32类型的数字</returns>
        public static bool IsNumeric(string str)
        {
            if (str != null)
            {
                try
                {
                    Convert.ToInt32(str);
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }

        /// <summary>
        /// 验证数据库字段值是否为空
        /// </summary>
        /// <param name="obj">要验证的字段值</param>
        /// <returns>是否为空</returns>
        public static bool IsNullByDB(object obj)
        {
            if (Convert.IsDBNull(obj) || IsNull(obj))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证对象是否为空
        /// </summary>
        /// <param name="obj">要验证的对象</param>
        /// <returns>是否为空</returns>
        public static bool IsNull(object obj)
        {
            if (obj == null || obj.ToString().Length == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证字符串是否为空
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为空</returns>
        public static bool IsNull(string str)
        {
            if (str == null || str.Length == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证字符串是否为空,如果是空将null转为空字符串
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>如果不是空值,返回字符串本身;如果是空值返回空字符串</returns>
        public static string CheckNull(object obj)
        {
            if (IsNull(obj))
            {
                return "";
            }
            return obj.ToString();
        }

        /// <summary>
        /// 验证字符串是否为空,如果是空将null转为空字符串
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>如果不是空值,返回字符串本身;如果是空值返回空字符串</returns>
        public static string CheckNull(string str)
        {
            if (IsNull(str))
            {
                return "";
            }
            return str;
        }

        /// <summary>
        /// 验证字符串是否由数字组成
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否是数字</returns>
        public static bool CheckNumber(string str)
        {
            Regex Re = new Regex(@"^[0-9]+$");
            return Re.IsMatch(str);
        }

        /// <summary>
        /// 验证数字字符串是否符合规则
        /// </summary>
        /// <param name="str">数字字符串</param>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        /// <returns>str是否大于或等于minNum且小于或等于maxNum</returns>
        public static bool CheckNumber(string str, int minNum, int maxNum)
        {
            int temp = ConvertHelper.StrToInt(str, 0);
            return temp >= minNum && temp <= maxNum;
        }

        /// <summary>
        /// 验证字符串是否包含规定字数的字母、数字
        /// </summary>
        /// <param name="minCount">最少字数(包含)</param>
        /// <param name="maxCount">最多字数(包含)</param>
        /// <param name="existsABCAndNum">是否必须包含字符和数字(true:字母和数字必须同时存在;false:字母或数字组成)</param>
        /// <param name="isNum">是否可以是纯数字(true:可以是纯数字;false:不能是纯数字)</param>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否包含规定字数的字母、数字</returns>
        public static bool CheckField(int minCount, int maxCount, bool existsABCAndNum, bool isNum, string str)
        {
            if ((minCount > 0 && str.Length < minCount) || (maxCount > 0 && str.Length > maxCount))
            {
                return false;
            }

            Regex Re = null;
            if (isNum)
            {
                Re = new Regex(@"^[0-9]+$");
                if (Re.IsMatch(str))
                {
                    return true;
                }
            }
            if (existsABCAndNum)
            {
                Re = new Regex(@"^[A-Za-z].*[0-9]|[0-9].*[A-Za-z]$");
                return Re.IsMatch(str);
            }
            Re = new Regex(@"^[A-Za-z0-9]+$");
            return Re.IsMatch(str);
        }

        /// <summary>
        /// 验证字符串是否符合规则(6~20个字符,区分大小写,无空格)
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>-1:长度不符;-2:包含空格;-3:过于简单;-4:包含数字、字母和符号之外的字符;0:符合规则</returns>
        public static int CheckSitePassword(string str)
        {
            //长度
            if (str.Length < 6 || str.Length > 20)
            {
                return -1;
            }
            //是否包含空格
            Regex re = new Regex(@"^\s+$");
            if (re.IsMatch(str))
            {
                return -2;
            }
            string[] list = { "123456", "123456789", "12345678", "123123", "5201314", "1234567", "7758521", "654321", "1314520", "123321", "1234567890", "147258369", "123654", "5211314", "woaini", "1230123", "987654321", "147258", "123123123", "7758258", "520520", "789456", "456789", "159357", "112233", "1314521", "456123", "110110", "521521", "zxcvbnm", "789456123", "0123456789", "0123456", "123465", "159753", "qwertyuiop", "987654", "115415", "1234560", "123000", "123789", "100200", "963852741", "121212", "111222", "123654789", "12301230", "456456", "741852963", "asdasd", "asdfghjkl", "369258", "863786", "258369", "8718693", "666888", "5845201314", "741852", "168168", "iloveyou", "852963", "4655321", "102030", "147852369", "321321" };
            int i = 0;
            //是否过于简单
            for (; i < list.Length; i++)
            {
                if (str == list[i])
                {
                    return -3;
                }
            }
            //是否符合密码规则
            string symbol = " 　`｀~～!！@·#＃$￥%％^…&＆()（）-－_—=＝+＋[]［］|·:：;；\"“\\、'‘,，<>〈〉?？/／*＊.。{}｛｝";
            char charCode = ' ';
            char[] charList = str.ToCharArray();
            for (i = 0; i < str.Length; i++)
            {
                charCode = charList[i];
                if (charCode >= 48 && charCode <= 57)//数字
                {
                    continue;
                }
                if (charCode >= 65 && charCode <= 90)//大写字母
                {
                    continue;
                }
                if (charCode >= 97 && charCode <= 122)//小写字母
                {
                    continue;
                }
                if (symbol.IndexOf(charCode) != -1)
                {
                    continue;
                }
                return -4;
            }
            return 0;
        }

        /// <summary>
        /// 验证是否为ip
        /// </summary>
        /// <param name="ip">要验证的字符串</param>
        /// <returns>是否为ip</returns>
        public static bool CheckIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 验证是否为身份证号码
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为身份证号码</returns>
        public static bool CheckIDCard(string str)
        {
            //检验是否由数字组成
            Regex Re = new Regex(@"^\d{15}|\d{18}$");
            if (Re.IsMatch(str) == false)
            {
                return false;
            }

            string temp = null;//临时字符串
            int MonthNum = 0;//月份数字
            int dayNum = 0;//日期数字
            //根据不同位数进行检查
            switch (str.Length)
            {
                case 15:
                    //检验月份（第九位和第十位）
                    temp = str.Substring(8, 2);
                    MonthNum = Convert.ToInt32(temp);
                    if (MonthNum < 1 || MonthNum > 12)
                    {
                        return false;
                    }

                    //检验日期格式
                    temp = str.Substring(10, 2);
                    dayNum = Convert.ToInt32(temp);
                    if (dayNum < 1 || dayNum > 31)
                    {
                        return false;
                    }
                    switch (MonthNum)
                    {
                        case 1:
                            return true;
                        case 2:
                            if (dayNum <= 29)
                            {
                                return true;
                            }
                            break;
                        case 3:
                            return true;
                        case 4:
                            if (dayNum <= 30)
                            {
                                return true;
                            }
                            break;
                        case 5:
                            return true;
                        case 6:
                            if (dayNum <= 30)
                            {
                                return true;
                            }
                            break;
                        case 7:
                            return true;
                        case 8:
                            return true;
                        case 9:
                            if (dayNum <= 30)
                            {
                                return true;
                            }
                            break;
                        case 10:
                            return true;
                        case 11:
                            if (dayNum <= 30)
                            {
                                return true;
                            }
                            break;
                        case 12:
                            return true;
                        default:
                            break;
                    }
                    break;
                default:
                    temp = str.Substring(17, 1).ToLower();
                    char[] arraySubStr = temp.ToCharArray();
                    char eightenCode = arraySubStr[0];
                    char VerifyCode = doVerify(str);
                    if (VerifyCode == eightenCode)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// 计算身份证校验位
        /// </summary>
        /// <param name="ID_Card">要验证的字符串</param>
        /// <returns>校验位的校验码</returns>
        public static char doVerify(string ID_Card)
        {
            char[] arrayID_Card = ID_Card.ToCharArray();
            int iS = 0;
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            char[] szVerCode = new char[] { '1', '0', 'x', '9', '8', '7', '6', '5', '4', '3', '2' };
            int i;
            for (i = 0; i < 17; i++)
            {
                iS += (int)(arrayID_Card[i] - '0') * iW[i];
            }
            int iY = iS % 11;
            return szVerCode[iY];
        }

        /// <summary>
        /// 验证是否为手机号码
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为手机号码</returns>
        public static bool CheckMobile(string str)
        {
            Regex Re = new Regex(@"^(13[0-9]|14[57]|15[012356789]|18[012356789])[0-9]{8}$");
            return Re.IsMatch(str);
        }

        /// <summary>
        /// 验证是否为电话号码
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为电话号码</returns>
        public static bool CheckPhone(string str)
        {
            Regex Re = new Regex(@"^(400|800)([0-9\\-]{7,10})$");
            if (Re.IsMatch(str))
            {
                return true;
            }
            Re = new Regex(@"^(\d{4}-|\d{3}-)?(\d{8}|\d{7})?$");
            if (Re.IsMatch(str))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证是否为传真号码
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为传真号码</returns>
        public static bool CheckFax(string str)
        {
            Regex Re = new Regex(@"^(400|800)([0-9\\-]{7,10})$");
            if (Re.IsMatch(str))
            {
                return true;
            }
            Re = new Regex(@"^(\d{4}-|\d{3}-)?(\d{8}|\d{7})?$");
            if (Re.IsMatch(str))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证是否为电子邮件地址
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为电子邮件地址</returns>
        public static bool CheckEmail(string str)
        {
            Regex Re = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return Re.IsMatch(str);
        }

        /// <summary>
        /// 验证是否为QQ号码
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为QQ号码</returns>
        public static bool CheckQQ(string str)
        {
            Regex Re = new Regex(@"^[1-9]{1}[0-9]{4,}$");
            return Re.IsMatch(str);
        }

        /// <summary>
        /// 验证是否为邮编
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否为邮编</returns>
        public static bool CheckPostalCode(string str)
        {
            Regex Re = new Regex(@"^\d{6}$");
            return Re.IsMatch(str);
        }

        /// <summary>
        /// 验证是否由英文字母组成
        /// </summary>
        /// <param name="str">字符串</param >
        /// <returns>是否是汉字</returns >
        public static bool CheckABC(string str)
        {
            Regex regex = new Regex("[A-Za-z]+$");
            Match m = regex.Match(str);
            return m.Success;
        }

        /// <summary>
        /// 验证是否存在汉字
        /// </summary>
        /// <param name="str">字符串</param >
        /// <returns>是否是汉字</returns >
        public static bool CheckChinese(string str)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+$");
            Match m = regex.Match(str);
            return m.Success;
        }

        /// <summary>
        /// 检查集合中是否存在某个键
        /// 存在,则返回对应的值;不存在,则返回默认值
        /// </summary>
        /// <typeparam name="TKey">键</typeparam>
        /// <typeparam name="TValue">值</typeparam>
        /// <param name="list">集合</param>
        /// <param name="key">要检查的键</param>
        /// <returns>值</returns>
        public static TValue CheckKey<TKey, TValue>(IDictionary<TKey, TValue> list, TKey key)
        {
            if (list.Keys.Contains(key))
            {
                return list[key];
            }
            return default(TValue);
        }
    }
}