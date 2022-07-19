using DPL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IUsersService
    {
        public void test(string username, CashFlowDbContext db);
    }
}
