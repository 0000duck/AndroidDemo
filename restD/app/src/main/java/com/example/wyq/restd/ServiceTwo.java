package com.example.wyq.restd;

import java.util.Timer;
import java.util.TimerTask;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.util.Log;

/**
 * Created by wyq on 2016/7/7.
 */



public class ServiceTwo extends Service {

    public final static String TAG = "com.example.servicedemo.ServiceTwo";

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
//        Log.e(TAG, "onStartCommand");

        thread.start();
        return START_REDELIVER_INTENT;
    }

    Thread thread = new Thread(new Runnable() {

        @Override
        public void run() {
            Timer timer = new Timer();
            TimerTask task = new TimerTask() {

                @Override
                public void run() {
//                    Log.e(TAG, "ServiceTwo Run: " + System.currentTimeMillis());
                    boolean b = MainActivity.isServiceWorked(ServiceTwo.this, "com.example.wyq.restd.ServiceOne");
                    if(!b) {
                        Intent service = new Intent(ServiceTwo.this, ServiceOne.class);
                        startService(service);
                    }
                }
            };
            timer.schedule(task, 0, 1000);
        }
    });

    @Override
    public IBinder onBind(Intent arg0) {
        return null;
    }

}
