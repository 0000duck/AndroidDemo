using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Demo.Areas.DKLManager.Models
{
    public class ArgumentVlueView
    {
       public ArgumentVlueView()
       {
           AnalyzePer = new AnalyzePerson();
           ArgumentHis = new ArgumentValueHistory();

       }
       public AnalyzePerson AnalyzePer;
       public ArgumentValueHistory ArgumentHis;


    }
}