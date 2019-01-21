using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DemoLoginServer
{
    public class LoginServer
    {
        private const int SocketBacklog = 32;

        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        public void Listen(int port)
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen(SocketBacklog);

            Console.WriteLine($"LoginServer listening for connections on {port}");
            while (true)
            {
                var socket = listener.Accept();
                Console.WriteLine($"Connection established: {socket.RemoteEndPoint}");
                Task.Run(() =>
                {
                    try
                    {
                        var connection = new LoginServerConnection(new ServerSocket(new AsyncSocket(socket), 0));
#pragma warning disable 4014
                        connection.Handle();
#pragma warning restore 4014
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                });
            }
        }
    }
}