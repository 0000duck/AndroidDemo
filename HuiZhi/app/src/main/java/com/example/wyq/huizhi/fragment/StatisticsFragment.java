package com.example.wyq.huizhi.fragment;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v7.app.ActionBarActivity;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.Toast;

import com.example.wyq.huizhi.R;
import com.example.wyq.huizhi.activity.PullToRefreshActivity;
import com.example.wyq.huizhi.gridViewBtn.MyGridAdapter;
import com.example.wyq.huizhi.gridViewBtn.MyGridView;
import com.example.wyq.huizhi.pullRefreshFragment.ListViewFragment;
import com.example.wyq.huizhi.pullRefreshFragment.RecyclerViewFragment;

import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;


/**
 * Created by wyq on 2015/12/21.
 */
public class StatisticsFragment extends Fragment implements AdapterView.OnItemClickListener {
    View view;
    private MyGridView gridview;
    int[] imgs = {R.drawable.app_transfer, R.drawable.app_fund,
            R.drawable.app_phonecharge, R.drawable.app_creditcard,
            R.drawable.app_movie, R.drawable.app_lottery,
            R.drawable.app_facepay, R.drawable.app_close, R.drawable.app_plane};

    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.page_03, container, false);
        initView();
        return view;
    }

    private void initView() {
        gridview = (MyGridView) view.findViewById(R.id.gridview);
        String[] btns = {getString(R.string.Btn1),
                getString(R.string.Btn2),
                getString(R.string.Btn3),
                getString(R.string.Btn4),
                getString(R.string.Btn5),
                getString(R.string.Btn6),
                getString(R.string.Btn7),
                getString(R.string.Btn8),
                getString(R.string.Btn9)};

        gridview.setAdapter(new MyGridAdapter(this.getContext(), btns, imgs));
        gridview.setOnItemClickListener(this);
    }

    public void onItemClick(AdapterView<?> adapterView, View view, int position, long id) {
        switch (imgs[position]) {
            case R.drawable.app_transfer:
                startActivity(new Intent(this.getContext(), PullToRefreshActivity.class));//启动另一个Activity
                //this.getActivity().finish();//结束此Activity，可回收
                break;
        }
    }

}
