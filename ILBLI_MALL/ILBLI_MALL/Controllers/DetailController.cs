using ILBLI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ILBLI_MALL.Controllers
{
    public class DetailController : ApiController
    {

        public CommonResult GetDetailList(DetailPage page)
        {
            CommonResult result = new CommonResult()
            {
                ResultValue = new List<GoodsEntity>() {
                    new GoodsEntity()  {  },
                    new GoodsEntity()  {  },
                    new GoodsEntity()  {  },
                    new GoodsEntity()  {  },
                    new GoodsEntity()  {  },
                    new GoodsEntity()  {  },
                    new GoodsEntity()  {  },
                }
            };

            return result;
        }

    }
}
