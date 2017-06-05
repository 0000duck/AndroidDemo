package wyq.gy;

import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.avos.avoscloud.AVException;
import com.avos.avoscloud.AVObject;
import com.avos.avoscloud.SaveCallback;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {

    private TextView tabview1;
    private TextView tabview2;
    private TextView tabview3;
    private TextView tabview4;
    private ImageView tabimg1;
    private ImageView tabimg2;
    private ImageView tabimg3;
    private ImageView tabimg4;

    private FragmentManager fragmentManager;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initView();
        fragmentManager = getSupportFragmentManager();
        setChioceItem(0);
//        // 测试 SDK 是否正常工作的代码
//        AVObject testObject = new AVObject("job");
//        testObject.put("jobTittle","Hello World!");
//        testObject.put("jobDescrip","Hello World, descrip!");
//        testObject.saveInBackground(new SaveCallback() {
//            @Override
//            public void done(AVException e) {
//                if(e == null){
//                    Log.d("saved","success!");
//                }
//            }
//        });
    }

    private void initView() {
        // 初始化底部导航栏的控件
        tabview1 = (TextView) findViewById(R.id.tab_text1);
        tabview2 = (TextView) findViewById(R.id.tab_text2);
        tabview3 = (TextView) findViewById(R.id.tab_text3);
        tabview4 = (TextView) findViewById(R.id.tab_text4);

        LinearLayout layout1 = (LinearLayout) findViewById(R.id.tab_1);
        LinearLayout layout2 = (LinearLayout) findViewById(R.id.tab_2);
        LinearLayout layout3 = (LinearLayout) findViewById(R.id.tab_3);
        LinearLayout layout4 = (LinearLayout) findViewById(R.id.tab_4);

        tabimg1 = (ImageView) findViewById(R.id.tab_img1);
        tabimg2 = (ImageView) findViewById(R.id.tab_img2);
        tabimg3 = (ImageView) findViewById(R.id.tab_img3);
        tabimg4 = (ImageView) findViewById(R.id.tab_img4);

        layout1.setOnClickListener(MainActivity.this);
        layout2.setOnClickListener(MainActivity.this);
        layout3.setOnClickListener(MainActivity.this);
        layout4.setOnClickListener(MainActivity.this);

    }

    private void setChioceItem(int index) {
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        hideFragments(fragmentTransaction);
        clearChioce(); // 清空, 重置选项, 隐藏所有Fragment

        switch (index) {
            case 0:
                tabview1.setTextColor(getResources().getColor(R.color.waterblue));
                tabimg1.setImageDrawable(getResources().getDrawable(R.drawable.tabimg1s));
//                if (homeFragment == null) {
//                    homeFragment = new HomeFragment();
//                    fragmentTransaction.add(R.id.content, homeFragment);  //content下创建mapFragment
//                } else {
//                    // 如果不为空，则直接将它显示出来
//                    fragmentTransaction.show(homeFragment);
//                }
                break;
            case 1:
                tabview2.setTextColor(getResources().getColor(R.color.waterblue));
                tabimg2.setImageDrawable(getResources().getDrawable(R.drawable.tabimg2s));
//                if (mapFragment == null) {
//                    mapFragment = new MapFragment();
//                    fragmentTransaction.add(R.id.content, mapFragment);  //content下创建mapFragment
//
//                } else {
//                    // 如果不为空，则直接将它显示出来
//                    fragmentTransaction.show(mapFragment);
//                }
                break;
            case 2:
                tabview3.setTextColor(getResources().getColor(R.color.waterblue));
                tabimg3.setImageDrawable(getResources().getDrawable(R.drawable.tabimg3s));
//                if (paikeFragment == null) {
//                    paikeFragment = new PaikeFragment();
//                    fragmentTransaction.add(R.id.content, paikeFragment);
//                } else {
//                    // 如果不为空，则直接将它显示出来
//                    fragmentTransaction.show(paikeFragment);
//                }
                break;
            case 3:
                if (false) {
//                    mine.setTextColor(getResources().getColor(R.color.waterblue));
//                    pic_mine.setImageDrawable(getResources().getDrawable(R.drawable.mine_pre));
//
//                    if (myloginFragment == null) {
//                        myloginFragment = new MyloginFragment();
//                        fragmentTransaction.add(R.id.content, myloginFragment);
//                    } else {
//                        // 如果不为空，则直接将它显示出来
//                        fragmentTransaction.show(myloginFragment);
//                    }
                } else {
                    tabview4.setTextColor(getResources().getColor(R.color.waterblue));
                    tabimg4.setImageDrawable(getResources().getDrawable(R.drawable.tabimg4s));
//                    if (mineFragment == null) {
//                        mineFragment = new MineFragment();
//                        fragmentTransaction.add(R.id.content, mineFragment);
//                    } else {
//                        // 如果不为空，则直接将它显示出来
//                        fragmentTransaction.show(mineFragment);
//                    }
                }
                break;
        }
        fragmentTransaction.commit(); // 提交
    }

    /**
     * 当选中其中一个选项卡时，其他选项卡重置为默认
     */
    private void clearChioce() {

        tabview1.setTextColor(getResources().getColor(R.color.unchoose));
        tabview2.setTextColor(getResources().getColor(R.color.unchoose));
        tabview3.setTextColor(getResources().getColor(R.color.unchoose));
        tabview4.setTextColor(getResources().getColor(R.color.unchoose));
        tabimg1.setImageDrawable(getResources().getDrawable(R.drawable.tabimg1));
        tabimg2.setImageDrawable(getResources().getDrawable(R.drawable.tabimg2));
        tabimg3.setImageDrawable(getResources().getDrawable(R.drawable.tabimg3));
        tabimg4.setImageDrawable(getResources().getDrawable(R.drawable.tabimg4));

    }

    /**
     * 隐藏Fragment
     */
    private void hideFragments(FragmentTransaction fragmentTransaction) {
//        if (mapFragment != null) {
//            fragmentTransaction.hide(mapFragment);
//        }
//        if (paikeFragment != null) {
//            fragmentTransaction.hide(paikeFragment);
//        }
//        if (homeFragment != null) {
//            fragmentTransaction.hide(homeFragment);
//        }
//        if (mineFragment != null) {
//            fragmentTransaction.hide(mineFragment);
//        }
//        if (myloginFragment != null) {
//            fragmentTransaction.hide(myloginFragment);
//        }

    }


    public void onClick(View v) {

        switch (v.getId()) {
            case R.id.tab_1:
                setChioceItem(0);
                break;
            case R.id.tab_2:
                setChioceItem(1);
                break;
            case R.id.tab_3:
                setChioceItem(2);
                break;
            case R.id.tab_4:
                setChioceItem(3);
                break;
        }

    }
}
