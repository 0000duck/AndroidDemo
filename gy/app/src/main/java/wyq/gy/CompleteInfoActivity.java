package wyq.gy;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.avos.avoscloud.AVException;
import com.avos.avoscloud.AVGeoPoint;
import com.avos.avoscloud.AVUser;
import com.avos.avoscloud.SignUpCallback;

import wyq.gy.lib.ActivityList;

public class CompleteInfoActivity extends AppCompatActivity {

    private String m_address;
    private double m_latitude;
    private double m_longitude;
    private TextView m_complete_name;
    private TextView m_complete_identity;
    private TextView m_complete_poi;
    private TextView m_complete_birthday;
    private TextView m_complete_birth_place;
    private TextView m_complete_self_intro;
    private String m_username;
    private String m_password;
    private EditText m_company;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_complete_info);
        ActivityList.getInstance().addActivity(this);
        android.support.v7.app.ActionBar actionBar = getSupportActionBar();
        if(actionBar != null){
            actionBar.setHomeButtonEnabled(true);
            actionBar.setDisplayHomeAsUpEnabled(true);
        }
        m_username = getIntent().getStringExtra("username");
        m_password = getIntent().getStringExtra("userPwd");
        Init();
    }
    private void Init(){
        m_complete_name = (TextView) findViewById(R.id.comlete_name);
        m_complete_identity = (TextView) findViewById(R.id.comlete_identity);
        m_complete_birthday = (TextView) findViewById(R.id.comlete_birthday);
        m_complete_birth_place = (TextView) findViewById(R.id.comlete_birth_place);
        m_complete_self_intro = (TextView) findViewById(R.id.comlete_self_intro);
        m_company = (EditText)findViewById(R.id.comlete_company_name);

        m_complete_poi = (TextView) findViewById(R.id.select_poi);
        m_complete_poi.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent();
                intent.setClass(CompleteInfoActivity.this, GetPositionActivity.class);
                startActivityForResult(intent,10);
            }
        });

        Button btn = (Button)findViewById(R.id.info_done_btn);
        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //实际注册
                AVUser user = new AVUser();// 新建 AVUser 对象实例
                user.setUsername(m_username);// 设置用户名
                user.setPassword(m_password);// 设置密码
                user.put("Name",m_complete_name.getText());
                user.put("IDNumber",m_complete_identity.getText());
                user.put("DateBirth",m_complete_birthday.getText());
                user.put("JiGuan",m_complete_birth_place.getText());
                user.put("selfintro",m_complete_self_intro.getText());
                AVGeoPoint point = new AVGeoPoint(m_latitude, m_longitude);
                user.put("RegCoordinate",point);
                user.put("RegLocation",m_complete_poi.getText());
                user.put("IdMark",1);
                user.put("CompanyName",m_company.getText());
                user.signUpInBackground(new SignUpCallback() {
                    @Override
                    public void done(AVException e) {
                        if (e == null) {
                            // 注册成功
                            Intent intent = new Intent();
                            intent.setClass(CompleteInfoActivity.this, MainActivity.class);
                            startActivity(intent);
                            ActivityList.getInstance().FinishAll();
                        } else {
                            // 失败的原因可能有多种，常见的是用户名已经存在。
                        }
                    }
                });

            }
        });
    }


    // 回调的方式来获取指定Activity返回的结果
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        // TODO Auto-generated method stub

        if(requestCode==10 && resultCode==10){
            if(data != null) {
                Bundle bundle = data.getBundleExtra("addr");
                m_address = bundle.getString("address");
                m_latitude = bundle.getDouble("latitude");
                m_longitude = bundle.getDouble("longitude");
                m_complete_poi.setText(m_address);
            }
        }
        super.onActivityResult(requestCode, resultCode, data);
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
}
