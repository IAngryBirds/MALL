
1.使用改中Redis操作类库需要注意：
	在使用该Redis的项目中需要添加4个Redis引用分别为
	【ServiceStack.Text.dll】
	【ServiceStack.Redis.dll】
	【ServiceStack.Interfaces.dll】
	【ServiceStack.Common】
	缺少其中一个dll类库，都无法使用该Redis操作类
2.在该Redis公共方法封装类中只需要且必须要引用：
	【ServiceStack.Redis.dll】
	【ServiceStack.Interfaces.dll】

