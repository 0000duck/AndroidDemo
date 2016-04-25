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
        stationList.setAdapter(new StationAdapter(stationSource));
        stationList.getParent().requestDisallowInterceptTouchEvent(true);
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState)  {
        realTimeStationList.loadListData("request tag","eventtag",1,true);

        mPullToRefreshView.setOnRefreshListener(new PullToRefreshView.OnRefreshListener() {
            @Override
            public void onRefresh() {
                mPullToRefreshView.postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        mPullToRefreshView.setRefreshing(false);
                        //update data
                        realTimeStationList.loadListData("request tag","eventtag",1,true);

                    }
                }, REFRESH_DELAY);
            }
        });
    }


    private class StationAdapter extends BaseAdapter {
        private boolean checked = false;
        private StationSource mmstationSource;

        public StationAdapter(StationSource stationSource){
            mmstationSource = new StationSource();
            for(int i = 0;i < stationSource.getCount();i++){
                mmstationSource.AddStationItem(stationSource.getItem(i));
            }
        }
        @Override
        public int getCount() {
            return mmstationSource.getCount();
        }

        @Override
        public StationItem getItem(int position) {
            return mmstationSource.getItem(position);
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            final ViewHolder holder;
            if (convertView == null) {
                convertView = View.inflate(Tab1Fragment.this.getContext(), R.layout.station_item, null);
                holder = new ViewHolder(convertView);
                convertView.setTag(holder);
            } else {
                holder = (ViewHolder) convertView.getTag();
            }

            StationItem item = getItem(position);
            holder.listView.setAdapter(new FactorAdapter(item.getFactorSource()));
            holder.stationName.setText(item.getStationName());
            holder.imageButton.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    if (checked) {
                        holder.imageButton.setBackground(getResources().getDrawable(R.drawable.noselect));
                        //这个记录需要 处理
                    } else {
                        holder.imageButton.setBackground(getResources().getDrawable(R.drawable.select));
                    }
                    checked = ! checked;
                }
            });

            return convertView;
        }
    }

    private static class ViewHolder {
        private TextView stationName;
        private ImageButton imageButton;
        private HorizontalListView listView;

        private ViewHolder(View view) {
            stationName = (TextView) view.findViewById(R.id.stationName);
            listView = (HorizontalListView) view.findViewById(R.id.staList);
            imageButton = (ImageButton)view.findViewById(R.id.focusBtn);
        }
    }

    private class FactorAdapter extends BaseAdapter {

        private FactorSource mfsource;
        public FactorAdapter(FactorSource fsource){
            mfsource = new FactorSource();
            for(int i = 0; i < fsource.getCount(); i++) {
                this.mfsource.AddFactorItem(fsource.getItem(i));
            }
        }

        @Override
        public int getCount() {
            return mfsource.getCount();
        }

        @Override
        public factorItem getItem(int position) {
            return mfsource.getItem(position);
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            FatorViewHolder holder;
            if (convertView == null) {
                convertView = View.inflate(Tab1Fragment.this.getContext(), R.layout.factor_item, null);
                holder = new FatorViewHolder(convertView);
                convertView.setTag(holder);
            } else {
                holder = (FatorViewHolder) convertView.getTag();
            }
            factorItem item = getItem(position);
            holder.fatorName.setText(item.getFactorName());
            holder.fatorValue.setText(item.getfactorValue());
            return convertView;
        }
    }

    private static class FatorViewHolder {

        private TextView fatorName;

        private TextView fatorValue;

        private FatorViewHolder(View view) {
            fatorName = (TextView) view.findViewById(R.id.factorName);
            fatorValue = (TextView) view.findViewById(R.id.fatorValue);
        }
    }

}
