using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Financial.Entity;
using Financial.BLL;

namespace Financial.WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        private ArticleBLL bll = new ArticleBLL();

        // GET api/values
        public IEnumerable<Article> Get()
        {
            return bll.GetALL();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            Article model = new Article();
            model.Contents = "1111";

        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}