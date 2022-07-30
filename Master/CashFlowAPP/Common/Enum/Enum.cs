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
        /// <summary>
        /// 驗證碼錯誤
        /// </summary>
        ValidateFail = 2003,
        /// <summary>
        /// 識別值無法重複建立
        /// </summary>
        IsCreated = 2004,
        /// <summary>
        /// 格式驗證錯誤
        /// </summary>
        FormatValidationError = 2005,
    }
    public enum StatusCode
    {
        /// <summary>
        /// 啟用
        /// </summary>
        Enable = 1,
        /// <summary>
        /// 未啟用
        /// </summary>
        Disable = 0,
    }

}
