using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Financial.CommonLib;
using Financial.DAL.MySql;
using Financial.Entity;

namespace Financial.BLL
{
    /// <summary>
    /// 基类
    /// </summary>
    public class BLLBase
    {
        /// <summary>
        /// 连接字符串KEY
        /// </summary>
        private const string CONN_KEY = "db_financial_mysql";

        /// <summary>
        /// 数据上下文
        /// </summary>
        protected FinancialDbContext dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BLLBase()
        {
            string connStr = DBConfig.Current[CONN_KEY];
            dbContext = new FinancialDbContext(connStr);
        }

        /// <summary>
        /// 提交更改
        /// </summary>
        protected int SaveChange()
        {
            return dbContext.SaveChanges();
        }
    }
}