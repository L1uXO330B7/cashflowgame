using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum
{
    /// <summary>
    /// https://dotblogs.com.tw/abbee/2017/01/06/101219
    /// </summary>
    public enum ResponseStatusCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0000,
        /// <summary>
        /// 找不到
        /// </summary>
        CannotFind = 2001,
        /// <summary>
        /// 例外
        /// </summary>
        ExMessage = 2002,
    }
}
