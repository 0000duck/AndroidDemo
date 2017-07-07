package wyq.gy;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.avos.avoscloud.AVException;
import com.avos.avoscloud.AVUser;
import com.avos.avoscloud.LogInCallback;

import wyq.gy.lib.ActivityList;

public class LoginActivity extends Activity implements View.OnClickListener{
    private EditText loginnameET;
    private EditText paswordET;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        ActivityList.getInstance().addActivity(this);
    }

    private void init() {
        loginnameET = (EditText) findViewById(R.id.login_nameet);
        paswordET = (EditText) findViewById(R.id.login_passwordet);

        Button login = (Button) findViewById(R.id.login);
        TextView login_registers = (TextView) findViewById(R.id.login_register);
        login.setOnClickListener(this);
        login_registers.setOnClickListener(this);
    }

    @Override
    protected void onStart() {
        super.onStart();
        init();

        AVUser currentUser = AVUser.getCurrentUser();
        if (currentUser != null) {
            Intent intent = new Intent();
            intent.setClass(LoginActivity.this, MainActivity.class);
            startActivity(intent);
            finish();
        } else {
            //缓存用户对象为空时，可打开用户注册界面…
        }
    }

    private void attempLogin() {
        String username = loginnameET.getText().toString();
        String psw = paswordET.getText().toString();
        if (TextUtils.isEmpty(username)){
            Toast.makeText(LoginActivity.this, R.string.enterAccount, Toast.LENGTH_SHORT).show();
        }else if(TextUtils.isEmpty(psw)){
            Toast.makeText(LoginActivity.this, R.string.enterPassword, Toast.LENGTH_SHORT).show();
        }else{
            AVUser.logInInBackground(username, psw, new LogInCallback<AVUser>() {
                @Override
                public void done(AVUser avUser, AVException e) {
                    Intent intent = new Intent();
                    intent.setClass(LoginActivity.this, MainActivity.class);
                    startActivity(intent);
                    finish();
                }
            });
        }
    }
    // 回调的方式来获取指定Activity返回的结果
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        // TODO Auto-generated method stub
        super.onActivityResult(requestCode, resultCode, data);

    }
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.login:
                attempLogin();
                break;
            case R.id.login_register:
                Intent intent = new Intent();
                intent.setClass(LoginActivity.this, RegisterActivity.class);
                startActivityForResult(intent,0);
                break;
        }
    }
}
