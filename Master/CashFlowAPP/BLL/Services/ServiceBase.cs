using Common.Enum;
using Common.Model;
using DPL.EF;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Text;

namespace BLL.Services
{
    public class ServiceBase
    {
        public CashFlowDbContext _CashFlowDbContext;
        public ServiceBase(
            CashFlowDbContext CashFlowDbContext)
        {
            _CashFlowDbContext = CashFlowDbContext;
        }
        public async Task<ApiResponse> SendMail(SMTP smtp,string email)
        {
            var JWTcode="";
            using (var _SmtpClient = new SmtpClient())
            {            // 建立郵件
                var _MimeMessage = new MimeMessage();

                // 添加收件者
                _MimeMessage.To.Add(new MailboxAddress("親愛的社畜們", email));


                // 寄件者
                _MimeMessage.From.Add(new MailboxAddress("錢董", smtp.SenderMail));


                // 設定郵件標題
                _MimeMessage.Subject = "錢董用戶驗證";

                // 使用 BodyBuilder 建立郵件內容
                var _BodyBuilder = new BodyBuilder();
                // 獲取驗證碼
                var ValidateCode = CreateValidateCode(4);

                // 設定 HTML 內容
                _BodyBuilder.HtmlBody = $@"<p>這是錢董驗證碼，如您無觸發程序請無視，感謝配合</p>
                                        <h5>{ValidateCode}</h5>";

                // 設定郵件內容 (文字、附件、 HTML ... 等)
                _MimeMessage.Body = _BodyBuilder.ToMessageBody();

                // JWT 加密
                JWTcode = Jose.JWT.Encode(ValidateCode, Encoding.UTF8.GetBytes("錢董"),Jose.JwsAlgorithm.HS256);

                // 連接 Mail Server ( 郵件伺服器網址, 連接埠, 是否使用 SSL )
                _SmtpClient.Connect(smtp.Server, 587, false);

                // 驗證
                _SmtpClient.Authenticate(smtp.Account, smtp.Password);

                // 寄出郵件
                _SmtpClient.Send(_MimeMessage);

                // 中斷連線
                _SmtpClient.Disconnect(true);
            }
               

            var Res = new ApiResponse();
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功寄出";
            Res.Data = JWTcode;
            return Res;
        }
        public MimeMessage MailSample()
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
        /// 獲取驗證碼
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string CreateValidateCode(int length) 
        {
            string validateCode = "";
            //int[] randMembers = new int[length];
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                validateCode += r.Next(0, 9).ToString();
            }
            return validateCode;
        }
    }
}
