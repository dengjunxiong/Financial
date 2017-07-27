using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace Financial.CommonLib
{
    /// <summary>
    /// Json数据处理
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 一级json处理
        /// </summary>
        /// <param name="str">json字符串(一级json,只有{},没有[])</param>
        /// <returns>json数据(键值对)</returns>
        public static IDictionary<string, string> GetJsonByStr(string str)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            str = Regex.Replace(str, "{|}|\\\\", "", RegexOptions.IgnoreCase);
            IList<string> list = StringHelper.SplitString(str, ",\"");
            string key = null;
            string val = null;
            string temp = null;
            foreach (string item in list)
            {
                if (VerifyHelper.IsNull(item))
                {
                    continue;
                }
                temp = item.Replace("\"", "");
                key = temp.Substring(0, temp.IndexOf(":"));
                val = temp.Substring(temp.IndexOf(":"));
                if (val == ":")
                {
                    val = "";
                    result.Add(key, val);
                    continue;
                }
                val = val.Remove(0, 1);
                result.Add(key, val);
            }
            return result;
        }

        /// <summary>
        /// 将对象序列化为JSON格式字符串
        /// </summary>
        /// <param name="obj">要转为json的对象实例</param>
        /// <returns>json字符串</returns>
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 解析JSON字符串为对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实例</returns>
        public static T ToObject<T>(string json) where T : class
        {
            StringReader sr = new StringReader(json);
            object obj = new JsonSerializer().Deserialize(new JsonTextReader(sr), typeof(T));
            return obj as T;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousType">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T ToAnonymousType<T>(string json, T anonymousType) where T : class
        {
            return JsonConvert.DeserializeAnonymousType(json, anonymousType);
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> ToObjectList<T>(string json) where T : class
        {
            StringReader sr = new StringReader(json);
            object obj = new JsonSerializer().Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = obj as List<T>;
            return list;
        }
    }
}