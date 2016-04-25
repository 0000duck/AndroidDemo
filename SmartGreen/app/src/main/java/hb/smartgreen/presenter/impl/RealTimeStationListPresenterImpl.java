package hb.smartgreen.presenter.impl;

import android.content.Context;

import hb.smartgreen.bean.StationSource;
import hb.smartgreen.interactor.CommonListInteractor;
import hb.smartgreen.interactor.impl.CommonListInteractorImpl;
import hb.smartgreen.listener.BaseMultiLoadedListener;
import hb.smartgreen.presenter.RealTimeStationListPresenter;
import hb.smartgreen.view.StationListView;

/**
 * Created by wyq on 2016/4/25.
 */
public class RealTimeStationListPresenterImpl implements RealTimeStationListPresenter, BaseMultiLoadedListener<StationSource> {
    private StationListView mStatoinListView = null;
    private Context mContext = null;
    private CommonListInteractor mCommonListInteractor = null;

    public RealTimeStationListPresenterImpl(Context context, StationListView statoinListView){
        mContext = context;
        mStatoinListView = statoinListView;
        mCommonListInteractor = new CommonListInteractorImpl(this);
    }


    public void onSuccess(int event_tag, StationSource data) {
        mStatoinListView.refreshListData(data);
    }


    public void onError(String msg) {

    }

    public void onException(String msg) {

    }


    @Override
    public void loadListData(String requestTag, int event_tag,String keywords, int page, boolean isSwipeRefresh){
        mCommonListInteractor.getCommonListData(requestTag,event_tag,keywords,page);
    }
}
