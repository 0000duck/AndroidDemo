/*
Copyright (c) 2017, qwinner

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using myconn.CmdUI;
using System;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace myconn
{
    public class SerialCom
    {
        private SerialPort m_serialPort;
        private Basefm m_fm;

        public static readonly SerialCom instance = new SerialCom();

        public void SetRecvFrm(Basefm fm)
        {
            m_fm = fm;
        }

        private SerialCom()
        {
            InitPort();
        }

        private void InitPort()
        {
            m_serialPort = new SerialPort(Global.gCOM);
            m_serialPort.BaudRate = 38400;
            m_serialPort.Parity = Parity.None;//(Parity)Enum.Parse(typeof(Parity), "None");
            m_serialPort.StopBits = StopBits.One;// (StopBits)Enum.Parse(typeof(StopBits), "1");
            m_serialPort.DataBits = 8;
            m_serialPort.DataReceived += new SerialDataReceivedEventHandler(m_serialPort_DataReceived);
        }

        public bool IsComOpened()
        {
            return m_serialPort.IsOpen;
        }

        public bool OpenComPort()
        {
            bool ret = true;
            try
            {
                m_serialPort.Open();
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public int SendFile(byte[] buffer)
        {
            try
            {
                if (buffer != null)
                {
                    m_serialPort.Write(buffer, 0, buffer.Count());
                }
            }
            catch(Exception)
            {
                return -1;
            }
            return 0;
        }

        public void ReadFile()
        {
            //也是发送一些命令，返回的数据：
        }

        private void m_serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] receivedData = new byte[m_serialPort.BytesToRead];
            m_serialPort.Read(receivedData, 0, receivedData.Length);

            if (m_fm != null)
            {
                m_fm.OnRecvData(receivedData);
            }
            //int int_Len = serialPort1.BytesToRead;

            //receivedData = new byte[serialPort1.BytesToRead];
            //serialPort1.Read(receivedData, 0, receivedData.Length);
            //string text = Encoding.ASCII.GetString(receivedData);
            //// serialPort1.DiscardInBuffer();
            //this.Invoke(textChanged, new string[] { text });

        }

    }
}
