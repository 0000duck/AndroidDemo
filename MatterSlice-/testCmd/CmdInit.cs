using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myconn
{
    public class CmdInit : CmdBase
    {
        public CmdInit()
        {

        }

        public override byte[] GetCmdStr()
        {
            return new byte[1];
        }


    }
}
