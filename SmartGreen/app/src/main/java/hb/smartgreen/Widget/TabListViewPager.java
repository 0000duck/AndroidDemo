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

    @Override
    public void scrollTo(int x, int y) {
        super.scrollTo(x, y);
    }
    // Detect if we get action down event
    private boolean mIsActionDown;
    private float beforeX;
    @Override
    public boolean onTouchEvent(MotionEvent event) {
        return super.onTouchEvent(event);
    }

    @Override
    public boolean onInterceptTouchEvent(MotionEvent arg0) {
            return super.onInterceptTouchEvent(arg0);
    }

    @Override
    public void setCurrentItem(int item, boolean smoothScroll) {
        super.setCurrentItem(item, smoothScroll);
    }

    @Override
    public void setCurrentItem(int item) {
        super.setCurrentItem(item);
    }
}
