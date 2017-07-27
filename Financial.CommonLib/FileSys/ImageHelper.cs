using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Financial.CommonLib.FileSys
{
    /// <summary>
    /// 图像处理类
    /// 该类用来生成缩略图，水印等
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 图像处理类构造方法
        /// </summary>
        public ImageHelper()
        {

        }

        /// <summary>
        /// 计算缩略图最终生成尺寸
        /// </summary>
        /// <param name="width">缩略图要求宽度</param>
        /// <param name="height">缩略图要求高度</param>
        /// <param name="originalWidth">原图宽度</param>
        /// <param name="originalHeight">原图高度</param>
        /// <returns>缩略图最终生成尺寸</returns>
        private Size NewSize(int width, int height, int originalWidth, int originalHeight, out int x, out int y)
        {
            int ow = originalWidth;
            int oh = originalHeight;

            int towidth = width;
            int toheight = height;

            x = 0;
            y = 0;

            if (width > 0 && height < 0)
            {
                height = oh * width / ow;
            }
            else if (height > 0 && width < 0)
            {
                width = ow * height / oh;
            }

            switch ((double)ow / (double)width > (double)oh / (double)height)
            {
                case true:
                    if (width > ow && height > oh)
                    {
                        towidth = ow;
                    }
                    if (ow >= towidth)
                    {
                        toheight = Convert.ToInt32(oh / ((double)ow / (double)towidth));
                        break;
                    }
                    toheight = Convert.ToInt32(oh * ((double)towidth / (double)ow));
                    break;
                default:
                    if (width > ow && height > oh)
                    {
                        toheight = oh;
                    }
                    if (oh >= toheight)
                    {
                        towidth = Convert.ToInt32(ow / ((double)oh / (double)toheight));
                        break;
                    }
                    towidth = Convert.ToInt32(ow * ((double)toheight / (double)oh));
                    break;
            }
            x = (width - towidth) / 2;
            y = (height - toheight) / 2;

            return new Size(Convert.ToInt32(towidth), Convert.ToInt32(toheight));
        }

        /// <summary>
        /// 根据图片数据流生成缩略图(支持透明背景,生成的是png图片)
        /// </summary>
        /// <param name="saveFormat">缩略图扩展名</param>
        /// <param name="oSourceStream">原图数据流</param>
        /// <param name="MaxSize">缩略图尺寸</param>
        /// <param name="oOutputStream">缩略图数据流</param>
        /// <returns>是否生成成功</returns>
        public bool GenThumbNail(string saveFormat, Stream oSourceStream, Size MaxSize, Stream oOutputStream)
        {
            Image oImageSrc = Image.FromStream(oSourceStream);
            ImageFormat rawFormat = ImageFormat.Png;

            int x = 0;
            int y = 0;
            Size newSize = NewSize(MaxSize.Width, MaxSize.Height, oImageSrc.Width, oImageSrc.Height, out x, out y);
            Bitmap outBmp = new Bitmap(MaxSize.Width, MaxSize.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(outBmp);

            // 设置画布的描绘质量
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            ImageFormat imgF = null;
            //判断缩略图扩展名
            switch (saveFormat.ToUpper())
            {
                case "BMP"://不支持透明色
                case "JPG"://不支持透明色
                case "JPEG"://不支持透明色
                case "PNG"://支持透明色
                case "GIF"://支持透明色
                    imgF = ImageFormat.Png;
                    g.Clear(Color.Transparent);
                    break;
                //case "GIF"://支持透明色
                //    imgF = ImageFormat.Gif;
                //    g.Clear(Color.Transparent);
                //    break;
                //case "BMP"://不支持透明色
                //    imgF = ImageFormat.Bmp;
                //    g.Clear(System.Drawing.Color.FromArgb(255, 250, 255, 249));
                //    break;
                //case "JPG"://不支持透明色
                //case "JPEG"://不支持透明色
                //    imgF = ImageFormat.Jpeg;
                //    g.Clear(System.Drawing.Color.FromArgb(255, 250, 255, 249));
                //    break;
                default://支持透明色
                    imgF = ImageFormat.Png;
                    g.Clear(Color.Transparent);
                    break;
            }

            //在指定位置并且按指定大小绘制原图片的指定部分
            //第一个参数:原图;第二个参数:绘制区域;第三个参数:填充图像(按原图缩放);第四个参数:单位
            g.DrawImage(oImageSrc, new Rectangle(x, y, newSize.Width, newSize.Height), new Rectangle(0, 0, oImageSrc.Width, oImageSrc.Height), GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 0;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            //ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            //ImageCodecInfo jpegICI = null;
            //for (int i = 0; i < arrayICI.Length; i++)
            //{
            //    if (arrayICI[i].FormatDescription.Equals("JPEG"))
            //    {
            //        jpegICI = arrayICI[i];//设置JPEG编码
            //        break;
            //    }
            //}

            //if (jpegICI != null)
            //{
            //    outBmp.Save(oOutputStream, jpegICI, encoderParams);
            //    oImageSrc.Dispose();
            //    outBmp.Dispose();
            //    return true;
            //}
            outBmp.Save(oOutputStream, rawFormat);
            oImageSrc.Dispose();
            outBmp.Dispose();
            return true;
        }

        /// <summary>
        /// 根据文件生成缩略图(支持透明背景,生成的是png图片)
        /// </summary>
        /// <param name="saveFormat">缩略图扩展名</param>
        /// <param name="sFilePath">文件路径</param>
        /// <param name="MaxSize">缩略图尺寸</param>
        /// <param name="sThumbNailPath">缩略图路径</param>
        /// <returns>是否生成成功</returns>
        public bool GenThumbNail(string saveFormat, string sFilePath, Size MaxSize, string sThumbNailPath)
        {
            try
            {
                Image oImageSrc = Image.FromFile(sFilePath);
                ImageFormat rawFormat = ImageFormat.Png;

                int x = 0;
                int y = 0;
                Size newSize = NewSize(MaxSize.Width, MaxSize.Height, oImageSrc.Width, oImageSrc.Height, out x, out y);
                Bitmap outBmp = new Bitmap(MaxSize.Width, MaxSize.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(outBmp);

                //设置画布的描绘质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                ImageFormat imgF = null;
                //判断缩略图扩展名
                switch (saveFormat.ToUpper())
                {
                    case "BMP"://不支持透明色
                    case "JPG"://不支持透明色
                    case "JPEG"://不支持透明色
                    case "PNG"://支持透明色
                    case "GIF"://支持透明色
                        imgF = ImageFormat.Png;
                        g.Clear(Color.Transparent);
                        break;
                    //case "GIF"://支持透明色
                    //    imgF = ImageFormat.Gif;
                    //    g.Clear(Color.Transparent);
                    //    break;
                    //case "BMP"://不支持透明色
                    //    imgF = ImageFormat.Bmp;
                    //    g.Clear(System.Drawing.Color.FromArgb(255, 250, 255, 249));
                    //    break;
                    //case "JPG"://不支持透明色
                    //case "JPEG"://不支持透明色
                    //    imgF = ImageFormat.Jpeg;
                    //    g.Clear(System.Drawing.Color.FromArgb(255, 250, 255, 249));
                    //    break;
                    default://支持透明色
                        imgF = ImageFormat.Png;
                        g.Clear(Color.Transparent);
                        break;
                }

                //在指定位置并且按指定大小绘制原图片的指定部分
                //第一个参数:原图;第二个参数:绘制区域;第三个参数:填充图像(按原图缩放);第四个参数:单位
                g.DrawImage(oImageSrc, new Rectangle(x, y, newSize.Width, newSize.Height), new Rectangle(0, 0, oImageSrc.Width, oImageSrc.Height), GraphicsUnit.Pixel);
                g.Dispose();

                //以下代码为保存图片时，设置压缩质量
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 0;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;

                //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
                //ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                //ImageCodecInfo jpegICI = null;
                //for (int i = 0; i < arrayICI.Length; i++)
                //{
                //    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                //    {
                //        jpegICI = arrayICI[i];//设置JPEG编码
                //        break;
                //    }
                //}

                byte[] imageBytes = File.ReadAllBytes(sThumbNailPath);
                Stream oOutputStream = new MemoryStream(imageBytes);
                //if (jpegICI != null)
                //{
                //    outBmp.Save(oOutputStream, jpegICI, encoderParams);
                //    oImageSrc.Dispose();
                //    outBmp.Dispose();
                //    return true;
                //}
                outBmp.Save(oOutputStream, rawFormat);
                oImageSrc.Dispose();
                outBmp.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="oImage">原图</param>
        /// <param name="oCfg">文件上传配置</param>
        /// <returns>是否添加成功</returns>
        public bool AddWarterMark(Stream oFileStream, UploadConfig oCfg)
        {
            int location = 9;//图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下
            try
            {
                switch (oCfg.WaterMarktype)
                {
                    case WaterMarkTypes.No://无水印
                        return true;
                    case WaterMarkTypes.Picture://图片水印
                        AddImageSignPic(oFileStream, oCfg.WarterMarkPicPath, location);
                        return true;
                    case WaterMarkTypes.Text://文字水印
                        string fontname = "";//文字水印字体名称
                        int fontsize = 10;//文字水印字体大小
                        AddImageSignText(oFileStream, oCfg.WarterMarkText, location, fontname, fontsize);
                        return true;
                    default:
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="oFileStream">需要添加水印的文件流</param>
        /// <param name="watermarkFilename">水印文件路径</param>
        /// <param name="watermarkLocation">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        public static void AddImageSignPic(Stream oFileStream, string watermarkFilename, int watermarkLocation)
        {
            if (watermarkLocation == 0 || !File.Exists(watermarkFilename))//水印不使用或水印文件不存在
            {
                return;
            }
            Image img = Image.FromStream(oFileStream);
            Image watermark = new Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)//水印尺寸比图片尺寸大
            {
                return;
            }
            ImageFormat rawFormat = img.RawFormat;

            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            int watermarkTransparency = 1;//水印的透明度 1--10 10为不透明
            float transparency = (watermarkTransparency / 10.0F);


            float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkLocation)
            {
                case 1://左上
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2://中上
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3://右上
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4://左中
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5://中中
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6://右中
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7://左下
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8://中下
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9://右下
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                {
                    ici = codec;
                    break;
                }
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];

            qualityParam[0] = 100;//附加水印图片质量(1-100)

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
            {
                img.Save(oFileStream, ici, encoderParams);
                g.Dispose();
                img.Dispose();
                watermark.Dispose();
                imageAttributes.Dispose();
                return;
            }
            img.Save(oFileStream, rawFormat);
            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="oFileStream">需要添加水印的文件流</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="watermarkLocation">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="fontname">字体</param>
        /// <param name="fontsize">字体大小</param>
        public static void AddImageSignText(Stream oFileStream, string watermarkText, int watermarkLocation, string fontname, int fontsize)
        {
            Image img = Image.FromStream(oFileStream);
            ImageFormat rawFormat = img.RawFormat;

            Graphics g = Graphics.FromImage(img);
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkLocation)
            {
                case 1:
                    xpos = (float)img.Width * (float).01;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 2:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (float)img.Height * (float).01;
                    break;
                case 3:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 4:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 5:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 6:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 7:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 8:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 9:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
            }

            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.Black), xpos, ypos);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                {
                    ici = codec;
                    break;
                }
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];

            qualityParam[0] = 100;//附加水印图片质量(1-100)

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
            {
                img.Save(oFileStream, ici, encoderParams);
                g.Dispose();
                img.Dispose();
                return;
            }
            img.Save(oFileStream, rawFormat);
            g.Dispose();
            img.Dispose();
        }
    }
}
