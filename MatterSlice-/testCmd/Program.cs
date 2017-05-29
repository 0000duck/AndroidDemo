using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace myconn
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((obj, args) => MiniDump.TryDump("error.dmp", MiniDump.MiniDumpType.WithFullMemory));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
        }
    }
}