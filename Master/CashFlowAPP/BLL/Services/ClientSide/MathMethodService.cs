using DPL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ClientSide
{
    public class MathMethodService : ServiceBase
    {
        private CashFlowDbContext Db;
        public MathMethodService(
        )
        {
            // 在建構子呼叫
            Db = base.GetDbContext();
        }

        private static Random StaticRandom()
        {
            return new Random(Guid.NewGuid().GetHashCode());
        }

        public decimal FoundationCount(decimal YourJobValue, decimal DailyExpeneseValue)
        {
            var _Random = StaticRandom();
            // 金融商品筆數
            var InvestCount = Db.AssetCategories.Where(x => x.ParentId == 46).ToList().Count;
            // 定存價值 = 薪水*隨機數字*
            var Dice = _Random.Next(1, InvestCount);
            var ResultValue = (YourJobValue - DailyExpeneseValue) * Dice;
            return ResultValue;
        }

        public decimal FoundationInterest(decimal FoundationCount)
        {
            var SavingInterest = Math.Round(FoundationCount / 1200, 0);

            return SavingInterest;

        }

        public decimal CarLoanCount(decimal YourJobValue)
        {
            var _Random = StaticRandom();
            var GuidCode = System.Guid.NewGuid().ToString("N");
            var CarValue = Math.Round((YourJobValue * 8), 0); // 車子價格大約是薪水*10
            var Ratio = _Random.Next((int)(YourJobValue * 2), (int)CarValue);
            var CarLoan = (decimal)Ratio * -1;

            // 車貸利息 => (貸款總金額 + 貸款總利息) / 貸款總期數 = 每月月付金
            return CarLoan;

        }
        public decimal CarInterest(decimal CarLoan)
        {   var Ratio = CarLoan * -1;
            var CarInterestValue = Math.Round((((decimal)Ratio * 28 / 1000 / 12) + ((decimal)Ratio / 60)), 0) * -1;

            return CarInterestValue;
        }
    }
}
