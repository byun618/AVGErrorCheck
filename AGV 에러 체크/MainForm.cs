using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace AGV_에러_체크
{
    public partial class MainForm : Form
    {
        private Thread monitorThread = null;
        private Serial_Connection conn = null;
        private bool monStart = false;

        private int timerCnt=0;
        private int errCnt = 0;

        enum AGVState { normal, error, shutdown }
        enum RelayState { open, close }

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int hWnd1, int hWnd2, string lpsz1, string lpsz2);
        [DllImport("user32.dll")]
        public static extern int IsWindowVisible(int hwnd);
        static int FindWindowByIndex(int hWnd, int index)
        {
            if (index == 0)
            {
                return hWnd;
            }
            else
            {
                int cnt = 0;
                int result = 0;

                do
                {
                    //result = FindWindowEx(hWnd, result, "ThunderRT6PictureBoxDC", "");
                    //result = FindWindowEx(hWnd, result, null, "");
                    result = FindWindowEx(hWnd, result, null, "label1");

                    if (result != 0)
                        ++cnt;

                } while (cnt < index && result != 0);

                return result;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TopMost = true;

            conn = new Serial_Connection();
            monitorThread = new Thread(new ThreadStart(DoMonitor));
            
            if (conn.Open_Port())
            {
                monStart = true;
                monitorThread.Start();

                //Search_Timer.Start();
            }
            else
            {
                MessageBox.Show("포트를 연결해주세요", "경고");
                Application.Exit();
            }

            WindowState = FormWindowState.Minimized;
        }

        private int GetAGVState()
        {
            int rtnState;
            int program = FindWindow(null, "Form1");
            int error = FindWindowByIndex(program, 1);
            error = IsWindowVisible(error);    
            //보이면 error는 1 안보이면 0

            if (!program.Equals(0)) //프로그램 실행중
            {
                if (error.Equals(0)) //정상작동
                {
                    rtnState = (int)AGVState.normal; 
                }
                else //에러
                {
                    rtnState = (int)AGVState.error;
                }
            }
            else //프로그램 동작 안함. 가동중지
            {
                rtnState = (int)AGVState.shutdown;
            }


            return rtnState;
        }

        //깜빡이는 속도가 ms * 번 보다 느리면(크면) 제대로 안된다.

        #region 스레드로
        private void DoMonitor()
        {
            while (monStart)
            {
                int state = GetAGVState();

                switch (state)
                {
                    case 0:
                        la_state.BackColor = Color.Lime;
                        la_state.Text = "정상작동중";
                        
                        conn.SendRelayState(6, (int)RelayState.open);
                        conn.SendRelayState(7, (int)RelayState.close);
                        conn.SendRelayState(8, (int)RelayState.close);
                        
                        break;

                    case 1:
                        //0.5초동안 에러가 보인 횟수를 체크하여 그 횟수가 0이 아니면 에러

                        la_state.BackColor = Color.Red;
                        la_state.Text = "에러발생";

                        conn.SendRelayState(7, (int)RelayState.open);
                        conn.SendRelayState(6, (int)RelayState.close);
                        conn.SendRelayState(8, (int)RelayState.close);
                        
                        int errCnt = 0;

                        //for문을 돌려 한번 돌때마다 sleep을 10으로 주고 50반복 그러면 500ms 
                        for (int i = 1; i <= 50; i++) //50번
                        {
                            int errState = GetAGVState();

                            if (errState.Equals(1))  //에러횟수 체크
                                errCnt++;

                            if (i >= 50) //for문이 끝에 도달 했을때 
                            {
                                if (!errCnt.Equals(0)) //에러횟수 체크
                                {
                                    i = 0;
                                }

                                errCnt = 0;
                            }

                            Thread.Sleep(10);  //10ms * 50 = 500ms
                        }

                        break;

                    case 2:
                        la_state.BackColor = Color.Orange;
                        la_state.Text = "가동중지";

                        conn.SendRelayState(8, (int)RelayState.open);
                        conn.SendRelayState(6, (int)RelayState.close);
                        conn.SendRelayState(7, (int)RelayState.close);
                        
                        break;
                }                               
            }
        } 
        #endregion

        #region 타이머로
        private void Search_Timer_Tick(object sender, EventArgs e)
        {
            int state = GetAGVState();

            switch (state)
            {
                case 0:
                    la_state.BackColor = Color.Lime;
                    la_state.Text = "정상작동중";

                    conn.SendRelayState(7, (int)RelayState.close);
                    conn.SendRelayState(8, (int)RelayState.close);
                    conn.SendRelayState(6, (int)RelayState.open);

                    break;

                case 1:
                    la_state.BackColor = Color.Red;
                    la_state.Text = "에러발생";

                    conn.SendRelayState(6, (int)RelayState.close);
                    conn.SendRelayState(8, (int)RelayState.close);
                    conn.SendRelayState(7, (int)RelayState.open);

                    Search_Timer.Stop();
                    ErrChk_Timer.Start();

                    break;

                case 2:
                    la_state.BackColor = Color.Orange;
                    la_state.Text = "가동중지";

                    conn.SendRelayState(6, (int)RelayState.close);
                    conn.SendRelayState(7, (int)RelayState.close);
                    conn.SendRelayState(8, (int)RelayState.open);

                    break;
            }
        }

        private void ErrChk_Timer_Tick(object sender, EventArgs e) //10ms
        {
            timerCnt++;

            if (GetAGVState().Equals(1))
                errCnt++;

            if (timerCnt >= 50)  //10ms * 50 = 500ms
            {
                if (errCnt.Equals(0))
                {
                    ErrChk_Timer.Stop();
                    Search_Timer.Start();

                }
                else
                {
                    timerCnt = 0;
                }

                errCnt = 0;
            }
        } 
        #endregion

        //폼이 닫힐때
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (monitorThread.IsAlive)
                    monitorThread.Abort();

                conn.Close_Port();

                Dispose();
            }
            else
            {
                e.Cancel = true;
                return;
            }

            
        }
    }
}
