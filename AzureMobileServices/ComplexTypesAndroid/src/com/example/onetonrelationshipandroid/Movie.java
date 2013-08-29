package com.example.onetonrelationshipandroid;
import com.google.gson.annotations.SerializedName;

public class Movie {
	private int id;

	@SerializedName("title")
	private String mTitle;
	
	@SerializedName("year")
	private int mReleaseYear;

	@SerializedName("reviews")
	private MovieReview[] mReviews;
	
	public Movie() {
		setReviews(new MovieReview[0]);
	}
	
	public Movie(String title, int year) {
		setTitle(title);
		setReleaseYear(year);
		setReviews(new MovieReview[0]);
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getTitle() {
		return mTitle;
	}

	public void setTitle(String title) {
		this.mTitle = title;
	}

	public int getReleaseYear() {
		return mReleaseYear;
	}

	public void setReleaseYear(int releaseYear) {
		this.mReleaseYear = releaseYear;
	}

	public MovieReview[] getReviews() {
		return mReviews;
	}

	public void setReviews(MovieReview[] reviews) {
		this.mReviews = reviews;
	}
	
}
