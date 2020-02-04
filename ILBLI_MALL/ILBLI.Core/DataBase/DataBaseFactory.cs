using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ILBLI.Core
{
    public class DataBaseFactory
    {
        #region 读取数据库配置文件 
        private static readonly string _DefaultConnectionString = ConfigManager.GetConfigAppsettingValueByKey("DefaultConnection");
      
        private static readonly string _ReadConnectionString = ConfigManager.GetConfigAppsettingValueByKey("ReadOnlyConnection");  

        private static readonly string _WriteConnectionString = ConfigManager.GetConfigAppsettingValueByKey("WriteOnlyConnection");

        private static readonly string _WriteRaadConnectionString = ConfigManager.GetConfigAppsettingValueByKey("WriteReadConnection");
        #endregion

        /// <summary>
        /// 返回指定的连接字符串
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private static string CreateConnectionString(DBOperationMode mode = DBOperationMode.Default)
        {
            switch (mode)
            {
                case DBOperationMode.WriteOnly:
                    return _WriteConnectionString;
                case DBOperationMode.ReadOnly:
                    return _ReadConnectionString;
                case DBOperationMode.WriteRead:
                    return _WriteRaadConnectionString;
                default:
                    return _DefaultConnectionString;
            }
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IDbConnection CreateDBConnection(DBOperationMode mode)
        {
            try
            {
                switch (DBConfigeString.DataBaseType)
                {
                    case DataBaseType.MySql:
                        return new MySqlConnection(CreateConnectionString(mode));
                    case DataBaseType.SqlServer:
                        return new SqlConnection(CreateConnectionString(mode));
                    case DataBaseType.Oracle:
                        return new OracleConnection(CreateConnectionString(mode));
                    default:
                        return new MySqlConnection(CreateConnectionString(mode));
                }
            }
            catch(Exception e)
            {
                throw new Exception( $"创建数据库连接失败-{e.Message}" );
            }
        }


    }
}
