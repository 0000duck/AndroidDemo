using System;
using System.Collections.Generic;
using System.Text;

namespace myconn
{
    // xx xx xx,
    //页，

    public class CommonCmd
    {
        private SerialCom m_SerialCom;
        public CommonCmd(SerialCom com)
        {
            m_SerialCom = com;
        }

        private int WritePage(int pageNum)
        {
            byte[] cmd = new byte[8];
            cmd[0] = 0XAA;
            cmd[0] = 0X07;
            cmd[0] = 0X01;
            cmd[0] = 0XD2;
            cmd[0] = 0X00;
            cmd[0] = 0X00;
            cmd[0] = 0X00;
            cmd[0] = 0XD4;
            return m_SerialCom.SendFile(cmd);
        }

        private int ExcutePage(int pageNum)
        {
            byte[] cmd = new byte[8];
            cmd[0] = 0XAA;
            cmd[0] = 0X06;
            cmd[0] = 0X01;
            cmd[0] = 0XDE;
            cmd[0] = 0X00;
            cmd[0] = 0X00;
            cmd[0] = 0XD9;
            return m_SerialCom.SendFile(cmd);
        }

        private int ReadPage(int pageNum)
        {
            byte[] cmd = new byte[8];
            cmd[0] = 0XAA;
            cmd[0] = 0X06;
            cmd[0] = 0X01;
            cmd[0] = 0XDE;
            cmd[0] = 0X00;
            cmd[0] = 0X00;
            cmd[0] = 0XD9;

            return m_SerialCom.SendFile(cmd);
        }

        private int WriteSector(int sectorNum)
        {
            return 0;
        }

        private int ExcuteSector(int sectorNum)
        {
            return 0;
        }

        private int ReadSector(int sectorNum)
        {
            return 0;
        }

        private int WriteBlock(int blockNum)
        {
            return 0;
        }

        private int ExcuteBlock(int blockNum)
        {
            return 0;
        }

        private int ReadBlock(int blockNum)
        {
            return 0;
        }
    }
}
