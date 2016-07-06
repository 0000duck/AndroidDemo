package com.example.wyq.amaploc;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

public class MainActivity extends Activity implements OnClickListener {
	private Button buttonLoction;
	private TextView txtView;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		buttonLoction = (Button) findViewById(R.id.btn_loction);
		txtView = (TextView) findViewById(R.id.loc);
		buttonLoction.setOnClickListener(this);
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.btn_loction:
			Intent intent = new Intent(this, LocationActivity.class);
			startActivityForResult(intent, 1001);
			break;

		default:
			break;
		}
	}
	
	@Override
	protected void onResume() {
		super.onResume();
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);
		switch (requestCode) {
		case 1001:
			if(data != null) {
				String drawableID = data.getStringExtra("location");
				if (drawableID != null) {
					txtView.setText(drawableID);
					buttonLoction.setText(drawableID);
				}
			}
			break;
		default:
			break;
		}

	}

}
