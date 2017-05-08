using AGL.DataAccess.Interfaces;
using AGL.Models.EntityModels;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AGL.DataAccess.Tests
{
    [TestFixture]
    public class PetsDataAccessTests
    {
        private IPetsDataAccess _petsDataAccess;
        private Mock<IRestClient> _moqClient;
        private static string _resourceUri = "/people.json";

        [SetUp]
        public void Setup()
        {
            _moqClient = new Mock<IRestClient>();
            _petsDataAccess = new PetsDataAccess(_moqClient.Object);
        }

        [Test]
        public void WhenConstructorCreated_Passednullparameter_ThrowsArgumentNullException()
        {
            try
            {
                IPetsDataAccess petsDataAccess = new PetsDataAccess(null);
            }
            catch (ArgumentNullException)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void WhenCalled_ApiReturnsValidResponse_OwnerPetsDataIsNotNull()
        {
            // Arrange
            string resource = string.Empty;
            var Owners = new List<Owners>()
            {
                    new Owners()
                    {
                        Age=20,
                        Gender ="Male",
                        Name ="Sasank",
                        Pets =new List<Pet>() { new Pet() { Name="Tommy",Type ="Dog"} }
                    }
            };
            
            var restResponse = new RestResponse<List<Owners>>()
            {
                Data = Owners,
                StatusCode = HttpStatusCode.OK
            };

            _moqClient.Setup(m => m.Execute<List<Owners>>(It.IsAny<IRestRequest>())).Returns(restResponse).
                Callback<IRestRequest>(rq => { resource = rq.Resource; });

            //Act
            OwnerPetsData ownerPetsData = _petsDataAccess.RetrievePets();

            // Assert
            Assert.AreEqual(0, ownerPetsData.Errors.Count);
        }

        [Test]
        public void WhenCalled_ApiReturnsValidResponse_ValidateValidIfResourceUriIsCalled()
        {
            // Arrange
            string resource = string.Empty;
            var Owners = new List<Owners>()
            {
                    new Owners()
                    {
                        Age=20,
                        Gender ="Male",
                        Name ="Sasank",
                        Pets =new List<Pet>() { new Pet() { Name="Tommy",Type ="Dog"} }
                    }
            };

            var restResponse = new RestResponse<List<Owners>>()
            {
                Data = Owners,
                StatusCode = HttpStatusCode.OK
            };

            _moqClient.Setup(m => m.Execute<List<Owners>>(It.IsAny<IRestRequest>())).Returns(restResponse).
                Callback<IRestRequest>(rq => { resource = rq.Resource; });

            //Act
            OwnerPetsData ownerPetsData = _petsDataAccess.RetrievePets();

            // Assert
            Assert.AreEqual(_resourceUri, resource);
        }

        [TestCase(HttpStatusCode.NotFound,"Resource not found")]
        [TestCase(HttpStatusCode.InternalServerError, "The remote server returned an error")]
        [TestCase(HttpStatusCode.RequestTimeout, "The request timed out")]
        public void WhenCalled_APIRespondsOtherthanSuccess_ReturnsErrorsinResponse(HttpStatusCode statusCode,string errorMessage)
        {
            // Arrange
            var restResponse = new RestResponse<List<Owners>>()
            {
                StatusCode = statusCode,
                ErrorException = new Exception(errorMessage)
            };

            _moqClient.Setup(m => m.Execute<List<Owners>>(It.IsAny<IRestRequest>())).Returns(restResponse);

            //Act
            OwnerPetsData ownerPetsData = _petsDataAccess.RetrievePets();

            // Assert
            Assert.IsTrue(ownerPetsData.Errors.Any(error => error.StatusCode == statusCode && error.ErrorMessage == errorMessage));
        }

        [Test]
        public void WhenCalled_UndhandledExceptionOccurs_ThrowsException()
        {
            // Arrange
            _moqClient.Setup(m => m.Execute<List<Owners>>(It.IsAny<IRestRequest>())).Throws(new NullReferenceException("Object reference not set to an instance"));

            //Act
            OwnerPetsData ownerPetsData = _petsDataAccess.RetrievePets();


            //Assert
            Assert.IsTrue(ownerPetsData.Errors.Any(e => e.ErrorMessage == "Object reference not set to an instance"));
        }


        [TearDown]
        public void Teardown()
        {
            _moqClient = null;
            _petsDataAccess = null;
        }
    }
}
