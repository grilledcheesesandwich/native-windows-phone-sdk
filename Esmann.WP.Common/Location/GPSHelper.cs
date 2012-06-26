using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Device.Location;
using System.Threading.Tasks;
using Microsoft.Phone.Reactive;

namespace Esmann.WP.Common.Location
{
    public class GPSHelper
    {
        GeoCoordinateWatcher gps;

        public GPSHelper()
        {
            
            gps = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        }

        public async Task<GeoCoordinate> GetPositionAsync()
        {
            var tcs = new TaskCompletionSource<GeoCoordinate>();

            if (gps.Permission != GeoPositionPermission.Granted)
            {
                tcs.SetResult(null);
            }
            bool gotLocation = false;

            var gpsAsObservable = Observable.FromEvent<GeoPositionChangedEventArgs<GeoCoordinate>>(
                        ev => gps.PositionChanged += ev,
                        ev => gps.PositionChanged -= ev);
            var timeout = DateTime.Now.AddSeconds(30);
            gpsAsObservable.Sample(TimeSpan.FromMilliseconds(500))
                //.Timeout(timeout)
                .Select(pos => pos.EventArgs.Position)
                .Where(pos => pos != null && !gotLocation)
                .Where(pos => !pos.Location.IsUnknown)
                .Where(pos => pos.Location.HorizontalAccuracy <= 100)
                .Where(pos => pos.Timestamp.Ticks >= DateTime.Now.AddSeconds(-10).Ticks)
                .Select(pos => pos.Location)
                .Subscribe(next => {
                        gotLocation = true;
                        gps.Stop();
                        tcs.SetResult(next);
                    });
                
            
            gps.Start();
            return await tcs.Task;
        }
    }
}
