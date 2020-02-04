using System.Data;

namespace ILBLI.Core
{ 
    #region 新版本 modify :20190730 ILBLI  将获取数据库连接部分放置到数据执行操作层
 
    /// <summary>
    /// 将DataBaseCommon设置为部分类
    /// </summary>
    public partial class DataBaseCommon
    {  
        /// <summary>
        /// 操作类型
        /// </summary>
        public DBOperationMode _dbMode { get; set; }
       
        /// <summary>
        /// 参数化查询拼接字符 @ / ；
        /// </summary>
        public string _parameterChar { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataBaseCommon()
        { 
            SetDialect(); 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mode"></param>
        public DataBaseCommon(DBOperationMode mode)
        { 
            _dbMode = mode;
            SetDialect(); 
        }

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        public IDbConnection CreateDbOperation()
        {
            return DataBaseFactory.CreateDBConnection(_dbMode);
        }

        /// <summary>
        /// 设置不同数据库的拼接字符串
        /// </summary>
        private void SetDialect()
        {
            switch (DBConfigeString.DataBaseType)
            {
                case DataBaseType.SqlServer:
                case DataBaseType.MySql:
                    _parameterChar = "@";
                    break;
                case DataBaseType.Oracle:
                    _parameterChar = ":";
                    break;
                default:
                    _parameterChar = "@";
                    break;
            }
        }

    }


    #endregion
}
