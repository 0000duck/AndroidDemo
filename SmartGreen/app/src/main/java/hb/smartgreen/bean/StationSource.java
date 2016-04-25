package hb.smartgreen.bean;

import java.util.ArrayList;

/**
 * Created by wyq on 2016/4/14.
 */
public class StationSource {
    private ArrayList<StationItem> mStationSource;

    public StationSource(){
        mStationSource = new ArrayList<StationItem>();
    }

    public void AddStationItem(StationItem item){
        mStationSource.add(item);
    }

    public int getCount() {
        return mStationSource.size();
    }

    public StationItem getItem(int position) {
        return mStationSource.get(position);
    }
}
