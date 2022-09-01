

        using DPL.EF;
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;

        namespace Common.Model.AdminSide
        {
            public class CreateRoleFunctionArgs : RoleFunction
            {
                public int Id { get;set; } 
public int RoleId { get;set; } 
public int FunctionId { get;set; } 

            }

            public class UpdateRoleFunctionArgs : RoleFunction
            {
                public int Id { get;set; } 
public int RoleId { get;set; } 
public int FunctionId { get;set; } 

            }

            /// <summary>
            /// 查詢條件
            /// </summary>
            public class ReadRoleFunctionArgs
            {
                public string Key { get; set; } = "Id";
                public string JsonString { get; set; } = "[1,2,3]";
            }
        }

