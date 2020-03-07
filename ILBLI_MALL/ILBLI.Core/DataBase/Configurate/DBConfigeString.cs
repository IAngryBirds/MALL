namespace ILBLI.Core
{
    public class DBConfigeString
    {

        public static readonly string _DbType =  ConfigManager.GetConfigAppsettingValueByKey("DataBaseType").ToLower(); 
        public static DataBaseType DataBaseType {

            get
            { 
                switch (_DbType) {
                    case "mysql":
                        return DataBaseType.MySql;
                    case "sqlserver":
                        return DataBaseType.SqlServer;
                    case "oracle":
                        return DataBaseType.Oracle;
                    default:
                        return DataBaseType.MySql;
                }
            }
        }
        

    }
}
