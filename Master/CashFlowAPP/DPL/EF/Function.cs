using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class Function
    {
        public Function()
        {
            RoleFunctions = new HashSet<RoleFunction>();
        }

        /// <summary>
        /// 功能流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 功能解釋:封鎖帳號...
        /// </summary>
        public string Description { get; set; } = null!;
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }

        public virtual ICollection<RoleFunction> RoleFunctions { get; set; }
    }
}
