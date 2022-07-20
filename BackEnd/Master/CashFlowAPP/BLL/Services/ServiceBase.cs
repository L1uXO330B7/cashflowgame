using DPL.EF;

namespace BLL.Services
{
    public class ServiceBase
    {
        public CashFlowDbContext cashFlowDbContext;
        public ServiceBase(
            CashFlowDbContext _CashFlowDbContext
        )
        {
            cashFlowDbContext = _CashFlowDbContext;
        }
    }
}
