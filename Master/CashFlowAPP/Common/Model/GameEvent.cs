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
}
