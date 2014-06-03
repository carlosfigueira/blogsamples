using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
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

namespace ClientApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://blog20140603.azure-mobile.net/",
            "epDwFBIUlocGnPhBUDUGSfvkBvxRQu65"
        );

        private const int PageSize = 10;

        private int currentIndex = 0;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await this.LoadData();
        }

        private async Task LoadData()
        {
            this.EnableButtons(false);
            this.progress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            var table = MobileService.GetTable<TodoItem>();
            var items = await table
                .Take(PageSize)
                .Skip(this.currentIndex)
                .OrderBy(t => t.Text)
                .IncludeTotalCount()
                .ToListAsync();
            this.lstItems.ItemsSource = items;
            int totalCount = (int)((ITotalCountProvider)items).TotalCount;
            this.lblStatus.Text = string.Format("Showing items from position {0} to {1} (of {2})", currentIndex, currentIndex + PageSize - 1, totalCount);
            this.progress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.EnableButtons(true);
        }

        private void EnableButtons(bool enabled)
        {
            this.btnPageBack.IsEnabled = enabled;
            this.btnPageForward.IsEnabled = enabled;
            this.btnPopulate.IsEnabled = enabled;
            this.btnRead.IsEnabled = enabled;
        }

        private async void btnPopulate_Click(object sender, RoutedEventArgs e)
        {
            this.EnableButtons(false);
            var table = MobileService.GetTable<TodoItem>();
            this.EnableButtons(false);
            var itemsToAdd = 100;
            for (var i = 0; i < itemsToAdd; i++)
            {
                this.lblStatus.Text = "Inserting item " + (i + 1) + " of " + itemsToAdd;
                var item = new TodoItem
                {
                    Text = "Item number " + (i + 1),
                    Complete = false
                };
                await table.InsertAsync(item);
            }

            this.lblStatus.Text = "Finished populating the table";
            this.EnableButtons(true);
        }

        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            await this.LoadData();
        }

        private async void btnPageBack_Click(object sender, RoutedEventArgs e)
        {
            this.currentIndex -= PageSize;
            await this.LoadData();
        }

        private async void btnPageForward_Click(object sender, RoutedEventArgs e)
        {
            this.currentIndex += PageSize;
            await this.LoadData();
        }
    }

    public class TodoItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("complete")]
        public bool Complete { get; set; }
    }
}
