/*
Copyright (c) 2017, qwinner

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myconn
{
    public class CmdMove : CmdBase
    {
        public CmdMove()
        {

        }

        public override byte[] GetCmdStr()
        {
            return new byte[1];
        }

        /*1.	单步运动命令：						0xB0	
         * 电机执行一次命令，走1步
         * 通道对应值：00=X轴，01=Y轴
         * 电机走的步长及方向：01=正向走1步，81=反向走1步
         * 命令数据域内容（1字节）=设置的电机通道+步长方向
         * 响应数据域内容（4字节）=电机坐标值
         * positive ture：正向，false:反向
         */
        public byte[] GetOneStepCmd(byte axis, bool positive)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xB0;
            cmd[4] = axis;
            cmd[5] = (byte)(positive ? 0x01 : 0x81);
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*2.	匀速定步长运动命令：				0xB1		
         * 通道对应值(axis)：00=X轴，01=Y轴
         * 匀速度：2字节，十进制
         * 步长方向：4字节，十进制
         * 命令数据域内容（1字节）=设置的电机通道+匀速度+步长方向
         * 响应数据域内容（4字节）=电机坐标值                          
         * 
         */
        public byte[] GetUniformStepCmd(byte axis, string rate, string stepDirection)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x0B;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xB1;
            cmd[4] = axis;

            cmd[5] = Convert.ToByte(rate.Substring(0, 2), 16);
            cmd[6] = Convert.ToByte(rate.Substring(2, 2), 16);
            cmd[7] = Convert.ToByte(stepDirection.Substring(0, 2), 16);
            cmd[8] = Convert.ToByte(stepDirection.Substring(2, 2), 16);
            cmd[9] = Convert.ToByte(stepDirection.Substring(4, 2), 16);
            cmd[10] = Convert.ToByte(stepDirection.Substring(6, 2), 16);
            cmd[11] = Global.GetCheckSum(cmd);
            return cmd;
        }


        /*3.	变速定步长运动命令：				0xB2
           * 通道对应值：00=X轴，01=Y轴
           * 初速：2字节，十进制0000-9999
           *末速：3字节，十进制000000-040000
           *升速斜率：1字节，十进制00-99
           *步长：4字节，十进制，89999999-09999999
           *命令数据域内容（11字节）=设置的电机通道+初速度+末速度+升速斜率+步长方向
           *响应数据域内容（4字节）=电机坐标值            
         * 
         */
        public byte[] GetVrateStillStepCmd(byte axis, string firstRate, string lastRate, string shengjiang,string step)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x0E;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xB2;
            cmd[4] = axis;

            cmd[5] = Convert.ToByte(firstRate.Substring(0, 2), 16);
            cmd[6] = Convert.ToByte(firstRate.Substring(2, 2), 16);
            cmd[7] = Convert.ToByte(lastRate.Substring(0, 2), 16);
            cmd[8] = Convert.ToByte(lastRate.Substring(2, 2), 16);
            cmd[9] = Convert.ToByte(lastRate.Substring(4, 2), 16);
            cmd[10] = Convert.ToByte(shengjiang.Substring(0, 2), 16);
            cmd[11] = Convert.ToByte(step.Substring(0, 2), 16);
            cmd[12] = Convert.ToByte(step.Substring(2, 2), 16);
            cmd[13] = Convert.ToByte(step.Substring(4, 2), 16);
            cmd[14] = Convert.ToByte(step.Substring(6, 2), 16);
            cmd[15] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*4.	匀速运动到规定通道置位命令：		0xB3
         *通道对应值：00=X轴，01=Y轴
         *速度：2字节，十进制0000-9999
         *方向：1字节，十进制000000-040000
         *命令数据域内容（5字节）=设置的电机通道+速度+输入通道号+方向
         *响应数据域内容（4字节）=电机坐标值                          
         * 
         */
        public byte[] GetSrateToAxisCmd(byte axis, string rate, string direction)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x09;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xB3;
            cmd[4] = axis;

            cmd[5] = Convert.ToByte(rate.Substring(0, 2), 16);
            cmd[6] = Convert.ToByte(rate.Substring(2, 2), 16);
            cmd[7] = Convert.ToByte(direction.Substring(0, 2), 16);
            cmd[8] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*5.	变速运动到规定通道置位命令：		0xB4
        *通道对应值：00=X轴，01=Y轴
        *初速：2字节，十进制0000-9999
        *末速：3字节，十进制000000-040000
        *升速斜率：1字节，十进制00-99
        *方向：1字节，十进制，00=正向，80=负向
        *命令数据域内容（8字节）=设置的电机通道+初速度+末速度+升速斜率+方向
        *响应数据域内容（4字节）=电机坐标值                          
         * 
         */
        public byte[] GetVrateToAxisCmd(byte axis, string firstRate, string lastRate, string shengjiang, string step)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x0D;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xB4;
            cmd[4] = axis;

            cmd[5] = Convert.ToByte(firstRate.Substring(0, 2), 16);
            cmd[6] = Convert.ToByte(firstRate.Substring(2, 2), 16);
            cmd[7] = Convert.ToByte(lastRate.Substring(0, 2), 16);
            cmd[8] = Convert.ToByte(lastRate.Substring(2, 2), 16);
            cmd[9] = Convert.ToByte(lastRate.Substring(4, 2), 16);
            cmd[10] = Convert.ToByte(shengjiang.Substring(0, 2), 16);
            cmd[11] = Convert.ToByte(step.Substring(0, 2), 16);
            cmd[12] = Global.GetCheckSum(cmd);
            return cmd;
        }
        /*6.	低速归零命令：						0xB5					
         *通道对应值：00=X轴，01=Y轴
         *匀速度：2字节，十进制0000-9999
         *命令数据域内容（3字节）=设置的电机通道+匀速度
         *响应数据域内容（4字节）=电机坐标值                          
         * 
         */
        public byte[] GetlowRateToZeroCmd(byte axis, string rate)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x07;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xB5;
            cmd[4] = axis;

            cmd[5] = Convert.ToByte(rate.Substring(0, 2), 16);
            cmd[6] = Convert.ToByte(rate.Substring(2, 2), 16);
            cmd[7] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*7.	高速归原点命令：					0xB6
         *通道对应值：00=X轴，01=Y轴
         *初速：2字节，十进制0000-9999
         *末速：3字节，十进制000000-040000
         *升速斜率：1字节，十进制00-99
         *命令数据域内容（7字节）=设置的电机通道+初速度+末速度+升速斜率
         *响应数据域内容（4字节）=电机坐标值                          
         * 
         */
        public byte[] GetHiRateToZeroCmd(byte axis, string firstRate, string lastRate, string shengjiang)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x0B;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xB6;
            cmd[4] = axis;

            cmd[5] = Convert.ToByte(firstRate.Substring(0, 2), 16);
            cmd[6] = Convert.ToByte(firstRate.Substring(2, 2), 16);
            cmd[7] = Convert.ToByte(lastRate.Substring(0, 2), 16);
            cmd[8] = Convert.ToByte(lastRate.Substring(2, 2), 16);
            cmd[9] = Convert.ToByte(lastRate.Substring(4, 2), 16);
            cmd[10] = Convert.ToByte(shengjiang.Substring(0, 2), 16);
            cmd[11] = Global.GetCheckSum(cmd);
            return cmd;
        }
    }
}
