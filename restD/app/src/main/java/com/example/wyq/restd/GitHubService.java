package com.example.wyq.restd;

import java.util.List;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.http.Body;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;

/**
 * Created by wyq on 2016/7/6.
 */
public interface GitHubService {
    @GET("user")
    Call<ResponseBody> getUsers();



    @POST("user")
    Call<ResponseBody> postUser(@Body user userD);
}

