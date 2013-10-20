package com.example.froyoandwams;

import java.net.MalformedURLException;

import com.google.gson.JsonObject;
import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.MobileServiceJsonTable;
import com.microsoft.windowsazure.mobileservices.ServiceFilterResponse;
import com.microsoft.windowsazure.mobileservices.TableJsonOperationCallback;

import android.os.Bundle;
import android.app.Activity;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

public class MainActivity extends Activity {

	private MobileServiceClient mClient;

	private static Activity mInstance;
	public static Activity getInstance() {
		return mInstance;
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);

		mInstance = this;

		try {
			mClient = new MobileServiceClient(
				      "https://blog20131019.azure-mobile.net/", // Replace with your
				      "umtzEajPoOBsnvufFUVbLFTtjQrPPd37",       // own URL / key
				      this
				);
		} catch (MalformedURLException e) {
			e.printStackTrace();
		}

		mClient.setAndroidHttpClientFactory(new FroyoAndroidHttpClientFactory());

		Button btn = (Button)findViewById(R.id.btnClickMe);
		btn.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View view) {
				final StringBuilder sb = new StringBuilder();
				final TextView txtResult = (TextView)findViewById(R.id.txtResult);
				MobileServiceJsonTable table = mClient.getTable("TodoItem");
				sb.append("Created the table object, inserting item");
				txtResult.setText(sb.toString());
				JsonObject item = new JsonObject();
				item.addProperty("text", "Buy bread");
				item.addProperty("complete", false);
				table.insert(item, new TableJsonOperationCallback() {
					@Override
					public void onCompleted(JsonObject inserted, Exception error,
							ServiceFilterResponse response) {
						if (error != null) {
							sb.append("\nError: " + error.toString());
							Throwable cause = error.getCause();
							while (cause != null) {
								sb.append("\n  Cause: " + cause.toString());
								cause = cause.getCause();
							}
						} else {
							sb.append("\nInserted: id=" + inserted.get("id").getAsString());
						}
						
						String txt = sb.toString();
						txtResult.setText(txt);
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
