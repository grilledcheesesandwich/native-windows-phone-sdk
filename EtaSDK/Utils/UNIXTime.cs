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

namespace EtaSDK.Utils
{
    public static class UNIXTime
    {
        static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// Gets the UNIX timestamp of a datetime object.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimestamp(DateTime time)
        {
            int seconds = (int)time.Subtract(epoch).TotalSeconds;
            return seconds.ToString();
        }

        public static DateTime GetDateTime(string timestamp)
        {
            int seconds = Convert.ToInt32(timestamp);
            return epoch.AddSeconds(seconds);
        }
    }
}
