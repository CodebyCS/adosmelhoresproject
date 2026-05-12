namespace adosmelhoresproject.src.Models
{
    public class Coordenador : Employee
    {
        public int FormadorId { get; set; }

        public List<Formador> FormadoresAssociados { get; set; } = new List<Formador>();
    }
}