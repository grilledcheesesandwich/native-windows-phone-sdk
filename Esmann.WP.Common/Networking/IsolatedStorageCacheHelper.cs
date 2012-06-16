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
using System.Threading;
using EtaSDK.Utils;
using System.IO.IsolatedStorage;
using System.Diagnostics;

namespace Esmann.WP.Common.Networking
{
    public class IsolatedStorageCacheHelper
    {
        IsoStorageHelper isoStore = new IsoStorageHelper();
        private object syncContext = new object();
        public void FetchResource(string isolatedStoragePath, Uri networkUri, Action<string> callback)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                if (isoStore.FileExists(isolatedStoragePath))
                {
                    callback(isolatedStoragePath);
                    return;
                }

                WebClient client = new WebClient();
                client.OpenReadAsync(networkUri);
                
                client.OpenReadCompleted += (sender, e) =>
                {
                    if (e.Cancelled || e.Error != null)
                    {
                        callback(null);
                        return;
                    }
                    using (var stream = e.Result)
                    {
                        lock (syncContext)
                        {
                            if (isoStore.FileExists(isolatedStoragePath))
                            {
                                callback(isolatedStoragePath);
                                return;
                            }

                            using (IsolatedStorageFileStream fileStream = isoStore.CreateFile(isolatedStoragePath))
                            {
                                stream.CopyTo(fileStream);
                                fileStream.Close();
                            }
                            Debug.WriteLine("Downloaded uri: " + networkUri.ToString() + " -->" + isolatedStoragePath);
                        }
                    }

                    callback(isolatedStoragePath);
                    return;
                };
            });
        }
    }
}
