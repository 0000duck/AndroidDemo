package hb.smartgreen.fragment;


import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentPagerAdapter;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.LinearLayout;

import com.bigkoo.convenientbanner.ConvenientBanner;

import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;

import hb.smartgreen.R;
import hb.smartgreen.activity.LoginActivity;
import hb.smartgreen.activity.NewsListActivity;

@ContentView(R.layout.fragment_dynamic)
public class DynamicFragment extends BaseFragment {

@ViewInject(R.id.newsBtn)
private LinearLayout mLayoutBtn;
    public DynamicFragment() {
        // Required empty public constructor
    }


    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        mLayoutBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(),NewsListActivity.class);
                startActivity(intent);
            }
        });
    }

}
