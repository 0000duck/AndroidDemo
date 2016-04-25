package hb.smartgreen.bean;

/**
 * Created by wyq on 2016/4/14.
 */
public class StationItem {
    private String stationID;
    private String stationName;
    private FactorSource factorSource;

    public StationItem(String stationID, String stationName, FactorSource factorSource){
        this.stationID = stationID;
        this.stationName = stationName;
        this.factorSource = factorSource;
    }

    public String getStationID() {
        return stationID;
    }

    public String getStationName() {
        return stationName;
    }

    public FactorSource getFactorSource() {
        return factorSource;
    }
}
