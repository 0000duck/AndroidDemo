using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Common.Enums
{
    public enum Permission
    {
        /// <summary>
        /// 读操作     0000 0000 0000 0001 (1)
        /// </summary>
        Read = 0x0001,
        /// <summary>
        /// 写入操作    0000 0000 0000 0010 (2)
        /// </summary>
        Write = 0x0002
    }
}
