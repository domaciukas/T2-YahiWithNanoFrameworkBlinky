using DevBot9.Protocols.Homie;
using System;
using System.Diagnostics;
using System.Threading;

namespace T2_YahiWithNanoFrameworkBlinky
{
    public class Program
    {
        private const string BrokerIp = "192.168.1.43";
        private const string WifiSsid = "";
        private const string WifiPassword = "";
        public static void Main()
        {
            var networkProvider = new NetworkProvider();
            var isConnectedToNetwork = networkProvider.ConnectToNetwork(WifiSsid, WifiPassword);

            var rgbLedProvider = new RgbLedProvider();
            var rgbLedConsumer = new RgbLedConsumer(BrokerIp);
            
            if (isConnectedToNetwork) {
                DeviceFactory.Initialize();
                rgbLedProvider.Initialize();

;
                rgbLedConsumer.MqttClientGuid = Guid.NewGuid().ToString();
                rgbLedConsumer.RgbLedProvider = rgbLedProvider;
                rgbLedConsumer.Initialize();

                Thread.Sleep(-1);
            }
            else {
                Debug.WriteLine("Exiting...");
            }
            
        }
    }
}
