using System.Diagnostics;
using System.IO;
using System.Text;

namespace DemoLoginServer
{
    public class LoginMessages
    {
        // Note: PangYa does not use UTF-8.
        public static readonly Encoding StringEncoding = Encoding.UTF8;

        public interface IMessage
        {
            byte[] ToBytes();
        }

        public struct ClientLoginMessage : IMessage
        {
            public const ushort MessageId = 0x0001;

            public readonly string Username;
            public readonly string Password;

            public ClientLoginMessage(string username, string password)
            {
                Username = username;
                Password = password;
            }

            public static ClientLoginMessage FromBytes(byte[] data)
            {
                using (var reader = new BinaryReader(new MemoryStream(data)))
                {
                    Debug.Assert(reader.ReadUInt16() == MessageId);
                    var username = reader.ReadPString(StringEncoding);
                    var password = reader.ReadPString(StringEncoding);
                    return new ClientLoginMessage(username, password);
                }
            }

            public byte[] ToBytes()
            {
                var stream = new MemoryStream();

                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(MessageId);
                    writer.WritePString(Username, StringEncoding);
                    writer.WritePString(Password, StringEncoding);
                }

                return stream.GetBuffer();
            }
        }

        public struct ServerSecurity1Message : IMessage
        {
            public const ushort MessageId = 0x0010;

            public readonly string Token;

            public ServerSecurity1Message(string token)
            {
                Token = token;
            }

            public static ServerSecurity1Message FromBytes(byte[] data)
            {
                using (var reader = new BinaryReader(new MemoryStream(data)))
                {
                    Debug.Assert(reader.ReadUInt16() == MessageId);
                    return new ServerSecurity1Message(reader.ReadPString(StringEncoding));
                }
            }

            public byte[] ToBytes()
            {
                var stream = new MemoryStream();

                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(MessageId);
                    writer.WritePString(Token, StringEncoding);
                }

                return stream.ToArray();
            }
        }

        public struct ServerEntry
        {
            public readonly string ServerName;
            public readonly ushort ServerId;
            public readonly ushort NumUsers;
            public readonly ushort MaxUsers;
            public readonly string IpAddress;
            public readonly ushort Port;
            public readonly ushort Flags;

            public ServerEntry(string serverName, ushort serverId, ushort numUsers, ushort maxUsers, string ipAddress,
                ushort port, ushort flags)
            {
                ServerName = serverName;
                ServerId = serverId;
                NumUsers = numUsers;
                MaxUsers = maxUsers;
                IpAddress = ipAddress;
                Port = port;
                Flags = flags;
            }

            public static ServerEntry FromReader(BinaryReader reader)
            {
                var serverName = reader.ReadFixedString(40, StringEncoding);
                var serverId = reader.ReadUInt16();
                var numUsers = reader.ReadUInt16();
                var maxUsers = reader.ReadUInt16();
                reader.ReadUInt32();
                reader.ReadUInt16();
                var ipAddress = reader.ReadFixedString(18, StringEncoding);
                var port = reader.ReadUInt16();
                reader.ReadUInt16();
                var flags = reader.ReadUInt16();
                reader.ReadBytes(16);
                return new ServerEntry(serverName, serverId, numUsers, maxUsers, ipAddress, port, flags);
            }

            public void ToWriter(BinaryWriter writer)
            {
                writer.WriteFixedString(ServerName, 40, StringEncoding);
                writer.Write(ServerId);
                writer.Write(NumUsers);
                writer.Write(MaxUsers);
                writer.Write((uint) 0);
                writer.Write((ushort) 0);
                writer.WriteFixedString(IpAddress, 18, StringEncoding);
                writer.Write(Port);
                writer.Write((ushort) 0);
                writer.Write(Flags);
                writer.Write(new byte[16]);
            }
        }

        public struct ServerListMessage : IMessage
        {
            public const ushort MessageId = 0x0002;

            public readonly ServerEntry[] Servers;

            public ServerListMessage(ServerEntry[] servers)
            {
                Servers = servers;
            }

            public static ServerListMessage FromBytes(byte[] data)
            {
                using (var reader = new BinaryReader(new MemoryStream(data)))
                {
                    Debug.Assert(reader.ReadUInt16() == MessageId);
                    var servers = new ServerEntry[reader.ReadByte()];
                    for (var i = 0; i < servers.Length; i++)
                        servers[i] = ServerEntry.FromReader(reader);
                    return new ServerListMessage(servers);
                }
            }

            public byte[] ToBytes()
            {
                var stream = new MemoryStream();

                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(MessageId);
                    writer.Write((byte) Servers.Length);
                    for (var i = 0; i < Servers.Length; i++)
                        Servers[i].ToWriter(writer);
                }

                return stream.ToArray();
            }
        }
    }
}