package com.example.wyq.restd;

/**
 * Created by wyq on 2016/7/6.
 */
public class user {
    public String ID;
    public String LoginName;
    public String PSW;
    public String Name;
    public String Gender;
    public String Tel;
    public String AccountType;
    public String HttpMethod;
    public String Remark;
    public user(){
        LoginName = "login";
        PSW = "login";
        Name = "login";
        Gender = "1";
        Tel = "1123132132";
        AccountType = "1";
        HttpMethod = "POST";
        Remark = "1";
    }
}
