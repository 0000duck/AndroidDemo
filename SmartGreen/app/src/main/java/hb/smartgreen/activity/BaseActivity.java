package hb.smartgreen.activity;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import org.xutils.x;

/**
 * Created by wyq on 2016/4/11.
 */
public class BaseActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        x.view().inject(this);
    }
}
