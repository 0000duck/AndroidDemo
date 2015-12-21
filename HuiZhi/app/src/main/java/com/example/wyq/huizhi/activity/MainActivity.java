package com.example.wyq.huizhi.activity;

import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.example.wyq.huizhi.R;
import com.example.wyq.huizhi.adapter.ContentAdapter;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity implements View.OnClickListener,ViewPager.OnPageChangeListener {

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

    // 中间内容区域
    private ViewPager viewPager;

    // ViewPager适配器ContentAdapter
    private ContentAdapter adapter;

    private List<View> views;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        // 初始化控件
        initView();
        // 初始化底部按钮事件
        initEvent();
    }

    private void initEvent() {
        // 设置按钮监听
        ll_home.setOnClickListener(this);
        ll_realtime.setOnClickListener(this);
        ll_statistics.setOnClickListener(this);
        ll_setting.setOnClickListener(this);

        //设置ViewPager滑动监听
        viewPager.addOnPageChangeListener(this);
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

        // 中间内容区域ViewPager
        this.viewPager = (ViewPager) findViewById(R.id.vp_content);

        // 适配器
        View page_01 = View.inflate(MainActivity.this, R.layout.page_01, null);
        View page_02 = View.inflate(MainActivity.this, R.layout.page_02, null);
        View page_03 = View.inflate(MainActivity.this, R.layout.page_03, null);
        View page_04 = View.inflate(MainActivity.this, R.layout.page_04, null);

        views = new ArrayList<View>();
        views.add(page_01);
        views.add(page_02);
        views.add(page_03);
        views.add(page_04);

        this.adapter = new ContentAdapter(views);
        viewPager.setAdapter(adapter);

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
               viewPager.setCurrentItem(0);
               break;
           case R.id.ll_realtime:
               //iv_realtime.setImageResource(R.drawable.tab_address_pressed);
               tv_realtime.setTextColor(0xff1B940A);
               viewPager.setCurrentItem(1);
               break;
           case R.id.ll_statistics:
               //iv_statistics.setImageResource(R.drawable.tab_find_frd_pressed);
               tv_statistics.setTextColor(0xff1B940A);
               viewPager.setCurrentItem(2);
               break;
           case R.id.ll_setting:
              // iv_setting.setImageResource(R.drawable.tab_find_frd_pressed);
               tv_setting.setTextColor(0xff1B940A);
               viewPager.setCurrentItem(3);
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
       tv_home.setTextColor(0x000);
       tv_realtime.setTextColor(0x000);
       tv_statistics.setTextColor(0x000);
       tv_setting.setTextColor(0x000);
 }

 @Override
 public void onPageScrollStateChanged(int arg0) {

 }

 @Override
 public void onPageScrolled(int arg0, float arg1, int arg2) {

 }

 @Override
 public void onPageSelected(int arg0) {
       restartBotton();
       //当前view被选择的时候,改变底部菜单图片，文字颜色
       switch (arg0) {
           case 0:
              // iv_home.setImageResource(R.drawable.tab_weixin_pressed);
               tv_home.setTextColor(0xff1B940A);
               break;
           case 1:
               //iv_realtime.setImageResource(R.drawable.tab_address_pressed);
               tv_realtime.setTextColor(0xff1B940A);
               break;
           case 2:
              // iv_statistics.setImageResource(R.drawable.tab_find_frd_pressed);
               tv_statistics.setTextColor(0xff1B940A);
               break;
           case 3:
              // iv_setting.setImageResource(R.drawable.tab_find_frd_pressed);
               tv_setting.setTextColor(0xff1B940A);
               break;

           default:
               break;
       }

    }
}
