using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Runtime.InteropServices;

namespace ConsoleLauncher
{
    public partial class ConsoleLauncherForm : Form
    {
        private Dictionary<DockingTabPage, ServicePanel> dicPanel = new Dictionary<DockingTabPage, ServicePanel>();
        private String CtrlCHelperPath = "SendCtrlC.exe";
        private String ConfLocation = "ConsoleLauncher.conf";
        private int maxConsoleLines = 1000;
        
        public ConsoleLauncherForm(String confLocation)
        {
            ConfLocation = confLocation;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartAllProcesses();
        }

        private void StartAllProcesses()
        {
            String Path = String.Empty;
            String Executable = String.Empty;
            String Arguments = String.Empty;
            String ProcessName = String.Empty;
            int StartDelay = 0;
            using (System.IO.FileStream fs = new System.IO.FileStream(ConfLocation, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    while (reader.ReadToFollowing("Service"))
                    {
                        if ( reader.MoveToAttribute("Path") )
                            Path = reader.Value;
                        if ( reader.MoveToAttribute("Executable") )
                        Executable = reader.Value;
                        if ( reader.MoveToAttribute("Arguments") )
                            Arguments = reader.Value;
                        if ( reader.MoveToAttribute("ProcessName") )
                            ProcessName = reader.Value;
                        if (reader.MoveToAttribute("StartDelay"))
                        {
                            int.TryParse(reader.Value, out StartDelay);
                        }

                        if (Executable != String.Empty)
                        {
                            Execute(Path, Executable, Arguments, ProcessName, StartDelay);
                        }
                    }
                    reader.Close();
                }

                fs.Close();
            }
        }

        public delegate void CreateTabPageDelegate(ConsoleData data);
        private void CreateTabPage(ConsoleData data)
        {
            DockingTabPage tab = new DockingTabPage(data.title);
            tab.ContextMenuStrip = contextMenuStrip2;
            tabControl1.ImageList = imageList1;
            tab.Controls.Add(data.panel);
            tab.ImageIndex = 0;
            tab.Tag = data;
            data.panel.Dock = DockStyle.Fill;
            tabControl1.TabPages.Add(tab);
            data.tab = tab;
            data.panel.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            data.panel.btnStart.Tag = data;
            if (System.IO.File.Exists(CtrlCHelperPath))
            {
                data.panel.btnSendCtrlC.Enabled = true;
                data.panel.btnSendCtrlC.Visible = true;
                data.panel.btnSendCtrlC.Click += new System.EventHandler(this.btnSendCtrlC_Click);
                data.panel.btnSendCtrlC.Tag = data;
            }
            else
            {
                data.panel.btnSendCtrlC.Enabled = false;
                data.panel.btnSendCtrlC.Visible = false;
            }

            data.panel.btnKill.Click += new System.EventHandler(this.btnKill_Click);
            data.panel.btnKill.Text = "Kill all " + data.title + "instances";
            data.panel.btnKill.Tag = data;

            data.panel.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            data.panel.btnClear.Tag = data;

            dicPanel[tab] = data.panel;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ConsoleData data = ((Button) sender).Tag as ConsoleData;
            if (data.proc != null)
            {
                data.proc.ErrorDataReceived -= new DataReceivedEventHandler(DataReceivedStderr);
                data.proc.OutputDataReceived -= new DataReceivedEventHandler(DataReceivedStdout);
            }
            if (data.bw != null)
            {
                data.bw.CancelAsync();
            }

            data.bw = new BackgroundWorker();

            data.bw.WorkerSupportsCancellation = true;
            data.bw.WorkerReportsProgress = false;
            data.bw.DoWork += new DoWorkEventHandler(bw_DoWork);

            data.bw.RunWorkerAsync(data);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ConsoleData data = ((Button)sender).Tag as ConsoleData;
            data.panel.textBox1.Clear();
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            ConsoleData data = ((Button)sender).Tag as ConsoleData;
            try
            {

                if (data.proc != null)
                {
                    data.proc.ErrorDataReceived -= new DataReceivedEventHandler(DataReceivedStderr);
                    data.proc.OutputDataReceived -= new DataReceivedEventHandler(DataReceivedStdout);

                    foreach (Process p in System.Diagnostics.Process.GetProcessesByName(data.proc.ProcessName))
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }

                if (data.bw != null && data.bw.IsBusy)
                {
                    data.bw.CancelAsync();
                }
                data.panel.textBox1.AppendText("---- Service Stopped ----\r\n");
            }
            catch (System.Exception)
            {
            	// if it failed to stop just leave it
            }
            finally
            {
                SetConsoleState(data, false);
            }
        }

        private void btnSendCtrlC_Click(object sender, EventArgs e)
        {
//            Invoke(new MethodInvoker(delegate
//            {
                ConsoleData data = ((Button)sender).Tag as ConsoleData;

                if (data.proc != null)
                {
                    SendCtrlC(data.proc.Id);
                }
//            }));
        }

        private bool SendCtrlC(int pid)
        {
            if (System.IO.File.Exists(CtrlCHelperPath))
            {
                try
                {
                    Process proc = new Process();
                    proc.StartInfo.FileName = CtrlCHelperPath;
                    proc.StartInfo.Arguments = pid.ToString();
                    proc.Start();
                    while (!proc.WaitForExit(1000));
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            } else {
                return false;
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ConsoleData data = e.Argument as ConsoleData;

            data.proc.StartInfo.FileName = data.path + data.exe;
            data.proc.StartInfo.Arguments = data.args;

            data.proc.StartInfo.RedirectStandardOutput = true;
            data.proc.StartInfo.RedirectStandardError = true;
            data.proc.StartInfo.UseShellExecute = false;
            data.proc.EnableRaisingEvents = true;
            data.proc.StartInfo.CreateNoWindow = true;

            data.proc.ErrorDataReceived += new DataReceivedEventHandler(DataReceivedStderr);
            data.proc.OutputDataReceived += new DataReceivedEventHandler(DataReceivedStdout);
            data.proc.Exited += new EventHandler(Exited);

            data.proc.Start();

            data.proc.BeginErrorReadLine();
            data.proc.BeginOutputReadLine();

            SetConsoleState(data, true);
            data.proc.WaitForExit();
            SetConsoleState(data, false);
        }

        public delegate void SetImageIndexDelegate(ConsoleData data, int index);
        void SetConsoleState(ConsoleData data, bool Enable)
        {
            try
            {
                Invoke(new MethodInvoker(delegate {
                    data.tab.ImageIndex = Enable ? 1 : 0;
                    data.panel.btnStart.Enabled = !Enable;
                    data.panel.btnSendCtrlC.Enabled = Enable;
                }));
            }
            catch (ObjectDisposedException)
            {
                // ignore this, it only happens when closing the app
            }
            catch (InvalidOperationException)
            {
                // ignore this, it only happens when closing the app
            }

        }

        void Execute(string cmdPath, string cmdExe, string cmdArgs, string title, int delay)
        {
            ConsoleData data = new ConsoleData(cmdPath, cmdExe, cmdArgs, title, delay, new ServicePanel(maxConsoleLines), new Process());
            
            CreateTabPageDelegate d = new CreateTabPageDelegate(CreateTabPage);
            Invoke(d, new object[] { data });
            btnStart_Click(data.panel.btnStart, new EventArgs());
        }

        ConsoleData FindConsoleData(Process proc)
        {
            foreach (KeyValuePair<DockingTabPage, ServicePanel> pair in dicPanel)
            {
                ConsoleData data = pair.Key.Tag as ConsoleData;
                if (data.proc == proc)
                    return data;
            }
            return null;
        }

        void Exited(object sender, EventArgs e)
        {
            ConsoleData data = FindConsoleData(sender as Process);
            if (data == null || data.proc == null)
                return;

            data.proc.CancelErrorRead();
            data.proc.CancelOutputRead();
            SetConsoleState(data, false);
        }

        void DataReceivedStderr(object sender, DataReceivedEventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (e.Data != null)
                    {
                        Process proc = sender as Process;
                        ServicePanel panel = null;
                        DockingTabPage tab = null;
                        ConsoleData data = null;
                        foreach (KeyValuePair<DockingTabPage, ServicePanel> pair in dicPanel)
                        {
                            data = pair.Key.Tag as ConsoleData;
                            if (data.proc == proc)
                            {
                                panel = pair.Value;
                                tab = pair.Key;
                                break;
                            }
                        }

                        if (panel == null || tab == null || data == null)
                            return;

                        lock (panel.textBox1)
                        {
                            Color tempColor = panel.textBox1.ForeColor;
                            panel.textBox1.SelectionColor = Color.Yellow;
                            panel.textBox1.AppendText(e.Data);
                            panel.textBox1.AppendText("\r\n");
                            panel.textBox1.SelectionColor = tempColor;
                        }

                        if (e.Data.Contains("Service started"))
                            SetConsoleState(data, true);

                        if (e.Data.Contains("Service Stopped"))
                            SetConsoleState(data, false);

                        panel.CheckMaxLines();
                        panel.ScrollToBottom();
                    }
                }));
            }
            catch (ObjectDisposedException)
            {
                // ignore this
            }
        }

        void DataReceivedStdout(object sender, DataReceivedEventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (e.Data != null)
                    {
                        Process proc = sender as Process;
                        ServicePanel panel = null;
                        DockingTabPage tab = null;
                        ConsoleData data = null;
                        foreach (KeyValuePair<DockingTabPage, ServicePanel> pair in dicPanel)
                        {
                            data = pair.Key.Tag as ConsoleData;
                            if (data.proc == proc)
                            {
                                panel = pair.Value;
                                tab = pair.Key;
                                break;
                            }
                        }

                        if (panel == null || tab == null || data == null)
                            return;

                        lock (panel.textBox1)
                        {
                            panel.textBox1.AppendText(e.Data);
                            panel.textBox1.AppendText("\r\n");
                        }

                        if (e.Data.Contains("Service started"))
                            SetConsoleState(data, true);

                        if (e.Data.Contains("Service Stopped"))
                            SetConsoleState(data, false);

                        panel.CheckMaxLines();
                        panel.ScrollToBottom();
                    }
                }));
            }
            catch (ObjectDisposedException)
            {
                // ignore this
            }
        }

        private void TerminateAllProcesses()
        {
            List<DockingTabPage> keys = new List<DockingTabPage>(dicPanel.Keys);
            foreach (DockingTabPage key in keys)
            {
                ServicePanel value = null;
                if ( dicPanel.TryGetValue(key, out value) )
                {
                    ConsoleData data = key.Tag as ConsoleData;
                    try
                    {
                        if (SendCtrlC(data.proc.Id))
                        {
                            data.proc.CancelErrorRead();
                            data.proc.CancelOutputRead();
                            tabControl1.TabPages.Clear();
                            tabControl1.Controls.Clear();
                            dicPanel.Remove(key);
                            tabControl1.Controls.Remove(key);
                        }
                    }
                    catch (System.Exception)
                    {
                        //
                    }
                }
            }
        }

        private void KillProcesses()
        {
            foreach (KeyValuePair<DockingTabPage, ServicePanel> pair in dicPanel)
            {
                try
                {
                    ConsoleData data = pair.Key.Tag as ConsoleData;
                    data.proc.CancelErrorRead();
                    data.proc.CancelOutputRead();
                    data.proc.Kill();
                }
                catch (System.Exception)
                {
                    //
                }
                finally
                {
                    tabControl1.Controls.Remove(pair.Key);

                }
            }
            tabControl1.TabPages.Clear();
            tabControl1.Controls.Clear();
            dicPanel.Clear();
        }

        private void KillAllProcesses()
        {
            foreach (KeyValuePair<DockingTabPage, ServicePanel> pair in dicPanel)
            {
                try
                {
                    ConsoleData data = pair.Key.Tag as ConsoleData;
                    data.proc.CancelErrorRead();
                    data.proc.CancelOutputRead();

                    foreach (Process p in System.Diagnostics.Process.GetProcessesByName(data.proc.ProcessName))
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }
                catch (System.Exception)
                {
                    //
                }
                finally
                {
                    tabControl1.Controls.Remove(pair.Key);
                    
                }
            }
            tabControl1.TabPages.Clear();
            tabControl1.Controls.Clear();
            dicPanel.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            TerminateAllProcesses();
        }

        private void btnStartAll_Click(object sender, EventArgs e)
        {
            KillAllProcesses();
            StartAllProcesses();
        }

        private void btnKillAll_Click(object sender, EventArgs e)
        {
            KillAllProcesses();
        }

        private void detachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DockingTabPage tabPage = detachToolStripMenuItem.Tag as DockingTabPage;
            if (tabPage != null)
            {
                tabPage.SwitchMode();
            }
        }

        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tabControl1.TabCount; i++)
                {
                    Rectangle r = tabControl1.GetTabRect(i);
                    if (r.Contains(e.Location))
                    {
                        DockingTabPage tabPage = tabControl1.TabPages[i] as DockingTabPage;
                        if (tabPage != null)
                        {
                            detachToolStripMenuItem.Tag = tabPage;
                            contextMenuStrip2.Show(PointToScreen(e.Location));
                        }
                        break;  
                    }
                }
            }
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: TerminateAllProcesses(); break;
                case 1: KillProcesses(); break;
                case 2: KillAllProcesses(); break;
            }
        }
    }
}
