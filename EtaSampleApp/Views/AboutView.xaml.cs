using System;
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
            emailTask.To = "support@etilbudsavis.dk";
            emailTask.Cc = "mesmann@microsoft.com";
            emailTask.Subject = "eTilbudsavis til Windows Phone - min mening";
            emailTask.Body = "Hej eTilbudsavis,\n";
            emailTask.Show();
        }
    }
}