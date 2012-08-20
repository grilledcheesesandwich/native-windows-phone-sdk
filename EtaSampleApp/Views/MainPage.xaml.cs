using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Eta.Controls;
using EtaSampleApp.Strings;
using EtaSampleApp.UserControls;
using EtaSDK.ApiModels;
using Microsoft.Phone.Shell;

namespace EtaSampleApp.Views
{
    public partial class MainPage : EtaBasePage
    {
        Popup splashScreenPopup = null;
        BackgroundWorker splachscreenWorker = null;
        public MainPage()
        {
            InitializeComponent();
            Initialize();
            LocalizeAppBar();
        }

        private void LocalizeAppBar()
        {
            foreach (ApplicationBarMenuItem menuItem in this.ApplicationBar.MenuItems)
            {
                if (menuItem.Text == "(update)")
                {
                    menuItem.Text = AppResources.AppBarUpdate;
                }
                else if (menuItem.Text == "(about)")
                {
                    menuItem.Text = AppResources.AppBarAbout;
                }
                else if (menuItem.Text == "(settings)")
                {
                    menuItem.Text = AppResources.AppBarSettings;
                }
            }
        }

        async private void Initialize()
        {
            this.ApplicationBar.IsVisible = false;
            etaSlider.UpdateEvent += Slider_UpdateEvent;
            await InitializeSplachScreenAsync();
            await WaitForNetwork();
            await InitializeUserDataAndServices();
            var sliderMoveTo = Eta.Controls.Slider.DistanceToStepValue((int)App.ViewModel.UserViewModel.Distance);
            etaSlider.EtaSliderControl.Value = sliderMoveTo;
        }
        async private Task WaitForNetwork()
        {
            if (IsNetworkAvailable)
            {
                return;
            }
            else
            {
                await TaskEx.Run(() =>
                {
                    while (!IsNetworkAvailable)
                    {
                        Thread.Sleep(250);
                    }
                });
            }
        }
        
        async private Task InitializeUserDataAndServices()
        {
            var userData = App.ViewModel.UserViewModel;
            bool isFirstTimeApplicationRuns = userData.FirstTimeApplicationRuns;
            bool showApplicationIntroductionGuide = true;

            if (isFirstTimeApplicationRuns)
            {
                // optional Show App introduction
                // Get User persmission and location information
                var locationSetup = new LocationSetupUserControl();
                var locationSetupPopup = new Popup()
                {
                    IsOpen = true,
                    Child = locationSetup
                };
                await TaskEx.Run(() => {
                    var run = true;
                    while (run) 
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => {
                            run =  locationSetupPopup.IsOpen;
                        });
                        Thread.Sleep(250);
                    } 
                });
            }
            else
            {
                // use excisting persmissions and location with respect to user choise!
            }
            splashScreenPopup = null;
            App.ViewModel.LoadData();

            if (showApplicationIntroductionGuide)
            {
                // optional show app intro guide (slide show)
            }
        }

        private Task<bool> InitializeSplachScreenAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            splashScreenPopup = new Popup();
            this.LayoutRoot.Children.Add(splashScreenPopup);  
            splashScreenPopup.IsOpen = true;
            splashScreenPopup.Child = new SplashScreenLoadingUserControl();

            splachscreenWorker = new BackgroundWorker();

            splachscreenWorker.DoWork += ((s, args) =>
            {
                Thread.Sleep(int.Parse(AppResources.SplashScreenPopupLoadtime));
            });

            splachscreenWorker.RunWorkerCompleted += ((s, args) =>
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    this.splashScreenPopup.IsOpen = false;
                    this.ApplicationBar.IsVisible = true;
                    tcs.TrySetResult(true);
                }
            );
            });
            splachscreenWorker.RunWorkerAsync();
            return tcs.Task;
        }

        void Slider_UpdateEvent(object sender, SliderEventArgs e)
        {
            if (App.ViewModel.IsUserViewModelLoaded)
            {
                App.ViewModel.UserViewModel.Distance = e.Value;
                App.ViewModel.UpdateViewModel();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.DataContext = App.ViewModel;
            base.OnNavigatedTo(e);

            CatalogsListBox.SelectedIndex = -1;
            StoresListBox.SelectedIndex = -1;
            searchListBox.SelectedIndex = -1;
            suggestedOffersListBox.SelectedIndex = -1;
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
       
        private void CatalogsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender == null){
                return;
            }

            var listbox =  (sender as ListBox);
            if(listbox == null){
                return;
            }

            if(listbox.SelectedIndex == -1){
                return;
            }

            if (listbox.SelectedItem == null)
            {
                return;
            }

            var catalog = listbox.SelectedItem as EtaSDK.ApiModels.Catalog;
            if (catalog != null)
            {
                string uri = String.Format("/Views/CatalogBrowsingView.xaml?catalogId={0}", catalog.Id);
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

        private void searchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            var listbox = sender as ListBox;
            if (listbox == null)
            {
                return;
            }
            if (listbox.SelectedIndex == -1)
            {
                return;
            }

            if (listbox.SelectedItem == null)
            {
                return;
            }
            var offer = listbox.SelectedItem as Offer;
            if (offer == null)
            {
                return;
            }
            App.ViewModel.SelectedOffer = offer;
            NavigationService.Navigate(new Uri("/Views/OfferView.xaml?offerId=" + offer.Id,UriKind.Relative));
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/UserView.xaml",UriKind.Relative));
        }

        private void PhoneTextBox_ActionIconTapped(object sender, EventArgs e)
        {
            this.Focus();
            UpdateSearchList();
        }

        private void UpdateSearchList()
        {
            searchListBox.SelectedIndex = -1;
            App.ViewModel.OfferSearchQueryText = phoneTextBox1.Text;
        }

        private void StoresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            var listbox = (sender as ListBox);
            if (listbox == null)
            {
                return;
            }

            if (listbox.SelectedIndex == -1)
            {
                return;
            }

            if (listbox.SelectedItem == null)
            {
                return;
            }

            var store = listbox.SelectedItem as EtaSDK.ApiModels.Store;
            if (store != null)
            {
                string uri = String.Format("/Views/StoreDetailsView.xaml?storeId={0}", store.Id);
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

        private void phoneTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
                e.Handled = true;
                UpdateSearchList();
            }
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutView.xaml", UriKind.Relative));
        }

        private void updateMenuItem_Click(object sender, EventArgs e)
        {
            App.ViewModel.UpdateViewModel();
        }
    }
}