package hb.smartgreen.activity;

import android.app.Activity;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Window;
import android.view.WindowManager;

import hb.smartgreen.R;
import hb.smartgreen.smartGreenApp;

public class SplashActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        getWindow().requestFeature(Window.FEATURE_NO_TITLE);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        getWindow().setBackgroundDrawableResource(R.drawable.splash);

        final Intent intent = new Intent(SplashActivity.this,LoginActivity.class);
        if(smartGreenApp.isFirst()){
            intent.setClass(getApplication(), GuidePageActivity.class);
        }else{
            intent.setClass(getApplication(), LoginActivity.class);
        }
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        new Thread(new Runnable() {

            @Override
            public void run() {
                try {
                    Thread.sleep(1000);
                    startActivity(intent);
                    finish();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }

            }
        }).start();
    }
}
