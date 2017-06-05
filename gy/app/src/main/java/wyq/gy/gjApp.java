package wyq.gy;

import android.app.Application;

import com.avos.avoscloud.AVOSCloud;

/**
 * Created by wyq on 2017/6/3.
 */


public class gjApp extends Application {

    @Override
    public void onCreate() {
        super.onCreate();

        // 初始化参数依次为 this, AppId, AppKey

        AVOSCloud.setDebugLogEnabled(true);
    }
}