package hb.smartgreen.activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;

import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.Toast;

import hb.smartgreen.R;
import hb.smartgreen.Widget.EditTextCanClean;
import hb.smartgreen.util.DbService;

/**
 * A login screen that offers login via email/password.
 */
public class LoginActivity extends AppCompatActivity {

    private EditTextCanClean mPasswordView;
    private View mProgressView;
    private View mLoginFormView;

    private EditTextCanClean etName;
    private CheckBox cbRemember;
    private CheckBox cbAutoLogin;

    private SharedPreferences sp;
    private SharedPreferences.Editor ed;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        // Set up the login form.
        etName = (EditTextCanClean) findViewById(R.id.name);
        mPasswordView = (EditTextCanClean) findViewById(R.id.password);
        cbRemember = (CheckBox)findViewById(R.id.cbRemember);
        cbAutoLogin = (CheckBox)findViewById(R.id.cbAutologin);
        AutoLogin();

        mLoginFormView = findViewById(R.id.login_form);
        mProgressView = findViewById(R.id.login_progress);
    }

    private void AutoLogin() {
        sp = getSharedPreferences("users", MODE_PRIVATE);
        ed = sp.edit();
        if (sp.getBoolean("IS_REME_CHECK", false)) {
            cbRemember.setChecked(true);
        }

        if (sp.getBoolean("AUTO_ISCHECK", false)) {
            cbAutoLogin.setChecked(true);
            cbRemember.setChecked(true);
        }
        if(cbRemember.isChecked()) {
            etName.setText(sp.getString("oa_name", ""));
            mPasswordView.setText(sp.getString("oa_pass", ""));
        }
        cbRemember.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                Boolean isChecked1 = cbRemember.isChecked();
                ed.putBoolean("IS_REME_CHECK", isChecked1);
                ed.commit();
            }
        });
        cbAutoLogin.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                Boolean isChecked2 = cbAutoLogin.isChecked();
                ed.putBoolean("AUTO_ISCHECK", isChecked2);
                ed.commit();
            }
        });

        if (cbRemember.isChecked() && cbAutoLogin.isChecked()) {
            attemptLogin();
        }

        Button mEmailSignInButton = (Button) findViewById(R.id.email_sign_in_button);
        mEmailSignInButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View view) {
                attemptLogin();
            }
        });
    }

    private void attemptLogin() {
        // Store values at the time of the login attempt.
        String name = etName.getText().toString();
        String password = mPasswordView.getText().toString();

        // 将信息存入到users里面
        ed.putString("oa_name", name);
        ed.putString("oa_pass", password);
        ed.commit();
        if (TextUtils.isEmpty(name)) {
            Toast.makeText(this, "请输入用户名", Toast.LENGTH_LONG).show();
            return;
        }
        if (TextUtils.isEmpty(password)) {
            Toast.makeText(this, "请输入密码", Toast.LENGTH_LONG).show();
            return;
        }

        //判断账号密码对不对。
        DbService db = new DbService();
        Boolean validation= db.ValidateUser(name,password);
        final Intent intent = new Intent(this,MainActivity.class);
        ;intent.addCategory(Intent.CATEGORY_HOME);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        this.startActivity(intent);
        finish();
    }

}

