namespace ILBLI.Core
{
    public enum DataBaseType
    {
        SqlServer = 100,
        MySql=200,
        Oracle=300
    }

    public enum DBOperationMode
    {
        Default =100,
        WriteOnly=200,
        ReadOnly=300,
        WriteRead=400
    }
}
