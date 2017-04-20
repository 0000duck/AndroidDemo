using System;
using System.Collections.Generic;
using System.Linq;
using BJWater.Contract;
using BJWater.DAL;
using BJWater.Contract.Model;

namespace BJWater.BLL
{
    public class Service : IHSManager
    {
      

        #region 控制器信息增删改查
        public void DeleteControler(List<int> ids)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Controler.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { context.Controler.Remove(a); });
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<Controler> SelectAllControler()
        {
            try
            {
                using (var context = new BJContext())
                {

                    return context.Controler.OrderByDescending(u => u.ID).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public Controler SelectControler(int id)
        {
            try
            {
                using (var context = new BJContext())
                {
                    return context.Controler.Where(u => u.ID == id).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
        public void UpdateControler(Controler model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Update<Controler>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InsertControler(Controler model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Insert<Controler>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion

        #region 控制器历史信息增删改查
        public void DeleteControlerHistory(List<int> ids)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.ControlerHistory.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { context.ControlerHistory.Remove(a); });
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<ControlerHistory> SelectAllControlerHistory()
        {
            try
            {
                using (var context = new BJContext())
                {

                    return context.ControlerHistory.OrderByDescending(u => u.ID).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public ControlerHistory SelectControlerHistory(int id)
        {
            try
            {
                using (var context = new BJContext())
                {
                    return context.ControlerHistory.Where(u => u.ID == id).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
        public void UpdateControlerHistory(ControlerHistory model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Update<ControlerHistory>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InsertControlerHistory(ControlerHistory model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Insert<ControlerHistory>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion

        #region 监视器信息增删改查
        public void DeleteMonitor(List<int> ids)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Monitor.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { context.Monitor.Remove(a); });
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<Monitor> SelectAllMonitor()
        {
            try
            {
                using (var context = new BJContext())
                {

                    return context.Monitor.OrderByDescending(u => u.ID).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public Monitor SelectMonitor(int id)
        {
            try
            {
                using (var context = new BJContext())
                {
                    return context.Monitor.Where(u => u.ID == id).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
        public void UpdateMonitor(Monitor model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Update<Monitor>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InsertMonitor(Monitor model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Insert<Monitor>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion

        #region 监视器历史信息增删改查
        public void DeleteMonitorHistory(List<int> ids)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.MonitorHistory.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { context.MonitorHistory.Remove(a); });
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<MonitorHistory> SelectAllMonitorHistory()
        {
            try
            {
                using (var context = new BJContext())
                {

                    return context.MonitorHistory.OrderByDescending(u => u.ID).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public MonitorHistory SelectMonitorHistory(int id)
        {
            try
            {
                using (var context = new BJContext())
                {
                    return context.MonitorHistory.Where(u => u.ID == id).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
        public void UpdateMonitorHistory(MonitorHistory model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Update<MonitorHistory>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InsertMonitorHistory(MonitorHistory model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Insert<MonitorHistory>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion


        #region 维修增删改查
        public void DeleteRepair(List<int> ids)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Repair.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { context.Repair.Remove(a); });
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<Repair> SelectAllRepair()
        {
            try
            {
                using (var context = new BJContext())
                {

                    return context.Repair.OrderByDescending(u => u.ID).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public Repair SelectRepair(int id)
        {
            try
            {
                using (var context = new BJContext())
                {
                    return context.Repair.Where(u => u.ID == id).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
        public void UpdateRepair(Repair model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Update<Repair>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InsertRepair(Repair model)
        {
            try
            {
                using (var context = new BJContext())
                {
                    context.Insert<Repair>(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion

    }
}
