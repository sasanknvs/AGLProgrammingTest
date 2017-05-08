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

        [TestCase("Male")]
        [TestCase("Female")]
        public void GroupPetsByGender_WhenOnlyOneGenderHasPets_ReturnPetsGroupedOnlyForThatGender(string gender)
        {
            // Arrange
            OwnerPetsData ownerPetsData = new OwnerPetsData();
            ownerPetsData.Owners = new List<Owners>()
            {
                new Owners()
                {
                    age=20,
                    gender=gender,
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
                    gender=gender,
                    name="John",
                    pets=new List<Pet>()
                    {
                        new Pet() { name="Tomy",type="Cat" },
                    }
                },
                new Owners()
                {
                    age=20,
                    gender=gender,
                    name="Johnny",
                    pets=new List<Pet>()
                    {
                        new Pet() { name="Fido",type="Cat" },
                    }
                }
            };

            // Act
            Dictionary<string, List<List<Pet>>> petsGroups = _petsBusinessLogic.GroupPetsByOwnerGender(ownerPetsData);

            // Assert
            Assert.IsTrue(petsGroups.Keys.Count == 1 && petsGroups.ContainsKey(gender));
        }

        [Test]
        public void GroupPetsByGender_WhenSingleOwnerHasNoPets_OwnerWithoutPetsIgnoredWhileGrouping()
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
                    pets=null
                }
            };

            // Act
            Dictionary<string, List<List<Pet>>> petsGroups = _petsBusinessLogic.GroupPetsByOwnerGender(ownerPetsData);

            // Assert
            Assert.AreEqual(1, petsGroups["Male"].Count,"Owner count does not match");
        }

        [Test]
        public void GroupPetsByGender_WhenNoOwnerHasPets_ShouldNotContainAnyPetsInResponse()
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
                    pets=null
                },
                new Owners()
                {
                    age=30,
                    gender="Female",
                    name="John",
                    pets=null
                },
                new Owners()
                {
                    age=20,
                    gender="Male",
                    name="Johnny",
                    pets=null
                }
            };

            // Act
            Dictionary<string, List<List<Pet>>> petsGroups = _petsBusinessLogic.GroupPetsByOwnerGender(ownerPetsData);

            // Assert
            Assert.AreEqual(0, petsGroups.Count, "Expected count of Owners is not Zero");
        }

        [Test]
        public void GroupPetsByGender_WhenBothOwnersHasPets_ShouldReturnPetsOfBothOwners()
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
            Dictionary<string, List<List<Pet>>> petsGroups = _petsBusinessLogic.GroupPetsByOwnerGender(ownerPetsData);

            // Assert
            Assert.IsTrue(petsGroups.Keys.Count == 2 && petsGroups.ContainsKey("Male") && petsGroups.ContainsKey("Female"),"Male Or Female Owner are not present in group");
        }

        [TestCase("Male",2)]
        [TestCase("Female", 1)]
        public void GroupPetsByGender_WhenPassedPetsOfBothGender_ReturnedPetsGroupedByMaleAndFemaleWithProperCount(string gender,int ownerCountByGender)
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
            Dictionary<string, List<List<Pet>>> petsGroups = _petsBusinessLogic.GroupPetsByOwnerGender(ownerPetsData);

            // Assert
            Assert.AreEqual(petsGroups[gender].Count, ownerCountByGender, string.Format("Invalid number of {0} Owner(s) in the result",gender));
        }

        [Test]
        public void FlattenPetsByGenderList_WhenPassedPetsGroupedByGender_ReturnDictionaryWithCount2()
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
            // Act
            Dictionary<string, List<Pet>> petsFilteredByCat = _petsBusinessLogic.FlattenPetsByGenderList(petsGroups);

            // Assert
            Assert.AreEqual(petsFilteredByCat.Count, 2);
        }

        [TestCase("Male",6)]
        [TestCase("Female", 3)]
        public void FlattenPetsByGender_WhenPassedPetGroupedByGender_ReturnListWithValidCountofPetsUnderEachGender(string gender,int petsCountByGender)
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

            // Act
            Dictionary<string, List<Pet>> petsFilteredByCat = _petsBusinessLogic.FlattenPetsByGenderList(petsGroups);

            //Assert
            Assert.AreEqual(petsCountByGender, petsFilteredByCat[gender].Count, string.Format("Invalid pets count for {0} Owner", gender));
        }

        [TestCase("Male","Cat",2)]
        [TestCase("Male","Dog",4)]
        [TestCase("Female", "Cat", 2)]
        [TestCase("Female", "Dog", 1)]
        public void FilterPetsByPetType_WhenPassedPetGroupedByGender_ReturnValidNumberOfPetsByGender(string genderOfOwner,string petType,int petCount)
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

            //Act
            List<PetsByGender> PetsByGenderList = _petsBusinessLogic.FilterPetsByTypeandSortByName(petsGroups,petType);

            // Assert
            Assert.AreEqual(petCount, PetsByGenderList.Where(p => p.Gender == genderOfOwner).First().petsList.Count,
                            string.Format("Test failed for Gender : {0},PetType: {1},PetCount: {2}",genderOfOwner,petType,petCount));
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
            Assert.IsTrue(petsByGender.Errors.Any(error => error.ErrorMessage == "The remote name could not be resolved: 'agl-developer-test.azurewebsites.net'"));
        }
    }
}
