using DPL.EF;
using System.ComponentModel.DataAnnotations;

namespace Common.Model
{
    public class ClientSideModel
    {
        public class ClientUserLogin
        {
            [Required(ErrorMessage ="帳號格式錯誤")]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            //[RegularExpression("Aa-Zz0-9{9}")]
            public string Password { get; set; }

        }
        public class UserSignUpDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
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
}
