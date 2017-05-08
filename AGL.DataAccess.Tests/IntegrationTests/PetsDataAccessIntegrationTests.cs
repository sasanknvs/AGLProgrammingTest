using AGL.Models.EntityModels;
using NUnit.Framework;
using RestSharp;
using System.Configuration;
using System.Linq;
using System.Net;

namespace AGL.DataAccess.Tests.IntegrationTests
{
    [TestFixture]
    public class PetsDataAccessIntegrationTests
    {
        private PetsDataAccess _petsDataAccess;

        [SetUp]
        public void Setup()
        {
            _petsDataAccess = new PetsDataAccess(new RestClient());
        }

        [Test]
        public void WhenAPIReturnsValidResponse_NoErrorsInResponse()
        {
            //Act
            OwnerPetsData ownerPetsData = _petsDataAccess.RetrievePets();

            // Assert
            Assert.IsTrue(ownerPetsData.Errors.Count == 0 && ownerPetsData.Owners.Count > 0);
        }

        [Test]
        public void WhenInvokedAPIWithInvalidResourceUri_ShouldReturn404NotFoundError()
        {
            // Arrange
            ConfigurationManager.AppSettings["BaseUri"] = "http://agl-developer-test.azurewebsites.net";
            ConfigurationManager.AppSettings["RetrievePetsUri"] = "/people.jso";  // Inject invalid resource uri
            _petsDataAccess = new PetsDataAccess(new RestClient());

            //Act
            OwnerPetsData ownerPetsData = _petsDataAccess.RetrievePets();
        
            // Assert
            Assert.IsTrue(ownerPetsData.Errors.Any(error => error.StatusCode == HttpStatusCode.NotFound
                                                            && !string.IsNullOrEmpty(error.ErrorMessage)));
        }

        [Test]
        public void WhenInvokedAPI_WithInvalidBaseUri_ShouldReturnError()
        {
            // Arrange
            ConfigurationManager.AppSettings["BaseUri"] = "http://agl-developer-test.azurewebsites.com"; // Invalid base uri
            ConfigurationManager.AppSettings["RetrievePetsUri"] = "/people.json";
            _petsDataAccess = new PetsDataAccess(new RestClient());
            //Act
            OwnerPetsData ownerPetsData = _petsDataAccess.RetrievePets();

            // Assert
            Assert.IsTrue(ownerPetsData.Errors.Any(error => error.StatusCode == 0
                                                            && !string.IsNullOrEmpty(error.ErrorMessage)));
        }

        [TearDown]
        public void Teardown()
        {
            _petsDataAccess = null;
            //Resetting the config parameters to the actual values
            ConfigurationManager.AppSettings["BaseUri"] = "http://agl-developer-test.azurewebsites.net"; 
            ConfigurationManager.AppSettings["RetrievePetsUri"] = "/people.json";
        }

    }
}
