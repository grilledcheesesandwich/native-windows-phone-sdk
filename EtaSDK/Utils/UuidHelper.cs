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
using System.IO.IsolatedStorage;
using System.Text;

namespace EtaSDK.Utils
{
    public class UuidHelper
    {
        private const string EtaUuidIsolatedStorageKey = "etauuid";
        
        public static string GetUuid()
        {
            string sGuid;
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>(EtaUuidIsolatedStorageKey, out sGuid))
            {
                Guid guid = System.Guid.NewGuid();

                StringBuilder sb = new StringBuilder();

                foreach (char c in guid.ToString())
                {
                    if (c != '-')
                    {
                        sb.Append(c);
                    }
                }

                sGuid = sb.ToString();
                IsolatedStorageSettings.ApplicationSettings.Add(EtaUuidIsolatedStorageKey, sGuid);
                IsolatedStorageSettings.ApplicationSettings.Save();
            }

            return sGuid;
        }
    }
}
