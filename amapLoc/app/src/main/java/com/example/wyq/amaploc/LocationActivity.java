package com.example.wyq.amaploc;

import java.util.List;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.Point;
import android.graphics.drawable.Drawable;
import android.location.Location;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.SystemClock;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.MeasureSpec;
import android.view.animation.BounceInterpolator;
import android.view.animation.Interpolator;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.amap.api.location.AMapLocation;
import com.amap.api.location.AMapLocationClient;
import com.amap.api.location.AMapLocationClientOption;
import com.amap.api.location.AMapLocationListener;

import com.amap.api.maps2d.CameraUpdateFactory;
import com.amap.api.maps2d.LocationSource;
import com.amap.api.maps2d.MapView;
import com.amap.api.maps2d.Projection;
import com.amap.api.maps2d.model.BitmapDescriptorFactory;
import com.amap.api.maps2d.model.CameraPosition;
import com.amap.api.maps2d.model.Marker;
import com.amap.api.maps2d.model.MarkerOptions;
import com.amap.api.maps2d.model.MyLocationStyle;
import com.amap.api.services.core.AMapException;
import com.amap.api.services.core.LatLonPoint;
import com.amap.api.services.geocoder.GeocodeSearch;
import com.amap.api.services.geocoder.RegeocodeAddress;
import com.amap.api.services.geocoder.RegeocodeQuery;
import com.amap.api.services.geocoder.RegeocodeRoad;
import com.amap.api.services.geocoder.StreetNumber;


import com.amap.api.maps2d.AMap;
import com.amap.api.maps2d.model.LatLng;
import com.amap.api.services.core.LatLonPoint;



public class LocationActivity extends Activity implements
        AMapLocationListener{
	private AMap aMap;
	private MapView mapView;
	private LocationSource.OnLocationChangedListener mListener;

	private TextView textViewLocationInfo;
	public static Marker locationMarker;
	private LinearLayout layoutLoading;
	private LatLng locationLatLng;
	private Handler handler = new Handler();

	private AMapLocationClient mlocationClient;
	private AMapLocationClientOption mLocationOption;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_loction);
		mapView = (MapView) findViewById(R.id.map);
		mapView.onCreate(savedInstanceState); //此处必须加上
		init();
	}

	private void init() {
		layoutLoading = (LinearLayout) findViewById(R.id.layout_loading);
		findViewById(R.id.btn_back).setOnClickListener(clickListener);
		if (aMap == null) {
			aMap = mapView.getMap();
			if (AMapUtil.checkReady(this, aMap)) {
				setUpMap();
			}
		}
	}

	private void setUpMap() {
		aMap.setOnCameraChangeListener(cameraChangeListener);
		aMap.setInfoWindowAdapter(infoWindowAdapter);

		aMap.setLocationSource(locationSource);
//		aMap.setMyLocationEnabled(true);// 设置为true表示系统定位按钮显示并响应点击，false表示隐藏，默认是false
//		aMap.getUiSettings().setMyLocationButtonEnabled(false);


		// 自定义系统定位小蓝点
		MyLocationStyle myLocationStyle = new MyLocationStyle();
		myLocationStyle.myLocationIcon(BitmapDescriptorFactory
				.fromResource(R.drawable.location_marker));// 设置小蓝点的图标
		myLocationStyle.strokeColor(Color.BLACK);// 设置圆形的边框颜色
		myLocationStyle.radiusFillColor(Color.argb(100, 0, 0, 180));// 设置圆形的填充颜色
		// myLocationStyle.anchor(int,int)//设置小蓝点的锚点
		myLocationStyle.strokeWidth(1.0f);// 设置圆形的边框粗细
		aMap.setMyLocationStyle(myLocationStyle);

		aMap.getUiSettings().setMyLocationButtonEnabled(true);// 设置默认定位按钮是否显示
		aMap.setMyLocationEnabled(true);// 设置为true表示显示定位层并可触发定位，false表示隐藏定位层并不可触发定位，默认是false
	}

	/**
	 * 往地图上添加marker
	 * 
	 * @param latLng
	 */
	private void addMarker(LatLng latLng, String desc) {
		MarkerOptions markerOptions = new MarkerOptions();
		markerOptions.position(latLng);
		markerOptions.title("[我的位置]");
		markerOptions.snippet(desc);
		markerOptions.icon(BitmapDescriptorFactory.defaultMarker());
		locationMarker = aMap.addMarker(markerOptions);
	}

	View.OnClickListener clickListener = new View.OnClickListener() {

		@Override
		public void onClick(View v) {
			switch (v.getId()) {
			
			case R.id.btn_back:
                String result = locationMarker.getSnippet() + "lat:"+ String.valueOf(locationLatLng.latitude) + "lng:"+ String.valueOf(locationLatLng.longitude);
                Intent intent = new Intent();
                intent.putExtra("location", result);
                setResult(1001, intent);
				finish();
				break;
			}
		}
	};

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if(keyCode == KeyEvent.KEYCODE_BACK && event.getAction() == KeyEvent.ACTION_DOWN){
            String result = locationMarker.getSnippet() + "lat:"+ String.valueOf(locationLatLng.latitude) + "lng:"+ String.valueOf(locationLatLng.longitude);
            Intent intent = new Intent();
            intent.putExtra("location", result);
            setResult(1001, intent);
            finish();
            return true;
        }else if(keyCode == KeyEvent.KEYCODE_HOME){
            return true;
        }else if(keyCode == KeyEvent.KEYCODE_SEARCH){
            return true;
        }else {}
        return super.onKeyDown(keyCode, event);
    }

	LocationSource locationSource = new LocationSource() {

		@Override
		public void deactivate() {
			mListener = null;
			if (mlocationClient != null) {
				mlocationClient.stopLocation();
				mlocationClient.onDestroy();
			}
			mlocationClient = null;
		}

		@Override
		public void activate(OnLocationChangedListener listener) {
			mListener = listener;
			if (mlocationClient == null) {
				mlocationClient = new AMapLocationClient(LocationActivity.this);
				mLocationOption = new AMapLocationClientOption();
				//设置定位监听
				mlocationClient.setLocationListener(LocationActivity.this);
				mLocationOption.setNeedAddress(true);
				//设置为高精度定位模式
				mLocationOption.setLocationMode(AMapLocationClientOption.AMapLocationMode.Hight_Accuracy);
				//设置定位参数
				mlocationClient.setLocationOption(mLocationOption);
				// 此方法为每隔固定时间会发起一次定位请求，为了减少电量消耗或网络流量消耗，
				// 注意设置合适的定位时间的间隔（最小间隔支持为2000ms），并且在合适时间调用stopLocation()方法来取消定位请求
				// 在定位结束后，在合适的生命周期调用onDestroy()方法
				// 在单次定位情况下，定位无论成功与否，都无需调用stopLocation()方法移除请求，定位sdk内部会移除
				mlocationClient.startLocation();
			}
		}
	};


    @Override
    public void onLocationChanged(AMapLocation aLocation) {
        if (mListener != null) {
            // 此处注释掉，表示不用系统提供的定位图标等
            // mListener.onLocationChanged(aLocation);
        }
        if (aLocation != null) {
            Double geoLat = aLocation.getLatitude();
            Double geoLng = aLocation.getLongitude();
            locationLatLng = new LatLng(geoLat, geoLng);

            String desc = "";
            Bundle locBundle = aLocation.getExtras();
            if (locBundle != null) {
                desc = locBundle.getString("desc");
            }
            addMarker(locationLatLng, desc);
            locationMarker.showInfoWindow();// 显示信息窗口
            aMap.moveCamera(CameraUpdateFactory.newLatLngZoom(
                    locationLatLng, 15));
            locationSource.deactivate();
            layoutLoading.setVisibility(View.GONE);
        }
    }



	AMap.OnCameraChangeListener cameraChangeListener = new AMap.OnCameraChangeListener() {

		@Override
		public void onCameraChangeFinish(CameraPosition position) {
			if (locationMarker != null) {

				final LatLng latLng = position.target;
				new Thread(new Runnable()
				{
					
					@Override
					public void run()
					{
						GeocodeSearch geocodeSearch = new GeocodeSearch(LocationActivity.this);
						LatLonPoint point =new LatLonPoint(latLng.latitude, latLng.longitude);
						RegeocodeQuery regeocodeQuery = new RegeocodeQuery(point, 1000,GeocodeSearch.AMAP);
						RegeocodeAddress address = null;
						try {
							address = geocodeSearch.getFromLocation(regeocodeQuery);
						} catch (AMapException e) {
							e.printStackTrace();
						}
						if(null==address){
							return;
						}
						StringBuffer stringBuffer = new StringBuffer();
						String area = address.getProvince();//省或直辖市
						String loc = address.getCity();//地级市或直辖市
						String subLoc = address.getDistrict();//区或县或县级市
						String ts = address.getTownship();//乡镇
						String thf = null;//道路
						List<RegeocodeRoad> regeocodeRoads = address.getRoads();//道路列表
						if(regeocodeRoads != null && regeocodeRoads.size() > 0)
						{
							RegeocodeRoad regeocodeRoad = regeocodeRoads.get(0);
							if(regeocodeRoad != null)
							{
								thf = regeocodeRoad.getName();
							}
						}
						String subthf = null;//门牌号
						StreetNumber streetNumber =	address.getStreetNumber();
						if(streetNumber != null)
						{
							subthf = streetNumber.getNumber();
						}
						String fn = address.getBuilding();//标志性建筑,当道路为null时显示
						if (area != null)
							stringBuffer.append(area);
						if(loc!=null&&!area.equals(loc))
							stringBuffer.append(loc);
						if (subLoc != null)
							stringBuffer.append(subLoc);
						if (ts != null)
							stringBuffer.append(ts);
						if (thf != null)
							stringBuffer.append(thf);
						if (subthf != null)
							stringBuffer.append(subthf);
						if ((thf == null && subthf == null) && fn != null&&!subLoc.equals(fn))
							stringBuffer.append(fn + "附近");
						locationMarker.setSnippet(stringBuffer.toString());
						handler.post(new Runnable()
						{
							
							@Override
							public void run()
							{
								locationMarker.showInfoWindow();
							}
						});
					}
				}).start();

			
			}
		}

		@Override
		public void onCameraChange(CameraPosition position) {
			if (locationMarker != null) {
				LatLng latLng = position.target;
				locationMarker.setPosition(latLng);
			}
		}
	};

	AMap.InfoWindowAdapter infoWindowAdapter = new AMap.InfoWindowAdapter() {

		@Override
		public View getInfoWindow(Marker marker) {
			return null;
		}

		@Override
		public View getInfoContents(Marker marker) {
			View mContents = getLayoutInflater().inflate(R.layout.custom_info_contents,
					null);
			render(marker, mContents);
			return mContents;
		}
	};
	


	/** 自定义infowindow的样式 */
	public void render(Marker marker, View view) {
		String title = marker.getTitle();
		TextView titleUi = ((TextView) view.findViewById(R.id.title));
		if (title != null) {
			titleUi.setText(title);
		} else {
			titleUi.setText("");
		}
		String snippet = marker.getSnippet();
		TextView snippetUi = ((TextView) view.findViewById(R.id.snippet));
		if (snippet != null) {
			snippetUi.setText(snippet);
		} else {
			snippetUi.setText("");
		}
	}

	/**
	 * marker点击时跳动一下
	 */
	public void jumpPoint(final Marker marker, final LatLng latLng) {

		final Handler handler = new Handler();
		final long start = SystemClock.uptimeMillis();
		Projection proj = aMap.getProjection();
		Point startPoint = proj.toScreenLocation(latLng);
		startPoint.offset(0, -100);
		final LatLng startLatLng = proj.fromScreenLocation(startPoint);
		final long duration = 1500;

		final Interpolator interpolator = new BounceInterpolator();
		handler.post(new Runnable() {
			@Override
			public void run() {
				long elapsed = SystemClock.uptimeMillis() - start;
				float t = interpolator.getInterpolation((float) elapsed
						/ duration);
				double lng = t * latLng.longitude + (1 - t)
						* startLatLng.longitude;
				double lat = t * latLng.latitude + (1 - t)
						* startLatLng.latitude;
				marker.setPosition(new LatLng(lat, lng));
				if (t < 1.0) {
					handler.postDelayed(this, 16);
				}
			}
		});
	}

	/**
	 * 把一个view转化成bitmap对象
	 */
	public static Bitmap getViewBitmap(View view) {
		view.measure(MeasureSpec.makeMeasureSpec(0, MeasureSpec.UNSPECIFIED),
				MeasureSpec.makeMeasureSpec(0, MeasureSpec.UNSPECIFIED));
		view.layout(0, 0, view.getMeasuredWidth(), view.getMeasuredHeight());
		view.buildDrawingCache();
		Bitmap bitmap = view.getDrawingCache();
		return bitmap;
	}

	/**
	 * 把一个xml布局文件转化成view
	 */
	public View getView(String title, String text) {
		View view = getLayoutInflater().inflate(R.layout.marker, null);
		// TextView text_title = (TextView)
		// view.findViewById(R.id.marker_title);
		textViewLocationInfo = (TextView) view.findViewById(R.id.marker_text);
		// text_title.setText(title);
		textViewLocationInfo.setText(text);
		return view;
	}

	/**
	 * 方法必须重写
	 */
	@Override
	protected void onResume() {
		super.onResume();
		mapView.onResume();
	}

	/**
	 * 方法必须重写
	 */
	@Override
	protected void onPause() {
		super.onPause();
		locationSource.deactivate();
		mapView.onPause();
	}
	
	/**
	 * 方法必须重写
	 */
	@Override
	protected void onSaveInstanceState(Bundle outState) {
		super.onSaveInstanceState(outState);
		mapView.onSaveInstanceState(outState);
	}
	
	/**
	 * 方法必须重写
	 */
	@Override
	protected void onDestroy()
	{
		super.onDestroy();
		mapView.onDestroy();
	}

}
