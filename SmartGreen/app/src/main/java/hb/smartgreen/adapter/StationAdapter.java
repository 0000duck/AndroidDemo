package hb.smartgreen.adapter;

import android.content.Context;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageButton;
import android.widget.TextView;

import hb.smartgreen.R;
import hb.smartgreen.Widget.HorizontalListView;
import hb.smartgreen.bean.StationItem;
import hb.smartgreen.bean.StationSource;

/**
 * Created by wyq on 2016/4/25.
 */
public class StationAdapter extends BaseAdapter {
    private boolean checked = false;
    private StationSource mmstationSource;
    private Context mContext;

    public StationAdapter(StationSource stationSource, Context context){
        mContext = context;
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
            convertView = View.inflate(mContext, R.layout.station_item, null);
            holder = new ViewHolder(convertView);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        StationItem item = getItem(position);
        holder.listView.setAdapter(new FactorAdapter(item.getFactorSource(), mContext));
        holder.stationName.setText(item.getStationName());
        holder.imageButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (checked) {
                    holder.imageButton.setBackground(mContext.getResources().getDrawable(R.drawable.noselect));
                    //这个记录需要 处理
                } else {
                    holder.imageButton.setBackground(mContext.getResources().getDrawable(R.drawable.select));
                }
                checked = ! checked;
            }
        });

        return convertView;
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
}

