using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using EtaSampleApp.Strings;
//using EtaSampleApp.Controls;

namespace Eta.Controls
{
    public partial class EtaSlider : UserControl
    {
        public delegate void UpdateEventHandler(object sender, SliderEventArgs e);
        public event UpdateEventHandler UpdateEvent;

        public double previous;
        public EtaSlider()
        {
            InitializeComponent();
            EtaSliderControl.Value = Slider.currentStep;
            EtaSliderTooltip.Text = AppResources.EtaSliderControlText + Slider.DistanceString();
            previous = EtaSliderControl.Value;
        }

        private void EtaSliderControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EtaSliderControl.Value = Math.Round(e.NewValue);
            Slider.currentStep = (int)EtaSliderControl.Value;
            EtaSliderTooltip.Text = AppResources.EtaSliderControlText + Slider.DistanceString();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void EtaSlider_Release(object sender, MouseButtonEventArgs e)
        {
            EtaFadeout.Begin();
            if (EtaSliderControl.Value != previous)
            {
                if (UpdateEvent != null)
                {
                    UpdateEvent(this, new SliderEventArgs { Value = Slider.Distance() });
                }
            }
        }

        private void EtaSliderFocus(object sender, MouseEventArgs e)
        {
            previous = EtaSliderControl.Value;
            EtaFadeout.Stop();
            EtaBubble.Opacity = 1;
        }
    }

    
}
