using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    //GD25Q16芯片有芯片擦除，块擦除及扇区擦除功能，有页编程功能，读功能是可以读出单个字节。
    public static class Global
    {
        //需要写到配置文件里，每次读取，同步。
        public static string gCOM = "COM1";
        public static int gRealBaudRate = 38400;
        public static byte gCommandAddr = 0x01;



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

        //command response code
        public static string gAddrRes = "";
    }
}
