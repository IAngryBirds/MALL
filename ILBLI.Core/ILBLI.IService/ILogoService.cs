using ILBLI.Model;
using System.Collections.Generic;

namespace ILBLI.IService
{
    public interface ILogoService : Unity.IService
    { 
        List<Logo> QueryLogoList();
        bool Insert();

        long GetUID();
    } 
}
