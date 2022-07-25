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
        public DateTime DataTime { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// 資料本體
        /// </summary>
        public object? Data { get; set; }
        /// <summary>
        /// 資料本體數目
        /// </summary>
        public int? TotalDataCount { get; set; }
    }
    public class SMTP
    {
            public string Port{get;set;}
            public string IsSSL{get;set;}
            public string AdminMails{get;set;}
            public string Server{get;set;}
            public string Account{get;set;}
            public string Password{get;set;}
            public string SenderMail{get;set;}
    }
}
