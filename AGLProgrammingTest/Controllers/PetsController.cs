using AGL.BusinessLogic.Interfaces;
using AGL.Models.TransactionModels;
using System;
using System.Web.Mvc;

namespace AGLProgrammingTest.Controllers
{
    //add exception filter
    public class PetsController : Controller
    {
        private IPetsBusinesslogic _petsBusinesslogic;
        public PetsController(IPetsBusinesslogic petsBusinesslogic)
        {
            _petsBusinesslogic = petsBusinesslogic;
        }

        // GET: Pets
        [Route("{petType=Cat}")]
        [Route("Pets/{petType=Cat}")]
        public ActionResult Get(string petType)
        {
            PetsByGenderResponse petsByGenderResponse = new PetsByGenderResponse();
            try
            {
                petsByGenderResponse = _petsBusinesslogic.RetreivePetsByType(petType);
            }
            catch (Exception exception)
            {
                petsByGenderResponse.Errors.Add(new AGL.Models.EntityModels.Error{ ErrorMessage = exception.Message });
            }
            return View("pets", petsByGenderResponse);
        }
    }
}