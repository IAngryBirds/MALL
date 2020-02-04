using ILBLI.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace ILBLI_MALL.Controllers
{
    public class DetailController : ApiController
    {
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="goodsID">商品ID</param>
        /// <returns></returns>
        public CommonResult GetGoodsDetail(string goodsID)
        {
            CommonResult result = new CommonResult()
            {
                ResultValue = new GoodsModel()
                {
                    ID = 101001,
                    Name = "Zespri佳沛新西兰阳光金奇异果6个92-114g/个",
                    Price = 111.0,
                    Specs = "3.3kg/箱",
                    Memo= "产地：泰国 发货地：北京",
                    Pic_url = "http ://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg",
                    ReviewsCounts =37
                },

            };

            return result;
        }

        /// <summary>
        /// 获取热推商品
        /// </summary>
        /// <returns></returns>
        public CommonResult GetHotGoods()
        {
            CommonResult result = new CommonResult()
            {
                ResultValue = new List<GoodsModel>() {
                    new GoodsModel{ ID=1,Pic_url="http://img10.yiguoimg.com/e/ad/2016/161008/585749449862226248_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img14.yiguoimg.com/e/ad/2016/160929/585749449767461181_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img12.yiguoimg.com/e/ad/2016/161009/585749449871663433_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img10.yiguoimg.com/e/ad/2016/161008/585749449862226248_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img14.yiguoimg.com/e/ad/2016/160929/585749449767461181_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img12.yiguoimg.com/e/ad/2016/161009/585749449871663433_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img10.yiguoimg.com/e/ad/2016/161008/585749449862226248_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img14.yiguoimg.com/e/ad/2016/160929/585749449767461181_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img12.yiguoimg.com/e/ad/2016/161009/585749449871663433_778x303.jpg" },
                    new GoodsModel{ ID=1,Pic_url="http://img10.yiguoimg.com/e/ad/2016/161008/585749449862226248_778x303.jpg" },
                    
                }
            };

            return result;
        }

    }
}
