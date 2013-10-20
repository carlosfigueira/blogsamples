package com.example.froyoandwams;

import android.net.http.AndroidHttpClient;

import com.microsoft.windowsazure.mobileservices.AndroidHttpClientFactoryImpl;

public class FroyoAndroidHttpClientFactory
	extends AndroidHttpClientFactoryImpl {

	@Override
	public AndroidHttpClient createAndroidHttpClient() {
		AndroidHttpClient client = super.createAndroidHttpClient();
		FroyoSupport.fixAndroidHttpClientForCertificateValidation(client);
		return client;
	}
}
