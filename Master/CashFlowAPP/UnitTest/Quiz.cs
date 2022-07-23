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
            var Count = new UsersService(db).test();
            Assert.AreEqual(2, Count);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var Count = new UsersService(db).test();
            Assert.AreEqual(3, Count);
        }
    }

}