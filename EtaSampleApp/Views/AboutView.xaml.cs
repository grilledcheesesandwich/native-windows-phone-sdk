﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using EtaSampleApp.Strings;

namespace EtaSampleApp.Views
{
    public partial class AboutView : EtaBasePage
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.To = AppResources.SupportEmail;
            emailTask.Subject = "eTilbudsavis til Windows Phone - min mening";
            emailTask.Body = "Hej eTilbudsavis,\n";
            emailTask.Show();
        }

        private void webSiteMenu_Click(object sender, EventArgs e)
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri( AppResources.WebSiteUrl);
            browser.Show();
        }

        private void privacyMenu_Click(object sender, EventArgs e)
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri(AppResources.PrivacyPolicyLocationDataUri);
            browser.Show();
        }

        private void rateMenu_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }

        private void facebookMenu_Click(object sender, EventArgs e)
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri(AppResources.FacebookUrl);
            browser.Show();
        }

        private void twitterMenu_Click(object sender, EventArgs e)
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri(AppResources.TwitterUrl);
            browser.Show();
        }

        private void shareAppMenu_Click(object sender, EventArgs e)
        {
            ShareLinkTask share = new ShareLinkTask();
            share.Message = "Prøv eTilbudsavis til Winodws Phone";
            share.Title = "eTilbudsavis til WP";
            share.LinkUri = new Uri(AppResources.WindowsPhoneMarketPlaceAppUri);
            share.Show();
        }
    }
}