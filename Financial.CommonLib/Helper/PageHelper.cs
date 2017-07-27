using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financial.CommonLib
{
    /// <summary>
    /// 页面处理
    /// </summary>
    public class PageHelper
    {
        /// <summary>
        /// 返回前台分页页码
        /// </summary>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="linkUrl">链接地址，__page__代表页码</param>
        /// <param name="centSize">中间页码数量(默认值:5)</param>
        /// <returns>分页HTML代码</returns>
        public static string OutPageList(int pageSize, int pageIndex, int totalCount, string linkUrl, int centSize = 5)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "";
            }
            int pageCount = totalCount / pageSize;//总页数
            if (pageCount < 1)
            {
                return "";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "";
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageIndex > pageCount)
            {
                pageIndex = pageCount;
            }
            StringBuilder pageStr = new StringBuilder();
            string pageId = "__page__";

            string homeBtn = string.Format("<a href=\"{0}\">&lt;&lt;</a>", StringHelper.ReplaceStr(linkUrl, pageId, "1"));//首页
            string prevBtn = string.Format("<a href=\"{0}\">&lt;</a>", StringHelper.ReplaceStr(linkUrl, pageId, (pageIndex - 1).ToString()));//上一页
            string nextBtn = string.Format("<a href=\"{0}\">&gt;</a>", StringHelper.ReplaceStr(linkUrl, pageId, (pageIndex + 1).ToString()));//下一页
            string lastBtn = string.Format("<a href=\"{0}\">&gt;&gt;</a>", StringHelper.ReplaceStr(linkUrl, pageId, pageCount.ToString()));//末页

            if (pageIndex <= 1)
            {
                homeBtn = string.Format("<a href=\"{0}\">&lt;&lt;</a>", "javascript:void(0);");//首页"
                prevBtn = string.Format("<a href=\"{0}\">&lt;</a>", "javascript:void(0);");//上一页
            }
            if (pageIndex >= pageCount)
            {
                nextBtn = string.Format("<a href=\"{0}\">&gt;</a>", "javascript:void(0);");//下一页
                lastBtn = string.Format("<a href=\"{0}\">&gt;&gt;</a>", "javascript:void(0);");//末页
            }

            int firstNum = pageIndex - centSize / 2; //中间开始的页码
            if (centSize % 2 == 0)//中间页码数量为偶数
            {
                firstNum += 1;
            }
            int lastNum = pageIndex + centSize / 2; //中间结束的页码
            if (pageCount <= centSize)
            {
                firstNum = 1;
                lastNum = pageCount;
            }
            else if (pageIndex <= centSize / 2)
            {
                firstNum = 1;
                lastNum = centSize;
            }
            else if (pageIndex + centSize / 2 >= pageCount)
            {
                firstNum = pageCount - centSize + 1;
                lastNum = pageCount;
            }
            pageStr.Append(homeBtn);
            pageStr.Append(prevBtn);
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)//当前页码
                {
                    pageStr.Append(string.Format("<a class=\"on\" href=\"{0}\">{1}</a>", StringHelper.ReplaceStr(linkUrl, pageId, i.ToString()), i));
                    continue;
                }
                pageStr.Append(string.Format("<a href=\"{0}\">{1}</a>", StringHelper.ReplaceStr(linkUrl, pageId, i.ToString()), i));
            }
            pageStr.Append(nextBtn);
            pageStr.Append(lastBtn);
            return pageStr.ToString();
        }

        /// <summary>
        /// 返回前台会员中心分页页码
        /// </summary>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="linkUrl">链接地址，__page__代表页码</param>
        /// <param name="centSize">中间页码数量(默认值:5)</param>
        /// <returns>分页HTML代码</returns>
        public static string OutMemberPageList(int pageSize, int pageIndex, int totalCount, string linkUrl, int centSize = 5)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "";
            }
            int pageCount = totalCount / pageSize;//总页数
            if (pageCount < 1)
            {
                return "";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "";
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageIndex > pageCount)
            {
                pageIndex = pageCount;
            }
            StringBuilder pageStr = new StringBuilder();
            string pageId = "__page__";

            string homeBtn = "<a href=\"" + StringHelper.ReplaceStr(linkUrl, pageId, "1") + "\">首页</a>";//首页
            string prevBtn = "<a href=\"" + StringHelper.ReplaceStr(linkUrl, pageId, (pageIndex - 1).ToString()) + "\">上一页</a>";//上一页
            string nextBtn = "<a href=\"" + StringHelper.ReplaceStr(linkUrl, pageId, (pageIndex + 1).ToString()) + "\">下一页</a>";//下一页
            string lastBtn = "<a href=\"" + StringHelper.ReplaceStr(linkUrl, pageId, pageCount.ToString()) + "\">尾页</a>";//尾页

            if (pageIndex <= 1)
            {
                homeBtn = "<a href=\"javascript:void(0);\" class=\"fan_page disabled\">首页</a>";//首页
                prevBtn = "<a href=\"javascript:void(0);\" class=\"fan_page disabled\">上一页</a>";//上一页
            }
            if (pageIndex >= pageCount)
            {
                nextBtn = "<a href=\"javascript:void(0);\" class=\"fan_page disabled\">下一页</a>";//下一页
                lastBtn = "<a href=\"javascript:void(0);\" class=\"fan_page disabled\">尾页</a>";//尾页
            }
            pageStr.Append(homeBtn);
            pageStr.Append(prevBtn);
            if (centSize > 0)
            {
                int firstNum = pageIndex - centSize / 2; //中间开始的页码
                if (centSize % 2 == 0)//中间页码数量为偶数
                {
                    firstNum += 1;
                }
                int lastNum = pageIndex + centSize / 2; //中间结束的页码
                if (pageCount <= centSize)
                {
                    firstNum = 1;
                    lastNum = pageCount;
                }
                else if (pageIndex <= centSize / 2)
                {
                    firstNum = 1;
                    lastNum = centSize;
                }
                else if (pageIndex + centSize / 2 >= pageCount)
                {
                    firstNum = pageCount - centSize + 1;
                    lastNum = pageCount;
                }
                for (int i = firstNum; i <= lastNum; i++)
                {
                    if (i == pageIndex)//当前页码
                    {
                        pageStr.Append("<a href=\"javascript:void(0);\" class=\"cur\">" + i + "</a>");
                        continue;
                    }
                    pageStr.Append("<a href=\"" + StringHelper.ReplaceStr(linkUrl, pageId, i.ToString()) + "\">" + i + "</a>");
                }
            }
            pageStr.Append(nextBtn);
            pageStr.Append(lastBtn);
            pageStr.Append(string.Format("&nbsp;第{0} 页/共 {1} 页", pageIndex, pageCount));
            return pageStr.ToString();
        }
    }
}
