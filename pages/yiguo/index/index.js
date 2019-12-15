
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
    newgoods: [],
    hotgoods: [],
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
   
  getNew_List: function () {
    let that = this;
    util.request(api.GetNews).then(function (res) {
      if (res.Code === 200) {
        that.setData({
          newgoods: res.ResultValue
        })
      }
    });
  },

  getHots_List: function () {
    let that = this;
    util.request(api.GetHots).then(function (res) {
      if (res.Code === 200) {
        that.setData({
          hotgoods: res.ResultValue
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
    this.getNew_List();
    this.getHots_List();
  }
  
})
