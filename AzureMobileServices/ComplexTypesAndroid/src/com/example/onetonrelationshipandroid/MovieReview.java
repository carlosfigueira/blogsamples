package com.example.onetonrelationshipandroid;

import com.google.gson.annotations.SerializedName;

public class MovieReview {

	@SerializedName("stars")
	private int mStars;
	
	@SerializedName("comment")
	private String mComment;

	public MovieReview() {
	
	}
	
	public MovieReview(int stars, String comment) {
		this.setStars(stars);
		this.setComment(comment);
	}
	
	public int getStars() {
		return mStars;
	}

	public void setStars(int mStars) {
		this.mStars = mStars;
	}

	public String getComment() {
		return mComment;
	}

	public void setComment(String mComment) {
		this.mComment = mComment;
	}
}
