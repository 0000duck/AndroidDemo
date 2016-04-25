package hb.smartgreen.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
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

/**
 * A simple {@link Fragment} subclass.
 */
@ContentView(R.layout.fragment_tab2)
public class Tab2Fragment extends BaseFragment {
    @ViewInject(R.id.pull_to_refresh)
    private PullToRefreshView mPullToRefreshView;

    public static final int REFRESH_DELAY = 2000;

    @ViewInject(R.id.alarmStationItem)
    private ListView stationList;

    private StationSource mStationSource;

    public Tab2Fragment() {
            InitData();
    }
    private void InitData()
    {
        factorItem item1 = new factorItem("湿度", "12.2");
        factorItem item2 = new factorItem("雨量", "15.2");
        factorItem item3 = new factorItem("CO2", "62.2");
        factorItem item4 = new factorItem("硫化氢", "1.2");
        factorItem item5 = new factorItem("铅含量", "0.53");
        factorItem item6 = new factorItem("SO2", "0.25");
        factorItem item7 = new factorItem("氨氮", "0.25");
        FactorSource fsource = new FactorSource();
        fsource.AddFactorItem(item1);
        fsource.AddFactorItem(item2);
        fsource.AddFactorItem(item3);
        fsource.AddFactorItem(item4);
        fsource.AddFactorItem(item5);
        fsource.AddFactorItem(item6);
        fsource.AddFactorItem(item7);
        StationItem sitem1 = new StationItem("001", "站点021", fsource);
        StationItem sitem2 = new StationItem("002", "站点002", fsource);
        StationItem sitem3 = new StationItem("003", "站点033", fsource);
        mStationSource = new StationSource();
        mStationSource.AddStationItem(sitem1);
        mStationSource.AddStationItem(sitem2);
        mStationSource.AddStationItem(sitem3);
    }


    @Override
    public void onViewCreated(View view, Bundle savedInstanceState)  {
        stationList.setAdapter(new StationAdapter());
        stationList.getParent().requestDisallowInterceptTouchEvent(true);

        mPullToRefreshView.setOnRefreshListener(new PullToRefreshView.OnRefreshListener() {
            @Override
            public void onRefresh() {
                mPullToRefreshView.postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        mPullToRefreshView.setRefreshing(false);
                        InitData();
                        stationList.setAdapter(new StationAdapter());
                        stationList.getParent().requestDisallowInterceptTouchEvent(true);
                    }
                }, REFRESH_DELAY);
            }
        });
    }




    private class StationAdapter extends BaseAdapter {
        private boolean checked = false;

        @Override
        public int getCount() {
            return mStationSource.getCount();
        }

        @Override
        public StationItem getItem(int position) {
            return mStationSource.getItem(position);
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            final ViewHolder holder;
            if (convertView == null) {
                convertView = View.inflate(Tab2Fragment.this.getContext(), R.layout.station_item, null);
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
                        holder.imageButton.setBackground(getResources().getDrawable(R.drawable.select));
                        //这个记录需要 处理
                    } else {
                        holder.imageButton.setBackground(getResources().getDrawable(R.drawable.noselect));
                    }

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
            for(int i = 0; i < fsource.getCount(); i++)
                this.mfsource.AddFactorItem(fsource.getItem(i));
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
                convertView = View.inflate(Tab2Fragment.this.getContext(), R.layout.factor_item, null);
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
