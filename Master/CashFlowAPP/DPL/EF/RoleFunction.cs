using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class RoleFunction
    {
        /// <summary>
        /// 權限功能流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 權限角色流水號 ( 外鍵 )
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 功能流水號 ( 外鍵 )
        /// </summary>
        public int FunctionId { get; set; }

        public virtual Function Function { get; set; }
        public virtual Role Role { get; set; }
    }
}
