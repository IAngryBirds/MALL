using ILBLI.Model;
using ILBLI.SqlSugar;
using System.Collections.Generic;

namespace ILBLI.IRepository
{
    public interface ILogoRepository : IDBRepository<Logo>, Unity.IRepository
    {
        List<Logo> GetPage();
    }
}
