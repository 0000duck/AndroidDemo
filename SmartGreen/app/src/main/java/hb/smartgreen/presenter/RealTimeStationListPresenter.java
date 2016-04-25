package hb.smartgreen.presenter;

/**
 * Created by wyq on 2016/4/25.
 */
public interface RealTimeStationListPresenter {
    void loadListData(String requestTag, int event_tag, String keywords, int page, boolean isSwipeRefresh);
}
