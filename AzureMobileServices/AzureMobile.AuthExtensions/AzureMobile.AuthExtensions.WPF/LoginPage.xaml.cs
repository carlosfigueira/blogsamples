using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace AzureMobile.AuthExtensions.WPF
{
    public partial class LoginPage : UserControl
    {
        private Uri startUri;
        private Uri endUri;

        private bool loginCancelled = false;
        private string loginToken = null;

        public LoginPage(Uri startUri, Uri endUri)
        {
            InitializeComponent();

            this.startUri = startUri;
            this.endUri = endUri;

            var bounds = Application.Current.MainWindow.RenderSize;
            // TODO: check if those values work well for all providers
            this.grdRootPanel.Width = Math.Max(bounds.Width, 640);
            this.grdRootPanel.Height = Math.Max(bounds.Height, 480);

            this.webControl.LoadCompleted += webControl_LoadCompleted;
            this.webControl.Navigating += webControl_Navigating;
            this.webControl.Navigate(this.startUri);
        }

        void webControl_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri.Equals(this.endUri))
            {
                string uri = e.Uri.ToString();
                int tokenIndex = uri.IndexOf("#token=");
                if (tokenIndex >= 0)
                {
                    this.loginToken = uri.Substring(tokenIndex + "#token=".Length);
                }
                else
                {
                    // TODO: better error handling
                    this.loginCancelled = true;
                }

                ((Popup)this.Parent).IsOpen = false;
            }
        }

        void webControl_LoadCompleted(object sender, NavigationEventArgs e)
        {
            this.progress.Visibility = System.Windows.Visibility.Collapsed;
            this.webControl.Visibility = System.Windows.Visibility.Visible;
        }

        public Task<string> Display()
        {
            Popup popup = new Popup();
            popup.Child = this;
            popup.PlacementRectangle = new Rect(new Size(SystemParameters.FullPrimaryScreenWidth, SystemParameters.FullPrimaryScreenHeight));
            popup.Placement = PlacementMode.Center;
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            popup.IsOpen = true;
            popup.Closed += (snd, ea) =>
            {
                 if (this.loginCancelled)
                {
                    tcs.SetException(new InvalidOperationException("Login cancelled"));
                }
                else
                {
                    tcs.SetResult(this.loginToken);
                }
            };

            return tcs.Task;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.loginCancelled = true;
            ((Popup)this.Parent).IsOpen = false;
        }
    }
}
