using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using NPOI.HSSF;//Excel97-2003
using NPOI.XSSF;//Excel2007

namespace Financial.CommonLib.FileSys
{
    /// <summary>
    /// Excel操作
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 创建Excel,并返回Excel的数据流
        /// </summary>
        /// <param name="ext">excel文件扩展名(xls或xlsx)</param>
        /// <param name="data">要写入的数据</param>
        /// <param name="names">表头</param>
        /// <returns>数据流</returns>
        public static MemoryStream CreateExcel(string ext, IDictionary<string, IList<string>> data, IList<string> names)
        {
            try
            {
                NPOI.XSSF.UserModel.XSSFWorkbook xsWorkBook = null;
                NPOI.HSSF.UserModel.HSSFWorkbook hsWorkbook = null;
                NPOI.SS.UserModel.ISheet sheet = null;
                NPOI.SS.UserModel.ICellStyle style = null;
                NPOI.SS.UserModel.IFont fontHead = null;
                NPOI.SS.UserModel.IFont fontData = null;
                switch (ext)
                {
                    case "xls":
                        hsWorkbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
                        sheet = hsWorkbook.CreateSheet();
                        style = hsWorkbook.CreateCellStyle();
                        fontHead = hsWorkbook.CreateFont();
                        fontData = hsWorkbook.CreateFont();
                        break;
                    case "xlsx":
                        xsWorkBook = new NPOI.XSSF.UserModel.XSSFWorkbook();
                        sheet = xsWorkBook.CreateSheet();
                        style = xsWorkBook.CreateCellStyle();
                        fontHead = xsWorkBook.CreateFont();
                        fontData = xsWorkBook.CreateFont();
                        break;
                    default:
                        return null;
                }
                fontHead.FontName = "宋体";//字体
                fontHead.FontHeightInPoints = 12;//字号
                fontHead.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;//粗体
                fontHead.IsBold = true;

                fontData.FontName = "宋体";
                fontData.FontHeightInPoints = 12;

                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                int i = 0;

                //表头
                style.SetFont(fontHead);
                NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
                NPOI.SS.UserModel.ICell cell = null;
                for (i = 0; i < names.Count; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = style;
                    cell.SetCellValue(names[i]);

                    sheet.AutoSizeColumn(i);//列宽自适应，只对英文和数字有效
                }
                if (data != null && data.Count > 0)
                {
                    //数据
                    style.SetFont(fontData);
                    i = 0;
                    int j = 0;
                    int count = data.Values.First().Count;
                    for (i = 0; i < count; i++)
                    {
                        j = 0;
                        row = sheet.CreateRow(i + 1);//从第二行开始
                        foreach (string item in data.Keys)
                        {
                            cell = row.CreateCell(j);
                            cell.CellStyle = style;
                            cell.SetCellValue(data[item][i]);
                            j++;
                        }
                    }
                    //获取当前列的宽度，然后对比本列的长度，取最大值
                    int x = 0;
                    int y = 0;
                    int leng = 0;
                    int width = 0;
                    for (; y <= names.Count; y++)
                    {
                        width = sheet.GetColumnWidth(y) / 256;
                        for (x = 0; x <= sheet.LastRowNum; x++)
                        {
                            //当前行未被使用过
                            row = sheet.GetRow(x);
                            if (row == null)
                            {
                                continue;
                            }

                            cell = row.GetCell(y);
                            if (cell != null)
                            {
                                leng = Encoding.Default.GetBytes(cell.ToString()).Length + 3;
                                if (width < leng)
                                {
                                    width = leng;
                                }
                            }
                        }
                        sheet.SetColumnWidth(y, width * 256);
                    }
                }
                MemoryStream ms = new MemoryStream();
                switch (ext)
                {
                    case "xls":
                        hsWorkbook.Write(ms);
                        hsWorkbook = null;
                        break;
                    case "xlsx":
                        xsWorkBook.Write(ms);
                        xsWorkBook = null;
                        break;
                }
                return ms;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 读取Excel(只读取第一个Sheet,且表头名称不能相同),返回读取到的集合
        /// </summary>
        /// <param name="ext">excel文件扩展名(xls或xlsx)</param>
        /// <param name="fs">Excel文件流</param>
        /// <returns>数据集合(键:表头去掉首尾空格;值:单元格的集合,单元格的值去掉首尾空格)</returns>
        public static IDictionary<string, IList<string>> ReadExcel(string ext,Stream fs)
        {
            try
            {
                NPOI.XSSF.UserModel.XSSFWorkbook xsWorkBook = null;
                NPOI.HSSF.UserModel.HSSFWorkbook hsWorkbook = null;
                NPOI.SS.UserModel.ISheet sheet = null;
                switch (ext)
                {
                    case "xls":
                        hsWorkbook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
                        sheet = hsWorkbook.GetSheetAt(0);
                        break;
                    case "xlsx":
                        xsWorkBook = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
                        sheet = xsWorkBook.GetSheetAt(0);
                        break;
                    default:
                        return null;
                }
                IDictionary<string, IList<string>> result = new Dictionary<string, IList<string>>();
                int i = 0;
                //表头
                NPOI.SS.UserModel.ICell cell = null;
                NPOI.SS.UserModel.IRow row = sheet.GetRow(0);//第一行
                int columnCount = row.Cells.Count;//列数(以表头为准,防止数据行出现空的情况)
                string cellVal = null;
                int j = 0;
                for (i = 0; i < columnCount; i++)
                {
                    cell = row.GetCell(i);
                    if (VerifyHelper.IsNull(cell))//单元格没有值
                    {
                        continue;
                    }
                    j++;
                    cellVal = cell.ToString();
                    result.Add(cellVal.Trim(), new List<string>());
                }
                columnCount = j;
                //数据
                for (i = 1; i < sheet.PhysicalNumberOfRows; i++)
                {
                    row = sheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }
                    for (j = 0; j < columnCount; j++)
                    {
                        cell = row.GetCell(j);
                        if (VerifyHelper.IsNull(cell))//单元格没有值
                        {
                            if (j == 0)
                            {
                                int nullCount = 1;
                                for (int k = 1; k < columnCount; k++)
                                {
                                    cell = row.GetCell(k);
                                    if (VerifyHelper.IsNull(cell))//单元格没有值
                                    {
                                        nullCount++;
                                    }
                                }
                                if (nullCount == columnCount)
                                {
                                    break;
                                }
                            }
                            result[result.Keys.ElementAt(j)].Add("");
                            continue;
                        }
                        cellVal = cell.ToString();
                        if (cellVal.IndexOf("/") != -1 || cellVal.IndexOf("-") != -1 || cellVal.IndexOf(":") != -1)
                        {
                            try
                            {
                                cellVal = cell.DateCellValue.ToString();
                            }
                            catch (InvalidDataException)
                            {
                                cellVal = cell.ToString();
                            }
                        }
                        result[result.Keys.ElementAt(j)].Add(cellVal.Trim());
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取文件扩展名(小写)
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        /// <returns>文件扩展名</returns>
        public static string GetFileExtension(string sFileName)
        {
            int nPos = sFileName.LastIndexOf(".");
            if (nPos == -1)
            {
                return string.Empty;
            }
            return sFileName.Substring(nPos + 1).ToLower();
        }
    }
}