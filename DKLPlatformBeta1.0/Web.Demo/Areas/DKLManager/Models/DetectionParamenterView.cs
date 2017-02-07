using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract.Model
{
   public class DetectionParamenterView
    {
       public DetectionParamenterView()
       {
           SampleRegister = new SampleRegisterTable();
           Param = new ArgumentValue();
           SampleHis = new SampleHistory();
           DetectionList = new List<SampleRegisterTable>();
           cookies = new Cookies();
           PhysicsModel = new TestBasicInfo();

       }
       public Cookies cookies;
       public SampleRegisterTable SampleRegister;
       public ArgumentValue Param;
       public SampleHistory SampleHis;
       public IList<SampleRegisterTable> DetectionList;
       public TestBasicInfo PhysicsModel;
    }
}
