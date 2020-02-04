配置信息(Oracle):
 "ConnectionConfig": {
    "ConnectionString": "Data Source=10.190.228.33:1521/geely;User Id=OAPortal;Password=aBqCxEBBop8UEdVN;",
    "DbType": "Oracle", //设置数据库类型
    "IsAutoCloseConnection": true, //自动释放数据务，如果存在事务，在事务结束后释放
    "InitKeyType": "InitKeyType.Attribute" //从实体特性中读取主键自增列信息
  }
  //第二种配置：主从读写分离配置（但这里有一个问题，就是HitRate这个权重设置在配置文件中配置无效）
  "ConnectionConfig": {
    "ConnectionString": "server=127.0.0.1;port=3306;  userid=root;password=123456;database=ILBLI", //主连接
    "DbType": "MySql",
    "IsAutoCloseConnection": "true",
    "SlaveConnectionConfigs": [ //从连接
      {
        "HitRate": 10, //这个设置无效，
        "ConnectionString": "server=127.0.0.1;port=3306;userid=jie;password=123456;database=ILBLI"
      },
      {
        "HitRate": 10,
        "ConnectionString": "server=127.0.0.1;port=3306;userid=zhangjie;password=123456;database=ILBLI"
      }
    ]
  },
1. 使用appsetting.json 作为配置信息：那么需要在StartUp中添加
        private readonly IConfiguration _Configuration;
        public Startup(IConfiguration configuration)
        {
            this._Configuration = configuration;
        }
2. 在StartUp.cs 中的ConfigureServices 方法中注册添加SqlSugar服务：
        //配置SqlSuager初始化数据
        services.AddSqlSurgar(this._Configuration);

3. 使用方式：
    3.1 在 ILBLI.IRepository 仓储接口层定义接口 ，继承 IDBRepository<Logo>接口 ,并可以扩展接口方法，
        注意这里需要继承实现Unity.IRepository接口 是为了进行Autofac依赖注入，实现服务层可以通过容器获取该实例
            public interface ILogoRepository : IDBRepository<Logo>, Unity.IRepository
            {
                List<Logo> GetPage();
            }
    3.2 在 ILBLI.Repository 仓储实现层实现接口 ，继承 DBRepository<Logo> 它已经提供了默认的实现IDBRepository<Logo>接口
            public class LogoRepository : DBRepository<Logo>, ILogoRepository
            { 
                public LogoRepository(IOptions<ConnectionConfig> config) : base(config) { }

                public List<Logo> GetPage()
                {
                    return base.FindAll(); 
                }
            }
    3.3 在 ILBLI.Service 服务实现层，就可以通过构造函数注入的方式获取该数据库仓储层实例
            public class LogoService :ILogoService
            {
                private readonly ILogoRepository _Logo = null;
                public LogoService(ILogoRepository loger)
                {
                    this._Logo = loger;
                }

                public List<Logo> QueryLogoList()
                {
                    return _Logo.GetPage();
                }
            }
4. 这里为了能通过容器注入获取实例，需要在Autofac中注入该仓储层：
    
    /// <summary>
    /// 自定义注入Autofac
    /// </summary>
    public static class UseAutofacInit
    {
        /// <summary>
        /// 需要注入的Assembly程序集
        /// </summary>
        private static readonly List<string> _LibNameList = new List<string>() { "ILBLI.Repository", "ILBLI.Service" }; 

        /// <summary>
        /// core 3.0下替换自己的Autofac
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder AddAutofacInit(this ContainerBuilder builder)
        {
            // 获取项目所有程序集，排除Microsft,Nuget下载的，并且程序集以ILBLI开头
            //var libs = DependencyContext.Default.CompileLibraries?.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Name.StartsWith("ILBLI."));

            List<Assembly> assemblys = new List<Assembly>();
            foreach (var libName in _LibNameList)
            {
                assemblys.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(libName)));
            }


            if (assemblys.Count > 0)
            {
                //将接口注入仓储的所有接口进行注入//过滤：留下继承了IBaseRepository/IService接口的类
                //AsImplementedInterfaces()让具体实现类型，可以该类型继承的所有接口类型找到该实现类型
                builder.RegisterAssemblyTypes(assemblys.ToArray())
                    .Where(x => typeof(IRepository).IsAssignableFrom(x) || typeof(IService).IsAssignableFrom(x))
                    .AsImplementedInterfaces();
            }

            return builder;
        }

    }