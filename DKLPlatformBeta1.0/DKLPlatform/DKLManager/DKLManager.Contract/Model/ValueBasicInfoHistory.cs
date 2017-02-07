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
    [System.ComponentModel.DataAnnotations.Schema.Table("ValueBasicInfoHistory")]

    public class ValueBasicInfoHistory : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }

        public static ValueBasicInfoHistory Clone(ValueBasicInfo info)
        {
            ValueBasicInfoHistory history = new ValueBasicInfoHistory();
            history.ProjectNumber = info.ProjectNumber;
            //history.ProjectName = info.ProjectName;
            //history.CompaneName = info.CompaneName;
            //history.CompanyAddress = info.CompanyAddress;
            //history.CompanyContact = info.CompanyContact;
            //history.ContactTel = info.ContactTel;
            //history.ZipCode = info.ZipCode;
            //history.ProjectStatus = info.ProjectStatus;
            //history.ProjectCategory = info.ProjectCategory;
            //history.ProjectClosingDate = info.ProjectClosingDate;
            //history.ProjectCheif = info.ProjectCheif;
            //history.ProjectLeader = info.ProjectLeader;
            //history.ProjectPerson = info.ProjectPerson;
            //history.CreateTime = info.CreateTime;

            return history;
        }
    }
}
