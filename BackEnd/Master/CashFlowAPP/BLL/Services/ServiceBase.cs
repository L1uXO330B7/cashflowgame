using DPL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
