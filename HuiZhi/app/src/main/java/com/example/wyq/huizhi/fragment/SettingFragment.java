package com.example.wyq.huizhi.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.wyq.huizhi.R;
import com.example.wyq.huizhi.View.MyView;

/**
 * Created by wyq on 2015/12/21.
 */
public class SettingFragment  extends Fragment {

    View view;
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view=inflater.inflate(R.layout.page_04, container, false);

        return view;
    }
}
