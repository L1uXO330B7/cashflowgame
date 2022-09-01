using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class Question
    {
        public Question()
        {
            AnswerQuestions = new HashSet<AnswerQuestion>();
            QustionEffects = new HashSet<QustionEffect>();
        }

        /// <summary>
        /// 問卷流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 問卷類型 1. 單選 2. 多選 3. 自由填文字 4. 數字
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 題目名稱 ,Ex.生幾個小孩
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 選項答案 :[&quot;1個,2個&quot;]
        /// </summary>
        public string Answer { get; set; } = null!;
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }

        public virtual ICollection<AnswerQuestion> AnswerQuestions { get; set; }
        public virtual ICollection<QustionEffect> QustionEffects { get; set; }
    }
}
