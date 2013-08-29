package com.example.onetonrelationshipandroid;

import java.lang.reflect.Type;

import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonParseException;
import com.google.gson.JsonParser;
import com.google.gson.JsonPrimitive;
import com.google.gson.JsonSerializationContext;
import com.google.gson.JsonSerializer;

public class MovieReviewArraySerializer 
	implements JsonSerializer<MovieReview[]>, JsonDeserializer<MovieReview[]> {

	@Override
	public JsonElement serialize(MovieReview[] value, Type type,
			JsonSerializationContext context) {
		String serialized = new Gson().toJson(value);
		return new JsonPrimitive(serialized);
	}

	@Override
	public MovieReview[] deserialize(JsonElement element, Type type,
			JsonDeserializationContext context) throws JsonParseException {
		String serialized = element.getAsString();
		JsonArray array = (JsonArray) new JsonParser().parse(serialized);
		return new Gson().fromJson(array, MovieReview[].class);
	}

}
