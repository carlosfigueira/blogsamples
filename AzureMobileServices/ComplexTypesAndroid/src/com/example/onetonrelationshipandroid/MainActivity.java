package com.example.onetonrelationshipandroid;

import java.net.MalformedURLException;
import java.util.List;
import java.util.Random;

import android.os.Bundle;
import android.app.Activity;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import com.microsoft.windowsazure.mobileservices.*;

public class MainActivity extends Activity {

	private MobileServiceClient mClient;
	private MobileServiceClient mClient_ComplexClientSide;

	private static Movie[] SampleMovies;
	private static MovieReview[] SampleReviews;
	private static Movie_ComplexClientSide[] SampleMovieComplexClientSide;
	
	private final static String MobileServiceUrl = "https://MOBILE-SERVICE-NAME.azure-mobile.net/";
	private final static String MobileServiceKey = "APPLICATION-KEY-HERE";
	
	static {
		SampleMovies = new Movie[13];
		SampleMovies[0] = new Movie("Argo", 2012);
		SampleMovies[1] = new Movie("Forrest Gump", 1994);
		SampleMovies[2] = new Movie("One Flew Over the Cuckoo's Nest", 1975);
		SampleMovies[3] = new Movie("Rocky", 1976);
		SampleMovies[4] = new Movie("The Godfather", 1972);
		SampleMovies[5] = new Movie("The Godfather: Part II", 1974);
		SampleMovies[6] = new Movie("Schindler's List", 1993);
		SampleMovies[7] = new Movie("The Silence of the Lambs", 1991);
		SampleMovies[8] = new Movie("American Beauty", 1999);
		SampleMovies[9] = new Movie("The Departed", 2006);
		SampleMovies[10] = new Movie("Gladiator", 2000);
		SampleMovies[11] = new Movie("Braveheart", 1995);
		SampleMovies[12] = new Movie("Unforgiven", 1992);

		SampleMovieComplexClientSide = new Movie_ComplexClientSide[13];
		SampleMovieComplexClientSide[0] = new Movie_ComplexClientSide("Argo", 2012);
		SampleMovieComplexClientSide[1] = new Movie_ComplexClientSide("Forrest Gump", 1994);
		SampleMovieComplexClientSide[2] = new Movie_ComplexClientSide("One Flew Over the Cuckoo's Nest", 1975);
		SampleMovieComplexClientSide[3] = new Movie_ComplexClientSide("Rocky", 1976);
		SampleMovieComplexClientSide[4] = new Movie_ComplexClientSide("The Godfather", 1972);
		SampleMovieComplexClientSide[5] = new Movie_ComplexClientSide("The Godfather: Part II", 1974);
		SampleMovieComplexClientSide[6] = new Movie_ComplexClientSide("Schindler's List", 1993);
		SampleMovieComplexClientSide[7] = new Movie_ComplexClientSide("The Silence of the Lambs", 1991);
		SampleMovieComplexClientSide[8] = new Movie_ComplexClientSide("American Beauty", 1999);
		SampleMovieComplexClientSide[9] = new Movie_ComplexClientSide("The Departed", 2006);
		SampleMovieComplexClientSide[10] = new Movie_ComplexClientSide("Gladiator", 2000);
		SampleMovieComplexClientSide[11] = new Movie_ComplexClientSide("Braveheart", 1995);
		SampleMovieComplexClientSide[12] = new Movie_ComplexClientSide("Unforgiven", 1992);

		SampleReviews = new MovieReview[6];
		SampleReviews[0] = new MovieReview(5, "Best. Movie. Ever.");
		SampleReviews[1] = new MovieReview(4, "Great movie");
		SampleReviews[2] = new MovieReview(3, "A good one");
		SampleReviews[3] = new MovieReview(2, "Just ok");
		SampleReviews[4] = new MovieReview(1, "Better wait for the TV release");
		SampleReviews[5] = new MovieReview(0, "I'll never get those two hours of my life back");
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		try {
			mClient = new MobileServiceClient(
				      MobileServiceUrl,
				      MobileServiceKey,
				      this
				);
			mClient_ComplexClientSide = new MobileServiceClient(
					  MobileServiceUrl,
					  MobileServiceKey,
				      this
				);
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		mClient_ComplexClientSide.registerSerializer(MovieReview[].class, new MovieReviewArraySerializer());
		mClient_ComplexClientSide.registerDeserializer(MovieReview[].class, new MovieReviewArraySerializer());
		mClient_ComplexClientSide = mClient_ComplexClientSide.withFilter(new ServiceFilter() {

			@Override
			public void handleRequest(ServiceFilterRequest request,
					NextServiceFilterCallback next,
					final ServiceFilterResponseCallback responseCallback) {
				Log.d("Request:", request.toString());
				next.onNext(request, new ServiceFilterResponseCallback() {
					@Override
					public void onResponse(ServiceFilterResponse response,
							Exception error) {
						if (error == null) {
							Log.d("Response: ", response.toString());
						} else {
							Log.d("Response error: ", error.toString());
						}
						responseCallback.onResponse(response, error);						
					}					
				});				
			}
			
		});

		Button btnInsertServer = (Button)findViewById(R.id.btnInsertMovieServer);
		Button btnReadServer = (Button)findViewById(R.id.btnReadMovieServer);
		Button btnInsertClient = (Button)findViewById(R.id.btnInsertMovieClient);
		Button btnReadClient = (Button)findViewById(R.id.btnReadMovieClient);
		final TextView tv = (TextView)findViewById(R.id.textView1);
		
		btnInsertClient.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Random rndGen = new Random();
				Movie_ComplexClientSide template = SampleMovieComplexClientSide[rndGen.nextInt(SampleMovieComplexClientSide.length)];
				final Movie_ComplexClientSide toInsert = new Movie_ComplexClientSide(template.getTitle(), template.getReleaseYear());
				int reviewCount = rndGen.nextInt(2) + 1;
				MovieReview[] reviews = new MovieReview[reviewCount];
				for (int i = 0; i < reviewCount; i++) {
					reviews[i] = SampleReviews[rndGen.nextInt(SampleReviews.length)];
				}
				toInsert.setReviews(reviews);
				MobileServiceTable<Movie_ComplexClientSide> table = mClient_ComplexClientSide.getTable("Movie_Client", Movie_ComplexClientSide.class);
				table.insert(toInsert, new TableOperationCallback<Movie_ComplexClientSide>() {

					@Override
					public void onCompleted(Movie_ComplexClientSide inserted, Exception error,
							ServiceFilterResponse response) {
						if (error != null) {
							tv.setText("Error: " + error.toString());
						} else {
							tv.setText("Inserted: id=" + inserted.getId());
						}
					}
				});
			}
		});

		btnReadClient.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				mClient_ComplexClientSide.getTable("Movie_Client", Movie_ComplexClientSide.class).execute(new TableQueryCallback<Movie_ComplexClientSide>() {

					@Override
					public void onCompleted(List<Movie_ComplexClientSide> movies, int totalCount,
							Exception error, ServiceFilterResponse response) {
						if (error != null) {
							tv.setText("Error: " + error.toString());
						} else {
							StringBuilder sb = new StringBuilder();
							sb.append("All movies: size=" + movies.size() + "\n");
							for (Movie_ComplexClientSide movie : movies) {
								String title = movie.getTitle();
								int releaseYear = movie.getReleaseYear();
								sb.append("  " + title + " - " + releaseYear + ": ");
								MovieReview[] reviews = movie.getReviews();
								for (MovieReview review : reviews) {
									for (int i = 0; i < review.getStars(); i++) {
										sb.append('*');
									}
									sb.append(' ');
								}
								sb.append('\n');
							}
							tv.setText(sb.toString());
						}
					}					
				});
			}
		});

		btnInsertServer.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Random rndGen = new Random();
				Movie template = SampleMovies[rndGen.nextInt(SampleMovies.length)];
				final Movie toInsert = new Movie(template.getTitle(), template.getReleaseYear());
				int reviewCount = rndGen.nextInt(2) + 1;
				MovieReview[] reviews = new MovieReview[reviewCount];
				for (int i = 0; i < reviewCount; i++) {
					reviews[i] = SampleReviews[rndGen.nextInt(SampleReviews.length)];
				}
				toInsert.setReviews(reviews);
				MobileServiceTable<Movie> table = mClient.getTable(Movie.class);
				table.insert(toInsert, new TableOperationCallback<Movie>() {

					@Override
					public void onCompleted(Movie inserted, Exception error,
							ServiceFilterResponse response) {
						if (error != null) {
							tv.setText("Error: " + error.toString());
						} else {
							tv.setText("Inserted: id=" + inserted.getId());
						}
					}
				});
			}
		});
		
		btnReadServer.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				mClient.getTable(Movie.class).execute(new TableQueryCallback<Movie>() {

					@Override
					public void onCompleted(List<Movie> movies, int totalCount,
							Exception error, ServiceFilterResponse response) {
						if (error != null) {
							tv.setText("Error: " + error.toString());
						} else {
							StringBuilder sb = new StringBuilder();
							sb.append("All movies: size=" + movies.size() + "\n");
							for (Movie movie : movies) {
								String title = movie.getTitle();
								int releaseYear = movie.getReleaseYear();
								sb.append("  " + title + " - " + releaseYear + ": ");
								MovieReview[] reviews = movie.getReviews();
								for (MovieReview review : reviews) {
									for (int i = 0; i < review.getStars(); i++) {
										sb.append('*');
									}
									sb.append(' ');
								}
								sb.append('\n');
							}
							tv.setText(sb.toString());
						}
					}					
				});
			}
		});
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

}
