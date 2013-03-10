using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace TestService
{
    public partial class TestService : ServiceBase
    {
        static bool Exit { get; set; }
        static void Main(string[] args)
        {
            TestService service = new TestService();
 
            if (Environment.UserInteractive)
            {
                service.OnStart(args);
                Console.WriteLine("This is written to stdout");
                Console.Error.WriteLine("This is written to stderr");
                Console.WriteLine("Press any key to exit...");
                Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
                Exit = false;
                while (!Exit)
                {
                    Console.WriteLine("still running at {0}", DateTime.Now);
                    System.Threading.Thread.Sleep(3000);
                }
                service.OnStop();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Exit = true;
            Console.WriteLine("Sending Ctrl-C...");
        }

        public TestService()
        {
            InitializeComponent();
        }
 
        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
        }
 
        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down
            //necessary to stop your service.
        }
    }
}
