using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace ComplexTypes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static MobileServiceClient MobileService = new MobileServiceClient(
            "https://YOUR-SERVICE-HERE.azure-mobile.net/",
            "AppKey"
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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (MobileService.ApplicationKey == "AppKey")
            {
                await new Windows.UI.Popups.MessageDialog(
                    "Please replace the URL/key on the MobileServiceClient constructor prior to running this app", "Error").ShowAsync();
            }
        }

        private async void btnInsertComplexClientSide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rnd = new Random();
                var template = Movies_Client[rnd.Next(Movies_Client.Length)];
                var movieToInsert = new Movie_ComplexClientSide
                {
                    Title = template.Title,
                    ReleaseYear = template.ReleaseYear,
                    Reviews = Enumerable.Range(0, rnd.Next(2) + 1).Select(_ => SampleReviews[rnd.Next(SampleReviews.Length)]).ToArray()
                };
                var table = MobileService.GetTable<Movie_ComplexClientSide>();
                await table.InsertAsync(movieToInsert);
                this.AddToDebug("Inserted movie {0} with id = {1}", movieToInsert.Title, movieToInsert.Id);
            }
            catch (Exception ex)
            {
                this.AddToDebug("Error: {0}", ex);
            }
        }

        private async void btnReadComplexClientSide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var table = MobileService.GetTable<Movie_ComplexClientSide>();
                var movies = await table.ToListAsync();
                this.AddToDebug("All movies in the server:");
                foreach (var movie in movies)
                {
                    this.AddToDebug("  {0} - {1} ({2})", movie.Id, movie.Title, movie.ReleaseYear);
                    this.AddToDebug("    Reviews: {0}", string.Join(", ", movie.Reviews.Select(r => r.Stars == 0 ? "-" : (new string('*', r.Stars)))));
                }
            }
            catch (Exception ex)
            {
                this.AddToDebug("Error: {0}", ex);
            }
        }

        private async void btnInsertComplexServerSide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rnd = new Random();
                var template = Movies[rnd.Next(Movies.Length)];
                var movieToInsert = new Movie
                {
                    Title = template.Title,
                    ReleaseYear = template.ReleaseYear,
                    Reviews = Enumerable.Range(0, rnd.Next(2) + 1).Select(_ => SampleReviews[rnd.Next(SampleReviews.Length)]).ToArray()
                };
                var table = MobileService.GetTable<Movie>();
                await table.InsertAsync(movieToInsert);
                this.AddToDebug("Inserted movie {0} with id = {1}", movieToInsert.Title, movieToInsert.Id);
            }
            catch (Exception ex)
            {
                this.AddToDebug("Error: {0}", ex);
            }
        }

        private async void btnReadComplexServerSide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var table = MobileService.GetTable<Movie>();
                var movies = await table.ToListAsync();
                this.AddToDebug("All movies in the server:");
                foreach (var movie in movies)
                {
                    this.AddToDebug("  {0} - {1} ({2})", movie.Id, movie.Title, movie.ReleaseYear);
                    this.AddToDebug("    Reviews: {0}", string.Join(", ", movie.Reviews.Select(r => r.Stars == 0 ? "-" : (new string('*', r.Stars)))));
                }
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

        #region Test data
        private static Movie_ComplexClientSide[] Movies_Client = new Movie_ComplexClientSide[]
        {
            new Movie_ComplexClientSide { Title = "Argo", ReleaseYear = 2012 },
            new Movie_ComplexClientSide { Title = "Forrest Gump", ReleaseYear = 1994 },
            new Movie_ComplexClientSide { Title = "One Flew Over the Cuckoo's Nest", ReleaseYear = 1975 },
            new Movie_ComplexClientSide { Title = "Rocky", ReleaseYear = 1976 },
            new Movie_ComplexClientSide { Title = "The Godfather", ReleaseYear = 1972 },
            new Movie_ComplexClientSide { Title = "The Godfather: Part II", ReleaseYear = 1974 },
            new Movie_ComplexClientSide { Title = "Schindler's List", ReleaseYear = 1993 },
            new Movie_ComplexClientSide { Title = "The Silence of the Lambs", ReleaseYear = 1991 },
            new Movie_ComplexClientSide { Title = "American Beauty", ReleaseYear = 1999 },
            new Movie_ComplexClientSide { Title = "The Departed", ReleaseYear = 2006 },
            new Movie_ComplexClientSide { Title = "Gladiator", ReleaseYear = 2000 },
            new Movie_ComplexClientSide { Title = "Braveheart", ReleaseYear = 1995 },
            new Movie_ComplexClientSide { Title = "Unforgiven", ReleaseYear = 1992 },
        };
        private static Movie[] Movies = new Movie[]
        {
            new Movie { Title = "Argo", ReleaseYear = 2012 },
            new Movie { Title = "Forrest Gump", ReleaseYear = 1994 },
            new Movie { Title = "One Flew Over the Cuckoo's Nest", ReleaseYear = 1975 },
            new Movie { Title = "Rocky", ReleaseYear = 1976 },
            new Movie { Title = "The Godfather", ReleaseYear = 1972 },
            new Movie { Title = "The Godfather: Part II", ReleaseYear = 1974 },
            new Movie { Title = "Schindler's List", ReleaseYear = 1993 },
            new Movie { Title = "The Silence of the Lambs", ReleaseYear = 1991 },
            new Movie { Title = "American Beauty", ReleaseYear = 1999 },
            new Movie { Title = "The Departed", ReleaseYear = 2006 },
            new Movie { Title = "Gladiator", ReleaseYear = 2000 },
            new Movie { Title = "Braveheart", ReleaseYear = 1995 },
            new Movie { Title = "Unforgiven", ReleaseYear = 1992 },
        };
        private static MovieReview[] SampleReviews = new MovieReview[]
        {
            new MovieReview { Stars = 5, Comment = "Best movie ever!" },
            new MovieReview { Stars = 4, Comment = "Great movie" },
            new MovieReview { Stars = 3, Comment = "A good one" },
            new MovieReview { Stars = 2, Comment = "Just ok" },
            new MovieReview { Stars = 1, Comment = "Better wait for the TV release" },
            new MovieReview { Stars = 0, Comment = "I'll never get those two hours of my life back" }
        };
        #endregion
    }

    [DataTable("Movie_Client")]
    public class Movie_ComplexClientSide
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("year")]
        public int ReleaseYear { get; set; }
        [JsonProperty("reviews")]
        [JsonConverter(typeof(ReviewArrayConverter))]
        public MovieReview[] Reviews { get; set; }
    }

    class ReviewArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MovieReview[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var reviewsAsString = serializer.Deserialize<string>(reader);
            return reviewsAsString == null ?
                null :
                JsonConvert.DeserializeObject<MovieReview[]>(reviewsAsString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var reviewsAsString = JsonConvert.SerializeObject(value);
            serializer.Serialize(writer, reviewsAsString);
        }
    }

    public class Movie
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("year")]
        public int ReleaseYear { get; set; }
        [JsonProperty("reviews")]
        public MovieReview[] Reviews { get; set; }
    }

    public class MovieReview
    {
        [JsonProperty("stars")]
        public int Stars { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
