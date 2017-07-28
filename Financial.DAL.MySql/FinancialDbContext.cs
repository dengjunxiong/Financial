using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Financial.Entity;
using MySql.Data.Entity;

namespace Financial.DAL.MySql
{
    /// <summary>
    /// 数据上下文
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class FinancialDbContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        public FinancialDbContext(string connStr)
            : base(connStr)
        {
            
        }

        /// <summary>
        /// 资讯类型
        /// </summary>
        public DbSet<Column> Columns { get; set; }

        /// <summary>
        /// 资讯
        /// </summary>
        public DbSet<Article> Articles { get; set; }
    }
}
