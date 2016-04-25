package hb.smartgreen.interactor.impl;

import hb.smartgreen.bean.FactorSource;
import hb.smartgreen.bean.StationItem;
import hb.smartgreen.bean.StationSource;
import hb.smartgreen.bean.factorItem;
import hb.smartgreen.interactor.CommonListInteractor;
import hb.smartgreen.listener.BaseMultiLoadedListener;


/**
 * Created by wyq on 2016/4/25.
 */
public class CommonListInteractorImpl implements CommonListInteractor {

    private BaseMultiLoadedListener<StationSource> loadedListener = null;

    public CommonListInteractorImpl(BaseMultiLoadedListener<StationSource> loadedListener){
        this.loadedListener = loadedListener;
    }

    @Override
    public void getCommonListData(String requestTag, final int event_tag, String keywords, int page){


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
        StationItem sitem1 = new StationItem("001", "站点001", fsource);
        StationItem sitem2 = new StationItem("002", "站点002", fsource);
        StationItem sitem3 = new StationItem("003", "站点003", fsource);
        StationItem sitem4 = new StationItem("001", "站点031", fsource);
        StationItem sitem5 = new StationItem("002", "站点022", fsource);
        StationItem sitem6 = new StationItem("003", "站点013", fsource);
        StationSource mStationSource = new StationSource();
        mStationSource.AddStationItem(sitem1);
        mStationSource.AddStationItem(sitem2);
        mStationSource.AddStationItem(sitem3);
        mStationSource.AddStationItem(sitem4);
        mStationSource.AddStationItem(sitem5);
        mStationSource.AddStationItem(sitem6);

        loadedListener.onSuccess(event_tag, mStationSource);
    }
}
