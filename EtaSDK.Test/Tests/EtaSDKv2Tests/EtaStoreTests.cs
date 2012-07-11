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
using EtaSDK.Utils;

namespace EtaSDK.Test.Tests.EtaSDKv2Tests
{
    [Ignore]
    [Tag("Store API")]
    [TestClass]
    public class EtaStoreTests : SilverlightTestEx
    {

        //[TestMethod, Asynchronous]
        //public void Test_LoadCatalogList()
        //{
        //    TestCompleteWithErrorsUISafe("msg"); // witnh erros!
        //    TestCompleteUISafe(); // success
        //}

        [TestMethod, Asynchronous]
        public void GetStoreList_test()
        {
            var api = new EtaSDKv2();
            api.GetStoreList(null, stores =>
            {
                if (stores == null || stores.Count == 0)
                {
                    TestCompleteWithErrorsUISafe("Stores is null or Empty");
                }
                TestCompleteUISafe();

            }, (error,uri) =>
            {
                TestCompleteWithErrorsUISafe(error.Message);

            });

        }

        [TestMethod, Asynchronous]
        public void GetStoreInfo_test()
        {
            var api = new EtaSDKv2();
            api.GetStoreInfo(null, store =>
            {
                if (store == null || string.IsNullOrWhiteSpace(store.Id))
                {
                    TestCompleteWithErrorsUISafe("Store is null or Empty");
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
