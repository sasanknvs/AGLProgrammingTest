using AGL.DataAccess.Interfaces;
using AGL.Models.EntityModels;
using AGL.Models.TransactionModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AGL.BusinessLogic.Tests
{
    [TestFixture]
    public class PetsBusinessLogicTests
    {
        private Mock<IPetsDataAccess> _moqPetsDataAccess;
        private PetsBusinesslogic _petsBusinessLogic;

        [SetUp]
        public void SetUp()
        {
            _moqPetsDataAccess = new Mock<IPetsDataAccess>();
            _petsBusinessLogic = new PetsBusinesslogic(_moqPetsDataAccess.Object);
        }

        [Test]
        public void WhenPassedOwnerandPets_ReturnedPetsGroupedByGender()
        {
            // Arrange
            OwnerPetsData ownerPetsData = new OwnerPetsData();
            ownerPetsData.Owners = new List<Owners>()
            {
                new Owners()
                {
                    age=20,
                    gender="Male",
                    name="John",
                    pets=new List<Pet>()
                    {
                        new Pet() { name="Tom",type="Cat" },
                        new Pet() { name="Garfield",type="Dog"}
                    }
                },
                new Owners()
                {
                    age=30,
                    gender="Female",
                    name="John",
                    pets=new List<Pet>()
                    {
                        new Pet() { name="Tomy",type="Cat" },
                    }
                },
                new Owners()
                {
                    age=20,
                    gender="Male",
                    name="Johnny",
                    pets=new List<Pet>()
                    {
                        new Pet() { name="Fido",type="Cat" },
                    }
                }
            };

            // Act
            Dictionary<string, List<List<Pet>>> petsGroups= _petsBusinessLogic.GroupPetsByOwnerGender(ownerPetsData);

            // Assert
            Assert.IsNotNull(petsGroups);
            Assert.AreEqual(2, petsGroups.Count);
            Assert.AreEqual(petsGroups["Male"].Count, 2);
            Assert.AreEqual(petsGroups["Female"].Count, 1);
        }

        [Test]
        public void FlattenPetsByGenderList_WhenPassedPetGroupedByGender_ReturnFalttenedPetsList()
        {
            // Arrange
            Dictionary<string, List<List<Pet>>> petsGroups = new Dictionary<string, List<List<Pet>>>();

            petsGroups.Add("Male", new List<List<Pet>>()
                            {
                                new List<Pet>
                                {
                                    new Pet() { name="Fido",type="Cat" },
                                    new Pet() { name="Fidi",type="Dog" },
                                },
                                new List<Pet>
                                {
                                    new Pet() { name="Tom",type="Cat" },
                                    new Pet() { name="Garfield",type="Dog"}
                                },
                                new List<Pet>
                                {
                                    new Pet() { name="Glen",type="Dog"},
                                    new Pet() { name="ceasar",type="Dog"}
                                }
                            }
                            );

            petsGroups.Add("Female", new List<List<Pet>>()
                            {
                                new List<Pet>
                                {
                                    new Pet() { name="Max",type="Cat" },
                                },
                                new List<Pet>
                                {
                                    new Pet() { name="Tom",type="Cat" },
                                },
                                new List<Pet>
                                {
                                    new Pet() { name="Alice",type="Dog"}
                                }
                            }
                            );

            Dictionary<string, List<Pet>> petsFilteredByCat = _petsBusinessLogic.FlattenPetsByGenderList(petsGroups);

            Assert.IsNotNull(petsFilteredByCat);
            Assert.AreEqual(6, petsFilteredByCat["Male"].Count);
            Assert.AreEqual(3, petsFilteredByCat["Female"].Count);
        }

        [Test]
        public void FilterPetsByType_WhenPassedPetGroupedByGender_ReturnOnlyCats()
        {
            // Arrange
            Dictionary<string, List<Pet>> petsGroups = new Dictionary<string, List<Pet>>();

            petsGroups.Add("Male",
                                new List<Pet>
                                {
                                    new Pet() { name="Tom",type="Cat" },
                                    new Pet() { name="Garfield",type="Dog"},
                                    new Pet() { name="Glen",type="Dog"},
                                    new Pet() { name="ceasar",type="Dog"},
                                    new Pet() { name="Fido",type="Cat" },
                                    new Pet() { name="Fidi",type="Dog" },
                                }
                            );

            petsGroups.Add("Female", new List<Pet>
                                {
                                    new Pet() { name="Max",type="Cat" },
                                    new Pet() { name="Tom",type="Cat" },
                                    new Pet() { name="Alice",type="Dog"}
                                }
                            );

            List<PetsByGender> PetsByGenderList = _petsBusinessLogic.FilterPetsByTypeandSortByName(petsGroups,"Cat");

            Assert.IsNotNull(PetsByGenderList);
            Assert.AreEqual(2, PetsByGenderList[0].petsList.Count);
            Assert.AreEqual(2, PetsByGenderList[1].petsList.Count);
        }

        [Test]
        public void RetrievePetByType_WhenCalled_ReturnCatsSortedByName()
        {
            // Arrange
            OwnerPetsData ownerPetsData = new OwnerPetsData();
            ownerPetsData.Owners = new List<Owners>()
            {
                new Owners()
                {
                    age=20,
                    gender="Male",
                    name="John",
                    pets=new List<Pet>()
                    {
                        new Pet() { name="Tom",type="Cat" },
                        new Pet() { name="Garfield",type="Dog"}
                    }
                },
                new Owners()
                {
                    age=30,
                    gender="Female",
                    name="John",
                    pets=new List<Pet>()
                    {
                        new Pet() { name="Tomy",type="Cat" },
                    }
                },
                new Owners()
                {
                    age=20,
                    gender="Male",
                    name="Johnny",
                    pets=new List<Pet>()
                    {
                        new Pet() { name="Fido",type="Cat" },
                    }
                }
            };
            _moqPetsDataAccess.Setup(m => m.RetrievePets()).Returns(ownerPetsData);

            // Act
            PetsByGenderResponse petsByGender = _petsBusinessLogic.RetreivePetsByType("Cat");

            //Assert
            Assert.IsNotNull(petsByGender);
            Assert.AreEqual(2, petsByGender.PetsByGenderList.Count);
            Assert.AreEqual("Male", petsByGender.PetsByGenderList[0].Gender);
            Assert.AreEqual(2, petsByGender.PetsByGenderList[0].petsList.Count);
            Assert.AreEqual("Female", petsByGender.PetsByGenderList[1].Gender);
            Assert.AreEqual(1, petsByGender.PetsByGenderList[1].petsList.Count);
        }

        [Test]
        public void RetrievePetByType_WhenErrorOccurredinDataAccess_ReturnErrorsInResponse()
        {
            // Arrange
            OwnerPetsData ownerPetsData = new OwnerPetsData
                                         {
                                            Errors = new List<Error>
                                            {
                                                new Error()
                                                {
                                                    ErrorMessage = "The remote name could not be resolved: 'agl-developer-test.azurewebsites.net'",
                                                    StatusCode = 0
                                                }
                                            }
                                        };
            
            _moqPetsDataAccess.Setup(m => m.RetrievePets()).Returns(ownerPetsData);

            // Act
            PetsByGenderResponse petsByGender = _petsBusinessLogic.RetreivePetsByType("Cat");

            //Assert
            Assert.IsNotNull(petsByGender);
            Assert.IsTrue(petsByGender.Errors.Count > 0 && petsByGender.Errors.Any(error => error.ErrorMessage == "The remote name could not be resolved: 'agl-developer-test.azurewebsites.net'"));
        }
    }
}
