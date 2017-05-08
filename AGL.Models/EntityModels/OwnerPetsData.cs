using System.Collections.Generic;

namespace AGL.Models.EntityModels
{
    public class OwnerPetsData
    {
        private List<Error> _errors;
        private List<Owners> _owners;

        public OwnerPetsData()
        {
             _errors= new List<Error>(); 
             _owners = new List<Owners>();
        }

        public List<Error> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        public List<Owners> Owners
        {
            get { return _owners; }
            set { _owners = value; }
        }
    }
}
