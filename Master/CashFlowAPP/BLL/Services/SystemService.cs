using DPL.EF;
using System.Reflection;

namespace BLL.Services
{
    public class SystemService
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public SystemService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public void CreateTemplateByTableName()
        {
            // 參考 https://stackoverflow.com/questions/79693/getting-all-types-in-a-namespace-via-reflection
            var types = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && t.Namespace == "DPL.EF")
                       .ToList();
        }
    }
}
