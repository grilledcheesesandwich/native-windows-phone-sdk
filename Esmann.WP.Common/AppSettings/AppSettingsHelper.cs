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

namespace Esmann.WP.Common.AppSettings
{
    public class AppSettingsHelper
    {
        IsolatedStorageSettings settings;
        public AppSettingsHelper()
        {
            // Get the settings for this application.
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public bool SetValue<T>(string key, T value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(key))
            {
                // If the value has changed
                var obj = settings[key];
                if (obj != (object)value)
                {
                    // Store the new value
                    settings[key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;

        }

        public T GetValue<T>(string key) where T : class, new()
        {
            // If the key exists
            if (settings.Contains(key))
            {
                var obj = settings[key];
                if (obj != null && obj is T)
                {
                    return obj as T;
                }
            }
            return default(T);
        }
        public T GetValueOrNew<T>(string key) where T : class, new()
        {
            var obj = GetValue<T>(key);
            if (obj == null)
            {
                return new T();
            }
            return obj;
        }

        public bool DeleteKey(string key)
        {
            if (settings.Contains(key))
            {
                settings.Remove(key);
                return true;
            }
            return false;
        }

        public void Save()
        {
            settings.Save();
        }


    }
}
