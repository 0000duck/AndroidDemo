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
    public class CmdSleep : CmdBase
    {

        /*1.	等待命令1：							0xC0
         *命令数据域内容（1字节）=设置的延时时间值00-99ms
         */
        public byte[] GetSleepOneCmd(byte value)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xC0;
            cmd[4] = value;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*2.	等待命令2：							0xC1
         *命令数据域内容（1字节）=设置的延时时间值00-99，单位=100ms
         */
        public byte[] GetSleepTwoCmd(byte value)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xC1;
            cmd[4] = value;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*3.	等待规定通道置位命令：				0xC2
         *通道对应值：00=X轴，01=Y轴
         * 命令数据域内容（1字节）=设置的输入通道号
         */
        public byte[] GetWaitAxisResetCmd(byte value)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xC2;
            cmd[4] = value;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }
    }
}
