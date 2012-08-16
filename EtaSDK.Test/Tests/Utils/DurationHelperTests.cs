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
using EtaSDK.Test.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Threading.Tasks;
using System.Linq;
using EtaSDK.Utils;

namespace EtaSDK.Test.Tests.Utils
{
    //[Ignore]
    [Tag("Util DurationHelper Tests")]
    [TestClass]
    public class DurationHelperTests : SilverlightTestEx
    {
        [TestMethod, Asynchronous]
        async public Task GetDuratioinTest_1()
        {
            var api = new EtaSDK.v3.EtaApi();
            var response = await api.GetCatalogListAsync(null);
            if (response.HasErrors)
            {
                TestCompleteWithErrorsUISafe("Catalog error: " + response.Error.Message);
            }
            else
            {
                var catalogs = response.Result;
                if (catalogs == null || !catalogs.Any())
                {
                    TestCompleteWithErrorsUISafe("Catalog null or Empty");
                }
                else
                {
                    var catalog = catalogs.First();
                    var durationLabel = DurationHelper.GetDurationLabel(catalog.RunFrom, catalog.RunTill);
                    if (string.IsNullOrWhiteSpace(durationLabel))
                    {
                        TestCompleteWithErrorsUISafe("durationLabel null or Empty");
                    }
                    TestCompleteUISafe();
                }
            }
        }
    }
}
