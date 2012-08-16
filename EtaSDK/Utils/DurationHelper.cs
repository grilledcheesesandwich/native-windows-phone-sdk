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
using EtaSDK.Properties;
using System.Globalization;

namespace EtaSDK.Utils
{
    public enum DurationState
    {
        Expired,
        Active,
        NotActive
    }

    public class DurationHelper
    {
        static CultureInfo culture = null;
        static DurationHelper()
        {
            culture = new CultureInfo(Resources.CultureString);
        }

        internal static DurationState GetDurationState(string runFrom, string runTill)
        {
            var from = UNIXTime.GetDateTime(runFrom);
            var till = UNIXTime.GetDateTime(runTill);

            // Three states:
            //		1) Expired
            //		2) Not active yet
            //		3) Active

            var today = DateTime.Now.Date;
            if (today > till)
            {
                return DurationState.Expired;
            }
            else if( (till - today).Days <= 1.0)
            {
                return DurationState.NotActive;
            }
            else
            {
                return DurationState.Active;
            }
        }
        public static string GetDurationColor(string runFrom, string runTill)
        {
            var state = GetDurationState(runFrom, runTill);
            string color = "Transparent";
            if (state == DurationState.Active)
            {
                color = Resources.ActiveColor;
            }
            else if (state == DurationState.Expired)
            {
                color = Resources.ExpiredColor;

            }
            else if (state == DurationState.NotActive)
            {
                color = Resources.NotActiveColor;

            }
            return color;
        }
        public static string GetDurationLabel(string runFrom, string runTill)
        {
            var state = GetDurationState(runFrom,runTill);
            var from = UNIXTime.GetDateTime(runFrom);
            var till = UNIXTime.GetDateTime(runTill);

            // Three states:
            //		1) Expired
            //		2) Not active yet
            //		3) Active

            var today = DateTime.Now.Date;
            string label = null;
            if (today > till)
            {
                label = Resources.ExpiredLabel;
            }
            else if ((from - today).TotalDays > 0 && (from - today).TotalDays <= 1)
            {
                label = string.Format("Fra i morgen t.o.m {0} ", DateTimeFormatInfo.GetInstance(culture).GetDayName(till.DayOfWeek));
            }
            else if ((from - today).TotalDays > 0 && (from - today).TotalDays > 1)
            {
                label = string.Format("Fra {0} t.o.m {1} ", DateTimeFormatInfo.GetInstance(culture).GetDayName(from.DayOfWeek), DateTimeFormatInfo.GetInstance(culture).GetDayName(till.DayOfWeek));
            }
            else if (till > today)
            {
                label = string.Format("t.o.m {0} ", 
                    DateTimeFormatInfo.GetInstance(culture).GetDayName(till.DayOfWeek));
            }
            else
            {
                label= "ukendt";    
            }
            return label;
        }
    }
}
