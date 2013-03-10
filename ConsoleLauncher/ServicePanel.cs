using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConsoleLauncher
{
    public partial class ServicePanel : UserControl
    {
        public int MaxConsoleLines { get; set; }
        public ServicePanel(int maxConsoleLines = 0)
        {
            MaxConsoleLines = maxConsoleLines;
            InitializeComponent();
        }
        public void ScrollToBottom()
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }
        public void CheckMaxLines()
        {
            Invoke(new MethodInvoker(delegate
            {
                if (MaxConsoleLines > 0)
                {
                    lock (textBox1)
                    {
                        if (textBox1.Lines.Length > MaxConsoleLines)
                        {
                            textBox1.SuspendLayout();
                            string[] tempArray = new string[MaxConsoleLines];
                            Array.Copy(textBox1.Lines, textBox1.Lines.Length - MaxConsoleLines, tempArray, 0, MaxConsoleLines);
                            textBox1.Lines = tempArray;
                            textBox1.ResumeLayout();
                        }
                    }
                }
            }));
        }
    }
}
