using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class AnswerQuestion
    {
        /// <summary>
        /// 使用者問卷答案流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 答案:[&quot;1個&quot;]
        /// </summary>
        public string Answer { get; set; } = null!;
        /// <summary>
        /// 問卷流水號 ( 外鍵 )
        /// </summary>
        public int QusetionId { get; set; }
        /// <summary>
        /// 使用者流水號 ( 外鍵 )
        /// </summary>
        public int UserId { get; set; }
    }
}
