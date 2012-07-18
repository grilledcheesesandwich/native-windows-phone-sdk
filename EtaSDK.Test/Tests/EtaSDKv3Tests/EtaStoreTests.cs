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
using System.Linq;
using System.Threading.Tasks;

namespace EtaSDK.Test.Tests.EtaSDKv3Tests
{
    //[Ignore]
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
        async public Task GetStoreList_test()
        {
            var api = new EtaSDK.v3.EtaApi();
            var response = await api.GetStoreListAsync(null);
            if (response.HasErrors)
            {
                TestCompleteWithErrorsUISafe("Store info error: " + response.Error.Message);
            }
            else
            {
                var stores = response.Result;

                if (stores == null || !stores.Any())
                {
                    TestCompleteWithErrorsUISafe("Stores is null or Empty");
                }
                TestCompleteUISafe();
            }
        }

        [TestMethod, Asynchronous]
        async public Task GetStoreInfo_test()
        {
            var api = new EtaSDK.v3.EtaApi();
            var response = await api.GetStoreInfoAsync(null);

            if (response.HasErrors)
            {
                TestCompleteWithErrorsUISafe("Store info error: " + response.Error.Message);
            }
            else
            {
                var store = response.Result;

                if (store == null || string.IsNullOrWhiteSpace(store.Id))
                {
                    TestCompleteWithErrorsUISafe("Store is null or Empty");
                }
                else
                {
                    TestCompleteUISafe();
                }
            }

        }
    }
}
