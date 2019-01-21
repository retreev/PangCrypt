using System;
using System.Threading.Tasks;

namespace DemoLoginServer
{
    public class LoginServerConnection
    {
        private readonly ServerSocket _socket;

        public LoginServerConnection(ServerSocket socket)
        {
            _socket = socket;
        }

        public async Task Handle()
        {
            await _socket.SendHello();

            var loginMessage = LoginMessages.ClientLoginMessage.FromBytes(await _socket.ReceivePacket());

            Console.WriteLine($"Login packet: {loginMessage.Username}:{loginMessage.Password}");

            await SendMessage(new LoginMessages.ServerSecurity1Message("7430F52"));

            await SendMessage(new LoginMessages.ServerListMessage(new[]
                {new LoginMessages.ServerEntry("PangCryptTest", 20202, 1, 2000, "127.0.0.1", 20202, 0x800)}));

            await _socket.ReceivePacket();
        }

        private async Task SendMessage(LoginMessages.IMessage message)
        {
            await _socket.SendPacket(message.ToBytes());
        }
    }
}