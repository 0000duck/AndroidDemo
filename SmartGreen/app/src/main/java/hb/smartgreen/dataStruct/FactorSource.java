package hb.smartgreen.dataStruct;

import java.util.ArrayList;

/**
 * Created by wyq on 2016/4/14.
 */
public class FactorSource {
    private ArrayList<factorItem> mDataSource;

    public FactorSource(){
        mDataSource = new ArrayList<factorItem>();
    }

    public void AddFactorItem(factorItem item){
        this.mDataSource.add(item);
    }

    public int getCount() {
        return mDataSource.size();
    }

    public factorItem getItem(int position) {
        return mDataSource.get(position);
    }
}
