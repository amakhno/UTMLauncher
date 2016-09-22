using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Diagnostics;

namespace UTMLauncher
{
    class UTM
    {
        public static void Run(string adress, string path, bool internetConnection)
        {
            if (!internetConnection)
            {
                throw new Exception("Нет соединения с Интернет");
            }
            ServiceController[] controllers = ServiceController.GetServices().Where(x => x.DisplayName.Contains("Transport")).ToArray();
            Process process = new Process();
            if (!Checks.CheckTransport(controllers.First(x => x.DisplayName == "Transport")))
            {
                process.StartInfo.FileName = path + "\\transporter\\bin\\RunDaemon.bat";
                process.Start();
                process.WaitForExit();
            }

            if (!Checks.CheckMonitoring(controllers.First(x => x.DisplayName == "Transport-Monitoring")))
            {
                process.StartInfo.FileName = path + "\\monitoring\\bin\\RunDaemon.bat";
                process.Start();
                process.WaitForExit();
            }

            if (!Checks.CheckMonitoring(controllers.First(x => x.DisplayName == "Transport-Updater")))
            {
                process.StartInfo.FileName = path + "\\updater\\bin\\RunDaemon.bat";
                process.Start();
                process.WaitForExit();
            }
        }

        public static void StopTransport(string path)
        {
            Process process = new Process();
            if (Checks.CheckTransport(ServiceController.GetServices().First(x => x.DisplayName == "Transport")))
            {
                process.StartInfo.FileName = path + "\\transporter\\bin\\StopDaemon.bat";
                process.Start();
                process.WaitForExit();
            }
        }

        public static void RunTransport(string adress, string path, bool internetConnection)
        {
            if (!internetConnection)
            {
                throw new Exception("Нет соединения с Интернет");
            }
            Process process = new Process();
            if (!Checks.CheckTransport(ServiceController.GetServices().First(x => x.DisplayName == "Transport")))
            {
                process.StartInfo.FileName = path + "\\transporter\\bin\\RunDaemon.bat";
                process.Start();
                process.WaitForExit();
            }
        }

        public static void Stop(string path)
        {
            ServiceController[] controllers = ServiceController.GetServices().Where(x => x.DisplayName.Contains("Transport")).ToArray();
            Process process = new Process();
            if (Checks.CheckTransport(controllers.First(x => x.DisplayName == "Transport")))
            {
                process.StartInfo.FileName = path + "\\transporter\\bin\\StopDaemon.bat";
                process.Start();
                process.WaitForExit();
            }

            if (Checks.CheckMonitoring(controllers.First(x => x.DisplayName == "Transport-Monitoring")))
            {
                process.StartInfo.FileName = path + "\\monitoring\\bin\\StopDaemon.bat";
                process.Start();
                process.WaitForExit();
            }

            if (Checks.CheckMonitoring(controllers.First(x => x.DisplayName == "Transport-Updater")))
            {
                process.StartInfo.FileName = path + "\\updater\\bin\\StopDaemon.bat";
                process.Start();
                process.WaitForExit();
            }
        }
    }
}
