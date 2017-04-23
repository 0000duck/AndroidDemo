using System.IO.Ports;
using System.Linq;

namespace myconn
{
    public class SerialCom
    {
        private SerialPort m_serialPort;
        public SerialCom(string COM)
        {
            InitPort(COM);
        }

        private void InitPort(string COM)
        {
            m_serialPort = new SerialPort(COM);
            m_serialPort.BaudRate = 38400;
            m_serialPort.Parity = Parity.None;//(Parity)Enum.Parse(typeof(Parity), "None");
            m_serialPort.StopBits = StopBits.One;// (StopBits)Enum.Parse(typeof(StopBits), "1");
            m_serialPort.DataBits = 8;
            m_serialPort.DataReceived += new SerialDataReceivedEventHandler(m_serialPort_DataReceived);
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

        public void SendFile(byte[] buffer)
        {
            m_serialPort.Write(buffer, 0, buffer.Count());
        }

        public void ReadFile()
        {
            //也是发送一些命令，返回的数据：
        }

        private void m_serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            //int int_Len = serialPort1.BytesToRead;

            //receivedData = new byte[serialPort1.BytesToRead];
            //serialPort1.Read(receivedData, 0, receivedData.Length);
            //string text = Encoding.ASCII.GetString(receivedData);
            //// serialPort1.DiscardInBuffer();
            //this.Invoke(textChanged, new string[] { text });

        }

    }
}
