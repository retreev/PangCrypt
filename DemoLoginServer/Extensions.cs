using System;
using System.IO;
using System.Text;

namespace DemoLoginServer
{
    public static class BinaryReaderExtensions
    {
        /// <summary>
        ///     Reads a fixed-length string from the stream.
        /// </summary>
        /// <param name="reader">BinaryReader to use.</param>
        /// <param name="length">Length of string in bytes.</param>
        /// <param name="encoding">Encoding to use.</param>
        /// <returns></returns>
        public static string ReadFixedString(this BinaryReader reader, int length, Encoding encoding)
        {
            var bytes = reader.ReadBytes(length);
            for (var i = bytes.Length - 1; i >= 0; i--)
                if (bytes[i] != 0)
                    Array.Resize(ref bytes, i + 1);
            return new string(encoding.GetChars(bytes));
        }

        /// <summary>
        ///     Reads a Pascal-style length-prefix string, stored as a Uint16
        ///     followed by a string.
        /// </summary>
        /// <param name="reader">BinaryReader to use.</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadPString(this BinaryReader reader, Encoding encoding)
        {
            var messageLength = reader.ReadUInt16();
            return new string(encoding.GetChars(reader.ReadBytes(messageLength)));
        }
    }

    public static class BinaryWriterExtensions
    {
        /// <summary>
        ///     Writes a fixed-length string to the stream.
        /// </summary>
        /// <param name="writer">BinaryWriter to use.</param>
        /// <param name="str">String to write.</param>
        /// <param name="length">Length of string in bytes.</param>
        /// <param name="encoding">Encoding to use.</param>
        public static void WriteFixedString(this BinaryWriter writer, string str, int length, Encoding encoding)
        {
            var bytes = encoding.GetBytes(str);
            Array.Resize(ref bytes, length);
            writer.Write(bytes);
        }

        /// <summary>
        ///     Writes a Pascal-style length-prefix string, stored as a Uint16
        ///     followed by a string.
        /// </summary>
        /// <param name="writer">BinaryWriter to use.</param>
        /// <param name="str">String to write.</param>
        /// <param name="encoding">Encoding to use.</param>
        public static void WritePString(this BinaryWriter writer, string str, Encoding encoding)
        {
            var bytes = encoding.GetBytes(str);
            writer.Write((ushort) bytes.Length);
            writer.Write(bytes);
        }
    }
}