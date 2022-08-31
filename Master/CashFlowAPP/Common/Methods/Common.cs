using Common.Model;
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

        /// <summary>
        /// 首字母大寫
        /// 參考：https://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case-with-maximum-performance/4135491#4135491
        /// </summary>
        public static string FirstCharToLower(this string input)
        => input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToLower(), input.AsSpan(1))
        };

        /// <summary>
        /// 轉換資料型態字串
        /// </summary>
        /// <param name="dataType">資料型態</param>
        /// <param name="returnType">回傳型態 1.SQL型態 2.變數宣告 3.變數預設值 7.Angular TypeScript 型別 8.Angular InputType</param>
        /// <returns></returns>
        public static string GetSqlDataTypeString(string dataType, int returnType)
        {
            string typeClassString = "";
            string typeDetailsString = "";
            string typeDefaultString = "";
            string returnString = "";
            string typeAngularString = "";
            string typeAngularInputString = "";

            if (dataType.Contains("Byte"))
            {
                typeClassString = "SqlDbType.TinyInt"; // SQL 型態
                typeDetailsString = "byte"; // 變數宣告
                typeDefaultString = "Default.MyByte"; // 變數預設值
                typeAngularString = "number"; // Angular TypeScript 型別
                typeAngularInputString = "number"; // Angular InputType
            }
            else if (dataType.Contains("Int16"))
            {
                typeClassString = "SqlDbType.SmallInt";
                typeDetailsString = "short";
                typeDefaultString = "Default.MyShort";
                typeAngularString = "number";
                typeAngularInputString = "number";
            }
            else if (dataType.Contains("Int32"))
            {
                typeClassString = "SqlDbType.Int";
                typeDetailsString = "int";
                typeDefaultString = "Default.MyInt";
                typeAngularString = "number";
                typeAngularInputString = "number";
            }
            else if (dataType.Contains("Int64"))
            {
                typeClassString = "SqlDbType.BigInt";
                typeDetailsString = "long";
                typeDefaultString = "Default.MyLong";
                typeAngularString = "number";
                typeAngularInputString = "number";
            }
            else if (dataType.Contains("String"))
            {
                typeClassString = "SqlDbType.NVarChar";
                typeDetailsString = "string";
                typeDefaultString = "Default.MyString";
                typeAngularString = "string";
                typeAngularInputString = "text";
            }
            else if (dataType.Contains("Guid"))
            {
                typeClassString = "SqlDbType.UniqueIdentifier";
                typeDetailsString = "Guid";
                typeDefaultString = "Default.MyGuid";
                typeAngularString = "string";
                typeAngularInputString = "text";
            }
            else if (dataType.Contains("Boolean"))
            {
                typeClassString = "SqlDbType.Bit";
                typeDetailsString = "bool";
                typeDefaultString = "Default.MyBoolean";
                typeAngularString = "boolean";
                typeAngularInputString = "number";
            }
            else if (dataType.Contains("DateTime"))
            {
                typeClassString = "SqlDbType.DateTime";
                typeDetailsString = "DateTime";
                typeDefaultString = "Default.MyDateTime";
                typeAngularString = "Date";
                typeAngularInputString = "Date";
            }
            else if (dataType.Contains("Double"))
            {
                typeClassString = "SqlDbType.Float";
                typeDetailsString = "double";
                typeDefaultString = "Default.MyDouble";
                typeAngularString = "number";
                typeAngularInputString = "number";
            }
            else if (dataType.Contains("Decimal"))
            {
                typeClassString = "SqlDbType.Decimal";
                typeDetailsString = "decimal";
                typeDefaultString = "Default.MyDecimal";
                typeAngularString = "number";
                typeAngularInputString = "number";
            }
            else if (dataType.Contains("Byte[]"))
            {
                typeClassString = "SqlDbType.Image";
                typeDetailsString = "byte[]";
                typeDefaultString = "Default.MyBytes";
                typeAngularString = "string";
                typeAngularInputString = "text";
            }
            else
            {
                typeClassString = "無法解析";
                typeDetailsString = "無法解析";
                typeDefaultString = "無法解析";
                typeAngularString = "無法解析";
                typeAngularInputString = "text";
            }

            if (dataType.Contains("Nullable"))
            {
                typeDetailsString += "?";
            }

            if (returnType == 1)
                returnString = typeClassString;
            else if (returnType == 2)
                returnString = typeDetailsString;
            else if (returnType == 7)
                returnString = typeAngularString;
            else if (returnType == 8)
                returnString = typeAngularInputString;
            else
                returnString = typeDefaultString;

            return returnString;
        }

        /// <summary>
        /// 控制随机抽中几率。
        /// 參考：https://www.cnblogs.com/over140/archive/2009/02/13/1387779.html
        /// 參考：https://ksjolin.pixnet.net/blog/post/150115680
        /// </summary>
       
        public static T RandomWithWeight<T>(List<RandomItem<T>> Items)
        {
            // TODO: 用一個 LIST 裝抽過的，收集抽到的 ITEM，降低他的權重，減少重複抽到的機會
            var _Random = new Random(Guid.NewGuid().GetHashCode()); // 讓隨機機率離散
            var Dices = new List<RandomItem<T>>();
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                var Dice = new RandomItem<T>();
                Dice.Weight = _Random.Next(100) * Items[i].Weight;
                Dice.SampleObj = Items[i].SampleObj;
                Dices.Add(Dice);
            }
            // .Next(100)=>0~100 抽一個數字 * 樣本權重，得到新權重，再從中抽出最大值，為該次抽出樣本
            var Result = Dices
                .OrderByDescending(x => x.Weight)
                .FirstOrDefault();

            return Result.SampleObj;
        }
    }
}
