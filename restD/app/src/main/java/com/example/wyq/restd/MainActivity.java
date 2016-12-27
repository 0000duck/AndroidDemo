package com.example.wyq.restd;

import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;

import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {
    private ListView mList;
    private ArrayList<String> arrayList;
    private MyCustomAdapter mAdapter;
    private TCPClient mTcpClient = null;
    private connectTask conctTask = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        setContentView(R.layout.activity_main);
        super.onCreate(savedInstanceState);

        arrayList = new ArrayList<String>();

        final EditText editText = (EditText) findViewById(R.id.editText);
        Button send = (Button)findViewById(R.id.send_button);

        mList = (ListView)findViewById(R.id.list);
        mAdapter = new MyCustomAdapter(this, arrayList);
        mList.setAdapter(mAdapter);

        mTcpClient = null;
        Connect();

        send.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String message = editText.getText().toString();
                arrayList.add("Android Client: " + message);
                if (mTcpClient != null){
                    mTcpClient.sendMessage("Android Client: "+message);
                }
                mAdapter.notifyDataSetChanged();
                editText.setText("");
            }
        });
        Button con = (Button)findViewById(R.id.con);
        con.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Connect();
            }
        });
        Button discon = (Button)findViewById(R.id.discon);
        discon.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DisConnect();
            }
        });
    }

    private void Connect(){
        conctTask = new connectTask();
        conctTask.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
    }

    private void DisConnect(){
        Log.e("TCP SI Client","onDestroy.");
        if(mTcpClient != null) {
            mTcpClient.sendMessage("disconnect");
            mTcpClient.stopClient();
        }
        if(conctTask != null) {
            conctTask.cancel(true);
            conctTask = null;
        }
    }


    public class connectTask extends AsyncTask<String,String,TCPClient> {
        @Override
        protected TCPClient doInBackground(String... message)
        {
            mTcpClient = new TCPClient(new TCPClient.OnMessageReceived(){
                @Override
                public void messageReceived(String message){
                    try{
                        //this method calls the onProgressUpdate
                        publishProgress(message);
                        if(message!=null){
                            Log.e("TCP SI Client","Return Message from Socket::::: >>>>> "+message);
                        }
                    }
                    catch (Exception e){
                        e.printStackTrace();
                    }
                }
            });
            mTcpClient.run();
            if(mTcpClient!=null){
                mTcpClient.sendMessage("Initial Message when connected with Socket Server");
            }
            return null;
        }

        @Override
        protected void onProgressUpdate(String... values) {
            super.onProgressUpdate(values);
            arrayList.add(values[0]);
            mAdapter.notifyDataSetChanged();
        }
    }

    @Override
    protected void onDestroy(){
        try{
            DisConnect();
        }catch (Exception e){
            e.printStackTrace();
        }
        super.onDestroy();
    }
}
