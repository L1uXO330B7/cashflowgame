using MimeKit;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Methods
{
    public static class Method
    {
        /// <summary>
        /// 獲取幾位數數字驗證碼
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateValidateCode(int length)
        {
            string validateCode = "";
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                validateCode += r.Next(0, 9).ToString();
            }
            return validateCode;
        }
        /// <summary>
        /// 取得 MimeMessage 信件範例
        /// </summary>
        /// <returns></returns>
        public static MimeMessage GetMimeMessageSample()
        {
            // 建立郵件
            var _MimeMessage = new MimeMessage();

            // 添加寄件者 - 多於一個的時候，實際的寄件者要設在Sender
            _MimeMessage.From.Add(new MailboxAddress("寄件者名稱", "寄件者信箱"));

            // 實際的寄件人
            _MimeMessage.Sender.Address = "寄件者信箱";
            _MimeMessage.Sender.Name = "寄件者名稱";

            // 添加收件者
            _MimeMessage.To.Add(new MailboxAddress("收件者名稱", "收件者信箱"));

            // 設定    副本
            _MimeMessage.Cc.Add(new MailboxAddress("收件者名稱", "收件者信箱"));

            // 設定    秘密副本
            _MimeMessage.Bcc.Add(new MailboxAddress("收件者名稱", "收件者信箱"));

            // 設定    回覆的收件者 (預設為 From)
            _MimeMessage.ReplyTo.Add(new MailboxAddress("收件者名稱", "收件者信箱"));

            // 設定郵件標題
            _MimeMessage.Subject = "郵件標題";

            // 使用 BodyBuilder 建立郵件內容
            var _BodyBuilder = new BodyBuilder();

            // 設定文字內容
            _BodyBuilder.TextBody = "文字內容";

            // 設定 HTML 內容
            _BodyBuilder.HtmlBody = "<p> HTML 內容 </p>";

            // 設定附件
            _BodyBuilder.Attachments.Add("檔案路徑");

            // 設定郵件內容 (文字、附件、 HTML ... 等)
            _MimeMessage.Body = _BodyBuilder.ToMessageBody();

            return _MimeMessage;
        }

        /// <summary>
        /// Serilog 程式註冊 ( 不需要設定 appsettings )
        /// FATAL(致命错误) > ERROR（一般错误） > Warning（警告） >
        /// Information（一般信息） > DEBUG（调试信息）>Verbose（详细模式，即全部）
        /// </summary>
        public static ILogger LogInit(
        )
        {
            string SerilogOutputTemplate = "{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);

            return new LoggerConfiguration()
                   .MinimumLevel.Verbose()
                   .Enrich.FromLogContext()
                   .WriteTo.Console(LogEventLevel.Verbose, SerilogOutputTemplate) // 將 Log 輸出到終端機
                   .WriteTo.File(
                            "Logs/Log-.txt",
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: SerilogOutputTemplate
                   )
                   .WriteTo.File(
                            "Logs/Information/Information-.txt",
                            restrictedToMinimumLevel: LogEventLevel.Information,
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: SerilogOutputTemplate
                   )
                   .WriteTo.File(
                            "Logs/Debug/Debug-.txt",
                            restrictedToMinimumLevel: LogEventLevel.Debug,
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: SerilogOutputTemplate
                   )
                   .WriteTo.File(
                            "Logs/Error/Error-.txt",
                            restrictedToMinimumLevel: LogEventLevel.Error,
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: SerilogOutputTemplate
                   )
                   .CreateLogger();
        }

        /// <summary>
        /// 沒有目錄則創建
        /// </summary>
        /// <param name="Root"></param>
        public static void CreateWithoutDirectory(string Root)
        {
            var IsExists = Directory.Exists(Root); // 檢查是否有該路徑的檔案或資料夾
            if (!IsExists)
            {
                //存檔資料夾不存在，新增資料夾
                Directory.CreateDirectory(Root);
            }
        }
    }
}
