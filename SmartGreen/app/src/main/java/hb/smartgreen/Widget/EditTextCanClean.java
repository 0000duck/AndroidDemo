package hb.smartgreen.Widget;

import android.content.Context;
import android.content.res.TypedArray;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.text.Editable;
import android.text.InputFilter;
import android.text.InputType;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.util.AttributeSet;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import hb.smartgreen.R;


public class EditTextCanClean extends RelativeLayout {
    private EditText et;
    private ImageView img;

    public EditTextCanClean(Context context) {
        super(context);
    }

    public EditTextCanClean(Context context, AttributeSet attrs) {
        super(context, attrs);
        int resourceId = -1;
        View view = LayoutInflater.from(context).inflate(R.layout.mould_layout, this, true);

        et = (EditText) view.findViewById(R.id.et);
        //给et添加输入内容变化的监听，控制清除按钮的显示与否
        et.addTextChangedListener(textWatcher);

        img = (ImageView) view.findViewById(R.id.imgClean);
        //初始无输入值时，清除按钮隐藏
        img.setVisibility(View.INVISIBLE);
        //清除按钮的点击响应
        img.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                et.setText("");
            }
        });

        //获取自定义参数，对自定义控件进行初始化设置
        TypedArray ta = context.obtainStyledAttributes(attrs, R.styleable.EditTextCanClean);
        int n = ta.getIndexCount();
        for (int i = 0; i < n; i++) {
            int attr = ta.getIndex(i);
            switch (attr) {
                case R.styleable.EditTextCanClean_hint:
                    resourceId = ta.getResourceId(R.styleable.EditTextCanClean_hint, 0);
                    et.setHint(resourceId > 0 ? getResources().getText(resourceId) : ta.getString(R.styleable.EditTextCanClean_hint));
                    break;

                case R.styleable.EditTextCanClean_text:
                    resourceId = ta.getResourceId(R.styleable.EditTextCanClean_text, 0);
                    et.setText(resourceId > 0 ? getResources().getText(resourceId) : ta.getString(R.styleable.EditTextCanClean_text));
                    break;

                case R.styleable.EditTextCanClean_cleanIconDrawable:
                    resourceId = ta.getResourceId(R.styleable.EditTextCanClean_cleanIconDrawable, 0);
                    img.setImageResource(resourceId > 0 ? resourceId : R.drawable.icon_clean);
                    break;

                case R.styleable.EditTextCanClean_textColor:
                    et.setTextColor(ta.getColor(R.styleable.EditTextCanClean_textColor, Color.BLACK));
                    break;

                case R.styleable.EditTextCanClean_textSize:
                    resourceId = ta.getResourceId(R.styleable.EditTextCanClean_textSize, 0);
                    et.setTextSize(resourceId > 0 ? getResources().getDimension(resourceId) : ta.getDimension(R.styleable.EditTextCanClean_textSize, 20));
                    break;

                case R.styleable.EditTextCanClean_maxLength:
                    et.setFilters(new InputFilter[]{new InputFilter.LengthFilter(ta.getInt(R.styleable.EditTextCanClean_maxLength, 0))});
                    break;

                case R.styleable.EditTextCanClean_password:
                    if(ta.getBoolean(R.styleable.EditTextCanClean_password, false)) {
                        et.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
                    }
                    else{
                        et.setInputType(InputType.TYPE_TEXT_VARIATION_VISIBLE_PASSWORD);
                    }
                    break;
                case R.styleable.EditTextCanClean_bkground:
                    resourceId = ta.getResourceId(R.styleable.EditTextCanClean_bkground, 0);
                    if(resourceId > 0) {
                        et.setBackgroundResource(resourceId);
                    }
                    break;

                default:
                    break;
            }
        }
        ta.recycle();   //一定要对TypedArray资源进行释放
    }

    //设置Hint提示字符串的方法，以便通过Java代码进行设置
    protected void setHint(String string){
        et.setHint(string);
    }

    //设置编辑框显示文字的方法，以便通过Java代码进行设置
    protected void setText(String string) {
        et.setText(string);
    }

    //获取输入值
    public String getText(){
        return et.getText().toString();
    }

    //设置清除按钮的图标，以便通过Java代码进行设置
    protected void setCleanIcon(int drawableId) {
        Drawable drawable = getResources().getDrawable(drawableId);
        img.setImageDrawable(drawable);
    }

    //***重要
    //暴露出EditText，以便设置EditText的更多属性，如inputType属性等等，是为了增加灵活性和兼容性
    protected EditText getEditText(){
        return et;
    }

    //输入框文本变化监听器
    private TextWatcher textWatcher = new TextWatcher() {
        @Override
        public void beforeTextChanged(CharSequence s, int start, int count, int after) {}
        @Override
        public void afterTextChanged(Editable s) {}
        @Override
        public void onTextChanged(CharSequence s, int start, int before, int count) {
            if (TextUtils.isEmpty(s)){
                img.setVisibility(View.INVISIBLE);
            }else {
                img.setVisibility(View.VISIBLE);
            }
        }
    };

}
