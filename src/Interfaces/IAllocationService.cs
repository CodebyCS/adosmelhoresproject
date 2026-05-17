using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Interfaces
{
    public interface IAllocationService
    {
        List<Allocation> GetAll();
        void Add(Allocation a);
    }
}
