using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    /// <summary>
    /// API呼叫時，統一要求
    /// 參考：https://blog.darkthread.net/blog/is-restful-required/
    /// </summary>
    public class ApiRequest<T>
    {
        /// <summary>
        /// 參數本體
        /// </summary>
        public T Args { get; set; }
        public object OrderBy { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        public ApiRequest()
        {
        }
    }

    /// <summary>
    /// API呼叫時，傳回的統一物件
    /// 參考：https://blog.darkthread.net/blog/is-restful-required/
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 執行成功與否
        /// </summary>
        public bool Succ { get; set; }
        /// <summary>
        /// 結果代碼 ( 0000 = 成功，其餘為錯誤代號 )
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 資料時間
        /// </summary>
        public DateTime DataTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 資料本體
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 資料本體數目
        /// </summary>
        public long TotalDataCount { get; set; }

        public ApiResponse()
        {
        }

        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data"></param>
        public ApiResponse(object data)
        {
            Code = "0000";
            Succ = true;
            DataTime = DateTime.Now;
            Data = data;
        }

        /// <summary>
        /// 建立失敗結果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ApiResponse(string code, string message)
        {
            Code = code;
            Succ = false;
            this.DataTime = DateTime.Now;
            Data = null;
            Message = message;
            TotalDataCount = 0;
        }
    }
}
