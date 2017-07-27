using Financial.CommonLib.FileSys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Financial.CommonLib
{
    /// <summary>
    /// HTTP
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 判断当前访问是否来自浏览器软件(内核为ie、chrome、firefox、safari等)
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "chrome", "firefox", "safari", "netscape", "konqueror", "opera", "mozilla" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return false;
            }
            string[] SearchEngine = { "baidu", "google", "sogou", "soso", "msn", "sina", "163", "gougou", "sohu", "yahoo", "lycos", "tom", "yisou", "iask", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="key">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string key)
        {
            string value = HttpContext.Current.Request.ServerVariables[key];
            if (VerifyHelper.IsNull(value))
            {
                return "";
            }
            return value;
        }

        /// <summary>
        /// 得到当前主机头
        /// 例:假设当前地址为:http://www.test.com:8080/default.aspx ,获取到的结果是:www.test.com
        /// </summary>
        /// <returns>主机头</returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 得到当前完整主机头
        /// 例:假设当前地址为:http://www.test.com:8080/default.aspx ,获取到的结果是:www.test.com:8080
        /// </summary>
        /// <returns>完整主机头</returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }

        /// <summary>
        /// 获取上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            try
            {
                string retVal = HttpContext.Current.Request.UrlReferrer.ToString();
                if (VerifyHelper.IsNull(retVal))
                {
                    return "";
                }
                return retVal;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        /// 获取当前请求的原始URL(URL中域信息之后的部分,包括查询字符串(如果存在))
        /// 例:假设当前地址为:http://www.test.com/default.aspx?id=1 ,获取到的结果是/default.aspx?id=1
        /// </summary>
        /// <returns>原始URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回表单和Url参数的总个数
        /// </summary>
        /// <returns>表单或Url参数的总个数</returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName)
        {
            return ConvertHelper.StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return ConvertHelper.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的float类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return ConvertHelper.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            return GetQueryString(strName, false);
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary> 
        /// <param name="strName">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            if (sqlSafeCheck && !StringHelper.IsSafeSqlString(HttpContext.Current.Request.QueryString[strName]))//存在SQL危险字符
            {
                return "";
            }//存在SQL危险字符
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得指定表单参数的short类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的short类型值</returns>
        public static short GetFormShort(string strName, short defValue)
        {
            return ConvertHelper.StrToShort(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName)
        {
            return GetFormInt(strName, 0);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return ConvertHelper.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return ConvertHelper.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的double类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的double类型值</returns>
        public static double GetFormDouble(string strName, double defValue)
        {
            return ConvertHelper.StrToDouble(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            return GetFormString(strName, false);
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            if (sqlSafeCheck && !StringHelper.IsSafeSqlString(HttpContext.Current.Request.Form[strName]))//存在SQL危险字符
            {
                return "";
            }//存在SQL危险字符
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            return GetQueryInt(strName, defValue);
        }

        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的float类型值</returns>
        public static float GetFloat(string strName, float defValue)
        {
            if (GetQueryFloat(strName, defValue) == defValue)
            {
                return GetFormFloat(strName, defValue);
            }
            return GetQueryFloat(strName, defValue);
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            return GetString(strName, false);
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName, bool sqlSafeCheck)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName, sqlSafeCheck);
            }
            return GetQueryString(strName, sqlSafeCheck);
        }

        /// <summary>
        /// 以GET方式访问站点，并返回访问结果
        /// </summary>
        /// <param name="url">站点地址</param>
        /// <returns>访问结果</returns>
        public static string ToSiteByGet(string url)
        {
            System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
            return ToSiteByGet(url, ref cookieContainer);
        }

        /// <summary>
        /// 以GET方式访问站点，并返回访问结果(保持会话)
        /// </summary>
        /// <param name="url">站点地址</param>
        /// <param name="cookieContainer">cookie容器</param>
        /// <returns>访问结果</returns>
        public static string ToSiteByGet(string url, ref System.Net.CookieContainer cookieContainer)
        {
            string result = null;
            try
            {
                //cookie是否存在
                bool existsCookie = cookieContainer != null && cookieContainer.Count > 0;

                Uri uri = new Uri(url);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = false;
                request.Timeout = 10000;
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

                if (!existsCookie)//保存cookie
                {
                    cookieContainer = request.CookieContainer;
                }

                Stream responseStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                result = readStream.ReadToEnd().ToString();
                readStream.Close();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 以POST方式访问站点，并返回访问结果
        /// </summary>
        /// <param name="url">站点地址</param>
        /// <param name="postData">参数</param>
        /// <returns>访问结果</returns>
        public static string ToSiteByPOST(string url, string postData)
        {
            System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
            return ToSiteByPOST(url, postData, ref cookieContainer);
        }

        /// <summary>
        /// 以POST方式访问站点，并返回访问结果(保持会话)
        /// </summary>
        /// <param name="url">站点地址</param>
        /// <param name="postData">参数</param>
        /// <param name="cookieContainer">cookie容器</param>
        /// <returns>访问结果</returns>
        public static string ToSiteByPOST(string url, string postData, ref System.Net.CookieContainer cookieContainer)
        {
            System.Net.HttpWebResponse response = null;
            string result = string.Empty;
            try
            {
                System.Net.HttpWebRequest request = System.Net.HttpWebRequest.Create(url) as System.Net.HttpWebRequest;
                request.Method = "POST";
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;
                request.Headers.Add(System.Net.HttpRequestHeader.AcceptCharset, "GBK,utf-8;q=0.7,*;q=0.3");
                request.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
                request.Headers.Add(System.Net.HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
                request.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.7 (KHTML, like Gecko) Chrome/7.0.517.5 Safari/534.7";
                request.ContentType = "application/x-www-form-urlencoded";

                //cookie是否存在
                bool existsCookie = cookieContainer != null && cookieContainer.Count > 0;
                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                StringBuilder UrlEncoded = new StringBuilder();
                //对参数进行encode   
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (postData != null)
                {
                    int i = 0, j;
                    while (i < postData.Length)
                    {
                        j = postData.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            UrlEncoded.Append(postData.Substring(i, postData.Length - i));
                            break;
                        }
                        UrlEncoded.Append(postData.Substring(i, j - i));
                        UrlEncoded.Append(postData.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.UTF8.GetBytes(UrlEncoded.ToString());
                    request.ContentLength = SomeBytes.Length;
                    Stream newStream = request.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                response = (System.Net.HttpWebResponse)request.GetResponse();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                Stream responseStream = null;
                switch (response.ContentEncoding.ToLower())
                {
                    case "gzip":
                        responseStream = new System.IO.Compression.GZipStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                        break;
                    case "deflate":
                        responseStream = new System.IO.Compression.DeflateStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                        break;
                    default:
                        responseStream = response.GetResponseStream();
                        break;
                }
                if (!existsCookie)//保存cookie
                {
                    cookieContainer = request.CookieContainer;
                }
                StreamReader sr = new StreamReader(responseStream, encode);
                result = sr.ReadToEnd();
            }
            catch
            {
                //dosomething   
            }
            finally
            {
                response.Close();
            }
            return result;
        }

        /// <summary>
        /// 以POST方式访问站点(可以上传文件)，并返回访问结果
        /// </summary>
        /// <param name="url">站点地址</param>
        /// <param name="parmsList">文本参数</param>
        /// <param name="fileList">要上传的文件</param>
        /// <param name="cookieContainer">Cookie</param>
        /// <param name="refererUrl">RefererHTTP标头的值</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeOut">响应时间</param>
        /// <returns>访问结果</returns>
        public static string ToSiteByPOST(string url, IDictionary<string, object> parmsList, IDictionary<string, UploadPostFileInfo> fileList, System.Net.CookieContainer cookieContainer = null, string refererUrl = null, Encoding encoding = null, int timeOut = 20000)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            #region 初始化请求对象
            request.Method = "POST";
            request.Timeout = timeOut;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            if (!string.IsNullOrEmpty(refererUrl))
            {
                request.Referer = refererUrl;
            }
            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            #endregion

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");//分隔符
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            //请求流
            var postStream = new MemoryStream();
            #region 处理Form表单请求内容

            //文本数据模板
            string dataFormdataTemplate =
                "\r\n--" + boundary +
                "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                "\r\n\r\n{1}";
            //上传文本
            foreach (var item in parmsList.Keys)
            {
                string formdata = string.Format(dataFormdataTemplate, item, parmsList[item]);
                byte[] formdataBytes = null;
                //第一行不需要换行
                if (postStream.Length == 0)
                {
                    formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                    postStream.Write(formdataBytes, 0, formdataBytes.Length);
                    continue;
                }
                formdataBytes = Encoding.UTF8.GetBytes(formdata);
                postStream.Write(formdataBytes, 0, formdataBytes.Length);
            }

            //文件数据模板
            string fileFormdataTemplate =
                "\r\n--" + boundary +
                "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
                "\r\nContent-Type: application/octet-stream" +
                "\r\n\r\n";
            //上传文件
            foreach (var item in fileList.Keys)
            {
                string formdata = formdata = string.Format(
                        fileFormdataTemplate,
                        item, //表单键
                        fileList[item].FilOrignalName);

                byte[] formdataBytes = null;
                //第一行不需要换行
                if (postStream.Length == 0)
                {
                    formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                }
                else
                {
                    formdataBytes = Encoding.UTF8.GetBytes(formdata);
                }
                postStream.Write(formdataBytes, 0, formdataBytes.Length);

                //写入文件内容
                using (var stream = fileList[item].PostedFile.InputStream)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        postStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            //结尾
            var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            postStream.Write(footer, 0, footer.Length);

            #endregion

            request.ContentLength = postStream.Length;

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;
                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                ////debug
                //postStream.Seek(0, SeekOrigin.Begin);
                //StreamReader sr = new StreamReader(postStream);
                //var postStr = sr.ReadToEnd();
                postStream.Close();//关闭文件访问
            }
            #endregion

            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            if (cookieContainer != null)
            {
                response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }

        /// <summary>
        /// 以POST方式访问站点(可以上传文件)，并返回访问结果
        /// </summary>
        /// <param name="url">站点地址</param>
        /// <param name="parmsList">文本参数</param>
        /// <param name="fileList">要上传的本地文件</param>
        /// <param name="cookieContainer">Cookie</param>
        /// <param name="refererUrl">RefererHTTP标头的值</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeOut">响应时间</param>
        /// <returns>访问结果</returns>
        public static string ToSiteByPOST(string url, IDictionary<string, object> parmsList, IDictionary<string, UploadLocalFileInfo> fileList, System.Net.CookieContainer cookieContainer = null, string refererUrl = null, Encoding encoding = null, int timeOut = 20000)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            #region 初始化请求对象
            request.Method = "POST";
            request.Timeout = timeOut;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            if (!string.IsNullOrEmpty(refererUrl))
            {
                request.Referer = refererUrl;
            }
            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            #endregion

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");//分隔符
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            //请求流
            var postStream = new MemoryStream();
            #region 处理Form表单请求内容

            //文本数据模板
            string dataFormdataTemplate =
                "\r\n--" + boundary +
                "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                "\r\n\r\n{1}";
            //上传文本
            foreach (var item in parmsList.Keys)
            {
                string formdata = string.Format(dataFormdataTemplate, item, parmsList[item]);
                byte[] formdataBytes = null;
                //第一行不需要换行
                if (postStream.Length == 0)
                {
                    formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                    postStream.Write(formdataBytes, 0, formdataBytes.Length);
                    continue;
                }
                formdataBytes = Encoding.UTF8.GetBytes(formdata);
                postStream.Write(formdataBytes, 0, formdataBytes.Length);
            }

            //文件数据模板
            string fileFormdataTemplate =
                "\r\n--" + boundary +
                "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
                "\r\nContent-Type: application/octet-stream" +
                "\r\n\r\n";
            //上传文件
            foreach (var item in fileList.Keys)
            {
                string formdata = formdata = string.Format(
                        fileFormdataTemplate,
                        item, //表单键
                        fileList[item].FilOrignalName);

                byte[] formdataBytes = null;
                //第一行不需要换行
                if (postStream.Length == 0)
                {
                    formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                }
                else
                {
                    formdataBytes = Encoding.UTF8.GetBytes(formdata);
                }
                postStream.Write(formdataBytes, 0, formdataBytes.Length);

                //写入文件内容
                using (var stream = fileList[item].LocalFile.OpenRead())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        postStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            //结尾
            var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            postStream.Write(footer, 0, footer.Length);

            #endregion

            request.ContentLength = postStream.Length;

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;
                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                ////debug
                //postStream.Seek(0, SeekOrigin.Begin);
                //StreamReader sr = new StreamReader(postStream);
                //var postStr = sr.ReadToEnd();
                postStream.Close();//关闭文件访问
            }
            #endregion

            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            if (cookieContainer != null)
            {
                response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }

        /// <summary>
        /// 以POST方式访问站点，无需响应结果
        /// </summary>
        /// <param name="url">站点地址</param>
        /// <param name="postData">参数</param>
        /// <returns>post提交是否成功</returns>
        public static bool ToSiteByPostNoRs(string url, string postData)
        {
            Stream outstream = null;
            System.Net.HttpWebRequest request = null;

            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);

            try
            {
                // 设置参数
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

                System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;


                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();

                //发送请求
                request.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns>客户端IP</returns>
        public static string GetClientIP()
        {
            string result = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) // 服务器
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();//得到真实的客户端地址
            }
            else//如果没有使用代理服务器或者得不到客户端的ip
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();//得到服务端的地址
            }
            if (VerifyHelper.IsNull(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (result == "::1")
            {
                result = "127.0.0.1";
            }
            return result;
        }

        /// <summary>
        /// 获取本机IPv4地址
        /// </summary>
        /// <returns>本机IPv4地址</returns>
        public static string GetLocalIP()
        {
            string result = string.Empty;
            System.Net.IPHostEntry ips = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (System.Net.IPAddress ip in ips.AddressList)
            {
                if (ip.IsIPv6LinkLocal || ip.IsIPv6Multicast || ip.IsIPv6SiteLocal || ip.IsIPv6Teredo)
                {
                    continue;
                }
                result = ip.ToString();
                break;
            }
            return result;
        }

        /// <summary>
        /// 获取站点的顶级域名
        /// </summary>
        /// <param name="siteUrl">URL</param>
        /// <returns>顶级域名</returns>
        public static string GetRootSiteUrl(string siteUrl)
        {
            if (siteUrl.IndexOf("localhost") != -1)//主域名
            {
                return "localhost";
            }
            if (siteUrl.IndexOf(".") == -1)
            {
                return siteUrl;
            }
            IList<string> sitePart = StringHelper.SplitString(siteUrl, ".");
            if (sitePart.Count == 2)//只存在一个"."
            {
                return siteUrl;
            }
            return sitePart[sitePart.Count - 2] + "." + sitePart[sitePart.Count - 1];
        }
    }
}
