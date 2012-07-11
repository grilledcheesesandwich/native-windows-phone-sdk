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
using Esmann.WP.Common.IsolatedStorage;

namespace EtaSDK.Utils
{
    public class EtaCatalogStorageHelper
    {
        public static void ResetStorage()
        {
            IsoStorageHelper iso = new IsoStorageHelper();
            iso.DeleteDiretory(EtaSDK.Properties.Resources.CatalogBaseIsoFolder);
        }
    }
}
