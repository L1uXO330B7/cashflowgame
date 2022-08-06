using DPL.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model.AdminSide
{
    public class CreateUserArgs : User
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號為必填")]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public byte Status { get; set; }

        public int RoleId { get; set; }
    }

    public class UpdateUserArgs : User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public byte Status { get; set; }
        public int RoleId { get; set; }
    }

    /// <summary>
    /// 查詢條件
    /// </summary>
    public class ReadUserArgs
    {
        public string Key { get; set; } = "Id";
        public string JsonString { get; set; } = "[1,2,3]";
    }
}
