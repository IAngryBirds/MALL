using ILBLI.IRepository;
using ILBLI.Model;
using ILBLI.SqlSugar;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System.Collections.Generic;

namespace ILBLI.Repository
{
    public class LogoRepository : DBRepository<Logo>, ILogoRepository
    { 
        public LogoRepository(IOptions<ConnectionConfig> config,ILogger<LogoRepository> logger) : base(config,logger) { }

        public List<Logo> GetPage()
        {
            return base.FindAll(); 
        }

        
    }
}
