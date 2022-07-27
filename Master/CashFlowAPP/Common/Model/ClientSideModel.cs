using DPL.EF;

namespace Common.Model
{
    public class ClientSideModel
    {
        public class ClientUserLogin
        {
            public string Email { get; set; }
            public string Password { get; set; }

        }
        public class UserSignUpDTO
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ValidateCode { get; set; }
            public string JWTcode { get; set; }
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
