using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Financial.CommonLib
{
    /// <summary>
    /// 字符串处理
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 获取第一个汉字的首字母
        /// </summary>
        /// <param name="str">汉字</param>
        /// <returns>首字母</returns>
        public static string GetFristABC(string str)
        {
            str = str.Substring(0, 1);
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(str);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return str;
            if (i < 0xB0C5) return "A";
            if (i < 0xB2C1) return "B";
            if (i < 0xB4EE) return "C";
            if (i < 0xB6EA) return "D";
            if (i < 0xB7A2) return "E";
            if (i < 0xB8C1) return "F";
            if (i < 0xB9FE) return "G";
            if (i < 0xBBF7) return "H";
            if (i < 0xBFA6) return "J";
            if (i < 0xC0AC) return "K";
            if (i < 0xC2E8) return "L";
            if (i < 0xC4C3) return "M";
            if (i < 0xC5B6) return "N";
            if (i < 0xC5BE) return "O";
            if (i < 0xC6DA) return "P";
            if (i < 0xC8BB) return "Q";
            if (i < 0xC8F6) return "R";
            if (i < 0xCBFA) return "S";
            if (i < 0xCDDA) return "T";
            if (i < 0xCEF4) return "W";
            if (i < 0xD1B9) return "X";
            if (i < 0xD4D1) return "Y";
            if (i < 0xD7FA) return "Z";
            return str;
        }

        /// <summary>
        /// 将字符串集合以连接符连接起来
        /// </summary>
        /// <param name="list">字符串集合</param>
        /// <param name="split">连接符</param>
        /// <param name="defaultStr">默认值(如果集合为空，则返回默认值)</param>
        /// <returns>连接字符串</returns>
        public static string JoinString(IList<string> list, string split, string defaultStr)
        {
            if (list == null || list.Count == 0)
            {
                return defaultStr;
            }
            StringBuilder result = new StringBuilder();
            foreach (string item in list)
            {
                if (item != null && item.Length > 0)
                {
                    result.Append(string.Format("{0}{1}", item, split));
                }
            }
            if (result.Length > 0 && split.Length > 0)
            {
                result.Remove(result.Length - (split.Length), split.Length);
            }
            return result.ToString();
        }

        /// <summary>
        /// 将字符串集合分割并以超链接格式连接起来
        /// </summary>
        /// <param name="list">字符串集合</param>
        /// <param name="linkFormat">超链接格式(如:<a href="localhost/{0}.html" title="{0}">{0}</a>{1},{0}为字符串集合项,{1}为连接符)</param>
        /// <param name="split">连接符</param>
        /// <param name="defaultStr">默认值(如果集合为空，则返回默认值)</param>
        /// <returns>连接字符串</returns>
        public static string JoinString(IList<string> list, string linkFormat, string split, string defaultStr)
        {
            if (list == null || list.Count == 0)
            {
                return defaultStr;
            }
            StringBuilder result = new StringBuilder();
            foreach (string item in list)
            {
                if (item != null && item.Length > 0)
                {
                    result.Append(string.Format(linkFormat, item, split));
                }
            }
            if (result.Length > 0 && split.Length > 0)
            {
                result.Remove(result.Length - (split.Length), split.Length);
            }
            return result.ToString();
        }

        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <param name="endingStr">末尾要显示的字符 比如:...或者为""</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(string inputString, int len, string endingStr)
        {
            if (VerifyHelper.IsNull(inputString))
            {
                return "";
            }
            string tempString = DropHTML(inputString);
            if (len >= tempString.Length)
            {
                return tempString;
            }

            return tempString.Substring(0, len) + endingStr;
        }

        /// <summary>
        /// 截取字符串长度
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="len">长度(最大允许汉字字数*2-3)</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(string inputString, int len)
        {
            if (VerifyHelper.IsNull(inputString))
            {
                return "";
            }
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                switch ((int)s[i])
                {
                    case 63:
                        tempLen += 2;
                        break;
                    default:
                        tempLen += 1;
                        break;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen + 1 >= len)
                {
                    break;
                }
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
            {
                tempString += "...";
            }
            return tempString;
        }

        /// <summary>
        /// 替换指定的字符串
        /// </summary>
        /// <param name="originalStr">原字符串</param>
        /// <param name="oldStr">旧字符串</param>
        /// <param name="newStr">新字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceStr(string originalStr, string oldStr, string newStr)
        {
            if (VerifyHelper.IsNull(oldStr))
            {
                return "";
            }
            return originalStr.Replace(oldStr, newStr);
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="lastStr">需要删除的字符串</param>
        /// <returns>删除后的字符串</returns>
        public static string DelLastStr(string str, string lastStr)
        {
            if (VerifyHelper.IsNull(str))
            {
                return "";
            }
            if (str.LastIndexOf(lastStr) >= 0 && str.LastIndexOf(lastStr) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(lastStr));
            }
            return str;
        }

        /// <summary>
        /// 计算字符串英文个数(英文算一个，中文算两个)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>英文个数(英文算一个，中文算两个)</returns>
        public static int StrLenByEN(string str)
        {
            int leng = 0;
            for (int i = 0; i < str.Length; i++)
            {
                byte[] b = System.Text.Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                {
                    leng += 2;
                    continue;
                }
                leng += 1;
            }
            return leng;
        }

        /// <summary>
        /// 计算字符串中文个数(英文算一个，中文算两个)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>中文个数(英文算一个，中文算两个)</returns>
        public static int StrLenByCH(string str)
        {
            int count = StrLenByEN(str);
            int leng = Convert.ToInt32(count / 2);
            if (leng * 2 < count)
            {
                leng += 1;
            }
            return leng;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <returns>分割后的字符串数组</returns>
        public static IList<string> SplitString(string str, string strSplit)
        {
            if (str != null && str.Length > 0)
            {
                if (str.IndexOf(strSplit) < 0)
                {
                    return new string[] { str };
                }
                return Regex.Split(str, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="count">获取分割后的数量</param>
        /// <returns>分割后的字符串数组</returns>
        public static IList<string> SplitString(string str, string strSplit, int count)
        {
            string[] result = new string[count];
            IList<string> splited = SplitString(str, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Count)
                {
                    result[i] = splited[i];
                    continue;
                }
                result[i] = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 将原分割符转为新分割符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strSplit">原分割符</param>
        /// <param name="split">新分割符</param>
        /// <returns>分割符转换后的字符串</returns>
        public static string SplitString(string str, string oldSplit, string newSplit)
        {
            if (VerifyHelper.IsNull(str))
            {
                return "";
            }
            StringBuilder result = new StringBuilder();
            foreach (string item in SplitString(str, oldSplit))
            {
                if (item.Length > 0)
                {
                    if (result.Length > 0)
                    {
                        result.Append(newSplit);
                    }
                    result.Append(item);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string ToUtf8(string str)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(str);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }

        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string ToUnicode(string str)
        {
            if (VerifyHelper.IsNull(str))
            {
                return str;
            }
            byte[] bts = Encoding.Unicode.GetBytes(str);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bts.Length; i += 2)
            {
                result.Append("\\u");
                result.Append(bts[i + 1].ToString("x").PadLeft(2, '0'));
                result.Append(bts[i].ToString("x").PadLeft(2, '0'));
            }
            return result.ToString();
        }

        /// <summary>
        /// 将Unicode编码转换为汉字字符串
        /// </summary>
        /// <param name="str">Unicode编码字符串</param>
        /// <returns>汉字字符串</returns>
        public static string ToGB2312(string str)
        {
            if (VerifyHelper.IsNull(str))
            {
                return str;
            }
            StringBuilder result = new StringBuilder();
            MatchCollection mc = Regex.Matches(str, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, System.Globalization.NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber);
                result.Append(Encoding.Unicode.GetString(bts));
            }
            return result.ToString();
        }

        /// <summary>
        /// 生成n个相同子字符串组成的字符串
        /// </summary>
        /// <param name="n">生成子字符串的个数</param>
        /// <param name="str">子字符串</param>
        /// <returns>生成后的字符串</returns>
        public static string StringOfChar(int n, string str)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                result.Append(str);
            }

            return result.ToString();
        }

        /// <summary>
        /// 清除HTML标记
        /// </summary>
        /// <param name="Htmlstring">html字符串</param>
        /// <returns>清除标记后的字符串</returns>
        public static string DropHTML(string Htmlstring)
        {
            if (VerifyHelper.IsNull(Htmlstring))
            {
                return "";
            }
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Trim();
            return Htmlstring;
        }

        /// <summary>
        /// 清除HTML标记后截取其一定长度
        /// </summary>
        /// <param name="Htmlstring">字符串</param>
        /// <param name="strLen">长度</param>
        /// <returns>清除HTML标记后的字符串</returns>
        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }

        /// <summary>
        /// 去除干扰json的数据使用
        /// 把字符串中的英文双引号替换成中文
        /// 把字符串中的"\"替换成"\\"
        /// </summary>
        /// <param name="str">替换前字符串</param>
        /// <returns>返回替换后的字符串</returns>
        public static string ReplaceByJson(string str)
        {
            if (VerifyHelper.IsNull(str))
            {
                return "";
            }
            str = str.Replace("\r", "\\r");
            str = str.Replace("\t", "\\t");
            str = str.Replace("\n", "\\n");
            if (str.IndexOf("\\") != -1)
            {
                str = str.Replace("\\", "\\\\");
            }
            if (str.IndexOf("\"") == -1)
            {
                return str;
            }
            //把字符串按照双引号截成数组
            IList<string> list = SplitString(str, "\"");
            //替换后的字符串
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Length > 0)
                {
                    result.Append("“" + list[i] + "”");
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 把TXT代码转换成HTML格式
        /// </summary>
        /// <param name="input">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static String ToHtml(string input)
        {
            StringBuilder sb = new StringBuilder(input);
            sb.Replace("\"", "&quot;");
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }

        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="input">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToTxt(String input)
        {
            StringBuilder sb = new StringBuilder(input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="input">需要过滤的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FilteHtmls(string input)
        {
            if (!VerifyHelper.IsNull(input))
            {
                string ihtml = input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查Sql危险字符，如果有sql危险字符，则过滤
        /// </summary>
        /// <param name="Input">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static string FilteSqlStr(string sInput)
        {
            if (VerifyHelper.IsNull(sInput))
            {
                return "";
            }
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                Regex r = new Regex(@"(update )|(drop )|(delete )|(exec )|(create )|(execute )|(<[\s]*?script[\S\s]*?>[^>]*?[>])|([<][\s]*?[%][^>]+?[>])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                output = r.Replace(output, "");
            }
            output = output.Replace("'", "''");
            return output;
        }

        /// <summary>
        /// 过滤SQL危险字符以及script
        /// </summary>
        /// <param name="Input">要判断字符串</param>
        /// <returns>过滤后结果</returns>
        public static string FilteSqlScript(string sInput)
        {
            if (VerifyHelper.IsNull(sInput))
            {
                return "";
            }
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                Regex r = new Regex(@"(update )|(drop )|(delete )|(exec )|(create )|(execute )|(<[\s]*?script[\S\s]*?>[^>]*?[>]))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                output = r.Replace(output, "");
            }
            output = output.Replace("'", "''");
            return output;
        }

        /// <summary>
        /// 过滤Sql注入，html 编辑器的恶意代码注入
        /// </summary>
        /// <param name="input">需要过滤的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FilteSqlHtmls(string input)
        {
            if (VerifyHelper.IsNull(input))
            {
                return "";
            }

            input = input.Replace("'", "");

            Regex r = new Regex(@"(update )|(drop )|(delete )|(exec )|(create )|(execute )|(<[\s]*?script[\S\s]*?>[^>]*?[>])|([<][\s]*?[%][^>]+?[>])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            input = r.Replace(input, "");

            return input;
        }

        /// <summary>
        /// 检测是否有危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary> 
        /// <param name="InText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool ExistsFilter(string word, string InText)
        {
            if (InText == null)
            {
                return false;
            }
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 生成日期随机码
        /// </summary>
        /// <returns>日期随机码</returns>
        public static string GetRamCode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// 生成数字随机码
        /// </summary>
        /// <param name="count">个数</param>
        /// <returns>数字随机码</returns>
        public static string RandomNumber(int count)
        {
            string s = "";
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                s = String.Concat(s, random.Next(10).ToString());
            }
            return s;
        }

        /// <summary>
        /// 生成数字+字母随机码
        /// </summary>
        /// <param name="count">个数</param>
        /// <returns>数字+字母随机码</returns>
        public static string GetRandomCode(int count)
        {
            string Key = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string s = "";
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                s = String.Concat(s, Key[random.Next(Key.Length)].ToString());
            }
            return s;
        }
    }
}
