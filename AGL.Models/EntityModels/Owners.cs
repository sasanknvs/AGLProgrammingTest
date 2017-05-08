using System.Collections.Generic;
using RestSharp.Serializers;

namespace AGL.Models.EntityModels
{
    public class Owners
    {
        [SerializeAs(Name ="age")]
        public int Age { get; set; }

        [SerializeAs(Name = "name")]
        public string Name { get; set; }

        [SerializeAs(Name = "gender")]
        public string Gender { get; set; }

        [SerializeAs(Name = "pets")]
        public List<Pet> Pets{ get; set; }
    }
}
