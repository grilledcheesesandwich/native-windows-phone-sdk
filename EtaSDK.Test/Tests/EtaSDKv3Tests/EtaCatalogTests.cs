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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using WindowsPhoneEssentials.Testing.Threading;
using EtaSDK.Test.Tests.Common;
using System.Linq;
using System.Threading.Tasks;
namespace EtaSDK.Test.Tests.EtaSDKv3Tests
{
    //[Ignore]
    [Tag("Catalog API")]
    [TestClass]
    public class EtaCatalogTests : SilverlightTestEx
    {
        /*
         * Minimum requirements, to implement and pass the following API calls:
             /catalog/list/
             /catalog/stats/collect/ (til pageflip)
         */
        
        //[TestMethod, Asynchronous]
        //public void Test_LoadCatalogList()
        //{
        //    TestCompleteWithErrorsUISafe("msg"); // witnh erros!
        //    TestCompleteUISafe(); // success
        //}

        [TestMethod, Asynchronous]
        async public Task GetCatalogList_test()
        {
            var api = new EtaSDK.v3.EtaApi();

            var response = await api.GetCatalogListAsync(null);

            if (response.HasErrors)
            {
                TestCompleteWithErrorsUISafe("Catalogs error: " + response.Error.Message);
            }
            var catalogs = response.Result;
            if (catalogs == null || !catalogs.Any())
            {
                TestCompleteWithErrorsUISafe("Catalogs == null or empty");
            }
            foreach (var catalog in catalogs)
            {
                if (catalog == null || catalog.Id == null)
                {
                    TestCompleteWithErrorsUISafe("Catalog == null");
                }

                if (catalog.Store == null || catalog.Store.Id == null)
                {
                    TestCompleteWithErrorsUISafe("Catalog.Store == null");
                }

                if (catalog.Store.Country == null || catalog.Store.Country.Alpha2 == null)
                {
                    TestCompleteWithErrorsUISafe("Catalog.Store.Country == null");
                }

                if (catalog.Store.Dealer == null || catalog.Store.Dealer.Id == null)
                {
                    TestCompleteWithErrorsUISafe("Catalog.Store.Dealer == null");
                }

                if (catalog.Store.Dealer.Branding == null || catalog.Store.Dealer.Branding.Color == null)
                {
                    TestCompleteWithErrorsUISafe("Catalog.Store.Dealer.Branding == null");
                }
            }
            TestCompleteUISafe();
        }

        [Ignore]
        [TestMethod, Asynchronous]
        async public Task GetCatalogInfo_test()
        {
            string id = "1a94vO"; // Irma catalog id ?!
            string publicKey = "1a94vO"; // Irma publickey ?!
            var api = new EtaSDK.v3.EtaApi();

            var response = await api.GetCatalogInfoAsync(id, publicKey);
            if (response.HasErrors)
            {
                TestCompleteWithErrorsUISafe("Catalog info error: " + response.Error.Message);
            }
            var catalog = response.Result;

            if (catalog == null || string.IsNullOrWhiteSpace(catalog.Id))
            {
                TestCompleteWithErrorsUISafe("catalog is null or empty");
            }
            else
            {
                TestCompleteUISafe();
            }
        }
    }
}
