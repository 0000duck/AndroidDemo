package com.example.wyq.restd;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.Socket;
import android.util.Log;


public class TCPClient {
 
    private String serverMessage;
    Socket socket;

	public static final String SERVERIP = "192.168.0.187"; // your computer IP address
    public static final int SERVERPORT = 5657;
    private OnMessageReceived mMessageListener = null;
    private boolean mRun = false;
 
    private PrintWriter out = null;
    private BufferedReader in = null;

    public TCPClient(final OnMessageReceived listener) 
    {
        mMessageListener = listener;
    }

    public void sendMessage(String message){
        if (out != null && !out.checkError()) {
            Log.e("TCP SI Client","message: "+ message);
            out.println(message);
            out.flush();
        }
    }

    private void Connect(){
        try {
            InetAddress serverAddr = InetAddress.getByName(SERVERIP);
            socket = new Socket(serverAddr, SERVERPORT);
            out = new PrintWriter(new BufferedWriter(new OutputStreamWriter(socket.getOutputStream())), true);
            in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
        } catch (Exception e) {
            Log.e("TCP SI Error", "SI: Error", e);
        }
    }
 
    public void stopClient(){
        mRun = false;
        try {
            socket.close();
        }catch (Exception e) {
            Log.e("TCP SI Error", "SI: Error", e);
        }
    }
    
    public void run() {
        mRun = true;
        Connect();
        try {
            //in this while the client listens for the messages sent by the server
            while (mRun) {
                serverMessage = in.readLine();
                if (serverMessage != null && mMessageListener != null) {
                    //call the method messageReceived from MyActivity class
                    mMessageListener.messageReceived(serverMessage);
                    Log.e("RESPONSE FROM SERVER", "S: Received Message: '" + serverMessage + "'");
                    Thread.sleep(10);
                }
                serverMessage = null;
            }
        }
        catch (Exception e){
            Log.e("TCP SI Error", "SI: Error", e);
        }

    }
 
    //Declare the interface. The method messageReceived(String message) will must be implemented in the MyActivity
    //class at on asynckTask doInBackground
    public interface OnMessageReceived {
        public void messageReceived(String message);
    }
}