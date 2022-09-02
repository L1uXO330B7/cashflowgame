using System.ComponentModel.DataAnnotations;

namespace Common.Model
{
    public class FromClientChat
    {
        public string sendToID { get; set; }
        public string selfID { get; set; }
        public string message { get; set; }
        public string Token { get; set; }
    }
    public class ClientUserLogin
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號為必填")]
        //[EmailAddress]
        public string Email { get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼為必填")]
        //[RegularExpression("Aa-Zz0-9{9}")]
        public string Password { get; set; }

    }
    public class UserSignUpDto
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號為必填")]
        public string Email { get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼為必填")]
        public string Password { get; set; }
        public string Name { get; set; }
        [Display(Name = "驗證碼")]
        [Required(ErrorMessage = "驗證碼為必填")]
        public string ValidateCode { get; set; }
        public string JwtCode { get; set; }
    }
    public class UserInfo
    {   /// <summary>
        /// 使用者流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 權限角色流水號 ( 外鍵 )
        /// </summary>
        public int RoleId { get; set; }
        public DateTime TokenCreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// Token 存活時間
        /// </summary>
        public int TokenExpiredHours { get; set; } = 3;
    }
}
