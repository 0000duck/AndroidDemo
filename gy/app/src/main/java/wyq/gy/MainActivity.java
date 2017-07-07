package wyq.gy;

import android.content.Intent;
import android.graphics.BitmapFactory;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.design.widget.NavigationView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.amap.api.maps2d.AMap;
import com.amap.api.maps2d.CameraUpdate;
import com.amap.api.maps2d.CameraUpdateFactory;
import com.amap.api.maps2d.MapView;
import com.amap.api.maps2d.model.BitmapDescriptorFactory;
import com.amap.api.maps2d.model.CameraPosition;
import com.amap.api.maps2d.model.LatLng;
import com.amap.api.maps2d.model.Marker;
import com.amap.api.maps2d.model.MarkerOptions;
import com.amap.api.maps2d.model.MyLocationStyle;
import com.avos.avoscloud.AVException;
import com.avos.avoscloud.AVGeoPoint;
import com.avos.avoscloud.AVObject;
import com.avos.avoscloud.AVQuery;
import com.avos.avoscloud.AVUser;
import com.avos.avoscloud.FindCallback;

import java.util.List;

import wyq.gy.lib.LocationTask;
import wyq.gy.lib.OnLocationGetListener;
import wyq.gy.lib.PositionEntity;
import wyq.gy.lib.RegeocodeTask;
import wyq.gy.lib.RouteTask;
import wyq.gy.lib.Utils;


public class MainActivity extends AppCompatActivity implements NavigationView.OnNavigationItemSelectedListener,
        AMap.OnCameraChangeListener,
        AMap.OnMapLoadedListener, OnLocationGetListener, View.OnClickListener {

    MapView mMapView = null;
    private AMap mAmap;
    private Marker mPositionMark;
    private LatLng mStartPosition;
    private RegeocodeTask mRegeocodeTask;
    private LocationTask mLocationTask;
    private ImageView mLocationImage;
    private boolean mIsFirst = true;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        init(savedInstanceState);
        mLocationTask = LocationTask.getInstance(getApplicationContext());
        mLocationTask.setOnLocationGetListener(this);

        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        toolbar.setTitle("快乐兄弟用工雇主端");
        setSupportActionBar(toolbar);

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
    }


    private void init(Bundle savedInstanceState) {
        mMapView = (MapView) findViewById(R.id.map);
        mMapView.onCreate(savedInstanceState);
        mAmap = mMapView.getMap();
        mAmap.getUiSettings().setZoomControlsEnabled(false);
        mAmap.setOnMapLoadedListener(this);
        mAmap.setOnCameraChangeListener(this);
        mLocationImage = (ImageView) findViewById(R.id.location_image);
        mLocationImage.setOnClickListener(this);
        Button mapBtn = (Button) findViewById(R.id.map_btn);
        mapBtn.setOnClickListener(this);
    }

    private void GetNearWorker(double latitue, double longitude){
        AVQuery<AVUser> query = new AVQuery<>("_User");
        AVGeoPoint point = new AVGeoPoint(latitue, longitude);
        query.limit(30);
        query.whereNear("RegCoordinate", point);
        query.findInBackground(new FindCallback<AVUser>() {
            @Override
            public void done(List<AVUser> list, AVException e) {
                if(list != null) {
                    List<AVUser> workers = list;// 离这个位置最近的 10 个 Todo 对象
                    for (int i = 0; i < workers.size(); i++) {
                        AVGeoPoint geo = (AVGeoPoint) workers.get(i).get("RegCoordinate");
                        MarkerOptions markerOptions = new MarkerOptions();
                        markerOptions.anchor(0.5f, 0.5f);
                        markerOptions.position(new LatLng(geo.getLatitude(), geo.getLongitude()));
                        markerOptions
                                .icon(BitmapDescriptorFactory.fromBitmap(BitmapFactory
                                        .decodeResource(getResources(),
                                                R.drawable.work)));
                        mAmap.addMarker(markerOptions);
                    }
                }
            }
        });
    }
    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        if (id == R.id.action_msg) {
            return true;
        }else if(id == R.id.action_fankui){
            Intent intent = new Intent();
            intent.setClass(MainActivity.this, SuggestionActivity.class);
            startActivity(intent);
        }
        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();

        if (id == R.id.undone_order) {
            // Handle the camera action
        } else if (id == R.id.my_team) {

        } else if (id == R.id.order_record) {
            Intent intent = new Intent();
            intent.setClass(MainActivity.this, HistoryOrderActivity.class);
            startActivity(intent);
        } else if (id == R.id.nav_manage) {
            Intent intent = new Intent();
            intent.setClass(MainActivity.this, SettingActivity.class);
            startActivity(intent);
        } else if (id == R.id.nav_share) {

        } else if (id == R.id.nav_send) {
            Intent intent = new Intent();
            intent.setClass(MainActivity.this, SuggestionActivity.class);
            startActivity(intent);
        }

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

    //地图
    @Override
    protected void onDestroy() {
        super.onDestroy();
        mMapView.onDestroy();
    }
    @Override
    protected void onResume() {
        super.onResume();
        mMapView.onResume();
    }
    @Override
    protected void onPause() {
        super.onPause();
        mMapView.onPause();
    }
    @Override
    protected void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);
        mMapView.onSaveInstanceState(outState);
    }
    @Override
    public void onMapLoaded() {
        MarkerOptions markerOptions = new MarkerOptions();
        markerOptions.anchor(0.5f, 0.5f);
        markerOptions.position(new LatLng(0, 0));
        markerOptions
                .icon(BitmapDescriptorFactory.fromBitmap(BitmapFactory
                        .decodeResource(getResources(),
                                R.drawable.map_pin)));
        mPositionMark = mAmap.addMarker(markerOptions);

        mPositionMark.setPositionByPixels(mMapView.getWidth() / 2,
                mMapView.getHeight() / 2);
        mLocationTask.startSingleLocate();
    }

    @Override
    public void onCameraChange(CameraPosition arg0) {

    }

    @Override
    public void onCameraChangeFinish(CameraPosition cameraPosition) {
        mStartPosition = cameraPosition.target;
        mRegeocodeTask.setOnLocationGetListener(this);
        mRegeocodeTask
                .search(mStartPosition.latitude, mStartPosition.longitude);
        if (mIsFirst) {
            Utils.addEmulateData(mAmap, mStartPosition);
            if (mPositionMark != null) {
                //mPositionMark.setToTop();
            }
            mIsFirst = false;
        }
        mAmap.clear();
        GetNearWorker(cameraPosition.target.latitude, cameraPosition.target.longitude);
    }

    //定位ok
    @Override
    public void onLocationGet(PositionEntity entity) {
        RouteTask.getInstance(getApplicationContext()).setStartPoint(entity);

        mStartPosition = new LatLng(entity.latitue, entity.longitude);
        CameraUpdate cameraUpate = CameraUpdateFactory.newLatLngZoom(
                mStartPosition, mAmap.getCameraPosition().zoom);
        mAmap.animateCamera(cameraUpate);
        GetNearWorker(entity.latitue, entity.longitude);
    }

    @Override
    public void onRegecodeGet(PositionEntity entity) {

    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.location_image:
                mLocationTask.startSingleLocate();
                break;
            case R.id.map_btn:
                //
                break;
        }
    }



}
