using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Demo.Areas.DKLManager.Models
{
    public class CreateNew
    {
        public CreateNew()
        {
            Costings = new Costing();
            ProjectContracts = new ProjectContract();

        }
        public Costing Costings;
        public ProjectContract ProjectContracts;


    }
}