using BLL.Services.AdminSide;
using DPL.EF;
using Microsoft.EntityFrameworkCore;

namespace Quiz
{
    [TestClass]
    public class Test
    {
        public CashFlowDbContext db;
        public Test()
        {
            var Builder = new DbContextOptionsBuilder<CashFlowDbContext>();
            Builder.UseSqlServer("Data Source=150.117.83.67;Initial Catalog=CashFlow;User ID=carl;Password=1165;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            db = new CashFlowDbContext(Builder.Options);
        }

        [TestMethod]
        public void TestMethod()
        {
            Assert.AreEqual(2, 2);
        }
    }

}