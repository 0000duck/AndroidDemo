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
    public class CmdFileManage : CmdBase
    {
        /*1.	擦除1个扇区命令	
         * 扇区地址为00 10，00 20...00 F0,2字节十六进制表示，存放通用文件
         * 
         */
        public byte[] GetEraseSectorCmd(byte hvalue, byte lvalue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD0;
            cmd[4] = hvalue;
            cmd[5] = lvalue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*2.	擦除1个块命令
         * 块地址为01 00,02 00...0F 00，用2字节十六进制表示，存放轨迹文件
         * 
         */
        public byte[] GetEraseBlockCmd(byte hvalue, byte lvalue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD1;
            cmd[4] = hvalue;
            cmd[5] = lvalue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*3.	擦除存储器芯片命令
         * 擦除除00 00扇区外的全部芯片,擦除大约要16秒钟
         * 
         */
        public byte[] GetEraseAllCmd()
        {
            byte[] cmd = new byte[5];
            cmd[0] = 0xAA;
            cmd[1] = 0x04;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD2;
            cmd[4] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*4.	传送一个文件（数据块）到扇区命令
         * 第一部分：
         * 命令参数：2字节，十六进制，扇区地址为00 10，00 20...00 F0,
         * 命令	0xAA 0x06 0x01 0xD3 0xXX 0xXX 0xXX （包头，长度，地址，命令，扇区地址,校验）	
         * 第二部分：（文件长度（3字节十六进制），文件，文件校验码）	
         * 命令	0xXX 0xXX 0xXX 文件...... 0xXX（长度，文件......校验）
         * 注：每个文件最大长度是4096字节
         * 
         * 
         */
        public byte[] GetWriteToSectorCmd(byte hvalue, byte lvalue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD3;
            cmd[4] = hvalue;
            cmd[5] = lvalue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*5.	传送一个文件（数据块）到块/区命令
         * 第一部分：
         * 命令参数：2字节，十六进制，块/区地址
         * 命令	0xAA 0x06 0x01 0xD4 0xXX 0xXX 0xXX （包头，长度，地址，命令，块/区地址,校验）	
         * 第二部分：（文件长度（3字节十六进制），文件，文件校验码）	
         * 命令	0xXX 0xXX 0xXX 文件...... 0xXX（长度，文件......校验）
         * 注：每个文件最大长度是64K字节
         * 
         * 
         */
        public byte[] GetWriteToBlockCmd(byte hvalue, byte lvalue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD4;
            cmd[4] = hvalue;
            cmd[5] = lvalue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*6.	读一页数据命令并输出
        * 命令参数：2字节，十六进制，页地址
        * 命令	0xAA 0x06 0x01 0xD5 0x00 0xXX 0xXX（包头，长度，地址，命令，页地址,校验）	
        * 
        * 
        */
        public byte[] GetReadOnePageCmd(byte hvalue, byte lvalue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD5;
            cmd[4] = hvalue;
            cmd[5] = lvalue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*7.	读扇区数据命令并输出
        * 命令参数：2字节，十六进制，扇区地址为00 10，00 20...00 F0,
        * 命令	0xAA 0x06 0x01 0xD6 0x00 0xXX 0xXX（包头，长度，地址，命令，扇区地址,校验）
        * 
        */
        public byte[] GetReadSectorCmd(byte hvalue, byte lvalue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD6;
            cmd[4] = hvalue;
            cmd[5] = lvalue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }

        /*8.	读块数据命令并输出
        * 命令参数：2字节，十六进制，块地址
        * 命令	0xAA 0x06 0x01 0xD7 0xXX 0x00 0xXX（包头，长度，地址，命令，块地址,校验）
        * 
        */
        public byte[] GetReadBlockCmd(byte hvalue, byte lvalue)
        {
            byte[] cmd = new byte[7];
            cmd[0] = 0xAA;
            cmd[1] = 0x06;
            cmd[2] = Global.gCommandAddr;//addr
            cmd[3] = 0xD6;
            cmd[4] = hvalue;
            cmd[5] = lvalue;
            cmd[6] = Global.GetCheckSum(cmd);
            return cmd;
        }
    }
}
