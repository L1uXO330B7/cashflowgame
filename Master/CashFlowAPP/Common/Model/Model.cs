using DPL.EF;

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
        /// <summary>
        /// 排序
        /// </summary>
        public object? OrderBy { get; set; }
        /// <summary>
        /// 第幾頁
        /// </summary>
        public int? PageIndex { get; set; } = 1;
        /// <summary>
        /// 一頁幾筆
        /// </summary>
        public int? PageSize { get; set; } = 50;
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
        public bool Success { get; set; } = false;
        /// <summary>
        /// 結果代碼 ( 0000 = 成功，其餘為錯誤代號 )
        /// </summary>
        public int? Code { get; set; }
        /// <summary>
        /// 訊息
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 資料時間
        /// </summary>
        public string DataTime { get; set; } = DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]");
        /// <summary>
        /// 資料本體
        /// </summary>
        public object? Data { get; set; }
        /// <summary>
        /// 資料本體數目
        /// </summary>
        public int? TotalDataCount { get; set; }
    }
    public class SmtpConfig
    {
        /// <summary>
        /// 連接埠
        /// </summary>
        public string? Port { get; set; }
        /// <summary>
        /// 是否使用 SSL
        /// </summary>
        public string? IsSSL { get; set; }
        /// <summary>
        /// 管理員信箱以逗點分號
        /// </summary>
        public string? AdminEmails { get; set; }
        /// <summary>
        /// 郵件伺服器網址
        /// </summary>
        public string? Server { get; set; }
        /// <summary>
        /// 郵件伺服器帳號
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        /// 郵件伺服器密碼
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// 寄件人信箱
        /// </summary>
        public string? SenderEmail { get; set; }
    }

    public class MailBag
    {
        public List<Mail>? Mails { get; set; }
    }

    public class Mail
    {
        /// <summary>
        /// 寄信對象
        /// </summary>
        public string SendToEmail { get; set; }
        /// <summary>
        /// 寄信對象姓名
        /// </summary>
        public string SendToName { get; set; }
        /// <summary>
        /// 信件標題
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 信件內容
        /// </summary>
        public string Content { get; set; }
    }
    public class RandomItem<T>
    {
        public T SampleObj { get; set; }
        public decimal Weight { get; set; } = 0;
    }
}
