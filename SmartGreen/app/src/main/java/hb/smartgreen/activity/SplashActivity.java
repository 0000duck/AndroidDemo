package hb.smartgreen.activity;

import android.app.Activity;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Window;
import android.view.WindowManager;
import android.view.animation.AlphaAnimation;
import android.view.animation.Animation;
import android.view.animation.AnimationSet;
import android.view.animation.ScaleAnimation;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.LinearLayout;

import hb.smartgreen.R;
import hb.smartgreen.smartGreenApp;

public class SplashActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);

        LinearLayout layout = (LinearLayout)findViewById(R.id.splashLayout);
        final Intent intent = new Intent(SplashActivity.this,LoginActivity.class);
        if(smartGreenApp.isFirst()){
            intent.setClass(getApplication(), GuidePageActivity.class);
        }else{
            intent.setClass(getApplication(), LoginActivity.class);
        }
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);

        ScaleAnimation topExit = new ScaleAnimation(1f, 1.2f, 1f, 1.2f, Animation.RELATIVE_TO_SELF,0.5f,Animation.RELATIVE_TO_SELF,0.5f);
        AlphaAnimation alphaAnimation = new AlphaAnimation(1,0.5f);

        AnimationSet animationSet = new AnimationSet(true);
        animationSet.setFillAfter(true);
        animationSet.setDuration(3000);
        animationSet.addAnimation(topExit);
        animationSet.addAnimation(alphaAnimation);

        new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    Thread.sleep(1500);
                    startActivity(intent);
                    finish();
                }catch (InterruptedException error){
                    Log.e("splash",error.toString());
                }
            }
        }).start();
        layout.startAnimation(animationSet);
    }


}
