using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.Models.TransactionModels
{
    public class PetsByGender
    {
        public string Gender { get; set; }
        public List<string> petsList { get; set; }
    }

}
