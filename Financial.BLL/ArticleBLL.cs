using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Financial.Entity;

namespace Financial.BLL
{
    /// <summary>
    /// 资讯
    /// </summary>
    public class ArticleBLL : BLLBase
    {
        public bool Add(Article model)
        {
            dbContext.Articles.Add(model);
            return SaveChange() > 0;
        }

        public IList<Article> GetALL()
        {
            return dbContext.Articles.ToList();
        }
    }
}
