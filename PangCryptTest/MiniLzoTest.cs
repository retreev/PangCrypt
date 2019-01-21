using Microsoft.VisualStudio.TestTools.UnitTesting;
using PangCrypt;

namespace PangCryptTest
{
    [TestClass]
    public class MiniLzoTest
    {
        [TestMethod]
        public void TestRoundTrip()
        {
            var input = new byte[6000];
            for (var i = 0; i < 100; i++) input[i] = (byte) i;
            for (var i = 100; i < 6000; i++) input[i] = (byte) (i / 10);
            CollectionAssert.AreEqual(input, MiniLzo.Compress(MiniLzo.Decompress(input)));
        }

        [TestMethod]
        public void TestDecompress()
        {
            byte[] expected =
            {
                0x4a, 0x6f, 0x69, 0x6e, 0x20, 0x75, 0x73, 0x20, 0x6e, 0x6f, 0x77, 0x20, 0x61, 0x6e, 0x64, 0x20, 0x73,
                0x68, 0x61, 0x72, 0x65, 0x20, 0x74, 0x68, 0x65, 0x20, 0x73, 0x6f, 0x66, 0x74, 0x77, 0x61, 0x72, 0x65,
                0x20, 0x4a, 0x6f, 0x69, 0x6e, 0x20, 0x75, 0x73, 0x20, 0x6e, 0x6f, 0x77, 0x20, 0x61, 0x6e, 0x64, 0x20,
                0x73, 0x68, 0x61, 0x72, 0x65, 0x20, 0x74, 0x68, 0x65, 0x20, 0x73, 0x6f, 0x66, 0x74, 0x77, 0x61, 0x72,
                0x65, 0x20
            };
            var actual = MiniLzo.Decompress(new byte[]
            {
                0x00, 0x0d, 0x4a, 0x6f, 0x69, 0x6e, 0x20, 0x75, 0x73, 0x20, 0x6e, 0x6f, 0x77, 0x20, 0x61, 0x6e, 0x64,
                0x20, 0x73, 0x68, 0x61, 0x72, 0x65, 0x20, 0x74, 0x68, 0x65, 0x20, 0x73, 0x6f, 0x66, 0x74, 0x77, 0x70,
                0x01, 0x32, 0x88, 0x00, 0x0c, 0x65, 0x20, 0x74, 0x68, 0x65, 0x20, 0x73, 0x6f, 0x66, 0x74, 0x77, 0x61,
                0x72, 0x65, 0x20, 0x11, 0x00, 0x00
            });
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}