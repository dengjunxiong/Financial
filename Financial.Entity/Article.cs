using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Financial.Entity
{
    /// <summary>
    /// 资讯
    /// </summary>
    public partial class Article
    {
        /// <summary>
        /// 资讯ID
        /// </summary>
        [Key]
        public int ArticleID { get; set; }

        /// <summary>
        /// 资讯类型
        /// </summary>
        [Required]
        public virtual Column Column { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 显示标题(短标题)
        /// </summary>
        [MaxLength(50)]
        public string DisTitle { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        [MaxLength(50)]
        public string KeyWord { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [MaxLength(100)]
        public string Source { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [MaxLength(200)]
        public string Desc { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 外链地址
        /// </summary>
        [MaxLength(100)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreteTime { get; set; }

        /// <summary>
        /// 是否推荐(true:推荐)
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 是否置顶(true:置顶)
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        public ArticleStatus Status { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        public int ClickCount { get; set; }
    }
}
