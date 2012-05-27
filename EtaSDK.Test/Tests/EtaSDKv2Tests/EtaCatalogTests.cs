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

namespace EtaSDK.Test.Tests.EtaSDKv2Tests
{
    [Tag("Catalog API")]
    [TestClass]
    public class EtaCatalogTests : SilverlightTestEx
    {

        //[TestMethod, Asynchronous]
        //public void Test_LoadCatalogList()
        //{
        //    TestCompleteWithErrorsUISafe("msg"); // witnh erros!
        //    TestCompleteUISafe(); // success
        //}

        [TestMethod, Asynchronous]
        public void GetCatalogList_test()
        {
            var api = new EtaSDKv2();
            api.GetCatalogList(null, catalogs =>
            {
                if (catalogs == null || catalogs.Count == 0)
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

                    if (catalog.Store.Country == null || catalog.Store.Country.Id == null)
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

            }, error =>
            {
                var msg = error.Message;
                TestCompleteWithErrorsUISafe(msg);
            });
        }

        //[Ignore]
        [TestMethod, Asynchronous]
        public void GetCatalogInfo_test()
        {
            string id = "2905aO"; // matas catalog id ?!
            var api = new EtaSDKv2();
            api.GetCatalogInfo(id, catalog =>
            {
                if (catalog != null && string.IsNullOrWhiteSpace( catalog.Id))
                {
                    TestCompleteWithErrorsUISafe("catalog is null or empty");
                }
                else
                {
                    TestCompleteUISafe();
                }

            }, error =>
            {
                TestCompleteWithErrorsUISafe(error.Message);
            });
        }

    }
}
