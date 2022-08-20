

        using DPL.EF;
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;

        namespace Common.Model.AdminSide
        {
            public class CreateCardEffectArgs : CardEffect
            {
                public int Id { get;set; } 
public int TableId { get;set; } 
public string Description { get;set; } 
public int CardId { get;set; } 
public int EffectTableId { get;set; } 

            }

            public class UpdateCardEffectArgs : CardEffect
            {
                public int Id { get;set; } 
public int TableId { get;set; } 
public string Description { get;set; } 
public int CardId { get;set; } 
public int EffectTableId { get;set; } 

            }

            /// <summary>
            /// 查詢條件
            /// </summary>
            public class ReadCardEffectArgs
            {
                public string Key { get; set; } = "Id";
                public string JsonString { get; set; } = "[1,2,3]";
            }
        }

