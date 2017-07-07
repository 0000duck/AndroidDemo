package wyq.gy;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.avos.avoscloud.AVException;
import com.avos.avoscloud.AVUser;
import com.avos.avoscloud.RequestMobileCodeCallback;
import com.avos.avoscloud.SignUpCallback;

import wyq.gy.lib.ActivityList;

public class RegisterActivity extends AppCompatActivity implements View.OnClickListener {
    private EditText mAccount;                        //用户名编辑
    private EditText mPwd;                            //密码编辑
    private EditText mPwdCheck;                       //密码编辑
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);
        ActivityList.getInstance().addActivity(this);

        mAccount = (EditText) findViewById(R.id.resetpwd_edit_name);
        mPwd = (EditText) findViewById(R.id.resetpwd_edit_pwd_old);
        mPwdCheck = (EditText) findViewById(R.id.resetpwd_edit_pwd_new);
        Button mSureButton = (Button) findViewById(R.id.register_btn_sure);
        mSureButton.setOnClickListener(this);
        TextView userAgree = (TextView)findViewById(R.id.userAgreements);
        userAgree.setOnClickListener(this);

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

    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.register_btn_sure://确认按钮的监听事件
                attempRegister();
//                AVUser.requestMobilePhoneVerifyInBackground(mAccount.getText().toString(),new RequestMobileCodeCallback() {
//                    @Override
//                    public void done(AVException e) {
//                        if (e == null) {
//                            // 注册成功
//                            Intent intent = new Intent();
//                            intent.setClass(RegisterActivity.this, MainActivity.class);
//                            startActivity(intent);
//                        } else {
//                            // 失败的原因可能有多种，常见的是用户名已经存在。
//                            Toast.makeText(RegisterActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
//                        }
//                    }
//                });

                break;
            case R.id.userAgreements://确认按钮的监听事件
                Intent intent = new Intent();
                intent.setClass(RegisterActivity.this, UserAgreeMentActivity.class);
                startActivity(intent);
                break;
        }
    }

    private void attempRegister() {
        String userName = mAccount.getText().toString();
        String userPwd = mPwd.getText().toString();
        String userPwdCheck = mPwdCheck.getText().toString();
        if (userName.isEmpty() || userPwd.isEmpty() || userPwdCheck.isEmpty()) {
            Toast.makeText(RegisterActivity.this, "输入信息为空", Toast.LENGTH_SHORT).show();
        } else if (!userPwd.equals(userPwdCheck)) {
            Toast.makeText(this, "两次密码不一样", Toast.LENGTH_SHORT).show();
        } else {

            Intent intent = new Intent();
            intent.putExtra("username",userName);
            intent.putExtra("userPwd",userPwd);
            intent.setClass(RegisterActivity.this, CompleteInfoActivity.class);
            startActivityForResult(intent,1);
        }
    }

    // 回调的方式来获取指定Activity返回的结果
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        // TODO Auto-generated method stub
        super.onActivityResult(requestCode, resultCode, data);

    }
}
