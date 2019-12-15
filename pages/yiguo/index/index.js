
var api = require('../../../config/api.js')
var util =require('../../../utils/util.js')


//获取应用实例
var app = getApp()

Page({
  data: {
    toView: "",
    motto: 'MiHome_Store',
    userInfo: {},
    indicatorDots: true,
    autoplay: true,
    interval: 3000,
    duration: 100,
    newgoods: [
      {
        "hg_pic": "http://img14.yiguoimg.com/e/ad/2016/160914/585749449477366062_260x320.jpg"
      }, {
        "hg_pic": "http://img09.yiguoimg.com/e/ad/2016/161011/585749449909281099_260x320.jpg"
      }, {
        "hg_pic": "http://img12.yiguoimg.com/e/ad/2016/160914/585749449480249646_260x320.jpg"
      }
    ],
    hotgoods: [{
      "more_pic": "http://img13.yiguoimg.com/e/albums/2017/170630/153403897791357662_800x468.jpg"
    }, {
      "more_pic": "http://img14.yiguoimg.com/e/albums/2017/170629/153403897729786589_800x468.jpg",
    }, {
      "more_pic": "http://img12.yiguoimg.com/e/albums/2017/170626/153403897618375386_596x379.jpg",
    }, {
      "more_pic": "http://img12.yiguoimg.com/e/albums/2017/170621/153403897468003029_800x468.jpg",
    } 
    ],
    banner_list: [],
  },
 
  onPullDownRefresh: function () {
    console.log('onPullDownRefresh')
  },
  scroll: function (e) {
    if (this.data.toView == "top") {
      this.setData({
        toView: ""
      })
    }
  },
  getBanner_List:function(){
    let that =this;
    util.request(api.GetBanner).then(function(res){
        if(res.Code===200){
          that.setData({
            banner_list:res.ResultValue
          })
        } 
    });
  },
   
  //事件处理函数
  bindViewTap: function () {
    wx.navigateTo({
      url: '../logs/logs'
    })
  },
  tap: function () {
    this.setData({
      toView: "top"
    })
  },
  onLoad: function () {
    this.getBanner_List(); 
  }
  
})
