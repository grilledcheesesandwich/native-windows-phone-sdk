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
using System.Collections.Generic;
using Microsoft.Phone.Controls;

namespace eta.sdk
{
    public class EtaApiOptions
    {
        public Dictionary<string, string> data;
        public string dataType;
        public string type;
        public string contentType;
        public PhoneApplicationPage pageInstance;

        public EtaApiOptions(string dataType, string type, PhoneApplicationPage pageInstance)
        {
            this.dataType = dataType;
            this.type = type;
            this.contentType = "application/x-www-form-urlencoded";
            this.data = new Dictionary<string, string>();
            this.pageInstance = pageInstance;
        }

        public void AddData(string key, string value)
        {
            this.data.Add(key, value);
        }
    }
}
