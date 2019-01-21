using System.Net.Sockets;
using System.Threading.Tasks;

namespace DemoLoginServer
{
    /// <summary>
    ///     Wrapper for Socket that provides awaitable asynchronous methods.
    /// </summary>
    public class AsyncSocket
    {
        private readonly Socket _socket;

        public AsyncSocket(Socket socket)
        {
            _socket = socket;
        }

        public Task<int> Send(byte[] buffer)
        {
            return Send(buffer, 0, buffer.Length);
        }

        public Task<int> Send(byte[] buffer, int offset, int size)
        {
            return Task.Factory.FromAsync(
                _socket.BeginSend(buffer, offset, size, SocketFlags.None, null, _socket),
                _socket.EndSend);
        }

        public Task<int> Receive(byte[] buffer)
        {
            return Receive(buffer, 0, buffer.Length);
        }

        public Task<int> Receive(byte[] buffer, int offset, int size)
        {
            return Task.Factory.FromAsync(
                _socket.BeginReceive(buffer, offset, size, SocketFlags.None, null, _socket),
                _socket.EndSend);
        }
    }
}