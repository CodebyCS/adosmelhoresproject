using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();

        void Add(Employee f);

        void ChangeCriminalRecord(int id, DateTime novaData);

        void ChangeContract(int id, DateTime novaData);

        List<Employee> GetValidContracts(DateTime dataAtual);

        List<Employee> GetCriminalRecordExpired(DateTime dataAtual);

        decimal CalculateTrainerPayment(int id, DateTime inicio, DateTime fim);
    }
}

