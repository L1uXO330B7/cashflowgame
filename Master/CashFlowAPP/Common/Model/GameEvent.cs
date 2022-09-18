using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class CategoryMix
    {
        public List<AssetAndCategoryModel> Assets { get; set; }
        public List<CashFlowAndCategoryModel> CashFlows { get; set; }
    }

    public class AssetAndCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public string AssetCategoryName { get; set; }
        public int AssetCategoryId { get; set; }
        public int ParentId { get; set; }
        public string GuidCode { get; set; }
    }

    public class CashFlowAndCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public string CashFlowCategoryName { get; set; }
        public int CashFlowCategoryId { get; set; }
        public int ParentId { get; set; }
        public string GuidCode { get; set; }
    }

    public class FiInfo
    {
        public string ConnectId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public decimal? CurrentMoney { get; set; } = 0;
        public decimal? TotalIncomce { get; set; } = 0;

        public decimal? TotalEarnings { get; set; } = 0;
        public decimal? TotalExpense { get; set; } = 0;

        /// <summary>
        /// 最高現金
        /// </summary>
        public decimal? TotoalNetProfit { get; set; } = 0;
        /// <summary>
        /// 最高負債
        /// </summary>
        public decimal? Debt { get; set; } = 0;
        /// <summary>
        /// 最高收入
        /// </summary>
        public decimal? Revenue { get; set; } = 0;
        /// <summary>
        /// 最高淨收入
        /// </summary>
        public decimal? NetProfit { get; set; } = 0;
        public List<CashFlowAndCategoryModel> CashFlowIncome { get; set; }
        public List<CashFlowAndCategoryModel> CashFlowExpense { get; set; }
        public List<AssetAndCategoryModel> Asset { get; set; }
        public List<AssetAndCategoryModel> Liabilities { get; set; }
        public int? NowCardId { get; set; }
        public AssetAndCategoryModel? NowCardAsset { get; set; }
        public CashFlowAndCategoryModel? ValueInterest { get; set; }
    }

    public class CardInfo
    {
        // 秀到前端
        public dynamic? Value { get; set; } = null;
        // 計算的整包資產
        public AssetAndCategoryModel? NowCardAsset { get; set; }
        // 計算的整包利息
        public CashFlowAndCategoryModel? ValueInterest { get; set; }
    }

    public class TopTotoalNetProfit
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public decimal UserValue { get; set; }
    }

    public class TopDebt
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }

        public decimal UserValue { get; set; }
    }

    public class TopRevenue
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public decimal UserValue { get; set; }
    }

    public class TopNetProfit
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public decimal UserValue { get; set; }
    }

    public class TopUser
    {
        public TopTotoalNetProfit? TotoalNetProfitUser { get; set; }
        public TopDebt? DebtUser { get; set; }
        public TopRevenue? RevenueUser { get; set; }
        public TopNetProfit? NetProfitUser { get; set; }
    }

    public class AssetForTrading
    {
        public AssetAndCategoryModel? BuyAsset { get; set; }
        public CashFlowAndCategoryModel? ValueInterest { get; set; }
        public int UserId { get; set; }
        public string ConnectId { get; set; }
    }

    public class BuyAsset
    {
        public FiInfo BuyerFiInfo { get; set; }
        public FiInfo SellerFiInfo { get; set; }
    }
}
