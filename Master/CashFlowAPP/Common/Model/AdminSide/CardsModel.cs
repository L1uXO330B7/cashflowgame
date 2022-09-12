

using DPL.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model.AdminSide
{
    public class CreateCardArgs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public decimal Weight { get; set; }
        public string Type { get; set; } = "交易機會";
    }

    public class UpdateCardArgs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public decimal Weight { get; set; }
        public string Type { get; set; } = "交易機會";
    }

    /// <summary>
    /// 查詢條件
    /// </summary>
    public class ReadCardArgs
    {
        public string Key { get; set; } = "Id";
        public string JsonString { get; set; } = "[1,2,3]";
    }
}

