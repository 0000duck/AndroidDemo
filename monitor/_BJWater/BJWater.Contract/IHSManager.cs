using System.Collections.Generic;
using BJWater.Contract.Model;
using System;

namespace BJWater.Contract
{
    public interface IHSManager
    {

        #region 控制器信息增删改查
        void DeleteControler(List<int> ids);
        List<Controler> SelectAllControler();

        Controler SelectControler(int id);
        void UpdateControler(Controler model);
        void InsertControler(Controler model);
        #endregion

        #region 控制器历史信息增删改查
        void DeleteControlerHistory(List<int> ids);
        List<ControlerHistory> SelectAllControlerHistory();

        ControlerHistory SelectControlerHistory(int id);
        void UpdateControlerHistory(ControlerHistory model);
        void InsertControlerHistory(ControlerHistory model);
        #endregion

        #region 监视器信息增删改查
        void DeleteMonitor(List<int> ids);
        List<Monitor> SelectAllMonitor();

        Monitor SelectMonitor(int id);
        void UpdateMonitor(Monitor model);
        void InsertMonitor(Monitor model);
        #endregion

        #region 监视器历史信息增删改查
        void DeleteMonitorHistory(List<int> ids);
        List<MonitorHistory> SelectAllMonitorHistory();

        MonitorHistory SelectMonitorHistory(int id);
        void UpdateMonitorHistory(MonitorHistory model);
        void InsertMonitorHistory(MonitorHistory model);
        #endregion

        #region 维修增删改查
        void DeleteRepair(List<int> ids);
        List<Repair> SelectAllRepair();

        Repair SelectRepair(int id);
        void UpdateRepair(Repair model);
        void InsertRepair(Repair model);
        #endregion
       
    }
}
