using System;
using System.Threading.Tasks;
using System.Windows;
using BingServices;
using esmann.WP.Common.ViewModels;
using Esmann.WP.Common.AppSettings;
using Esmann.WP.Common.Location;

namespace EtaSampleApp.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        const string SettingsKeyName = "UserViewModel";

        public bool Save()
        {
            if (FirstTimeApplicationRuns)
            {
                FirstTimeApplicationRuns = false;
            }
            var settings = new AppSettingsHelper();
            return settings.SetValue(SettingsKeyName, this);
        }

        public static async Task<UserViewModel> LoadModelAsync()
        {
            var userModel = await GetUserViewModelFromISOAsync();
          
            var tcs = new TaskCompletionSource<UserViewModel>();
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

        bool allowGPS = true;
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

        double distance = 10000;
        public double Distance
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

        string zipCode = "1577";
        public string ZipCode
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
