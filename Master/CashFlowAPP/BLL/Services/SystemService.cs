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
            var test = Type.GetType("DPL.EF, DPL");
        }
    }
}
