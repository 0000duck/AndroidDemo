using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
using HYZK.FrameWork.Common;

namespace Account.Contract
{
    [Table("User")]
    public class User : ModelBase
    {
        [Key]
        public override int ID { get; set; }                
                     
        [Required]
        [StringLength(100)]                                                
        public string LoginName { get; set; }  //登录名                             
       
        [Required]
        [StringLength(100)]                                                
        public string Psw { get; set; }       //密码
                                        
        [StringLength(100)]  
        public string Name { get; set; }           ///姓名

        public int Gender { get; set; }       //性别
        public enum EnumGender                
        {
          [EnumTitle("男")]
            Man = 1,

          [EnumTitle("女")]
            Woman = 2
        }

        public string Tel { get; set; }        //电话                                                                   
        [StringLength(100)]
        public string Email { get; set; }      //电子邮件
                          
        [StringLength(100)]
        public string Company { get; set; }	   //所属公司
        public int AccountType { get; set; }
        public enum EnumAccountType            //用户类型
        {
            [EnumTitle("普通用户")]
            User = 0,

            [EnumTitle("会员")]
            Vip = 1,

            [EnumTitle("管理员")]
            Manager = 2,
        }     
    }    
}
