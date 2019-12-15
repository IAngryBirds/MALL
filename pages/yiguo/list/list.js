var api = require('../../../config/api.js')
var util = require('../../../utils/util.js')

var app = getApp()
Page({
  data:{
    current: 0,
    listgoods:[],  
  swiper:{
      indicatorDots: false,
      autoplay: false,
      interval: 5000,
      duration: 1000
      }
  },
  onPullDownRefresh: function () {
    console.log('onPullDownRefresh')
  },

  getGoods_List: function () {
    let that = this;
    util.request(api.GetGoodsList).then(function (res) {
      if (res.Code === 200) {
        that.setData({
          listgoods: res.ResultValue
        })
      }
    });
  },

  onLoad:function(options){
    // 页面初始化 options为页面跳转所带来的参数
    var that = this;
    this.getGoods_List();
    //对商品进行价格排序 
    switch1(this.data.listgoods);
    function switch1(odata){
        for(var i=0;i<odata.length-1;i++){
                 //console.log(odata[i].price);
                for(var j=0;j<odata.length-i-1;j++){
                       // console.log(parseInt(odata[j].price)+"-----"+parseInt(odata[j+1].price));
                  if(parseInt(odata[j].price)>parseInt(odata[j+1].price)){
                    var temp=odata[j];
                        odata[j]=odata[j+1];
                        odata[j+1]=temp;
                  }
                }
        }
    }    
  },
  //详情页跳转
  lookdetail:function(e){
    var lookid=e.currentTarget.dataset;
    console.log(e.currentTarget.dataset.id);
    wx.navigateTo({
      url:"/pages/yiguo/detail/detail?id="+lookid.id
    })
  },
  switchSlider:function(e){
    this.setData({
      current:e.target.dataset.index
    })
  },
  changeSlider:function(e){
    this.setData({
      current: e.detail.current
    })
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
