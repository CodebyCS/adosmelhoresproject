namespace adosmelhoresproject.src.Models
{
    public class Alocacao
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 8); // Gera um ID curto automático
        public DateTime DataInicio { get; set; } = DateTime.Now;
        public DateTime DataFim { get; set; } = DateTime.Now.AddHours(4);
        public string NomeFormacao { get; set; }
        public int HorasPorDia { get; set; } = 6;

        // Novos campos para bater com a View
        public decimal ValorReceita { get; set; }
        public string Notas { get; set; }

        // Relacionamento com o Funcionário
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } // Facilitador para mostrar na tabela
    }
}