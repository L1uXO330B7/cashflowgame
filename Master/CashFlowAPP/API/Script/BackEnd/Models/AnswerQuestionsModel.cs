

        using DPL.EF;
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;

        namespace Common.Model.AdminSide
        {
            public class CreateAnswerQuestionArgs : AnswerQuestion
            {
                public int Id { get;set; } 
public string Answer { get;set; } 
public int QusetionId { get;set; } 
public int UserId { get;set; } 

            }

            public class UpdateAnswerQuestionArgs : AnswerQuestion
            {
                public int Id { get;set; } 
public string Answer { get;set; } 
public int QusetionId { get;set; } 
public int UserId { get;set; } 

            }

            /// <summary>
            /// 查詢條件
            /// </summary>
            public class ReadAnswerQuestionArgs
            {
                public string Key { get; set; } = "Id";
                public string JsonString { get; set; } = "[1,2,3]";
            }
        }

