package wyq.gy;

import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.Toast;

import com.avos.avoscloud.AVException;
import com.avos.avoscloud.AVObject;
import com.avos.avoscloud.AVQuery;
import com.avos.avoscloud.AVUser;
import com.avos.avoscloud.FindCallback;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;
import java.util.Map;

import wyq.gy.Bean.historyOrder;
import wyq.gy.Tool.util;
import wyq.gy.adapter.HistoryOrderAdapter;

public class HistoryOrderActivity extends AppCompatActivity {
    private List<historyOrder> datalist;
    private ListView mlistView;
    private HistoryOrderAdapter mAdapter;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_history_order);
        mlistView = (ListView) findViewById(R.id.historyOrderList);
        datalist = new ArrayList<>();
        getData();

        android.support.v7.app.ActionBar actionBar = getSupportActionBar();
        if(actionBar != null){
            actionBar.setHomeButtonEnabled(true);
            actionBar.setDisplayHomeAsUpEnabled(true);
        }
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                this.finish(); // back button
                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    public void getData() {
        datalist.clear();
        if (util.isNetworkAvailable(this)) {
            AVQuery<AVObject> query = new AVQuery<>("MyOrderList");
            AVUser currentUser = AVUser.getCurrentUser();
            query.orderByAscending("Status");
            //query.whereEqualTo("EmployerID", currentUser.getMobilePhoneNumber());//通过关键字查找表单
            query.findInBackground(new FindCallback<AVObject>() {
                @Override
                public void done(List<AVObject> listorder, AVException e) {
                for (AVObject avObject : listorder) {
                    historyOrder order = new historyOrder(avObject.getString("RepairCategory"),GetStatusText(avObject.getString("Status")),avObject.getString("HandleTime"),avObject.getString("RealAddress"),avObject.getString("ServiceContent"));
                    datalist.add(order);
                }
                mAdapter = new HistoryOrderAdapter(HistoryOrderActivity.this, datalist);
                mlistView.setAdapter(mAdapter);
                mlistView.setOnItemClickListener(new AdapterView.OnItemClickListener() {

                    @Override
                    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                        historyOrder data = datalist.get(position);
                        Toast.makeText(HistoryOrderActivity.this, data.getAddress(), Toast.LENGTH_SHORT).show();
                    }
                });
                }
            });

        } else {
            Toast.makeText(this, "当前无网络连接", Toast.LENGTH_SHORT).show();
        }
    }

    private String GetStatusText(String statusId){
        String ret = "刚发布";
        switch (statusId){
            case "1":
                ret = "刚发布";
                break;
            case "2":
                ret = "已接单";
                break;
            case "3":
                ret = "工人完成";
                break;
            case "4":
                ret = "雇主确认";
                break;
            case "5":
                ret = "全部完成";
                break;
            case "6":
                ret = "已评价";
                break;
            default:
                break;
        }
        return ret;
    }
}
