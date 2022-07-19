using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class User
    {
        /// <summary>
        /// 使用者流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 密碼 hash
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// 權限角色流水號 ( 外鍵 )
        /// </summary>
        public int RoleId { get; set; }
    }
}
