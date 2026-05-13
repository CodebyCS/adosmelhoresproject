namespace adosmelhoresproject.src.Models
{
    public class Coordenador : Employee
    {
        public int FormadorId { get; set; }

        public List<Trainer> FormadoresAssociados { get; set; } = new List<Trainer>();
    }
}