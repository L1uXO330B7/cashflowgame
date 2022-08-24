using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class Asset
    {
        /// <summary>
        /// 資產流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 資產名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 資產價值 ( 台幣 )
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// 資產類別流水號 ( 外鍵 )
        /// </summary>
        public int AssetCategoryId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public virtual AssetCategory AssetCategory { get; set; }
    }
}
