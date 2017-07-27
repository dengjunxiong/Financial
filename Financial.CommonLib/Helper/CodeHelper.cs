using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Financial.CommonLib
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class CodeHelper
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CodeHelper()
        {
            Length = 4;//验证码长度
            FontSize = 40;//为了显示扭曲效果，默认40像素
            Padding = 6;//1像素
            Chaos = true;//不输出
            ChaosColor = Color.LightGray;//灰色
            BackgroundColor = Color.White;//白色
            Colors = new List<Color>();
            Colors.Add(Color.Black);
            Colors.Add(Color.Red);
            Colors.Add(Color.DarkBlue);
            Colors.Add(Color.Green);
            Colors.Add(Color.Orange);
            Colors.Add(Color.Brown);
            Colors.Add(Color.DarkCyan);
            Colors.Add(Color.Purple);
            Fonts = new List<string>();
            Fonts.Add("Arial");
            Fonts.Add("Georgia");
            CodeSerial = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        }

        #region 常量

        /// <summary>
        /// 圆周率
        /// </summary>
        private const double PI = 3.1415926535897932384626433832795;

        #endregion 常量

        #region 属性

        /// <summary>
        /// 验证码长度
        /// </summary>
        public int Length
        {
            get;
            set;
        }

        /// <summary>
        /// 验证码字体大小
        /// </summary>
        public int FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// 边框补
        /// </summary>
        public int Padding
        {
            get;
            set;
        }

        /// <summary>
        /// 是否输出燥点
        /// </summary>
        public bool Chaos
        {
            get;
            set;
        }

        /// <summary>
        /// 输出燥点的颜色
        /// </summary>
        public Color ChaosColor
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义背景色
        /// </summary>
        public Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义随机颜色
        /// </summary>
        public IList<Color> Colors
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义字体
        /// </summary>
        public IList<string> Fonts
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义随机码字符串序列
        /// </summary>
        public string CodeSerial
        {
            get;
            set;
        }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            // 将位图背景填充为白色
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            double piBase = PI * 2;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (piBase * (double)j) / dBaseAxisLen : (piBase * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }

        /// <summary>
        /// 生成默认长度(4)的随机字符码
        /// </summary>
        /// <returns>随机字符码</returns>
        public string CreateCode()
        {
            return CreateCode(Length);
        }

        /// <summary>
        /// 生成随机字符码
        /// </summary>
        /// <param name="codeLen">字符码长度</param>
        /// <returns>随机字符码</returns>
        public string CreateCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = 4;
            }
            string[] arr = CodeSerial.Split(',');
            string code = "";
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }

        /// <summary>
        /// 生成校验码图片
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public Bitmap CreateImage(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;
            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;
            Bitmap image = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(BackgroundColor);
            Random rand = new Random();
            //给背景添加随机生成的燥点
            if (this.Chaos)
            {
                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 10;
                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);
                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }
            int left = 0, top = 0, top1 = 1, top2 = 1;
            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;
            Font f;
            Brush b;
            int cindex, findex;
            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Count - 1);
                findex = rand.Next(Fonts.Count - 1);
                f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                b = new System.Drawing.SolidBrush(Colors[cindex]);
                switch (i%2)
                {
                    case 1:
                        top = top2;
                        break;
                    default:
                        top = top1;
                        break;
                }
                left = i * fWidth;
                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }
            //画一个边框 边框颜色为Color.Gainsboro
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();
            //产生波形
            image = TwistImage(image, false, 1, 4);
            return image;
        }

        /// <summary>
        /// 生成校验码图片(二进制)
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns>二进制图片</returns>
        public byte[] GetImage(string code)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap image = this.CreateImage(code);
                image.Save(ms, ImageFormat.Jpeg);
                return ms.GetBuffer();
            }
        }

        #endregion
    }
}