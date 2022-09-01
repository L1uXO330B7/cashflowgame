

        using DPL.EF;
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;

        namespace Common.Model.AdminSide
        {
            public class CreateLogArgs : Log
            {
                public int Id { get;set; } 
public int UserId { get;set; } 
public string UserName { get;set; } 
public int TableId { get;set; } 
public string TableName { get;set; } 
public byte Action { get;set; } 
public DateTime ActionDate { get;set; } 

            }

            public class UpdateLogArgs : Log
            {
                public int Id { get;set; } 
public int UserId { get;set; } 
public string UserName { get;set; } 
public int TableId { get;set; } 
public string TableName { get;set; } 
public byte Action { get;set; } 
public DateTime ActionDate { get;set; } 

            }

            /// <summary>
            /// 查詢條件
            /// </summary>
            public class ReadLogArgs
            {
                public string Key { get; set; } = "Id";
                public string JsonString { get; set; } = "[1,2,3]";
            }
        }

