using AGL.Models.EntityModels;

namespace AGL.DataAccess.Interfaces
{
    public interface IPetsDataAccess
    {
        OwnerPetsData RetrievePets();
    }
}
