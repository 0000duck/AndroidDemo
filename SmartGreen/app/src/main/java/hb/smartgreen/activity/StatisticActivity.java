package hb.smartgreen.activity;

import android.graphics.Color;
import android.os.Bundle;

import com.github.mikephil.charting.charts.BarChart;
import com.github.mikephil.charting.charts.LineChart;
import com.github.mikephil.charting.charts.PieChart;
import com.github.mikephil.charting.charts.RadarChart;
import com.github.mikephil.charting.data.BarData;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.PieData;

import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;

import hb.smartgreen.R;
import hb.smartgreen.chart.ConstructBarChart;
import hb.smartgreen.chart.ConstructLineChart;
import hb.smartgreen.chart.ConstructPieChart;

/**
 * Created by wyq on 2016/4/20.
 */
@ContentView(R.layout.fragment_statistic)
public class StatisticActivity extends BaseActivity {

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

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ConstructPieChart pc = new ConstructPieChart(this.getApplicationContext());
        PieData mPieData = pc.getPieData(4, 100);
        pc.showChart(mPieChart, mPieData);

        ConstructLineChart clc = new ConstructLineChart(this.getApplicationContext());
        LineData lineData = clc.getLineData(36, 100);
        clc.showChart(mLineChart, lineData, Color.rgb(114, 188, 223));

        ConstructLineChart cblc = new ConstructLineChart(this.getApplicationContext());
        LineData blineData = cblc.getFilledLineData(16, 50);
        cblc.showChart(mbLineChart, blineData, Color.rgb(897, 188, 223));

        ConstructBarChart cbc = new ConstructBarChart();
        BarData mBarData = cbc.getBarData(4, 32);
        cbc.showBarChart(mBarChart, mBarData);
    }
}
