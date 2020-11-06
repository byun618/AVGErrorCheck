using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace AGV_에러_체크
{
    class Serial_Connection
    {
        private SerialPort port;

        public bool Open_Port()
        {
            bool rtnBool = false;  //포트가 연결여부 리턴

            try
            {
                string[] s;
                s = SerialPort.GetPortNames();

                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].Equals("COM6"))
                        rtnBool = true;
                }

                if (rtnBool)
                {
                    port = new SerialPort("COM6");
                    port.BaudRate = 115200;
                    port.DataBits = 8;
                    port.Parity = Parity.None;
                    port.StopBits = StopBits.One;
                    port.ReadTimeout = 1000;

                    if (!port.IsOpen)
                        port.Open();
                }

                return rtnBool;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return rtnBool;
            }
        }

        public void Close_Port()
        {
            try
            {
                if (port.IsOpen)
                    port.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SendRelayState(int num, int state)
        {
            // 램프 Open or Close
            if (state.Equals(0)) //open
            {   
                // 6: 녹색 7: 적색 8: 황색
                switch (num)
                {
                    case 6:
                        SendCommand("55 AA 01 02 00 06 01 09");
                        break;
                    case 7:
                        SendCommand("55 AA 01 02 00 07 01 0A");
                        break;
                    case 8:
                        SendCommand("55 AA 01 02 00 08 01 0B");
                        break;
                }
            }
            else //close
            {
                switch (num)
                {
                    case 6:
                        SendCommand("55 AA 01 02 00 06 00 08");
                        break;
                    case 7:
                        SendCommand("55 AA 01 02 00 07 00 09");
                        break;
                    case 8:
                        SendCommand("55 AA 01 02 00 08 00 0A");
                        break;
                }
            }
        }

        private void SendCommand(string sendStr)
        {
            try
            {
                byte[] sendBytes = sendStr.Split(' ').Select(s => Convert.ToByte(s, 16)).ToArray();

                port.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
