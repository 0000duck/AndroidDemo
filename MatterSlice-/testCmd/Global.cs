using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myconn
{
    //GD25Q16芯片有芯片擦除，块擦除及扇区擦除功能，有页编程功能，读功能是可以读出单个字节。
    public static class Global
    {
        public static byte gCommandAddr = 0x00;

        //波特率对应值:	0:2400 1：4800 2：9600；3：19200；4：38400 5：57600
        public static byte gBaudRate = 0x02;

 

        public static byte GetCheckSum(byte[] cmds)
        {
            byte sum = 0;
            for(int i = 1; i < cmds.Count(); i ++)
            {
                sum = (byte)(sum ^ cmds[i]);
            }
            return sum;
        }
    }
}
