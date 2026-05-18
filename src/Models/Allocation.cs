namespace adosmelhoresproject.src.Models
{
    public class Allocation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString()[..8]; 
        public DateTime DataInicio { get; set; } = DateTime.Now;
        public DateTime DataFim { get; set; } = DateTime.Now.AddHours(4);
        public string NomeFormacao { get; set; }
        public int HorasPorDia { get; set; } = 6;

        public decimal ValorReceita { get; set; }
        public string Notas { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}