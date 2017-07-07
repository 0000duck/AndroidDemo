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
        //AVOSCloud.initialize(this,"wFiM28xy2Cjt8rAiSrbuKSwo-9Nh9j0Va","0Upey8kYi3iYd9bAWgpqVnbP");
        AVOSCloud.initialize(this,"r0edKFe15H1sA94SGb84egLm-9Nh9j0Va","qqt9e6rONAEqGdp8qqIoOofb");
        AVOSCloud.setDebugLogEnabled(true);
    }
}