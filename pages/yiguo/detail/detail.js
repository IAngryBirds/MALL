var api = require('../../../config/api.js')
var util = require('../../../utils/util.js')
Page({
  data:{
      detailgood:{},
      hotgoods:[]
  },
  getGoodsDetail: function (goodsID) {
    let that = this;
    var datas = {GoodsID:goodsID}
    util.request(api.GetGoodsDetail, datas).then(function (res) {
      if (res.Code === 200) {
        that.setData({
          detailgood: res.ResultValue
        })
      }
    });
  },
  getHotsGoods:function(){
    let that =this;
    util.request(api.GetHotsGoods).then(function(res){
      if (res.Code === 200) {
        that.setData({
          hotgoods: res.ResultValue
        })    
      }
    })
  },
  onLoad:function(options){
    // 页面初始化 options为页面跳转所带来的参数
    var id=options.id;
    this.getGoodsDetail(id);
    this.getHotsGoods();
    console.log(this.data.detailgood)
  },

  onReady:function(){
 
    // 页面渲染完成
  },
  onShow:function(){
    // 页面显示
  },
  onHide:function(){
    // 页面隐藏
  },
  onUnload:function(){
    // 页面关闭
  }
})
