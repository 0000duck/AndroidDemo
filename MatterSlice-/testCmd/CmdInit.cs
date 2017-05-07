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

        //0xA0
        public byte[] GetAddrCmd(byte commandAddr)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xA0;
            cmd[4] = commandAddr;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }

        //0xA1
        public byte[] GetBaudRateCmd(byte baudRateCmd)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA1;
            cmd[4] = baudRateCmd;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }

        //0xA2====//axisValue：坐标系对应值: 00=绝对坐标，01=相对坐标
        public byte[] GetXAxisCmd(byte axisValue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA2;
            cmd[4] = 0X00;//X轴
            cmd[5] = axisValue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        //0xA2====//axisValue：坐标系对应值: 00=绝对坐标，01=相对坐标
        public byte[] GetYAxisCmd(byte axisValue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA2;
            cmd[4] = 0X01;//X轴
            cmd[5] = axisValue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        //0xA3====设置输入信号有效电平命令
        /*有效电平对应值: 00=4个输入通道低电平有效，正反限位及零开关低电平有效
        01=4个输入通道高电平有效，正反限位及零开关低电平有效
        02=4个输入通道低电平有效，正反限位及零开关高电平有效
        03=4个输入通道高电平有效，正反限位及零开关高电平有效
         */
        public byte[] GetInLevelUsefulCmd(byte value)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA3;
            cmd[4] = value;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }

        //设置输出信号有效电平命令			0xA4		
        /*
         * 该命令设置加电后输出口的状态，设置00H，表示加电输出0电平，01H就表示输出高电平
	     *有效电平对应值: 00=低电平有效，01=高电平有效
         *数据域内容（1字节）=设置的有效电平对应值
         *缺省：设置地址号为0x01的控制板输出通道高电平有效
         */
        public byte[] GetOutLevelUsefulCmd(byte value)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA4;
            cmd[4] = value;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }


        /*设定坐标极限值命令：				0xA5
         * 极限值参数：绝对坐标6字节，最大为00000/+65535(表达为：00000/065535)
	     *极限值参数：相对坐标6字节，最大为-32768/+32768(表达为：832768/032768)
         *通道对应值：00=X轴，01=Y轴
         *数据域内容（9字节）=电机通道+设置的正向/负向坐标极限值
         * axis:0x00 -- x;0x01--y;
         */
        public byte[] GetRelativeAxLitCmd(byte axis)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x0b;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA5;
            cmd[4] = axis;
            cmd[5] = 0x83;
            cmd[6] = 0x27;
            cmd[7] = 0x68;
            cmd[8] = 0x03;
            cmd[9] = 0x27;
            cmd[10] = 0x68;
            cmd[11] = Global.GetCheckSum(cmd);
            return cmd;
        }
        public byte[] GetAbsoluteAxLitCmd(byte axis)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x0b;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA5;
            cmd[4] = axis;
            cmd[5] = 0x00;
            cmd[6] = 0x00;
            cmd[7] = 0x00;
            cmd[8] = 0x06;
            cmd[9] = 0x55;
            cmd[10] = 0x35;
            cmd[11] = Global.GetCheckSum(cmd);
            return cmd;
        }
	
        /*设置当前位置为坐标零点命令：		0xA6
         * 通道对应值(axis)：00=X轴，01=Y轴==axis
	     *数据域内容（1字节）=设置的电机通道
         */
        public byte[] GetNowZeroCmd(byte axis)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA6;
            cmd[4] = axis;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*读电机坐标值命令：					0xA7
        * 通道对应值(axis)：00=X轴，01=Y轴
        * 命令数据域内容（1字节）=设置的电机通道
        * 响应数据域内容（4字节）=电机坐标值
        */
        public byte[] GetAxisValueCmd(byte axis)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA7;
            cmd[4] = axis;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*读电机参数命令：					0xA8
        * 本命令是对控制板而言，和电机通道无关
        * 响应数据域内容（5字节）=波特率，X坐标系，Y坐标系，输入信号有效电平，输出信号有效电平
        */
        public byte[] GetMachineValueCmd(byte axis)
        {
            byte[] cmd = new byte[5];
            cmd[0] = 0xAA;
            cmd[1] = 0x04;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA8;
            cmd[4] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*读已设定的极限值命令：				0xA9
        * 通道对应值：00=X轴，01=Y轴
        * 命令数据域内容（1字节）=设置的电机通道
        * 响应数据域内容（3*2字节）=负向极限值+正向极限值
        */
        public byte[] GetSettedLimiteValueCmd(byte axis)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xA9;
            cmd[4] = axis;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }
    }
}
