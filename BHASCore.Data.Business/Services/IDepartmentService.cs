using BHASCore.Data.Business.Entities;
//

namespace BHASCore.Data.Business.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAll();
    }
}
