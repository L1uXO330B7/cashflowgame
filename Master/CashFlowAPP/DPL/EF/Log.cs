using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class Log
    {
        /// <summary>
        /// 日誌流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 使用者流水號 ( 外鍵 )
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 資料表內流水號
        /// </summary>
        public int TableId { get; set; }
        /// <summary>
        /// 資料表名稱
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 動作 1 新增 2 修改 3 刪除
        /// </summary>
        public byte Action { get; set; }
        /// <summary>
        /// 執行動作時間
        /// </summary>
        public DateTime ActionDate { get; set; }
    }
}
