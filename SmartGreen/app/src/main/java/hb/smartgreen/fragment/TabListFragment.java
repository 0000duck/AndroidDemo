package hb.smartgreen.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.view.View;
import android.widget.TextView;
import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;
import java.util.ArrayList;

import hb.smartgreen.R;
import hb.smartgreen.Widget.NoScrollViewPager;

@ContentView(R.layout.fragment_tab_list)
public class TabListFragment extends BaseFragment implements View.OnClickListener{
    @ViewInject(R.id.tab1_tv)
    private TextView tab1Tv;
    @ViewInject(R.id.tab2_tv)
    private TextView tab2Tv;
    @ViewInject(R.id.tab3_tv)
    private TextView tab3Tv;

    @ViewInject(R.id.third_vp)
    private NoScrollViewPager viewPager;
    // fragment对象集合
    private ArrayList<Fragment> fragmentsList;

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        init();
    }

    private void init() {
        tab1Tv.setOnClickListener(this);
        tab2Tv.setOnClickListener(this);
        tab3Tv.setOnClickListener(this);
        initViewPager();
    }

    /**
     * 初始化viewpager
     */
    private void initViewPager() {
        fragmentsList = new ArrayList<>();
        Fragment fragment = new Tab1Fragment();
        fragmentsList.add(fragment);
        fragment = new Tab2Fragment();
        fragmentsList.add(fragment);
        fragment = new Tab3Fragment();
        fragmentsList.add(fragment);

        viewPager.setAdapter(new FragmentPagerAdapter(getChildFragmentManager()) {
            @Override
            public int getCount() {
                return fragmentsList.size();
            }

            @Override
            public android.support.v4.app.Fragment getItem(int item) {
                return fragmentsList.get(item);
            }
        });
        viewPager.setCurrentItem(0);
        tab1Tv.setBackgroundResource(R.color.gainsboro);
        tab2Tv.setBackgroundResource(R.color.encode_view);
        tab3Tv.setBackgroundResource(R.color.encode_view);
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.tab1_tv:
                viewPager.setCurrentItem(0);
                tab1Tv.setBackgroundResource(R.color.gainsboro);
                tab2Tv.setBackgroundResource(R.color.encode_view);
                tab3Tv.setBackgroundResource(R.color.encode_view);
                break;
            case R.id.tab2_tv:
                viewPager.setCurrentItem(1);
                tab1Tv.setBackgroundResource(R.color.encode_view);
                tab2Tv.setBackgroundResource(R.color.gainsboro);
                tab3Tv.setBackgroundResource(R.color.encode_view);
                break;
            case R.id.tab3_tv:
                viewPager.setCurrentItem(2);
                tab1Tv.setBackgroundResource(R.color.encode_view);
                tab2Tv.setBackgroundResource(R.color.encode_view);
                tab3Tv.setBackgroundResource(R.color.gainsboro);
                break;
        }
    }

}
