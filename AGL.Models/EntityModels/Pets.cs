using RestSharp.Serializers;

namespace AGL.Models.EntityModels
{
    public class Pet
    {
        [SerializeAs(Name = "name")]
        public string Name { get; set; }

        [SerializeAs(Name = "name")]
        public string Type { get; set; }
    }
}
