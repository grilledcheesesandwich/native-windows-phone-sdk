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
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EtaSDK.Test.Tests.Common
{
    public class SilverlightTestEx : SilverlightTest
    {
        public void TestCompleteWithErrorsUISafe(string msg)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Assert.IsTrue(false, "Assert description: " + msg);
                EnqueueTestComplete();
            });
        }
        public void TestCompleteUISafe()
        {

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                EnqueueTestComplete();
            });
        }
    }
}
