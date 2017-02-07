using HYZK.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
    public class CostingRequest : Request
    {
        private int costingState = -1;
        public string userName { get; set; }
        public int CostingState
        {
            get { return costingState; }
            set { costingState = value; }
        }
        private int userAccountType = -1;
        public int UserAccountType
        {
            get { return userAccountType; }
            set { userAccountType = value; }
        }
    }
}
