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
using EtaSDK.ApiModels;
using System.Threading.Tasks;
using System.Linq;

namespace EtaSDK.Test.Tests.EtaSDKv3Tests
{
    //[Ignore]
    [Tag("Offer API")]
    [TestClass]
    public class EtaOfferTests : SilverlightTestEx
    {
        //[Ignore]
        [TestMethod, Asynchronous]
        async public Task GetOfferList_test()
        {
            var api = new EtaSDK.v3.EtaApi();
            var response = await api.GetOfferListAsync(null);
            if (response.HasErrors)
            {
                TestCompleteWithErrorsUISafe("offer list error: " + response.Error.Message);
            }
            else
            {
                var catalogs = response.Result;

                if (catalogs.Any())
                {
                    TestCompleteUISafe();

                }
                else
                {
                    TestCompleteWithErrorsUISafe("Offers is null or Empty");
                }
            }
        }

        [TestMethod, Asynchronous]
        async public Task GetOfferSearch_test()
        {
            var api = new EtaSDK.v3.EtaApi();
            var response = await api.GetOfferSearchAsync(null, "kaffe");
            if (response.HasErrors)
            {
                TestCompleteWithErrorsUISafe("offer search error: " + response.Error.Message);
            }
            else
            {
                var offers = response.Result;

                if (offers.Any())
                {
                    TestCompleteUISafe();
                }
                else
                {
                    TestCompleteWithErrorsUISafe("Offer Search result is null or Empty");
                }
            }
        }


        [TestMethod, Asynchronous]
        async public Task GetOfferListSuggested_test()
        {
            var options = new EtaApiQueryStringParameterOptions();
            
            options.AddParm(EtaApiConstants.EtaApi_Latitude, "55.77012");
            options.AddParm(EtaApiConstants.EtaApi_Longitude, "12.46320");
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "50");
            options.AddParm(EtaApiConstants.EtaApi_Ditance, "10000");

            //options.AddParm(EtaApiConstants.EtaApi_OfferId, "");
            options.AddParm("type", "suggested");
            //options.AddParm("api_limit", "100");
            options.AddParm("from", UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm("to", UNIXTime.GetTimestamp(DateTime.Now.AddDays(14)));

            var api = new EtaSDK.v3.EtaApi();

            var response = await api.GetOfferListAsync(options);
            if (response.HasErrors)
            {
                TestCompleteWithErrorsUISafe("suggeted offers search error: " + response.Error.Message);
            }
            else
            {
                var offers = response.Result;

                if (offers.Any())
                {
                    TestCompleteUISafe();
                }
                else
                {
                    TestCompleteWithErrorsUISafe("Suggested Offers Search result is null or Empty");
                }
            }
        }
    }
    
}
