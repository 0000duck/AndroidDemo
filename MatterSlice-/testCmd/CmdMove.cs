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
        public byte[] GetUniformStepCmd(byte axis, int rate, int stepDirection)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0xAA;
            cmd[1] = 0x0B;
            cmd[2] = Global.gCommandAddr;// 0x00;//addr
            cmd[3] = 0xB1;
            cmd[4] = axis;
            cmd[5] = 0xB1;// (byte)((rate & 0xFF00) >> 8);
            cmd[6] = 0xB1;// (byte)(rate & 0x00FF);
            cmd[7] = 0xB1;
            cmd[8] = 0xB1;
            cmd[9] = 0xB1;
            cmd[10] = (byte)(stepDirection & 0x00FF); ;
            cmd[11] = Global.GetCheckSum(cmd);
            return cmd;
        }


        //ing
    }
}
