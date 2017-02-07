using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class EduPayInfo
    {
        public EduPayInfo() {
            id = Guid.Empty;
            company = string.Empty;
            idcard = string.Empty;
            territory = string.Empty;
            payment = string.Empty;
            status = string.Empty;
        }
        private Guid id;
        private string company;//企业
        private string idcard;//身份证号
        private string territory;//所属区域
        private int createdTime;
        private string payment;//支付类型{none，alipay，tenpay，coin，wxpay}
        private string status;//支付状态{created，paid，refunding，refunded，cancelled}
        private DateTime createTime;
        public Guid Id
        {
            set { id = value; }
            get { return id; }
        }
        public string Company
        {
            set { company = value; }
            get { return company; }
        }
        public string Territory
        {
            set { territory = value; }
            get { return territory; }
        }
        public string Idcard
        {
            set { idcard = value; }
            get { return idcard; }
        }
        public int CreatedTime
        {
            set { createdTime = value; }
            get { return createdTime; }
        }
        public string Payment
        {
            set { payment = value; }
            get { return payment; }
        }
        public string Status
        {
            set { status = value; }
            get { return status; }
        }
        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
    }
}
