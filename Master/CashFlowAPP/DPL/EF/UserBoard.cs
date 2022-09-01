using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class UserBoard
    {
        /// <summary>
        /// 排行榜流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 總時長淨收入
        /// </summary>
        public decimal? TotoalNetProfit { get; set; }
        /// <summary>
        /// 半小時內最高負債
        /// </summary>
        public decimal? Debt { get; set; }
        /// <summary>
        /// 半小時內最高收入
        /// </summary>
        public decimal? Revenue { get; set; }
        /// <summary>
        /// 半小時內最高淨收入
        /// </summary>
        public decimal? NetProfit { get; set; }
        /// <summary>
        /// 使用者流水號 ( 外鍵 )
        /// </summary>
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
