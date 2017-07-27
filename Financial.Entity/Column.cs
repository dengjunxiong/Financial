using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Financial.Entity
{
    /// <summary>
    /// 资讯类型
    /// </summary>
    public partial class Column
    {
        /// <summary>
        /// 资讯类型ID
        /// </summary>
        [Key]
        public int ColumnID { get; set; }

        /// <summary>
        /// 资讯类型名称
        /// </summary>
        [MaxLength(20)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 资讯类型关键词
        /// </summary>
        [MaxLength(50)]
        public string ColumnKeyName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortNum { get; set; }

        /// <summary>
        /// 资讯集合
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }
    }
}