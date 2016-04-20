package hb.smartgreen.Widget;

import android.content.Context;
import android.support.v4.view.ViewPager;
import android.util.AttributeSet;
import android.util.Log;
import android.view.MotionEvent;

import hb.smartgreen.fragment.BaseFragment;
import hb.smartgreen.fragment.TabListFragment;

/**
 * Created by wyq on 2016/4/13.
 */
public class TabListViewPager extends ViewPager {

    private TabListFragment mTabListFragment;
    public TabListViewPager(Context context, AttributeSet attrs) {
        super(context, attrs);
        // TODO Auto-generated constructor stub
    }

    public TabListViewPager(Context context) {
        super(context);
    }

    public void SetFragment(TabListFragment baseFragment){
        this.mTabListFragment= baseFragment;
    }


    // Detect if we get action down event
    private boolean mIsActionDown;
    private float beforeX;


}
