using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("ProjectInfoHistory")]

    public class ProjectInfoHistory : ModelBase
    {
        public ProjectInfoHistory()
        {
            ProjectCategory = -1;
        }
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "不能为空")]

        public string CompaneName { get; set; }

        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string ContactTel { get; set; }
        public string ZipCode { get; set; }
        public int ProjectStatus { get; set; }
        private int projectCategory = -1;
        public int ProjectCategory
        {
            get { return projectCategory; }

            set { projectCategory = value; }
        }
        public DateTime ProjectClosingDate { get; set; }
        public DateTime ProjectRealClosingDate { get; set; }
        public string ProjectCheif { get; set; }//项目负责人
        public string ProjectLeader { get; set; }//项目组长
        public string ProjectPerson { get; set; }//组员



        public static ProjectInfoHistory Clone(ProjectInfo info)
        {
            ProjectInfoHistory history = new ProjectInfoHistory();
            history.ProjectNumber = info.ProjectNumber;
            history.ProjectName = info.ProjectName;
            history.CompaneName = info.CompaneName;
            history.CompanyAddress = info.CompanyAddress;
            history.CompanyContact = info.CompanyContact;
            history.ContactTel = info.ContactTel;
            history.ZipCode = info.ZipCode;
            history.ProjectStatus = info.ProjectStatus;
            history.ProjectCategory = info.ProjectCategory;
            history.ProjectClosingDate =(DateTime)info.ProjectClosingDate;
            history.ProjectCheif = info.ProjectCheif;
            history.ProjectLeader = info.ProjectLeader;
            history.ProjectPerson = info.ProjectPerson;
            history.CreateTime = info.CreateTime;

            return history;
        }

    }
}
