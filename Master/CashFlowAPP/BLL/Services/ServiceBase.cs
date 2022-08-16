using Common.Enum;
using Common.Methods;
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
        public async Task<ApiResponse> SendMail(SmtpConfig smtp, Mail mail)
        {
            var JWTcode = "";
            using (var _SmtpClient = new SmtpClient())
            {
                // 建立郵件
                var _MimeMessage = new MimeMessage();

                // 添加收件者
                _MimeMessage.To.Add(new MailboxAddress($@"親愛的 {mail.SendToName} 社畜", mail.SendToEmail));

                // 寄件者
                _MimeMessage.From.Add(new MailboxAddress("錢董💰", smtp.SenderEmail));

                // 設定郵件標題
                _MimeMessage.Subject = string.IsNullOrEmpty(mail.Title) ? "錢董使用者註冊驗證碼通知" : mail.Title;

                // 使用 BodyBuilder 建立郵件內容
                var _BodyBuilder = new BodyBuilder();

                // 獲取驗證碼
                var ValidateCode = Method.CreateValidateCode(4);

                // 設定 HTML 內容
                _BodyBuilder.HtmlBody = string.IsNullOrEmpty(mail.Content) ? $@"<p>這是錢董驗證碼，如您無觸發程序請無視，感謝配合</p>
                                        <h5>{ValidateCode}</h5>" : mail.Content;

                // 設定郵件內容 (文字、附件、 HTML ... 等)
                _MimeMessage.Body = _BodyBuilder.ToMessageBody();

                // JWT 加密
                JWTcode = Jose.JWT.Encode(ValidateCode, Encoding.UTF8.GetBytes("錢董"), Jose.JwsAlgorithm.HS256);

                // 連接 Mail Server ( 郵件伺服器網址, 連接埠, 是否使用 SSL )
                _SmtpClient.Connect(smtp.Server, int.Parse(smtp.Port), bool.Parse(smtp.IsSSL));

                // 登陸帳號密碼
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
    }
}
