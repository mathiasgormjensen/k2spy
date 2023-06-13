using K2Spy.ExtensionMethods;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //SourceCode.Hosting.Client.BaseAPI.SCConnectionStringBuilder xxx = new SourceCode.Hosting.Client.BaseAPI.SCConnectionStringBuilder(Connections.Default.Items.First().ConnectionString);
            //xxx = xxx;
            //return;
            try
            {
                System.AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
                {
                    //Console.WriteLine(e.Name);
                    if (e.Name.IndexOf(".resources,") >= 0 || e.Name.EndsWith(".XmlSerializers") || e.Name.IndexOf(".XmlSerializers,") >= 0)
                        return null;

                    using (Form topMost = new Form() { TopMost = true })
                    {
                        using (OpenFileDialog dlg = new OpenFileDialog())
                        {
                            dlg.Title = "Locate " + e.Name;
                            if (dlg.ShowDialog(System.Windows.Forms.Form.ActiveForm ?? topMost) == DialogResult.OK)
                            {
                                string dir = System.IO.Path.GetDirectoryName(dlg.FileName);
                                return System.Reflection.Assembly.LoadFrom(dlg.FileName);
                            }
                        }
                    }
                    return null;
                };

                Serilog.Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
#if false
                    .WriteTo.File(path: @"logs\log-.txt", rollingInterval: RollingInterval.Day)
                    .WriteTo.Console()
#else
                    .WriteTo.Async(c => c.File(path: @"logs\log-.txt", rollingInterval: RollingInterval.Day))
                    .WriteTo.Async(c => c.Console(), 500)
#endif
                    .CreateLogger();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += Program.Application_ThreadException;
                System.AppDomain.CurrentDomain.UnhandledException += Program.CurrentDomain_UnhandledException;

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"Exception occurred in Main: {ex}");
                System.Windows.Forms.MessageBox.Show(ex.Message.ToString(), null, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Serilog.Log.Error($"Unhandled exception: {e.ExceptionObject}");
            System.Windows.Forms.MessageBox.Show($"An unhandled exception occurred: {(e.ExceptionObject as Exception)?.BuildDetailedMessage()}", null, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Serilog.Log.Error($"Thread exception: {e.Exception}");
            System.Windows.Forms.MessageBox.Show("Thread exception: " + e.Exception.ToString(), null, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        }
    }
}