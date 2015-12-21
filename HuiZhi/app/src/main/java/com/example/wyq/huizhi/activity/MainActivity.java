package com.example.wyq.huizhi.activity;

import android.graphics.Color;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import com.example.wyq.huizhi.HZApplication;
import com.example.wyq.huizhi.R;
import com.example.wyq.huizhi.adapter.ChartDataAdapter;
import com.example.wyq.huizhi.adapter.ContentAdapter;
import com.example.wyq.huizhi.fragment.HomeFragment;
import com.example.wyq.huizhi.fragment.RealTimeFragment;
import com.example.wyq.huizhi.fragment.SettingFragment;
import com.example.wyq.huizhi.fragment.StatisticsFragment;
import com.example.wyq.huizhi.listviewitems.BarChartItem;
import com.example.wyq.huizhi.listviewitems.ChartItem;
import com.example.wyq.huizhi.listviewitems.LineChartItem;
import com.example.wyq.huizhi.listviewitems.PieChartItem;
import com.github.mikephil.charting.data.BarData;
import com.github.mikephil.charting.data.BarDataSet;
import com.github.mikephil.charting.data.BarEntry;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.data.PieData;
import com.github.mikephil.charting.data.PieDataSet;
import com.github.mikephil.charting.utils.ColorTemplate;


import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {

    // 底部菜单4个Linearlayout
    private LinearLayout ll_home;
    private LinearLayout ll_realtime;
    private LinearLayout ll_statistics;
    private LinearLayout ll_setting;

    // 底部菜单4个ImageView
    private ImageView iv_home;
    private ImageView iv_realtime;
    private ImageView iv_statistics;
    private ImageView iv_setting;

    // 底部菜单4个菜单标题
    private TextView tv_home;
    private TextView tv_realtime;
    private TextView tv_statistics;
    private TextView tv_setting;

    // 4个Fragment
    private Fragment homeFragment;
    private Fragment realtimeFragment;
    private Fragment statisticsFragment;
    private Fragment settingFragment;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        // 初始化控件
        initView();
        // 初始化底部按钮事件
        initEvent();

        // 初始化并设置当前Fragment
        initFragment(0);
    }

    private void initFragment(int index) {
        // 由于是引用了V4包下的Fragment，所以这里的管理器要用getSupportFragmentManager获取
        FragmentManager fragmentManager = getSupportFragmentManager();
        // 开启事务
        FragmentTransaction transaction = fragmentManager.beginTransaction();
        // 隐藏所有Fragment
        hideFragment(transaction);
        switch (index) {
            case 0:
                if (homeFragment == null) {
                    homeFragment = new HomeFragment();
                    transaction.add(R.id.fl_content, homeFragment);
                } else {
                    transaction.show(homeFragment);
                }
                break;
            case 1:
                if (realtimeFragment == null) {
                    realtimeFragment = new RealTimeFragment();
                    transaction.add(R.id.fl_content, realtimeFragment);
                } else {
                    transaction.show(realtimeFragment);
                }

                break;
            case 2:
                if (statisticsFragment == null) {
                    statisticsFragment = new StatisticsFragment();
                    transaction.add(R.id.fl_content, statisticsFragment);
                } else {
                    transaction.show(statisticsFragment);
                }

                break;
            case 3:
                if (settingFragment == null) {
                    settingFragment = new SettingFragment();
                    transaction.add(R.id.fl_content, settingFragment);
                } else {
                    transaction.show(settingFragment);
                }
                break;

            default:
                break;
        }

        // 提交事务
        transaction.commit();

    }

    //隐藏Fragment
    private void hideFragment(FragmentTransaction transaction) {
        if (homeFragment != null) {
            transaction.hide(homeFragment);
        }
        if (realtimeFragment != null) {
            transaction.hide(realtimeFragment);
        }
        if (statisticsFragment != null) {
            transaction.hide(statisticsFragment);
        }
        if (settingFragment != null) {
            transaction.hide(settingFragment);
        }

    }



    private void initEvent() {
        // 设置按钮监听
        ll_home.setOnClickListener(this);
        ll_realtime.setOnClickListener(this);
        ll_statistics.setOnClickListener(this);
        ll_setting.setOnClickListener(this);


    }

    private void initView() {

        // 底部菜单4个Linearlayout
        this.ll_home = (LinearLayout) findViewById(R.id.ll_home);
        this.ll_realtime = (LinearLayout) findViewById(R.id.ll_realtime);
        this.ll_statistics = (LinearLayout) findViewById(R.id.ll_statistics);
        this.ll_setting = (LinearLayout) findViewById(R.id.ll_setting);

        // 底部菜单4个ImageView
        this.iv_home = (ImageView) findViewById(R.id.iv_home);
        this.iv_realtime = (ImageView) findViewById(R.id.iv_realtime);
        this.iv_statistics = (ImageView) findViewById(R.id.iv_statistics);
        this.iv_setting = (ImageView) findViewById(R.id.iv_setting);

        // 底部菜单4个菜单标题
        this.tv_home = (TextView) findViewById(R.id.tv_home);
        this.tv_realtime = (TextView) findViewById(R.id.tv_realtime);
        this.tv_statistics = (TextView) findViewById(R.id.tv_statistics);
        this.tv_setting = (TextView) findViewById(R.id.tv_setting);

    }

    @Override
    public void onClick(View v) {
        // 在每次点击后将所有的底部按钮(ImageView,TextView)颜色改为灰色，然后根据点击着色
        restartBotton();
        // ImageView和TetxView置为绿色，页面随之跳转
       switch (v.getId()) {
           case R.id.ll_home:
               //iv_home.setImageResource(R.drawable.tab_weixin_pressed);
               tv_home.setTextColor(0xff1B940A);
               initFragment(0);
               break;
           case R.id.ll_realtime:
               //iv_realtime.setImageResource(R.drawable.tab_address_pressed);
               tv_realtime.setTextColor(0xff1B940A);
               initFragment(1);
               break;
           case R.id.ll_statistics:
               //iv_statistics.setImageResource(R.drawable.tab_find_frd_pressed);
               tv_statistics.setTextColor(0xff1B940A);
               initFragment(2);
               break;
           case R.id.ll_setting:
              // iv_setting.setImageResource(R.drawable.tab_find_frd_pressed);
               tv_setting.setTextColor(0xff1B940A);
               initFragment(3);
               break;

           default:
               break;
       }

 }

 private void restartBotton() {
     // ImageView置为灰色
//       iv_home.setImageResource(R.drawable.tab_weixin_normal);
//       iv_realtime.setImageResource(R.drawable.tab_address_normal);
//       iv_statistics.setImageResource(R.drawable.tab_find_frd_normal);
//       iv_setting.setImageResource(R.drawable.tab_settings_normal);
     // TextView置为白色
       tv_home.setTextColor(0xC0C0C0);
       tv_realtime.setTextColor(0xC0C0C0);
       tv_statistics.setTextColor(0xC0C0C0);
       tv_setting.setTextColor(0xC0C0C0);
 }




}
