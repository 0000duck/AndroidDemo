package hb.smartgreen.fragment;



import android.os.Bundle;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageButton;
import android.widget.ListView;
import android.widget.TextView;

import com.example.wyq.pullrefreshlibrary.PullToRefreshView;

import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;

import hb.smartgreen.R;
import hb.smartgreen.Widget.HorizontalListView;
import hb.smartgreen.adapter.StationAdapter;
import hb.smartgreen.bean.FactorSource;
import hb.smartgreen.bean.StationItem;
import hb.smartgreen.bean.StationSource;
import hb.smartgreen.bean.factorItem;
import hb.smartgreen.presenter.RealTimeStationListPresenter;
import hb.smartgreen.presenter.impl.RealTimeStationListPresenterImpl;
import hb.smartgreen.view.StationListView;

@ContentView(R.layout.fragment_tab1)
public class Tab1Fragment extends BaseFragment implements StationListView {

    @ViewInject(R.id.pull_to_refresh)
    private PullToRefreshView mPullToRefreshView;

    public static final int REFRESH_DELAY = 2000;
    @ViewInject(R.id.allStationItem)
    private ListView stationList;


    private RealTimeStationListPresenter realTimeStationList = null;

    public Tab1Fragment() {
        realTimeStationList = new RealTimeStationListPresenterImpl(this.getContext(),this);
    }



    @Override
    public void refreshListData(StationSource stationSource){
        stationList.setAdapter(new StationAdapter(stationSource,this.getContext()));
        stationList.getParent().requestDisallowInterceptTouchEvent(true);
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState)  {
        realTimeStationList.loadListData("request tag", 1, "eventtag", 1, true);

        mPullToRefreshView.setOnRefreshListener(new PullToRefreshView.OnRefreshListener() {
            @Override
            public void onRefresh() {
                mPullToRefreshView.postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        mPullToRefreshView.setRefreshing(false);
                        //update data
                        realTimeStationList.loadListData("request tag", 1, "eventtag", 1, true);

                    }
                }, REFRESH_DELAY);
            }
        });
    }


}
