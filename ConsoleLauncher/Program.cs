using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace ConsoleLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            String confLocation = "ConsoleLauncher.conf";

            if (args.Length > 0)
            {
                if ((new FileInfo(args[0])).Exists)
                {
                    confLocation = args[0];
                }
            }

            if (!(new FileInfo(confLocation)).Exists)
            {
                MessageBox.Show("Could not find the config file. The application will now quit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConsoleLauncherForm(confLocation));
        }
    }
}
