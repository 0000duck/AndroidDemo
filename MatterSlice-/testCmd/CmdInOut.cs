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
        public byte[] GetIOCmd(byte axis,byte value)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xC3;
            cmd[4] = axis;
            cmd[5] = value;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }
    }
}
