using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class Role
    {
        /// <summary>
        /// 權限角色流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 權限角色名稱 管理員、玩家
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }
    }
}
