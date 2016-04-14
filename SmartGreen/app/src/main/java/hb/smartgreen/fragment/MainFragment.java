package hb.smartgreen.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.ListView;

import com.ToxicBakery.viewpager.transforms.AccordionTransformer;
import com.ToxicBakery.viewpager.transforms.BackgroundToForegroundTransformer;
import com.ToxicBakery.viewpager.transforms.CubeInTransformer;
import com.ToxicBakery.viewpager.transforms.CubeOutTransformer;
import com.ToxicBakery.viewpager.transforms.DefaultTransformer;
import com.ToxicBakery.viewpager.transforms.DepthPageTransformer;
import com.ToxicBakery.viewpager.transforms.FlipHorizontalTransformer;
import com.ToxicBakery.viewpager.transforms.FlipVerticalTransformer;
import com.ToxicBakery.viewpager.transforms.ForegroundToBackgroundTransformer;
import com.ToxicBakery.viewpager.transforms.RotateDownTransformer;
import com.ToxicBakery.viewpager.transforms.RotateUpTransformer;
import com.ToxicBakery.viewpager.transforms.StackTransformer;
import com.ToxicBakery.viewpager.transforms.ZoomInTransformer;
import com.bigkoo.convenientbanner.ConvenientBanner;
import com.bigkoo.convenientbanner.holder.CBViewHolderCreator;
import com.nostra13.universalimageloader.cache.disc.naming.Md5FileNameGenerator;
import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;
import com.nostra13.universalimageloader.core.assist.QueueProcessingType;

import org.xutils.view.annotation.ContentView;
import org.xutils.view.annotation.ViewInject;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import hb.smartgreen.R;
import hb.smartgreen.util.NetworkImageHolderView;

@ContentView(R.layout.fragment_main)
public class MainFragment extends BaseFragment {
    @ViewInject(R.id.cBannerList)
    private ListView mBannerList;

    @ViewInject(R.id.vp_tablist_frament)
    private ViewPager viewPager;

    private TabListFragment tabListFragment;
    private ConvenientBanner convenientBanner;//顶部广告栏控件
    private List<String> networkImages;
    private String[] images = {
            "https://img3.doubanio.com/view/photo/photo/public/p2323129960.jpg",
            "https://img1.doubanio.com/view/photo/photo/public/p2323129717.jpg",
            "http://img5.imgtn.bdimg.com/it/u=1647875596,2342657881&fm=206&gp=0.jpg",
            "http://pic5.nipic.com/20091223/3821057_091221087136_2.jpg",
            "http://pic1.nipic.com/2008-11-07/200811792932383_2.jpg",
            "https://img1.doubanio.com/view/photo/photo/public/p2311539539.jpg"
    };

    public MainFragment()
    {
        tabListFragment = new TabListFragment();
    }

    private ArrayAdapter transformerArrayAdapter;
    private ArrayList<String> transformerList = new ArrayList<String>();

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        convenientBanner = new ConvenientBanner(this.getContext(),true);
        transformerArrayAdapter = new ArrayAdapter(this.getContext(),R.layout.adapter_transformer,transformerList);
        mBannerList.setAdapter(transformerArrayAdapter);


        viewPager.setAdapter(new FragmentPagerAdapter(getChildFragmentManager()) {
            @Override
            public int getCount() {
                return 1;
            }

            @Override
            public android.support.v4.app.Fragment getItem(int item) {
                return tabListFragment;
            }
        });
        initBanner();
    }

    private void initImageLoader(){
        //网络图片例子,结合常用的图片缓存库UIL,你可以根据自己需求自己换其他网络图片库
        DisplayImageOptions defaultOptions = new DisplayImageOptions.Builder().
                showImageForEmptyUri(R.drawable.ic_default_adimage)
                .cacheInMemory(true).cacheOnDisk(true).build();

        ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(
                getContext()).defaultDisplayImageOptions(defaultOptions)
                .threadPriority(Thread.NORM_PRIORITY - 2)
                .denyCacheImageMultipleSizesInMemory()
                .diskCacheFileNameGenerator(new Md5FileNameGenerator())
                .tasksProcessingOrder(QueueProcessingType.LIFO).build();
        ImageLoader.getInstance().init(config);
    }
    private void initBanner() {
        initImageLoader();
        networkImages = Arrays.asList(images);
        convenientBanner.setMinimumHeight(600);
        convenientBanner.setPages(new CBViewHolderCreator<NetworkImageHolderView>() {
            @Override
            public NetworkImageHolderView createHolder() {
                return new NetworkImageHolderView();
            }
        }, networkImages)
                .setPageIndicator(new int[]{R.drawable.ic_page_indicator, R.drawable.ic_page_indicator_focused})
                .setPointViewVisible(true)
                .setPageIndicatorAlign(ConvenientBanner.PageIndicatorAlign.ALIGN_PARENT_RIGHT);

        mBannerList.addHeaderView(convenientBanner);
    }

    @Override
    public void onResume() {
        super.onResume();
        convenientBanner.startTurning(5000);
    }

    @Override
    public void onPause() {
        super.onPause();convenientBanner.stopTurning();
    }
}
