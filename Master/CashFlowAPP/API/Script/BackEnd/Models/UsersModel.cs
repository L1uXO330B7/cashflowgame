

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
                public int Id { get;set; } 
public string Email { get;set; } 
public string Password { get;set; } 
public string Name { get;set; } 
public byte Status { get;set; } 
public int RoleId { get;set; } 

            }

            public class UpdateUserArgs : User
            {
                public int Id { get;set; } 
public string Email { get;set; } 
public string Password { get;set; } 
public string Name { get;set; } 
public byte Status { get;set; } 
public int RoleId { get;set; } 

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

