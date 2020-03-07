using ILBLI.IRepository;
using ILBLI.IService;
using ILBLI.Model;
using ILBLI.SnowFlak;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ILBLI.Service
{
    public class LogoService :ILogoService
    {
        private readonly ILogoRepository _Logo = null;
        private readonly ISnowFlak _SnowFlak = null;
        public LogoService(ILogoRepository logo,ISnowFlak snowFlak)
        {
            this._Logo = logo;
            this._SnowFlak = snowFlak;
        }

        public List<Logo> QueryLogoList()
        {
            return _Logo.GetPage();
        }

        public bool Insert()
        {
            _Logo.Inset(new Logo
            {
                LogoID = _SnowFlak.GetNextID().ToString(),
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.UtcNow,
                SortNum=1,
                LogoName_CN="测试的数据",
                LogoName_EN="Test Data",
                LogoURL="www.ilbli.com"
            }) ;
            return true;
        }


        public long GetUID()
        {
            return _SnowFlak.GetNextID();
        }
    }
}
