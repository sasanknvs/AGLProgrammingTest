using AGL.BusinessLogic.Interfaces;
using AGL.DataAccess.Interfaces;
using AGL.Models.EntityModels;
using AGL.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AGL.BusinessLogic
{
    /// <summary>
    /// Business logic layer
    /// </summary>
    public class PetsBusinesslogic : IPetsBusinesslogic
    {
        private IPetsDataAccess _petsDataAccess;

        public PetsBusinesslogic(IPetsDataAccess petsDataAccess)
        {
            if (petsDataAccess == null)
                throw new ArgumentException();
            _petsDataAccess = petsDataAccess;
        }


        public PetsByGenderResponse RetreivePetsByType(string petType)
        {
            PetsByGenderResponse petsByGenderResponse = new PetsByGenderResponse();
            List<PetsByGender> petsByGenderList = null;

            try
            {
                // Get data from Data Access
                OwnerPetsData ownerPetsData = _petsDataAccess.RetrievePets();

                if (ownerPetsData != null && ownerPetsData?.Errors.Count == 0)
                {
                    // Group pets by Owner gender
                    Dictionary<string, List<List<Pet>>> petsGroupByOwnerGender = GroupPetsByOwnerGender(ownerPetsData);

                    //Flatten pets under gender
                    Dictionary<string, List<Pet>> flattenedPetsList = FlattenPetsByGenderList(petsGroupByOwnerGender);

                    // Filter and sort pets by name
                    petsByGenderList = FilterPetsByTypeandSortByName(flattenedPetsList, petType);

                    petsByGenderResponse.PetsByGenderList = petsByGenderList;
                }
                else
                {
                    // Map the errors to the response
                    petsByGenderResponse.Errors = ownerPetsData.Errors;
                }
                
            }
            catch (Exception exception)
            {
                petsByGenderResponse.Errors.Add(new Error { ErrorMessage = exception.Message });
            }
            return petsByGenderResponse;
        }

        /// <summary>
        /// Group pets by Owner's gender
        /// </summary>
        /// <param name="ownerPetsData"></param>
        /// <returns></returns>
        public Dictionary<string,List<List<Pet>>> GroupPetsByOwnerGender(OwnerPetsData ownerPetsData)
        {
            Dictionary<string, List<List<Pet>>> ownerpetsDic = new Dictionary<string, List<List<Pet>>>();
            var ownerGroupedByGender = (from owner in ownerPetsData.Owners
                                        where owner.Pets !=null
                                        group owner.Pets by owner.Gender into petsGrp 
                                        select new { Gender = petsGrp.Key, Pets = petsGrp.ToList() });

            foreach (var genderGrp in ownerGroupedByGender)
            {
                ownerpetsDic.Add(genderGrp.Gender, genderGrp.Pets);
            }

            return ownerpetsDic;
        }

        /// <summary>
        /// Flatten the list of pets of different owners
        /// </summary>
        /// <param name="petsGroups"></param>
        /// <returns></returns>
        public Dictionary<string, List<Pet>> FlattenPetsByGenderList(Dictionary<string, List<List<Pet>>> petsGroups)
        {
            Dictionary<string, List<Pet>> petsByGender = new Dictionary<string, List<Pet>>();

            foreach (string gender in petsGroups.Keys)
            {
                List<Pet> flattenedpetsList = new List<Pet>();
                foreach (var petlist in petsGroups[gender])
                {
                    flattenedpetsList.AddRange(petlist);
                }
                petsByGender.Add(gender, flattenedpetsList);
            }
            return petsByGender;
        }

        /// <summary>
        /// Filter the list of pets by pet type
        /// </summary>
        /// <param name="petsGroups"></param>
        /// <param name="petType"></param>
        /// <returns></returns>
        public List<PetsByGender> FilterPetsByTypeandSortByName(Dictionary<string, List<Pet>> petsGroups, string petType)
        {
            List<PetsByGender> petsByGenderList = new List<PetsByGender>();
            foreach (string gender in petsGroups.Keys)
            {
                PetsByGender petsByGender = new PetsByGender();
                petsByGender.Gender = gender;
                petsByGender.petsList = petsGroups[gender].Where(pet => string.Equals(pet.Type,petType,StringComparison.InvariantCultureIgnoreCase))
                                                           .Select(pet => pet.Name)
                                                           .ToList();
                petsByGender.petsList.Sort();

                petsByGenderList.Add(petsByGender);
            }
            return petsByGenderList;
        }
    }
       
}
