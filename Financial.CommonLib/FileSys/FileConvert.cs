using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Office.Core;

namespace Financial.CommonLib.FileSys
{
    /// <summary>
    /// 文件转换
    /// </summary>
    public class FileConvert
    {
        #region 把Word文件转换成为PDF格式文件

        /// <summary>  
        /// 把Word文件转换成为PDF格式文件  
        /// </summary>  
        /// <param name="sourcePath">源文件路径</param>  
        /// <param name="targetPath">目标文件路径</param>   
        /// <returns>true=转换成功</returns>  
        public static bool WordToPDF(out string error, string sourcePath, string targetPath)
        {
            error = "";
            bool result = false;
            Microsoft.Office.Interop.Word.WdExportFormat exportFormat = Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF;
            Microsoft.Office.Interop.Word.ApplicationClass application = null;

            Microsoft.Office.Interop.Word._Document document = null;
            try
            {
                application = new Microsoft.Office.Interop.Word.ApplicationClass();
                application.Visible = false;
                document = application.Documents.Open(sourcePath);
                document.SaveAs();
                document.ExportAsFixedFormat(targetPath, exportFormat);
                result = true;
            }
            catch (Exception e)
            {
                error = e.Message;
                //Console.WriteLine(e.Message);
                //System.IO.StreamWriter sw = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/Admin/1.txt"));
                //sw.Write("sourcePath :" + sourcePath + "\n\n targetPath :" + targetPath + "\n\n" + e.ToString());
                //sw.Close();

                //Grimmbro.CommonLib.LogHelper.AddLog(e.ToString());//添加报错日志

                result = false;
            }
            finally
            {
                if (document != null)
                {
                    document.Close();
                    document = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        #endregion 把Word文件转换成为PDF格式文件

        #region 把Microsoft.Office.Interop.Excel文件转换成PDF格式文件

        /// <summary>  
        /// 把Microsoft.Office.Interop.Excel文件转换成PDF格式文件  
        /// </summary>  
        /// <param name="sourcePath">源文件路径</param>  
        /// <param name="targetPath">目标文件路径</param>   
        /// <returns>true=转换成功</returns>  
        public static bool ExcelToPDF(out string error, string sourcePath, string targetPath)
        {
            error = "";
            bool result = false;
            Microsoft.Office.Interop.Excel.XlFixedFormatType targetType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.ApplicationClass application = null;
            Microsoft.Office.Interop.Excel.Workbook workBook = null;
            try
            {
                application = new Microsoft.Office.Interop.Excel.ApplicationClass();
                application.Visible = false;
                workBook = application.Workbooks.Open(sourcePath);
                workBook.SaveAs();
                workBook.ExportAsFixedFormat(targetType, targetPath);
                result = true;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                //System.IO.StreamWriter sw = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/Admin/1.txt"));
                //sw.Write(e.ToString());
                //sw.Close();

                //Grimmbro.CommonLib.LogHelper.AddLog(e.ToString());//添加报错日志
                error = e.Message;
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        #endregion 把Microsoft.Office.Interop.Excel文件转换成PDF格式文件

        #region 把PowerPoint文件转换成PDF格式文件

        /// <summary>  
        /// 把PowerPoint文件转换成PDF格式文件  
        /// </summary>  
        /// <param name="sourcePath">源文件路径</param>  
        /// <param name="targetPath">目标文件路径</param>   
        /// <returns>true=转换成功</returns>  
        public static bool PowerPointToPDF(out string error, string sourcePath, string targetPath)
        {
            error = "";
            bool result;
            Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType targetFileType = Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
            object missing = Type.Missing;
            Microsoft.Office.Interop.PowerPoint.ApplicationClass application = null;
            Microsoft.Office.Interop.PowerPoint.Presentation persentation = null;
            try
            {
                application = new Microsoft.Office.Interop.PowerPoint.ApplicationClass();
                //application.Visible = MsoTriState.msoFalse;  
                persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);

                result = true;
            }
            catch (Exception e)
            {
                //System.IO.StreamWriter sw = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/Admin/1.txt"));
                //sw.Write(e.ToString());
                //sw.Close();
                //Grimmbro.CommonLib.LogHelper.AddLog(e.ToString());//添加报错日志
                error = e.Message;
                //Console.WriteLine(e.Message);
                result = false;
            }
            finally
            {
                if (persentation != null)
                {
                    persentation.Close();
                    persentation = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        #endregion 把PowerPoint文件转换成PDF格式文件

        #region 把PDF文件转化为SWF文件

        /// <summary>
        /// 把PDF文件转化为SWF文件
        /// </summary>
        /// <param name="toolPah">pdf转换为swf工具的路径</param>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转化成功</returns>
        public static bool PDFToSWF(out string error, string toolPah, string sourcePath, string targetPath)
        {
            error = "";
            Process pc = new Process();
            bool returnValue = true;

            string cmd = toolPah;
            string args = " -t " + sourcePath + " -s flashversion=9 -o " + targetPath;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(cmd, args);
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                pc.StartInfo = psi;
                pc.Start();
                pc.WaitForExit();
            }
            catch (Exception ex)
            {
                //System.IO.StreamWriter sw = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/Admin/1.txt"));
                //sw.Write(ex.ToString());
                //sw.Close();

                //Grimmbro.CommonLib.LogHelper.AddLog(ex.ToString());//添加报错日志
                error = ex.Message;
                returnValue = false;
            }
            finally
            {
                pc.Close();
                pc.Dispose();
            }
            return returnValue;
        }

        #endregion 把PDF文件转化为SWF文件
    }
}
