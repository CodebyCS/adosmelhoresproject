using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Models;
using Microsoft.AspNetCore.Hosting;

namespace adosmelhoresproject.src.Services
{
    public class EmployeeService : JsonRepository<Employee>, IEmployeeService
    {
        public EmployeeService(IWebHostEnvironment env)
            : base(env, "funcionarios.json") { }

        public List<Employee> GetAll() => ReadAll();

        public void Add(Employee f)
        {
            var lista = ReadAll();
            f.Id = lista.Any() ? lista.Max(e => e.Id) + 1 : 1;
            lista.Add(f);
            WriteAll(lista);
        }

        public void Update(Employee fAtualizado)
        {
            var lista = ReadAll();
            var idx = lista.FindIndex(e => e.Id == fAtualizado.Id);
            if (idx != -1)
            {
                lista[idx] = fAtualizado;
                WriteAll(lista);
            }
        }

        public void ChangeCriminalRecord(int id, DateTime newDate)
        {
            var lista = ReadAll();
            var emp = lista.FirstOrDefault(f => f.Id == id);
            if (emp != null) { emp.CriminalRecordDate = newDate; WriteAll(lista); }
        }

        public void ChangeContract(int id, DateTime newDate)
        {
            var lista = ReadAll();
            var emp = lista.FirstOrDefault(f => f.Id == id);
            if (emp != null) { emp.ContractEndDate = newDate; WriteAll(lista); }
        }

        public List<Employee> GetValidContracts(DateTime date) =>
            ReadAll().Where(f => f.ContractEndDate >= date).ToList();

        public List<Employee> GetCriminalRecordExpired(DateTime date) =>
            ReadAll().Where(f => f.CriminalRecordDate < date).ToList();

        public decimal CalculateTrainerPayment(int id, DateTime inicio, DateTime fim)
        {
            var emp = ReadAll().FirstOrDefault(f => f.Id == id);
            if (emp is Trainer t)
            {
                int days = (fim.Date - inicio.Date).Days + 1;
                return days > 0 ? days * 6 * t.HourlyRate : 0;
            }
            return 0;
        }
    }
}