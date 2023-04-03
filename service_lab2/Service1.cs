using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

namespace service_lab2
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            this.CanHandleSessionChangeEvent = true;
            InitializeComponent();
        }

        // define

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSSendMessage(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.I4)] int SessionId,
            string pTitle,
            [MarshalAs(UnmanagedType.U4)] int TitleLength,
            string pMessage,
            [MarshalAs(UnmanagedType.U4)] int MessageLength,
            [MarshalAs(UnmanagedType.I4)] int Style,
            [MarshalAs(UnmanagedType.I4)] int Timeout,
            [MarshalAs(UnmanagedType.I4)] out int pResponse,
            bool bWait
        );

        private const int MB_OK = 0x00000000;
        private const int MB_OKCANCEL = 0x00000001;
        private const int MB_YESNO = 0x00000004;
        public static bool ShowMessage(string title, string message)
        {
            int sessionId = WTSGetActiveConsoleSessionId();
            int response;
            return WTSSendMessage(
                IntPtr.Zero,
                sessionId,
                title,
                title.Length,
                message,
                message.Length,
                MB_OK,
                0,
                out response,
                true
            );
        }

        [DllImport("kernel32.dll")]
        private static extern int WTSGetActiveConsoleSessionId();


        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            if (
                changeDescription.Reason == SessionChangeReason.SessionLogon
                || changeDescription.Reason == SessionChangeReason.SessionUnlock
            )
            {
                Thread t = new Thread(() =>
                {
                    try
                    {
                        ShowMessage("20251635", "Login Sucess");
                    }
                    catch { }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

            }
            base.OnSessionChange(changeDescription);
        
        }
    protected override void OnStart(string[] args)
        {
        }

    protected override void OnStop()
        {
        }
    }
}
