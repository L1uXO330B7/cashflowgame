using DPL.EF;

namespace BLL.Services
{
    public class ServiceBase
    {
        public CashFlowDbContext _CashFlowDbContext;
        public ServiceBase(
            CashFlowDbContext CashFlowDbContext
        )
        {
            _CashFlowDbContext = CashFlowDbContext;
        }
    }
}
