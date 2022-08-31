using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class QustionEffect
    {
        /// <summary>
        /// 問卷影響類別流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 影響類別內流水號
        /// </summary>
        public int TableId { get; set; }
        /// <summary>
        /// 影響效果
        /// </summary>
        public string Description { get; set; } = null!;
        /// <summary>
        /// 問卷流水號 ( 外鍵 )
        /// </summary>
        public int QuestionId { get; set; }
        /// <summary>
        /// 影響資料表流水號 ( 外鍵 )
        /// </summary>
        public int EffectTableId { get; set; }

        public virtual EffectTable EffectTable { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
    }
}
