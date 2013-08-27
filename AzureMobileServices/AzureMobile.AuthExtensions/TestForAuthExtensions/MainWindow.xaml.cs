using System;
using System.Windows;
using AzureMobile.AuthExtensions.WPF;
using Microsoft.WindowsAzure.MobileServices;

namespace TestForAuthExtensions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://YOUR-SERVICE.azure-mobile.net/",
            "YOUR-APPLICATION-KEY"
        );

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = await MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
                AddToDebug("User: {0}", user.UserId);
                var apiResult = await MobileService.InvokeApiAsync("user");
                AddToDebug("API result: {0}", apiResult);
            }
            catch (Exception ex)
            {
                AddToDebug("Error: {0}", ex);
            }
        }

        private void AddToDebug(string text, params object[] args)
        {
            if (args != null && args.Length > 0) text = string.Format(text, args);
            this.txtDebug.Text = this.txtDebug.Text + text + Environment.NewLine;
        }
    }
}
