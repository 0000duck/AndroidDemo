package hb.smartgreen.adapter;

import android.content.Context;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import hb.smartgreen.R;
import hb.smartgreen.bean.FactorSource;
import hb.smartgreen.bean.factorItem;

/**
 * Created by wyq on 2016/4/25.
 */
public class FactorAdapter extends BaseAdapter {

    private Context mContext;
    private FactorSource mfsource;

    public FactorAdapter(FactorSource fsource, Context context){
        mContext = context;
        mfsource = new FactorSource();
        for(int i = 0; i < fsource.getCount(); i++) {
            this.mfsource.AddFactorItem(fsource.getItem(i));
        }
    }

    @Override
    public int getCount() {
        return mfsource.getCount();
    }

    @Override
    public factorItem getItem(int position) {
        return mfsource.getItem(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        FatorViewHolder holder;
        if (convertView == null) {
            convertView = View.inflate(mContext, R.layout.factor_item, null);
            holder = new FatorViewHolder(convertView);
            convertView.setTag(holder);
        } else {
            holder = (FatorViewHolder) convertView.getTag();
        }
        factorItem item = getItem(position);
        holder.fatorName.setText(item.getFactorName());
        holder.fatorValue.setText(item.getfactorValue());
        return convertView;
    }

    private static class FatorViewHolder {

        private TextView fatorName;

        private TextView fatorValue;

        private FatorViewHolder(View view) {
            fatorName = (TextView) view.findViewById(R.id.factorName);
            fatorValue = (TextView) view.findViewById(R.id.fatorValue);
        }
    }
}

