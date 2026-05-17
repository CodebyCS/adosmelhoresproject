using System.Text.Json.Serialization;

namespace adosmelhoresproject.src.Models

{
    [JsonDerivedType(typeof(Director), "Diretor")]
    [JsonDerivedType(typeof(Coordinator), "Coordenador")]
    [JsonDerivedType(typeof(Trainer), "Formador")]
    [JsonDerivedType(typeof(Secretary), "Secretaria")]

    public abstract class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Contact { get; set; }
        public DateTime CriminalRecordDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public DateTime ContractStartDate { get; set; }
        public decimal Salary { get; set; }
        public bool Active { get; set; }
        
        public abstract decimal CalculatePayment(DateTime startDate, DateTime endDate);
    }
}