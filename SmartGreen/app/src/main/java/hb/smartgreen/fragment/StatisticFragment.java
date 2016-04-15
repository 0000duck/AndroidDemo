package hb.smartgreen.fragment;



import android.graphics.Color;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.View;
import android.widget.ListView;

import com.github.mikephil.charting.charts.BarChart;
import com.github.mikephil.charting.charts.LineChart;
import com.github.mikephil.charting.charts.PieChart;
import com.github.mikephil.charting.charts.RadarChart;
import com.github.mikephil.charting.components.Legend;
import com.github.mikephil.charting.data.BarData;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.data.PieData;
import com.github.mikephil.charting.data.PieDataSet;

import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;

import java.util.ArrayList;

import hb.smartgreen.R;
import hb.smartgreen.chart.ConstructBarChart;
import hb.smartgreen.chart.ConstructLineChart;
import hb.smartgreen.chart.ConstructPieChart;
import hb.smartgreen.db.sgUser;
import hb.smartgreen.util.DbService;

@ContentView(R.layout.fragment_statistic)
public class StatisticFragment extends BaseFragment {

    @ViewInject(R.id.pie_chart)
    private PieChart mPieChart;

    @ViewInject(R.id.aline_chart)
    private LineChart mLineChart;

    @ViewInject(R.id.bline_chart)
    private LineChart mbLineChart;

    @ViewInject(R.id.bar_chart)
    private BarChart mBarChart;

    @ViewInject(R.id.radar_chart)
    private RadarChart mRadarChart;

    public StatisticFragment() {
        //test for temply
        DbService db = new DbService();
        sgUser tuser = db.GetUserByName("123");
        if(tuser == null) {
            sgUser user = new sgUser();
            user.setUsername("123");
            user.setPassword("123");
            db.InsertUser(user);
            Log.e("setting fragment", "insert user");
        }
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        ConstructPieChart pc = new ConstructPieChart(this.getContext());
        PieData mPieData = pc.getPieData(4, 100);
        pc.showChart(mPieChart, mPieData);

        ConstructLineChart clc = new ConstructLineChart(this.getContext());
        LineData lineData = clc.getLineData(36, 100);
        clc.showChart(mLineChart, lineData, Color.rgb(114, 188, 223));

        ConstructLineChart cblc = new ConstructLineChart(this.getContext());
        LineData blineData = cblc.getFilledLineData(16, 50);
        cblc.showChart(mbLineChart, blineData, Color.rgb(897, 188, 223));

        ConstructBarChart cbc = new ConstructBarChart();
        BarData mBarData = cbc.getBarData(4, 32);
        cbc.showBarChart(mBarChart, mBarData);
    }



}
