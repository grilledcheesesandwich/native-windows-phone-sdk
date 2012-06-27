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
using esmann.WP.Common.ViewModels;
using Esmann.WP.Common.AppSettings;
using System.ComponentModel;
using BingServices;
using Esmann.WP.Common.Location;
using System.Threading.Tasks;

namespace EtaSampleApp.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        const string SettingsKeyName = "UserViewModel";

        #region Statics
        #endregion

        #region Properties

        #endregion

        //[Obsolete("Use GetUserViewModel", true)]
        public UserViewModel()
        {
            Location = new Location();
        }

        private UserViewModel(object dummy)
        {

        }

        private static UserViewModel Load()
        {
            var settings = new AppSettingsHelper();
            return settings.GetValueOrNew<UserViewModel>(SettingsKeyName);
        }

        public bool Save()
        {
            //if (instance != null)
            //{
                if (FirstTimeApplicationRuns)
                {
                    FirstTimeApplicationRuns = false;
                }
                var settings = new AppSettingsHelper();
                return settings.SetValue(SettingsKeyName, this);
            //}
            //return false;
        }

        bool isUpdateingLocation = false;
        public bool IsUpdateingLocation
        {
            get { return isUpdateingLocation; }
            set
            {
                if (value != isUpdateingLocation)
                {
                    isUpdateingLocation = value;
                    NotifyPropertyChanged(() => IsUpdateingLocation);
                }
            }
        }
        //public void UpdateLocationInformationGPS(){
        //    if (!IsUpdateingLocation)
        //    {
        //        IsUpdateingLocation = true;
        //        UpdateLocationInformationGPSInternal();
        //    }
        //}
        public static async Task<UserViewModel> LoadModelAsync()
        {
            var userModel = await GetUserViewModelFromISOAsync();
            //if (userModel != null)
            //{
            //    userModel.AllowGPS = userModel.AllowGPS;
            //    userModel.Distance = userModel.Distance;
            //    userModel.FirstTimeApplicationRuns = userModel.FirstTimeApplicationRuns;
            //    userModel.Location = userModel.Location;
            //}

            var tcs = new TaskCompletionSource<UserViewModel>();
            //Location newLocation = null;

            if (userModel.AllowGPS)
            {
                userModel.Location = await GetLocationFromGPSAsync();
            }
            else
            {
                if (userModel.Location.ZipCode == -1)
                {
                    userModel.Location.ZipCode = 2900;
                }
                userModel.Location = await GetLocationFromPostalCodeAsync(userModel.Location.ZipCode.ToString());
                if (!userModel.Location.IsValid)
                {
                    int lksajf = 0;
                }

            }

            tcs.SetResult(userModel);

            return await tcs.Task;
        }

        public static async Task<UserViewModel> GetUserViewModelFromISOAsync()
        {
            var tcs = new TaskCompletionSource<UserViewModel>();
            var settings = new AppSettingsHelper();
            var model = settings.GetValueOrNew<UserViewModel>(SettingsKeyName);
            tcs.SetResult(model);
            return await tcs.Task;
        }

        private static async Task<Location> GetLocationFromGPSAsync()
        {
            var tcs = new TaskCompletionSource<Location>();
            var gps = new GPSHelper();
            var result = await gps.GetPositionAsync();
            Location location = null;
            if (result == null)
            {
                tcs.SetException(new Exception("Could not get position from GPS"));
            }
            location = new Location()
                {
                    IsGeoCoded = true,
                    IsValid = true,
                    Latitude = result.Latitude,
                    Longitude = result.Longitude,
                    Timestamp = DateTime.Now.Ticks
                };

            var locationAPI = new LocationsAPIHelper();
            var address = await locationAPI.GeoCoordinateToZipCodeAsync(result.Latitude, result.Longitude);

            int zipCode = 0;
            if (!string.IsNullOrWhiteSpace(address) && int.TryParse(address, out zipCode))
            {
                location.ZipCode = zipCode;
            }
            else
            {
                location.ZipCode = 0;
                MessageBox.Show("Kunne ikke matche din lokation med et postnummer.", "Postnummer", MessageBoxButton.OK);
            }
            tcs.SetResult(location);
            //IsUpdateingLocation = false;
            return await tcs.Task;

        }

        public static async Task<Location> GetLocationFromPostalCodeAsync(string postalCode)
        {
            var tcs = new TaskCompletionSource<Location>();
            int post = 0;
            if (int.TryParse(postalCode, out post))
            {

            }
            //if (Location != null && Location.ZipCode.ToString() == postalCode)
            //{
            //    tcs.SetResult(Location);
            //}
            //else
            //{
            var location = new BingServices.LocationsAPIHelper();
            var result = await location.ZipCodeToGeoCoordinateAsync(postalCode);
            Location locationResult = null;
            if (result != null)
            {
                locationResult = new Location
                {
                    IsGeoCoded = false,
                    IsValid = true,
                    Latitude = result.Latitude,
                    Longitude = result.Longitude,
                    Timestamp = DateTime.Now.Ticks,
                    ZipCode = post
                };
            }
            else
            {
                locationResult = new Location
                {
                    IsGeoCoded = false,
                    IsValid = false,
                    Latitude = 0,
                    Longitude = 0,
                    Timestamp = DateTime.Now.Ticks,
                    ZipCode = post
                };
                MessageBox.Show("Kunne ikke genkende postnummeret.", "Postnummer", MessageBoxButton.OK);
            }
            tcs.SetResult(locationResult);
            //}
            return await tcs.Task;
        }

        bool firstTimeApplicationRuns = true;
        public bool FirstTimeApplicationRuns
        {
            get { return firstTimeApplicationRuns; }
            set
            {
                if (value != firstTimeApplicationRuns)
                {
                    firstTimeApplicationRuns = value;
                    NotifyPropertyChanged(() => FirstTimeApplicationRuns);
                }
            }
        }

        bool allowGPS = false;
        public bool AllowGPS
        {
            get { return allowGPS; }
            set
            {
                if (value != allowGPS)
                {
                    allowGPS = value;
                    NotifyPropertyChanged(() => AllowGPS);
                }
            }
        }

        int distance = 10000;
        public int Distance
        {
            get { return distance; }
            set
            {
                if (value != distance)
                {
                    distance = value;
                    NotifyPropertyChanged(() => Distance);
                }
            }
        }

        Location location = new Location();
        public Location Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    NotifyPropertyChanged(() => Location);
                }
            }
        }
    }

    public class Location : ViewModelBase
    {
        long timestamp = 0;
        public long Timestamp
        {
            get { return timestamp; }
            set
            {
                if (value != timestamp)
                {
                    timestamp = value;
                    NotifyPropertyChanged(() => Timestamp);
                }
            }
        }

        double latitude = 0;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (value != latitude)
                {
                    latitude = value;
                    NotifyPropertyChanged(() => Latitude);
                }
            }
        }

        int accuracy = int.MaxValue ;
        public int Accuracy
        {
            get { return accuracy; }
            set
            {
                if (value != accuracy)
                {
                    accuracy = value;
                    NotifyPropertyChanged(() => Accuracy);
                }
            }
        }

        double longitude = 0;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (value != longitude)
                {
                    longitude = value;
                    NotifyPropertyChanged(() => Longitude);
                }
            }
        }

        bool isGeoCoded = true;
        public bool IsGeoCoded
        {
            get { return isGeoCoded; }
            set
            {
                if (value != isGeoCoded)
                {
                    isGeoCoded = value;
                    NotifyPropertyChanged(() => IsGeoCoded);
                }
            }
        }
        
        int zipCode = -1;
        public int ZipCode
        {
            get { return zipCode; }
            set
            {
                if (value != zipCode)
                {
                    zipCode = value;
                    NotifyPropertyChanged(() => ZipCode);
                }
            }
        }

        bool isValid = false;
        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (value != isValid)
                {
                    isValid = value;
                    NotifyPropertyChanged(() => IsValid);
                }
            }
        }
    }
}
