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

namespace EtaSDK
{
    public class EtaSDKv3
    {
        private async Task<string> GetWebpage()
        {
            return await new WebClient().DownloadStringTaskAsync(new Uri("http://www.bing.com"));
        }

    }
}
