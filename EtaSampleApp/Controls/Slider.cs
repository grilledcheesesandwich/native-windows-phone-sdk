using System;

namespace Eta.Controls
{
    public class Slider
    {
        private static int _currentStep = 55;
        public static int currentStep
        {
            get
            {
                return Slider._currentStep;
            }
            set
            {
                Slider._currentStep = Math.Min(Math.Max(value, 0), 55);
            }
        }

        private static int[] stepValues = {100, 150, 200, 250, 300, 350, 400, 450, 500,
        600, 700, 800, 900, 1000,
        1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000,
        15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 65000, 70000, 75000, 80000, 85000, 90000, 95000, 100000,
        200000, 300000, 400000, 500000, 600000, 700000};

        public static int Distance()
        {
            return Slider.stepValues[Slider.currentStep];
        }

        public static string DistanceString()
        {
            double distance = (double)Slider.Distance();

            if (distance >= 1000)
            {
                distance /= 1000;
                string temp = distance.ToString();
                if (distance == Math.Round(distance))
                    temp += ".0";
                return temp.Replace('.', ',') + " km.";
            }

            return distance.ToString() + " m.";
        }


    }
}
