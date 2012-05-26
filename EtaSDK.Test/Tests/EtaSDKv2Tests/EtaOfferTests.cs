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
    [Tag("Offer API")]
    //[TestClass]
    public class EtaOfferTests : SilverlightTestEx
    {

        //[TestMethod, Asynchronous]
        //public void Test_LoadCatalogList()
        //{
        //    TestCompleteWithErrorsUISafe("msg"); // witnh erros!
        //    TestCompleteUISafe(); // success
        //}

        [TestMethod, Asynchronous]
        public void GetOfferList_test()
        {
            var api = new EtaSDKv2();
            api.GetOfferList(null, offers =>
            {
                if(string.IsNullOrWhiteSpace(offers)){
                    TestCompleteWithErrorsUISafe("Offers is null or Empty");
                }
                TestCompleteUISafe();

            }, error =>
            {
                TestCompleteWithErrorsUISafe(error.Message);

            });

        }
    }
}
