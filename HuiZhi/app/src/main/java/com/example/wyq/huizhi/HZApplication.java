package com.example.wyq.huizhi;

import android.app.Application;
import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

import com.squareup.leakcanary.LeakCanary;
import com.squareup.leakcanary.RefWatcher;
/**
 * Created by wyq on 2015/12/19.
 */
public class HZApplication extends Application {
    private static RefWatcher mRefWatcher;
    private static SharedPreferences mSpSetting;

    @Override
    public void onCreate() {
        super.onCreate();
        mRefWatcher = LeakCanary.install(this);
        initSharedPreferences(getApplicationContext());
    }

    public static void WatchLeak(Object ob)
    {
        mRefWatcher.watch(ob);
    }
    //初始化SharePreference
    public static void initSharedPreferences(Context context) {
        mSpSetting = PreferenceManager.getDefaultSharedPreferences(context);
    }

    //初次打开
    public static void setisFirst(boolean is){
        SharedPreferences.Editor editor = mSpSetting.edit();
        editor.putBoolean("ISFIRST",is).commit();
    }

    //判断是不是第一次打开
    public static boolean isFirst(){
        return mSpSetting.getBoolean("ISFIRST", true);
    }
}
