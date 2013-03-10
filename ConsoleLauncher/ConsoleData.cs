using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ConsoleLauncher
{
    public class ConsoleData : Object
    {
        public ConsoleData(string cmdPath = "", string cmdExe = "", string cmdArgs = "", string cmdTitle = "", int cmdDelay = 0, ServicePanel servicePanel = null, Process process = null)
        {
            path = cmdPath;
            exe = cmdExe;
            args = cmdArgs;
            title = cmdTitle;
            delay = cmdDelay;
            panel = servicePanel;
            proc = process;
            bw = null;
            tab = null;
            Color tabColor = SystemColors.Control;
        }

        public String path { get; set; }
        public String exe { get; set; }
        public String args { get; set; }
        public String title { get; set; }
        public int delay { get; set; }
        public DockingTabPage tab { get; set; }
        public ServicePanel panel { get; set; }
        public Process proc { get; set; }
        public BackgroundWorker bw { get; set; }
        public Color tabColor { get; set; }
    }
}
