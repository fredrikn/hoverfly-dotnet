using System;
using System.Linq;

namespace Hoverfly.Core.Configuration
{
    using System.Configuration;
    using System.Net.NetworkInformation;

    public class PortHelper
    {
        public static int GetRandomPort()
        {
            var random = new Random();
            var randomPort = random.Next(1, 65000);
            var counter = 0;

            while (IsPortAlreadyInUse(randomPort))
            {
                if (counter >= 65000)
                    throw new ConfigurationErrorsException("There are no ports available for Hoverfly.");

                randomPort = random.Next(1, 65000);
                counter++;
            }
            return randomPort;
        }

        public static bool IsPortAlreadyInUse(int port)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            return (tcpConnInfoArray.Any(endpoint => endpoint.Port == port));
        }
    }
}
