/*
Copyright (c) 2017, qwinner

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace myconn
{
    public class CmdLoop : CmdBase
    {
        /*1.	循环开始命令：						0xD0	
         * 循环次数：1字节，十进制，1-99次
         * 命令数据域内容（1字节）=循环次数
         */
        public byte[] GetLoopStartCmd( byte value)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x05;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD0;
            cmd[4] = value;
            cmd[5] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*1.	循环开始命令：						0xD0	
         * 循环次数：1字节，十进制，1-99次
         * 命令数据域内容（1字节）=循环次数
         */
        public byte[] GetLoopStopCmd()
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0x04;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD1;
            cmd[4] = Global.GetCheckSum(cmd);
            return cmd;
        }
    }
}
