using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GettingUserInfoFromAuthProviders
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://YOUR-SERVICE-NAME.azure-mobile.net/",
            "YOUR-SERVICE-APP-KEY"
        );
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void btnFacebook_Click(object sender, RoutedEventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.Facebook);
        }

        private async void btnGoogle_Click(object sender, RoutedEventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.Google);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MobileService.Logout();
        }

        private async void btnTwitter_Click(object sender, RoutedEventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.Twitter);
        }

        private async void btnMicrosoft_Click(object sender, RoutedEventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.MicrosoftAccount);
        }

        private async Task Login(MobileServiceAuthenticationProvider provider)
        {
            try
            {
                var user = await MobileService.LoginAsync(provider);
                AddToDebug("Logged in as {0}", user.UserId);
            }
            catch (Exception ex)
            {
                this.AddToDebug("Error: {0}", ex);
            }
        }

        private async void btnCallAPI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await MobileService.InvokeApiAsync("userinfo", HttpMethod.Get, null);
                this.AddToDebug("Result: {0}", result);
            }
            catch (Exception ex)
            {
                this.AddToDebug("Error: {0}", ex);
            }
        }

        private void AddToDebug(string text, params object[] args)
        {
            if (args != null && args.Length > 0) text = string.Format(text, args);
            this.txtDebug.Text = this.txtDebug.Text + text + Environment.NewLine;
        }
    }
}
