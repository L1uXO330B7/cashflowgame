

        using DPL.EF;
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;

        namespace Common.Model.AdminSide
        {
            public class CreateUserBoardArgs : UserBoard
            {
                public int Id { get;set; } 
public decimal? TotoalNetProfit { get;set; } 
public decimal? Debt { get;set; } 
public decimal? Revenue { get;set; } 
public decimal? NetProfit { get;set; } 
public int UserId { get;set; } 

            }

            public class UpdateUserBoardArgs : UserBoard
            {
                public int Id { get;set; } 
public decimal? TotoalNetProfit { get;set; } 
public decimal? Debt { get;set; } 
public decimal? Revenue { get;set; } 
public decimal? NetProfit { get;set; } 
public int UserId { get;set; } 

            }

            /// <summary>
            /// 查詢條件
            /// </summary>
            public class ReadUserBoardArgs
            {
                public string Key { get; set; } = "Id";
                public string JsonString { get; set; } = "[1,2,3]";
            }
        }

