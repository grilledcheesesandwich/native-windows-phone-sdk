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
using EtaSDK.ApiModels;

namespace EtaSampleApp.ViewModels
{
    public class CatalogDetailsViewModel
    {
        public CatalogDetailsViewModel(string catalogId, string pageCount)
        {
            int count;
            if (int.TryParse(pageCount, out count))
            {
                Pages = new CatalogPages(catalogId, count, 0 /*CacheSize*/);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Catalog.PageCount");
            }
        }

        public CatalogPages Pages { get; private set; }
        

    }
}
