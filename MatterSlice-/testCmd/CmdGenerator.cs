using System;
using System.Collections.Generic;
using System.Text;

namespace myconn
{
    public class CmdGenerator
    {
        //3个字节代表长度，+（循环）<一个字节（xyzw,每个两位,每次走一步或者走几步）+ 一个字节（间隔等待时间） >
        //前 3个字节代表文件长度。
        private int length;
        //private static string CmdHead = "01";
        //private  char cmd = '1';
        public CmdGenerator()
        {

        }

        private string GetLen()
        {
            string ret = string.Empty;
            int first = (length >> 2) & 0x00ff;
            int second = (length >> 1) & 0x00ff;
            int third = length & 0x00ff;
            ret += (char)first + (char)second + (char)third;
            return ret;
        }

        private string CalculateChecksum(string dataToCalculate)
        {
            byte[] byteToCalculate = Encoding.ASCII.GetBytes(dataToCalculate);
            int checksum = 0;
            foreach (byte chData in byteToCalculate)
            {
                checksum += chData;
            }
            checksum &= 0xff;
            return checksum.ToString("X2");
        }

        private char GetChecksum(string dataToCalculate)
        {
            byte[] byteToCalculate = Encoding.ASCII.GetBytes(dataToCalculate);
            int checksum = 0;
            foreach (byte chData in byteToCalculate)
            {
                checksum += chData;
            }
            checksum &= 0xff;
            return (char)checksum;
        }


        public string GetAssembledCmd(List<string> content)
        {
            string ret = string.Empty;
            

            return ret;
        }
    }
}
