using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Demo.Areas.DKLManager.Models
{
    public class SampleTableHistoryViewModel
    {
        public SampleTableHistoryViewModel()
        {
             AnalyzePer = new AnalyzePerson();
            ArgumentHis = new ArgumentValueHistory();
            sampleReg = new SampleRegisterTable();
            sampleHis = new SampleHistory();
        }
        public SampleRegisterTable sampleReg;
        public SampleHistory sampleHis;
       public AnalyzePerson AnalyzePer;
       public ArgumentValueHistory ArgumentHis;
    }
}