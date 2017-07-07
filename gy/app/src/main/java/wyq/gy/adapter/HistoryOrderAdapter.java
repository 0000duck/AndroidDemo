package wyq.gy.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;
import java.util.List;
import wyq.gy.Bean.historyOrder;
import wyq.gy.R;

/**
 * Created by wangcheng on 2017/6/19.
 */

public class HistoryOrderAdapter extends BaseAdapter {

    private List<historyOrder> datalist;
    private LayoutInflater layoutInflater;
    private Context context;
    private int id;
    public HistoryOrderAdapter(Context context, List<historyOrder> datalist){
        super();
        this.context=context;
        this.datalist=datalist;
        this.layoutInflater= LayoutInflater.from(context);
    }



    /**
     * 组件集合，对应list.xml中的控件
     * @author Administrator
     */
    public final class Zujian{
        public TextView title;
        public TextView state;
        public TextView date;
        public TextView address;
        public TextView workContent;
    }
    @Override
    public int getCount() {
        return datalist.size();//根据他的返回值得到listView的长度

    }
    /**
     * 获得某一位置的数据
     */
    @Override
    public Object getItem(int position) {
        return datalist.get(position);
    }
    /**
     * 获得唯一标识
     */
    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(final int position, View convertView, ViewGroup parent) {
        Zujian zujian=null;

        if(convertView==null){
            zujian=new Zujian();
            //获得组件，实例化组件
            convertView=layoutInflater.inflate(R.layout.history_order_ct, null);
            zujian.state=(TextView)convertView.findViewById(R.id.state);
            zujian.title=(TextView)convertView.findViewById(R.id.title);
            zujian.date=(TextView)convertView.findViewById(R.id.date);
            zujian.address=(TextView)convertView.findViewById(R.id.address);
            zujian.workContent=(TextView)convertView.findViewById(R.id.workContent);
            convertView.setTag(zujian);
        }else{
            zujian=(Zujian)convertView.getTag();
        }
        //绑定数据
        zujian.state.setText(datalist.get(position).getState());
        zujian.title.setText(datalist.get(position).getTitle());
        zujian.date.setText(datalist.get(position).getDate());
        zujian.address.setText(datalist.get(position).getAddress());
        zujian.workContent.setText(datalist.get(position).getworkContent());
        return convertView;
    }

}
