using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ============================================
 * 通讯协议：
数据帧格式：（8 位数据位，1 位停止位，无校验，默认速率9600）
标 示 符	数据长度	地 址 码	命令字		数 据 域	 校 验 和
(1byte)		(1byte)		(1byte)		(1byte)					(1byte)

1.	数据格式: 			16 进制
2.	标示符：			发送命令为0xAA，回复命令为0xEE
3.	数据长度：		从数据长度字节到校验和字节（包括校验和）的长度
4.	地址码：			采集模块的地址，默认为00
5.	数据域：			根据命令字不同内容和长度相应变化。
6.	校验和：			数据长度、地址码、命令字和数据域的异或，不考虑进位

 ============================================== */



namespace myconn
{
    public class CmdBase
    {
        protected byte[] m_cmdStr;
        protected byte m_mark;
        protected byte m_len;
        protected byte m_addr;

        public CmdBase()
        {
            m_mark = 0xAA;
            m_addr = 0x00;

        }

        public virtual byte[] GetCmdStr()
        {
            return new byte[1];
        }
    }
}
