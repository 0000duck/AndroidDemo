package hb.smartgreen.fragment;



import android.util.Log;

import org.xutils.view.annotation.ContentView;

import hb.smartgreen.R;
import hb.smartgreen.db.sgUser;

@ContentView(R.layout.fragment_setting)
public class SettingFragment extends BaseFragment {


    public SettingFragment() {
        // Required empty public constructor
        DbService db = new DbService();
        sgUser tuser = db.GetUserByName("123");
        if(tuser == null) {
            sgUser user = new sgUser();
            user.setUsername("123");
            user.setPassword("123");
            db.InsertUser(user);
            Log.e("setting fragment", "insert user");
        }
    }


}
