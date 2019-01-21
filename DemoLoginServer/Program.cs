namespace DemoLoginServer
{
    internal static class Program
    {
        private const int PangYaUsLoginServerPort = 10103;

        private static void Main()
        {
            var loginServer = new LoginServer();

            loginServer.Listen(PangYaUsLoginServerPort);
        }
    }
}