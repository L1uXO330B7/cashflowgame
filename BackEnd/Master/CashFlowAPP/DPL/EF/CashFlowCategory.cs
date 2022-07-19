using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class CashFlowCategory
    {
        /// <summary>
        /// 現金流類別流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 現金流類別名稱
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 父類別流水號
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 所花費小時
        /// </summary>
        public byte Time { get; set; }
        /// <summary>
        /// 現金流類型 1. 主動 2. 被動
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }
    }
}
