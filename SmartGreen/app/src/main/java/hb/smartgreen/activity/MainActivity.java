package hb.smartgreen.activity;

import android.graphics.Color;
import android.os.Handler;
import android.os.Message;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.View;
import android.widget.Toast;

import com.gigamole.library.NavigationTabBar;
import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;
import java.util.ArrayList;
import java.util.List;
import hb.smartgreen.R;
import hb.smartgreen.fragment.DynamicFragment;
import hb.smartgreen.fragment.FindFragment;
import hb.smartgreen.fragment.MainFragment;
import hb.smartgreen.fragment.MineFragment;
import hb.smartgreen.fragment.StatisticFragment;

@ContentView(R.layout.activity_main)
public class MainActivity extends BaseActivity {

    @ViewInject(R.id.vp_horizontal_ntb)
    private ViewPager viewPager;

    @ViewInject(R.id.ntb_horizontal)
    private NavigationTabBar navigationTabBar;

    @ViewInject(R.id.bg_ntb_horizontal)
    private View bgNavigationTabBar;

    private List<Fragment> fragments;
    private ArrayList<NavigationTabBar.Model> models;





    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        mIsExit = false;
        initUI();
    }
    @Override
    protected void onDestroy() {
        super.onDestroy();
        System.runFinalization();
        Runtime.getRuntime().gc();
        System.gc();
    }

    private  void initUIData() {
        fragments = new ArrayList<Fragment>() ;
        fragments.add(new MainFragment());
        fragments.add(new DynamicFragment());
        //fragments.add(new StatisticFragment());
        fragments.add(new FindFragment());
        fragments.add(new MineFragment());

        models = new ArrayList<>();
        final String[] colors = getResources().getStringArray(R.array.default_preview);
        models.add(new NavigationTabBar.Model(
                getResources().getDrawable(R.drawable.ic_sixth), Color.parseColor(colors[0]), "首页"));
        models.add(new NavigationTabBar.Model(
                getResources().getDrawable(R.drawable.ic_second), Color.parseColor(colors[1]), "动态"));
//        models.add(new NavigationTabBar.Model(
//                getResources().getDrawable(R.drawable.ic_third), Color.parseColor(colors[2]), "统计"));
        models.add(new NavigationTabBar.Model(
                getResources().getDrawable(R.drawable.ic_fourth), Color.parseColor(colors[2]), "发现"));
        models.add(new NavigationTabBar.Model(
                getResources().getDrawable(R.drawable.ic_fifth), Color.parseColor(colors[3]), "我"));
    }
    private void initUI() {
        initUIData();
        viewPager.setAdapter(new FragmentPagerAdapter(getSupportFragmentManager()) {
            @Override
            public int getCount() {
                return fragments.size();
            }

            @Override
            public android.support.v4.app.Fragment getItem(int item) {
                return fragments.get(item);
            }

        });
        navigationTabBar.setModels(models);
        navigationTabBar.setViewPager(viewPager, 0);

        navigationTabBar.setOnTabBarSelectedIndexListener(new NavigationTabBar.OnTabBarSelectedIndexListener() {
            @Override
            public void onStartTabSelected(final NavigationTabBar.Model model, final int index) {
            }

            @Override
            public void onEndTabSelected(final NavigationTabBar.Model model, final int index) {
                model.hideBadge();
            }
        });

        navigationTabBar.post(new Runnable() {
            @Override
            public void run() {
                bgNavigationTabBar.getLayoutParams().height = (int) navigationTabBar.getBarHeight();
                bgNavigationTabBar.requestLayout();
            }
        });

        navigationTabBar.postDelayed(new Runnable() {
            @Override
            public void run() {
                for (int i = 0; i < navigationTabBar.getModels().size(); i++) {
                    final NavigationTabBar.Model model = navigationTabBar.getModels().get(i);
                    switch (i) {
                        case 0:
                            model.setBadgeTitle("1");
                            break;
                        case 1:
                            model.setBadgeTitle("2");
                            break;
                        case 2:
                            model.setBadgeTitle("3");
                            break;
                        case 3:
                            model.setBadgeTitle("4");
                            break;
//                        case 4:
//                            model.setBadgeTitle("5");
//                            break;
                    }
                    navigationTabBar.postDelayed(new Runnable() {
                        @Override
                        public void run() {
                            model.showBadge();
                        }
                    }, i * 100);
                }
            }
        }, 500);
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_BACK){
            exit();
            return false;
        }
        return super.onKeyDown(keyCode, event);
    }

    /**
     * 实现点击两次退出程序
     */
    private boolean mIsExit;
    //点击返回键时，延时 TIME_TO_EXIT 毫秒发送此handler重置mIsExit，再其被重置前如果再按一次返回键则退出应用
    private Handler mExitHandler = new Handler(){
        @Override
        public void handleMessage(Message msg) {
            super.handleMessage(msg);
            mIsExit = false;
        }
    };
    final static int TIME_TO_EXIT = 2000;

    private void exit(){
        if (mIsExit){
            finish();
            System.exit(0);
        }else {
            mIsExit = true;
            Toast.makeText(getApplicationContext(),R.string.click_to_exit, Toast.LENGTH_SHORT).show();
            //两秒内不点击back则重置mIsExit
            mExitHandler.sendEmptyMessageDelayed(0,TIME_TO_EXIT);
        }
    }
}
