using System.Collections.Generic;

namespace AGL.Models.EntityModels
{
    public class Owners
    {
        public int age { get; set; }

        public string name { get; set; }

        public string gender { get; set; }

        public List<Pet> pets{ get; set; }
    }
}
