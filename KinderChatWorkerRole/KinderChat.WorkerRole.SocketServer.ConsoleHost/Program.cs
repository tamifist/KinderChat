using System;
using System.Threading;
using KinderChat.WorkerRole.SocketServer;
using KinderChat.WorkerRole.SocketServer.Infrastructure.Logging;
using KinderChat.WorkerRole.SocketServer.Infrastructure.Logging.Loggers;

namespace WorkerRoleConsoleHost
{
    class Program
    {
        private static int port;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter port to host websockets (0 for default & single instance):");
                port = int.Parse(Console.ReadLine());

                LogFactory.Initialize(name => new ConsoleLogger(name));
                ThreadPool.QueueUserWorkItem(_ => Initialize());
                Console.ReadKey();
                Bootstrapper.Stop();
            }
            catch (Exception ex)
            {
                var test = ex;
            }
        }

        private static async void Initialize()
        {
            if (port == 0)
            {
                Bootstrapper.RunInSingleInstanceMode(serverName: "Server", port: 6102,
                    commonServiceBusConnectionString: "Endpoint=sb://inner6.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=e2m6DE0uqc9eWnPEd8ACzRhew6NLT7Kxe0Nv+qW4dVM=",
                    connections: 1000);
                return;
            }
            Bootstrapper.RunInMultiInstanceMode(serverName: "Server" + port, port: port,
                redisSessionsConnectionString: "inner6.redis.cache.windows.net,password=Bc84QBvQZmCt7ytbDpN3tGElxNf4FozxR18MnntJ800=,syncTimeout=10000",
                redisEventsConnectionString: "inner6.redis.cache.windows.net,password=Bc84QBvQZmCt7ytbDpN3tGElxNf4FozxR18MnntJ800=,syncTimeout=10000",
                internalServiceBusConnectionString: "Endpoint=sb://inner6.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=e2m6DE0uqc9eWnPEd8ACzRhew6NLT7Kxe0Nv+qW4dVM=",
                commonServiceBusConnectionString: "Endpoint=sb://inner6.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=e2m6DE0uqc9eWnPEd8ACzRhew6NLT7Kxe0Nv+qW4dVM=",
                connections: 1000);
        }
    }
}
