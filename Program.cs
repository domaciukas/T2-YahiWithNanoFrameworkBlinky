using DevBot9.Protocols.Homie;
using System;
using System.Diagnostics;

namespace T2_YahiWithNanoFrameworkBlinky
{
    public class Program
    {
        private const string BrokerIp = "192.168.1.42";
        private const string WifiSsid = "";
        private const string WifiPassword = "";
        public static void Main()
        {
            var networkProvider = new NetworkProvider();
            var isConnectedToNetwork = networkProvider.ConnectToNetwork(WifiSsid, WifiPassword);

            var rgbLedProvider = new RgbLedProvider();
            var rgbLedConsumer = new RgbLedConsumer();
            
            if (isConnectedToNetwork) {
                DeviceFactory.Initialize();
                rgbLedProvider.Initialize();

                rgbLedConsumer.MqttBrokerIp = BrokerIp;
                rgbLedConsumer.MqttClientGuid = Guid.NewGuid().ToString();
                rgbLedConsumer.RgbLedProvider = rgbLedProvider;
                rgbLedConsumer.Initialize();
            }
            else {
                Debug.WriteLine("Exiting...");
            }
            
        }
    }
}
