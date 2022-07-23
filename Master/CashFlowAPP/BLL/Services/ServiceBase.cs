using DPL.EF;
using System.Text;

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
