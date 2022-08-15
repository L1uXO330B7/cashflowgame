using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model.AdminSide
{
    public class QuestionsModel
    {
        public class ReadQuestionArgs
        {
            public string Key { get; set; } = "Id";
            public string JsonString { get; set; } = "[1,2,3]";
        }
    }
}
