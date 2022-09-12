using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class CashFlow
    {
        /// <summary>
        /// 現金流流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 現金流名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 現金流價值
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 現金流類別流水號
        /// </summary>
        public int CashFlowCategoryId { get; set; }
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 權重
        /// </summary>
        public decimal? Weight { get; set; }

        public virtual CashFlowCategory CashFlowCategory { get; set; }
    }
}
