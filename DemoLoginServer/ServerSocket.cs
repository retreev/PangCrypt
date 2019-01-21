using System;
using System.Threading.Tasks;
using PangCrypt;

namespace DemoLoginServer
{
    /// <summary>
    ///     Implements PangYa server-side transport framing protocol.
    /// </summary>
    public class ServerSocket
    {
        private readonly byte _key;
        private readonly AsyncSocket _socket;

        public ServerSocket(AsyncSocket socket, byte key)
        {
            _socket = socket;
            _key = key;
        }

        public async Task SendHello()
        {
            // TODO: Remove hardcoded values. This is hardcoded for LoginServer.
            await _socket.Send(new byte[]
                {0x00, 0x0b, 0x00, 0x00, 0x00, 0x00, _key, 0x00, 0x00, 0x00, 0x75, 0x27, 0x00, 0x00});
        }

        public async Task SendPacket(byte[] message)
        {
            await _socket.Send(ServerCipher.Encrypt(message, _key, 0));
        }

        public async Task<byte[]> ReceivePacket()
        {
            var message = new byte[4];
            await _socket.Receive(message);

            var pLen = (message[2] << 8) | message[1];
            Array.Resize(ref message, pLen + 4);
            await _socket.Receive(message, 4, pLen);

            return ClientCipher.Decrypt(message, _key);
        }
    }
}