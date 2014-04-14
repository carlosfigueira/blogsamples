using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UsersFeature
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://blog20131216.azure-mobile.net/",
            "YOUR-APP-KEY-HERE"
        );

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.Facebook);
        }

        private async void btnMicrosoftLogin_Click(object sender, RoutedEventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.MicrosoftAccount);
        }

        private async void btnGoogleLogin_Click(object sender, RoutedEventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.Google);
        }

        private async Task Login(MobileServiceAuthenticationProvider provider)
        {
            try
            {
                var user = await MobileService.LoginAsync(provider);
                this.AddToDebug("Logged in as {0}", user.UserId);
            }
            catch (Exception ex)
            {
                this.AddToDebug("Error during login: {0}", ex);
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MobileService.Logout();
            AddToDebug("Logged out");
        }

        private async void btnCallAPI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await MobileService.InvokeApiAsync("identities", HttpMethod.Get, null);
                this.AddToDebug("API result: {0}", result);
            }
            catch (Exception ex)
            {
                this.AddToDebug("Error calling API: {0}", ex);
            }
        }

        private async void btnCallTableScript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var table = MobileService.GetTable("testtable");
                var result = await table.ReadAsync("");
                this.AddToDebug("Table script result: {0}", result);
            }
            catch (Exception ex)
            {
                this.AddToDebug("Error calling API: {0}", ex);
            }
        }

        private void AddToDebug(string text, params object[] args)
        {
            if (args != null && args.Length > 0) text = string.Format(text, args);
            this.txtDebug.Text = this.txtDebug.Text + text + Environment.NewLine;
        }

        private async void btnTwitterLogin_Click(object sender, RoutedEventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.Twitter);
        }
    }
}
