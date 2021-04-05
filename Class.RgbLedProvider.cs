using Windows.Devices.Pwm;

namespace T2_YahiWithNanoFrameworkBlinky
{
    class RgbLedProvider
    {
        private PwmController _redController = PwmController.FromId("TIM0");
        private PwmController _greenController = PwmController.FromId("TIM1");
        private PwmController _blueController = PwmController.FromId("TIM2");

        private PwmPin _redPin;
        private PwmPin _greenPin;
        private PwmPin _bluePin;
        public void Initialize() {
            _redController.SetDesiredFrequency(10000);
            _greenController.SetDesiredFrequency(10000);
            _blueController.SetDesiredFrequency(10000);

            _redPin = _redController.OpenPin(0);
            _greenPin = _greenController.OpenPin(2);
            _bluePin = _blueController.OpenPin(4);

            _redPin.SetActiveDutyCyclePercentage(0.000f);
            _greenPin.SetActiveDutyCyclePercentage(0.000f);
            _bluePin.SetActiveDutyCyclePercentage(0.000f);

            _redPin.Start();
            _greenPin.Start();
            _bluePin.Start();
        }
        public void SetValuesFromRgb(int red, int green, int blue) {
            var redCycle = red / 255.000f * 100.000f;
            var greenCycle = green / 255.000f * 100.000f;
            var blueCycle = blue / 255.000f * 100.000f;

            _redPin.SetActiveDutyCyclePercentage(redCycle);
            _greenPin.SetActiveDutyCyclePercentage(greenCycle);
            _bluePin.SetActiveDutyCyclePercentage(blueCycle);
        }
    }

    
}
