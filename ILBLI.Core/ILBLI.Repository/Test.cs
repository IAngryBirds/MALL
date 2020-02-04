using ILBLI.IRepository;
using Microsoft.Extensions.Logging;

namespace ILBLI.Repository
{
    public class Test : ITest
    {
        private ILogger<Test> _Logger;

        public Test(ILogger<Test> logger)
        {
            this._Logger = logger;
        }
        public void Doing()
        {
            _Logger.LogWarning("just doing ...");
        }
    }
}
