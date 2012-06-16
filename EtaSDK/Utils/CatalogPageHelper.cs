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

namespace EtaSDK.Utils
{
    public enum ImageResolution
    {
        Thumb,
        View,
        Zoom
    }

    public class CatalogPageHelper
    {
        /// <summary>
        /// https://etastaging.s3.amazonaws.com/img/catalog/view/3c41wO-19.jpg
        /// det lader ikke til at login er krævet for at hente den konkrete JPG....
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static string GetPageUri(string catalogId, string pageIndex, ImageResolution resolution = ImageResolution.Thumb)
        {
            if (resolution == ImageResolution.Thumb)
            {
                return string.Format("https://etastaging.s3.amazonaws.com/img/catalog/thumb/{0}-{1}.jpg", catalogId, pageIndex);

            }
            else if(resolution == ImageResolution.View)
            {
                return string.Format("https://etastaging.s3.amazonaws.com/img/catalog/view/{0}-{1}.jpg", catalogId, pageIndex);

            }
            else if (resolution == ImageResolution.Zoom)
            {
                return string.Format("https://etastaging.s3.amazonaws.com/img/catalog/zoom/{0}-{1}.jpg", catalogId, pageIndex);

            }
            throw new ArgumentOutOfRangeException("ImageResolution");
        }

        public static string GetPageUri(Catalog catalog, int pageIndex, ImageResolution resolution = ImageResolution.Thumb)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException("Catalog");
            }
            int pageCount;
            if(!int.TryParse(catalog.PageCount,out pageCount)){
                throw new ArgumentOutOfRangeException("Catalog.PageCount is not a valid number!");
            }
            if (pageIndex <= 0 || pageIndex > pageCount)
            {
                throw new ArgumentOutOfRangeException("PageIndex should be between 1 and pageCount");
            }

            return GetPageUri(catalog.Id, pageIndex.ToString(), resolution);
        }
    }
}
