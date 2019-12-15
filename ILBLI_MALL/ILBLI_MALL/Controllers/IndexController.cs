using ILBLI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ILBLI_MALL.Controllers
{
    public class IndexController : ApiController
    {

        /// <summary>
        /// 获取轮播图
        /// </summary>
        /// <returns></returns>
        public CommonResult GetBanner()
        {
            CommonResult result = new CommonResult()
            {
                ResultValue = new List<BannerEntity>()
                {
                    new BannerEntity() { Pic_url ="http://img09.yiguoimg.com/e/ad/2016/160825/585749448986042649_800x400.jpg",GoodsID=1 },
                    new BannerEntity() { Pic_url ="http://img11.yiguoimg.com/e/ad/2016/160927/585749449690947899_800x400.jpg",GoodsID=2 },
                    new BannerEntity() { Pic_url ="http://img14.yiguoimg.com/e/ad/2016/160923/585749449636290871_800x400.jpg",GoodsID=3 },
                    new BannerEntity() { Pic_url ="http://img13.yiguoimg.com/e/ad/2016/160914/585749449480315182_800x400.jpg",GoodsID=4 },
                    new BannerEntity() { Pic_url ="http://img14.yiguoimg.com/e/ad/2016/161010/585749449889390922_800x400.jpg",GoodsID=5  }
               }
            };
            return result;
        }

        /// <summary>
        /// 新品推荐
        /// </summary>
        /// <returns></returns>
        public CommonResult GetNews()
        {
            CommonResult result = new CommonResult()
            {
                ResultValue = new List<GoodsEntity>()
                {
                    new GoodsEntity() { Pic_url ="http://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg",GoodsID=6 }, 
                    new GoodsEntity() { Pic_url ="http://img09.yiguoimg.com/e/ad/2016/161011/585749449909281099_260x320.jpg",GoodsID=7 },
                    new GoodsEntity() { Pic_url ="http://img12.yiguoimg.com/e/ad/2016/160914/585749449480249646_260x320.jpg",GoodsID=8 }, 
               }
            };
            return result;
        }

        /// <summary>
        /// 超值购
        /// </summary>
        /// <returns></returns>
        public CommonResult GetHots()
        {
            CommonResult result = new CommonResult()
            {
                ResultValue = new List<GoodsEntity>()
                {
                    new GoodsEntity() { Pic_url ="http://img13.yiguoimg.com/e/albums/2017/170630/153403897791357662_800x468.jpg",GoodsID=11 },
                    new GoodsEntity() { Pic_url ="http://img14.yiguoimg.com/e/albums/2017/170629/153403897729786589_800x468.jpg",GoodsID=12 },
                    new GoodsEntity() { Pic_url ="http://img12.yiguoimg.com/e/albums/2017/170626/153403897618375386_596x379.jpg",GoodsID=13 },
                    new GoodsEntity() {Pic_url="http://img12.yiguoimg.com/e/albums/2017/170621/153403897468003029_800x468.jpg",GoodsID=14 } 
               }
            };
            return result;
        }

    }
}
