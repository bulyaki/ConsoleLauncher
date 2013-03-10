using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace SendCtrlC
{
    class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            try
            {
                int processId = int.Parse(args[0]);
                FreeConsole();
                AttachConsole(processId);
                GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                return 0;
            }
            catch (System.Exception)
            {
                return -1;
            }
        }

        public enum ConsoleCtrlEvent
        {
            CTRL_C = 0,
            CTRL_BREAK = 1,
            CTRL_CLOSE = 2,
            CTRL_LOGOFF = 5,
            CTRL_SHUTDOWN = 6
        }
        [DllImport("kernel32.dll")]
        static extern bool GenerateConsoleCtrlEvent(ConsoleCtrlEvent
        sigevent,
        int dwProcessGroupId);

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
    }
}
