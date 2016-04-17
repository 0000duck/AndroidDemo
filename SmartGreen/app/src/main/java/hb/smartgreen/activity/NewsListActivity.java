package hb.smartgreen.activity;

import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.app.FragmentActivity;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.ListView;

import com.example.wyq.pullrefreshlibrary.PullToRefreshView;

import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;

import java.util.ArrayList;

import hb.smartgreen.R;

@ContentView(R.layout.activity_news_list)
public class NewsListActivity extends BaseActivity {

    @ViewInject(R.id.pull_to_refresh)
    private PullToRefreshView mPullToRefreshView;

    public static final int REFRESH_DELAY = 2000;

    @ViewInject(R.id.fab)
    private  FloatingActionButton fab;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
            }
        });

        mPullToRefreshView.setOnRefreshListener(new PullToRefreshView.OnRefreshListener() {
            @Override
            public void onRefresh() {
                mPullToRefreshView.postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        mPullToRefreshView.setRefreshing(false);
                        initView();
                    }
                }, REFRESH_DELAY);
            }
        });
    }


    //曲线显示
    private void initView() {

    }
}
