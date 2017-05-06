using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

    public class Error
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
