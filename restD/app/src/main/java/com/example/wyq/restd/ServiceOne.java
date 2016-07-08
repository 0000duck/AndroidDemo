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


public class ServiceOne extends Service {

    public final static String TAG = "com.example.servicedemo.ServiceOne";

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        thread.start();
        return START_STICKY;
    }

    Thread thread = new Thread(new Runnable() {

        @Override
        public void run() {
            Timer timer = new Timer();
            TimerTask task = new TimerTask() {

                @Override
                public void run() {
//                    Log.e(TAG, "ServiceOne Run: "+System.currentTimeMillis());
                    boolean b = MainActivity.isServiceWorked(ServiceOne.this, "com.example.wyq.restd.ServiceTwo");
                    if(!b) {
                        Intent service = new Intent(ServiceOne.this, ServiceTwo.class);
                        startService(service);
//                        Log.e(TAG, "Start ServiceTwo");
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