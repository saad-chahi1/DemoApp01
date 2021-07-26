using Prysm.AppVision.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppDemo
{
    class Program
    {
        private static bool isClosed = false;
        private static AppServerForDriver appServerForDriver;

        static void Main(string[] args)
        {
            //AppServer
            /*var appServer = new AppServer();

            appServer.Open("localhost:8080");
            appServer.Login("ADM", "123");

            Console.WriteLine("Is Connected : " + appServer.IsConnected);

            */

            //AppServerForDriver
            appServerForDriver = new AppServerForDriver();
            var arguments = Environment.GetCommandLineArgs();

            //ProtocolName@Hostaname

            var arg = arguments[1].Split('@');

            var protocolName = arg[0];
            var hostname = arg.Length > 1 ? arg[1] : "";

            appServerForDriver.Open(hostname);
            appServerForDriver.Login(protocolName);

            appServerForDriver.AddFilterNotifications("$V.*");
            appServerForDriver.ProtocolSynchronized();
            appServerForDriver.StartNotifications(false);

            appServerForDriver.ControllerManager.Closed += ControllerManager_closed;
            appServerForDriver.VariableManager.StateChanged += VariableManager_StateChanged;

            Console.WriteLine("Is connected : " + appServerForDriver.IsConnected);
            Console.WriteLine($"Current protocol : {appServerForDriver.CurrentProtocol.Name}");

            var variables = appServerForDriver.VariableManager.GetRows();
            foreach(var variable in variables)
            {
                Console.WriteLine("variable : " + variable.Name);
            }

            while (!isClosed)
            {
                Thread.Sleep(100);
            }

        }

        private static void VariableManager_StateChanged(Prysm.AppVision.Data.VariableState obj)
        {
            Console.WriteLine($"variable : {obj.Name} | state : {obj.Value}");
        }

        private static void ControllerManager_closed()
        {
            appServerForDriver.Close();
            isClosed = true;
        }
    }
}
