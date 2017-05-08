using AGL.BusinessLogic.Interfaces;
using AGL.Models.TransactionModels;
using AGLProgrammingTest.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AGL.MVC.Tests
{
    [TestFixture]
    public class PetsControllerTests
    {
        private Mock<IPetsBusinesslogic> _moqPetsBusinesslogic;

        [SetUp]
        public void Setup()
        {
            _moqPetsBusinesslogic = new Mock<IPetsBusinesslogic>();
        }

        [Test]
        public void WhenControllerCalled_verifyViewname()
        {
            //Arrange
            PetsByGenderResponse moqResponse = new PetsByGenderResponse()
            {
                PetsByGenderList=new List<PetsByGender>
                {
                    new PetsByGender()
                    {
                        Gender="Male",
                        petsList=new List<string> { "Max","Tom"}
                    },
                    new PetsByGender()
                    {
                        Gender="Female",
                        petsList=new List<string> { "Maxi"}
                    }
                }
            };

            PetsController controllerUnderTest = new PetsController(_moqPetsBusinesslogic.Object);
            _moqPetsBusinesslogic.Setup(m => m.RetreivePetsByType(It.IsAny<string>())).Returns(moqResponse);

            // Act
            var viewresult = controllerUnderTest.Get("Cat") as ViewResult;

            // Assert
            Assert.AreEqual("pets", viewresult.ViewName);
        }

        [Test]
        public void WhenValidResponseFromAPI_ValidateResponseModel()
        {
            //Arrange
            PetsByGenderResponse moqResponse = new PetsByGenderResponse()
            {
                PetsByGenderList = new List<PetsByGender>
                {
                    new PetsByGender()
                    {
                        Gender="Male",
                        petsList=new List<string> { "Max","Tom"}
                    },
                    new PetsByGender()
                    {
                        Gender="Female",
                        petsList=new List<string> { "Maxi"}
                    }
                }
            };

            PetsController controllerUnderTest = new PetsController(_moqPetsBusinesslogic.Object);
            _moqPetsBusinesslogic.Setup(m => m.RetreivePetsByType(It.IsAny<string>())).Returns(moqResponse);

            // Act
            var viewresult = controllerUnderTest.Get("Cat") as ViewResult;
            var responseModel = (PetsByGenderResponse)viewresult.Model;

            // Assert
            Assert.Greater(responseModel.PetsByGenderList.Count, 0);
        }

        [Test]
        public void WhenUnhandledExceptionOccursInController_ResponseContainsErrors()
        {
            //Arrange
            PetsController controllerUnderTest = new PetsController(_moqPetsBusinesslogic.Object);
            _moqPetsBusinesslogic.Setup(m => m.RetreivePetsByType(It.IsAny<string>())).Throws(new Exception("object reference not set to instance"));

            // Act
            var viewresult = controllerUnderTest.Get("Cat") as ViewResult;
            var responseModel = (PetsByGenderResponse)viewresult.Model;
            
            // Assert
            Assert.IsTrue(responseModel.Errors.Any(error => error.ErrorMessage == "object reference not set to instance"),"");
        }
    }
}
