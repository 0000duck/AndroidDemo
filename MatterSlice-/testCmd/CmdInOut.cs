using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myconn
{
    public class CmdInOut : CmdBase
    {
        public CmdInOut()
        {

        }

        /*1.	输出命令：							0xC3	
         * 规定输出通道置位或清零
         * 通道对应值：00=X轴，01=Y轴
         * 通道输出值：00=输出低电平，01=输出高电平
         * 命令数据域内容（2字节）=设置的输入通道号+置位/清零值
         */
        public byte[] GetSleepOneCmd(byte axis,byte value)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xC3;
            cmd[4] = axis;
            cmd[4] = value;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }
    }
}
