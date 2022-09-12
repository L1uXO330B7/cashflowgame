using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class CardEffect
    {
        /// <summary>
        /// 卡片影響類別流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 影響類別內流水號 ( 外鍵 )
        /// </summary>
        public int TableId { get; set; }
        /// <summary>
        /// 影響效果
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 卡片流水號 ( 外鍵 )
        /// </summary>
        public int CardId { get; set; }
        /// <summary>
        /// 影響資料表流水號 ( 外鍵 )
        /// </summary>
        public int EffectTableId { get; set; }
        /// <summary>
        /// 影響值
        /// </summary>
        public decimal? Value { get; set; }

        public virtual Card Card { get; set; }
        public virtual EffectTable EffectTable { get; set; }
    }
}
