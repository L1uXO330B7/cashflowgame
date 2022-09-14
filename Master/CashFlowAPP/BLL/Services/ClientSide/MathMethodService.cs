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
    }
}
