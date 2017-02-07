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
    [System.ComponentModel.DataAnnotations.Schema.Table("ProjectFileHistory")]

    public class ProjectFileHistory : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
        public string FilePath { get; set; }

        public static ProjectFileHistory Clone(ProjectFile info)
        {
            ProjectFileHistory history = new ProjectFileHistory();
            history.ProjectNumber = info.ProjectNumber;
            history.FilePath = info.FilePath;
            history.CreateTime = info.CreateTime;
            return history;
        }

    }
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table("ProjectDocFileHistory")]

    public class ProjectDocFileHistory : ModelBase
    {
        [Key]
        public override int ID { set; get; }
        [Required(ErrorMessage = "不能为空")]
        public string ProjectNumber { get; set; }
        public string FilePath { get; set; }
        public string suggenstion { get; set; }
        public int Status { get; set; }

        public static ProjectDocFileHistory Clone(ProjectDocFile info)
        {
            ProjectDocFileHistory history = new ProjectDocFileHistory();
            history.ProjectNumber = info.ProjectNumber;
            history.FilePath = info.FilePath;
            history.suggenstion = info.suggenstion;
            history.Status = info.Status;
            history.CreateTime = info.CreateTime;
            return history;
        }

    }
}
