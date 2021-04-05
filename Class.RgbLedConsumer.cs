using System.Diagnostics;
using System.Text;
using DevBot9.Protocols.Homie;
using uPLibrary.Networking.M2Mqtt;

namespace T2_YahiWithNanoFrameworkBlinky
{
    internal class RgbLedConsumer {
        public string MqttBrokerIp;
        public string MqttClientGuid;
        public RgbLedProvider RgbLedProvider;
        
        private int _redValue;
        private int _greenValue;
        private int _blueValue;
        private bool _isOn;
        private int _intensityValue;
        private string _rgbValueString;
        private string[] _rgbValueStringSplit;

        private MqttClient _mqttClient;
        private HostDevice _hostDevice;
        private HostBooleanProperty _onOffSwitch;
        private HostColorProperty _color;
        private HostIntegerProperty _intensity;

        public RgbLedConsumer() {
            _mqttClient = new MqttClient(MqttBrokerIp);
            _mqttClient.MqttMsgPublishReceived += HandlePublishReceived;
        }

        public void Initialize() {
            _mqttClient.Connect(MqttClientGuid);

            _hostDevice = DeviceFactory.CreateHostDevice("lightbulb", "Colorful lightbulb on ESP32");

            _hostDevice.UpdateNodeInfo("general", "General information and properties", "no-type");

            _color = _hostDevice.CreateHostColorProperty(PropertyType.Parameter, "general", "color", "Color", ColorFormat.Rgb);
            _color.PropertyChanged += HandleColorPropertyChanged;
            _onOffSwitch = _hostDevice.CreateHostBooleanProperty(PropertyType.Parameter, "general", "turn-on-off", "Turn device on or off");
            _onOffSwitch.PropertyChanged += HandleOnOffSwitchPropertyChanged;
            _intensity = _hostDevice.CreateHostIntegerProperty(PropertyType.Parameter, "general", "intensity", "Intensity", 0, "%");
            _intensity.PropertyChanged += HandleIntensityPropertyChanged;

            _hostDevice.Initialize((topic, value, qosLevel, isRetained) => {
                _mqttClient.Publish(topic, Encoding.UTF8.GetBytes(value), 1, true);

            }, topic => {
                _mqttClient.Subscribe(new string[] { topic }, new byte[] { 1 });
            });
        }

        private void HandlePublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e) {
            _hostDevice.HandlePublishReceived(e.Topic, Encoding.UTF8.GetString(e.Message,0,e.Message.Length));
        }

        private void HandleColorPropertyChanged (object sender, PropertyChangedEventArgs e) {
            _rgbValueString = _color.Value.ToRgbString();
            Debug.WriteLine($"Color changed to {_rgbValueString}");
            
            _rgbValueStringSplit = _rgbValueString.Split(',');
            _redValue = int.Parse(_rgbValueStringSplit[0]) * _intensityValue / 100;
            _greenValue = int.Parse(_rgbValueStringSplit[1]) * _intensityValue / 100;
            _blueValue = int.Parse(_rgbValueStringSplit[2]) * _intensityValue / 100;
            
            RgbLedProvider.SetValuesFromRgb(_redValue, _greenValue, _blueValue);
        }

        private void HandleOnOffSwitchPropertyChanged(object sender, PropertyChangedEventArgs e) {
            _isOn = _onOffSwitch.Value;
            Debug.WriteLine($"Switch changed to {_onOffSwitch.Value}");

            if (_isOn == false) RgbLedProvider.SetValuesFromRgb(0,0,0);
            else RgbLedProvider.SetValuesFromRgb(_redValue, _greenValue, _blueValue);
        }

        private void HandleIntensityPropertyChanged(object sender, PropertyChangedEventArgs e) {
            _intensityValue = _intensity.Value;
            Debug.WriteLine($"Intensity changed to {_intensity.Value}");

            _redValue *= _intensityValue / 100;
            _greenValue *= _intensityValue / 100;
            _blueValue *= _intensityValue / 100;

            RgbLedProvider.SetValuesFromRgb(_redValue, _greenValue, _blueValue);
        }

        
    }
}
