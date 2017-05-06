using AGL.Models.EntityModels;
using System.Collections.Generic;

namespace AGL.Models.TransactionModels
{
    public class PetsByGenderResponse
    {
        private List<Error> _errors;
        private List<PetsByGender> _petsByGenderList;

        public PetsByGenderResponse()
        {
            _errors = new List<Error>();
            _petsByGenderList = new List<PetsByGender>();
        }

        public List<PetsByGender> PetsByGenderList
        {
            get { return _petsByGenderList; }
            set { _petsByGenderList = value; }
        }

        public List<Error> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }
    }
}
