package hb.smartgreen.util;

import android.util.Log;

import org.xutils.DbManager;
import org.xutils.x;

import java.util.ArrayList;
import java.util.List;

import hb.smartgreen.bean.sgUser;

/**
 * Created by wyq on 2016/4/11.
 */
public class DbService {

    private DbManager.DaoConfig daoConfig ;
    public  DbService() {
        daoConfig = new DbManager.DaoConfig()
                .setDbName("sg.db")
                        // 不设置dbDir时, 默认存储在app的私有目录.
//            .setDbDir(new File("/sdcard")) // "sdcard"的写法并非最佳实践, 这里为了简单, 先这样写了.
                .setDbVersion(2)
                .setDbOpenListener(new DbManager.DbOpenListener() {
                    @Override
                    public void onDbOpened(DbManager db) {
                        // 开启WAL, 对写入加速提升巨大
                        db.getDatabase().enableWriteAheadLogging();
                    }
                })
                .setDbUpgradeListener(new DbManager.DbUpgradeListener() {
                    @Override
                    public void onUpgrade(DbManager db, int oldVersion, int newVersion) {
                        // TODO: ...
                        // db.addColumn(...);
                        // db.dropTable(...);
                        // ...
                        // or
                        // db.dropDb();
                    }
                });
    }

    public int InsertUser(sgUser user) {
        int ret = 0;
        try{
            DbManager db = x.getDb(daoConfig);
            db.save(user);
        }
        catch (Throwable e){
            ret = -1;
            Log.e("InsertUser", e.getMessage());
        }
        return ret;
    }

    public int UpdateUser(sgUser user) {
        int ret = 0;
        try{
            DbManager db = x.getDb(daoConfig);
            db.update(user);
        }
        catch (Throwable e){
            ret = -1;
            Log.e("UpdateUser", e.getMessage());
        }
        return ret;
    }

    public int DeleteUserByName(String name) {
        int ret = 0;
        try{
            DbManager db = x.getDb(daoConfig);
            db.delete(db.selector(sgUser.class).where("username", "=", name).findAll());
        }
        catch (Throwable e){
            ret = -1;
            Log.e("DeleteUserByName", e.getMessage());
        }
        return ret;
    }

    public List<sgUser> GetUserList() {
        try{
            DbManager db = x.getDb(daoConfig);
            return  db.findAll(sgUser.class);
        }
        catch (Throwable e){
            Log.e("GetUserList", e.getMessage());
            return new ArrayList<sgUser>();
        }
    }

    public sgUser GetUserByName(String name) {

        try{
            DbManager db = x.getDb(daoConfig);
            return  db.selector(sgUser.class).where("username","=", name).findFirst();
        }
        catch (Throwable e){
            Log.e("GetUserByName", e.getMessage());
            return null;
        }
    }

    public boolean ValidateUser(String name,String password) {
        return true;
//        boolean ret = false;
//        try{
//            DbManager db = x.getDb(daoConfig);
//            List<sgUser> userList = db.selector(sgUser.class).where("username","=", name).and("password","=",password).findAll();
//            int userCount =  userList.size();
//            Log.e("user count:", "" + userCount);
//            if (userCount > 0){
//                ret = true;
//            }
//        }
//        catch (Throwable e){
//            Log.e("ValidateUser", e.getMessage());
//        }
//        return ret;
    }

}
