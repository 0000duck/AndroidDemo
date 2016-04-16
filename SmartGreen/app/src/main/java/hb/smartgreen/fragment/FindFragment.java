package hb.smartgreen.fragment;


import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.AttributeSet;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.TextView;

import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;

import hb.smartgreen.R;

@ContentView(R.layout.fragment_find)
public class FindFragment extends BaseFragment {
    @ViewInject(R.id.gridBtns)
    private GridView mGridView;

    public FindFragment() {
        // Required empty public constructor
    }


    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        initView();
    }


    private void initView() {
        String[] btns = {"保修","巡检","统计"};
        int[] imgs = {R.drawable.ib1, R.drawable.ib2,
                R.drawable.ib3};
        mGridView.setAdapter(new MyGridAdapter(this.getContext(), btns, imgs));
        //mGridView.setOnItemClickListener(this);
    }

    public class MyGridAdapter extends BaseAdapter {
        private Context mContext;

        public String[] img_text;
        public int[] imgs;

        public MyGridAdapter(Context mContext, String[] objects, int[] imgs) {
            super();
            this.mContext = mContext;
            this.img_text = objects;
            this.imgs = imgs;
        }

        @Override
        public int getCount() {
            // TODO Auto-generated method stub
            return img_text.length;
        }

        @Override
        public Object getItem(int position) {
            // TODO Auto-generated method stub
            return position;
        }


        @Override
        public long getItemId(int position) {
            // TODO Auto-generated method stub
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            if (convertView == null) {
                convertView = LayoutInflater.from(mContext).inflate(
                        R.layout.grid_item, parent, false);
            }
            TextView tv = (TextView) convertView.findViewById(R.id.tv_item);
            ImageView iv = (ImageView) convertView.findViewById(R.id.iv_item);
            iv.setBackgroundResource(imgs[position]);
            tv.setText(img_text[position]);
            return convertView;
        }
    }

    public class MyGridView extends GridView {
        public MyGridView(Context context, AttributeSet attrs) {
            super(context, attrs);
        }

        public MyGridView(Context context) {
            super(context);
        }

        public MyGridView(Context context, AttributeSet attrs, int defStyle) {
            super(context, attrs, defStyle);
        }

        @Override
        public void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
            int expandSpec = MeasureSpec.makeMeasureSpec(Integer.MAX_VALUE >> 2,
                    MeasureSpec.AT_MOST);
            super.onMeasure(widthMeasureSpec, expandSpec);
        }
    }
}
