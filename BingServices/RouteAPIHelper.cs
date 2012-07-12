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
using System.Threading.Tasks;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using BingServices.RoutesAPI;
using System.Linq;
using System.Collections.ObjectModel;

namespace BingServices
{
    public class RouteAPIHelper
    {
        async public Task<MapPolyline> GetRouteAsync(GeoCoordinate from, GeoCoordinate to)
        {
            TaskCompletionSource<MapPolyline> tcs = new TaskCompletionSource<MapPolyline>();

            RouteServiceClient routeService = new RouteServiceClient("BasicHttpBinding_IRouteService");

            routeService.CalculateRouteCompleted += (sender, e) =>
            {
                var points = e.Result.Result.RoutePath.Points;
                var coordinates = points.Select(x => new GeoCoordinate(x.Latitude, x.Longitude));

                var routeColor = Colors.Blue;
                var routeBrush = new SolidColorBrush(routeColor);

                var routeLine = new MapPolyline()
                {
                    Locations = new LocationCollection(),
                    Stroke = routeBrush,
                    Opacity = 0.65,
                    StrokeThickness = 5.0,
                };

                foreach (var location in points)
                {
                    routeLine.Locations.Add(new GeoCoordinate(location.Latitude, location.Longitude));
                }

                tcs.TrySetResult(routeLine);
            };
            

            routeService.CalculateRouteAsync(new RouteRequest()
            {
                Credentials = new RoutesAPI.Credentials()
                {
                    ApplicationId = Settings.BingmapsAPIKey
                },
                Options = new RouteOptions()
                {
                    RoutePathType = RoutePathType.Points
                },
                Waypoints = new ObservableCollection<Waypoint>(
                    new Waypoint[]{
                        new Waypoint{Location = new Location{Latitude =to.Latitude, Longitude = to.Longitude}},
                        new Waypoint{Location = new Location{Latitude =from.Latitude, Longitude = from.Longitude}}
                
                })
            });

            return await tcs.Task;
        }
    }
}
