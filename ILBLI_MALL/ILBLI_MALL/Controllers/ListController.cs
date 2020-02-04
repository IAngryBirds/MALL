using ILBLI.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace ILBLI_MALL.Controllers
{
    public class ListController : ApiController
    {

        public CommonResult GetList()
        {
            CommonResult result = new CommonResult()
            {
                ResultValue = new List<GoodsModel>()
                {
                    new GoodsModel{ Pic_url="http://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg",Memo="3.3kg/箱",Price=111.0,Name="Zespri佳沛新西兰阳光金奇异果6个92-114g/个(北京)",ID=0101001},
                    new GoodsModel{ Pic_url="http://img09.yiguoimg.com/e/ad/2016/161011/585749449909281099_260x320.jpg",Memo="3.3kg/箱",Price=177.0,Name="智利蓝莓2盒（约125g/盒）",ID=0101002},
                    new GoodsModel{ Pic_url="http://img12.yiguoimg.com/e/ad/2016/160914/585749449480249646_260x320.jpg",Memo="3.3kg/箱",Price=178.0,Name="澳大利亚脐橙12个约160g/个(北京)",ID=0101003},
                    new GoodsModel{ Pic_url="http://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg",Memo="3.3kg/箱",Price=172.0,Name="Zespri佳沛新西兰阳光金奇异果6个92-114g/个(北京)",ID=0102001},
                    new GoodsModel{ Pic_url="http://img09.yiguoimg.com/e/ad/2016/161011/585749449909281099_260x320.jpg",Memo="3.3kg/箱",Price=171.0,Name="智利蓝莓2盒（约125g/盒）",ID=0102002},
                    new GoodsModel{ Pic_url="http://img12.yiguoimg.com/e/ad/2016/160914/585749449480249646_260x320.jpg",Memo="3.3kg/箱",Price=135.0,Name="澳大利亚脐橙12个约160g/个(北京)",ID=0102003},
                    new GoodsModel{ Pic_url="http://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg",Memo="3.3kg/箱",Price=111.0,Name="Zespri佳沛新西兰阳光金奇异果6个92-114g/个(北京)",ID=0103001},
                    new GoodsModel{ Pic_url="http://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg",Memo="3.3kg/箱",Price=111.0,Name="Zespri佳沛新西兰阳光金奇异果6个92-114g/个(北京)",ID=0101001},
                    new GoodsModel{ Pic_url="http://img09.yiguoimg.com/e/ad/2016/161011/585749449909281099_260x320.jpg",Memo="3.3kg/箱",Price=177.0,Name="智利蓝莓2盒（约125g/盒）",ID=0101002},
                    new GoodsModel{ Pic_url="http://img12.yiguoimg.com/e/ad/2016/160914/585749449480249646_260x320.jpg",Memo="3.3kg/箱",Price=178.0,Name="澳大利亚脐橙12个约160g/个(北京)",ID=0101003},
                    new GoodsModel{ Pic_url="http://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg",Memo="3.3kg/箱",Price=172.0,Name="Zespri佳沛新西兰阳光金奇异果6个92-114g/个(北京)",ID=0102001},
                    new GoodsModel{ Pic_url="http://img09.yiguoimg.com/e/ad/2016/161011/585749449909281099_260x320.jpg",Memo="3.3kg/箱",Price=171.0,Name="智利蓝莓2盒（约125g/盒）",ID=0102002},
                    new GoodsModel{ Pic_url="http://img12.yiguoimg.com/e/ad/2016/160914/585749449480249646_260x320.jpg",Memo="3.3kg/箱",Price=135.0,Name="澳大利亚脐橙12个约160g/个(北京)",ID=0102003},
                    new GoodsModel{ Pic_url="http://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg",Memo="3.3kg/箱",Price=111.0,Name="Zespri佳沛新西兰阳光金奇异果6个92-114g/个(北京)",ID=0103001},
                }
            };

            return result;
        }
    }
}
