package hb.smartgreen;

import android.app.Application;
import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

import org.xutils.x;

/**
 * Created by wyq on 2016/4/7.
 */
public class smartGreenApp extends Application {
    private static SharedPreferences mSpSetting;
    @Override
    public void onCreate() {
        super.onCreate();
        x.Ext.init(this);
        x.Ext.setDebug(BuildConfig.DEBUG); // 开启debug会影响性能
        initSharedPreferences(getApplicationContext());

//        PluginHelper.getInstance().applicationOnCreate(getBaseContext()); //must behind super.onCreate()
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

    @Override
    public void onLowMemory() {
        android.os.Process.killProcess(android.os.Process.myPid());
        super.onLowMemory();
    }

    public void exitApp() {
        System.gc();
        android.os.Process.killProcess(android.os.Process.myPid());
    }

}
