using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Common.Enums
{
    /// <summary>
    /// 统计类型
    /// </summary>
    public enum StatisticalTime
    {
        /// <summary>
        /// 按年统计
        /// </summary>
        Year=4,
        /// <summary>
        /// 按月份统计
        /// </summary>
        Month=7,
        /// <summary>
        /// 按日统计
        /// </summary>
        Day=10,
        /// <summary>
        /// 按小时统计
        /// </summary>
        Hour=13,
        /// <summary>
        /// 按分钟统计
        /// </summary>
        Minuter=16,
        /// <summary>
        /// 按秒统计
        /// </summary>
        Second=19
    }
}
