using AGL.Models.TransactionModels;

namespace AGL.BusinessLogic.Interfaces
{
    public interface IPetsBusinesslogic
    {
        PetsByGenderResponse RetreivePetsByType(string petType);
    }
}
