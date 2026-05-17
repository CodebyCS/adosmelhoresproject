using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Services
{
    public class AllocationService : JsonRepository<Allocation>, IAllocationService
    {
        public AllocationService(IWebHostEnvironment env)
            : base(env, "alocacoes.json") { }

        public List<Allocation> GetAll() => ReadAll();

        public void Add(Allocation a)
        {
            var list = ReadAll();
            list.Add(a);
            WriteAll(list);
        }
    }
}
