using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HYZK.FrameWork.DAL;
using DKLManager.Contract.Model;
using DKLManager.Contract;
using HYZK.Core.Config;
using HYZK.Core.Cache;
using DKLManager.Dal;



namespace DKLManager.Bll
{
    public class DKLManagerSevice:IDKLManager
    {
        public IList<DeviceStateModel> GetStateList()
        {
            using (var dbContext = new DKLManagerDbContext())
           {
               return dbContext.FindAll<DeviceStateModel>();
            }
        }

        public DeviceStateModel Select(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceState.Where(u => u.ID == id).SingleOrDefault();
            }
        }
        public void InsertState(DeviceStateModel state)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<DeviceStateModel>(state);
            }
        }
        public void DeleteState(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.DeviceState.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.DeviceState.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void UpDateState(DeviceStateModel state)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<DeviceStateModel>(state);
            }
        }

        public ConsultBasicInfo GetConsultBasicInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ConsultBasicInfo.Where(c => c.ID == id).SingleOrDefault();
            }
        }
        public IEnumerable<ConsultBasicInfo> GetConsultBasicInfoList()
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.FindAll<ConsultBasicInfo>();
            }
        }
        public void AddConsultBasicInfo(ConsultBasicInfo role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<ConsultBasicInfo>(role);
            }
        }
        public void UpdateConsultBasicInfo(ConsultBasicInfo role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<ConsultBasicInfo>(role);
            }
        }
        public void DeleteConsultBasicInfo(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ConsultBasicInfo.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ConsultBasicInfo.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public ProjectFile GetProjectFile(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectFile.Where(c => c.ID == id).SingleOrDefault();
            }
        }
        public IEnumerable<ProjectFile> GetProjectFileList()
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.FindAll<ProjectFile>();
            }
        }
        public void AddProjectFile(ProjectFile role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<ProjectFile>(role);
            }
        }
        public void UpdateProjectFile(ProjectFile role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<ProjectFile>(role);
            }
        }
        public void DeleteProjectFile(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectFile.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ProjectFile.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public ProjectInfo GetProjectInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectInfo.Where(c => c.ID == id).SingleOrDefault();
            }
        }
        public IEnumerable<ProjectInfo> GetProjectInfoList()
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.FindAll<ProjectInfo>();
            }
        }
        public void AddProjectInfo(ProjectInfo role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<ProjectInfo>(role);
            }
        }
        public void UpdateProjectInfo(ProjectInfo role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<ProjectInfo>(role);
            }
        }
        public void DeleteProjectInfo(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectInfo.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ProjectInfo.Remove(a); });
                dbContext.SaveChanges();
            }
        }


        public ValueProjectInfo GetVlaueProjectInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ValueProjectInfo.Where(c => c.ID == id).SingleOrDefault();
            }
        }
        public IEnumerable<ValueProjectInfo> GetVlaueProjectInfoList()
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.FindAll<ValueProjectInfo>();
            }
        }
        public void AddVlaueProjectInfo(ValueProjectInfo role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<ValueProjectInfo>(role);
            }
        }
        public void UpdateVlaueProjectInfo(ValueProjectInfo role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<ValueProjectInfo>(role);
            }
        }
        public void DeleteVlaueProjectInfo(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ValueProjectInfo.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ValueProjectInfo.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public IEnumerable<ValueProjectInfo> GetVlaueProjectInfoList()
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.FindAll<ValueProjectInfo>();
            }
        }
        public void AddVlaueProjectInfo(ValueProjectInfo role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<ValueProjectInfo>(role);
            }
        }
        public void UpdateVlaueProjectInfo(ValueProjectInfo role)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<ValueProjectInfo>(role);
            }
        }
        public void DeleteVlaueProjectInfo(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ValueProjectInfo.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ValueProjectInfo.Remove(a); });
                dbContext.SaveChanges();
            }
        }

    }

}
