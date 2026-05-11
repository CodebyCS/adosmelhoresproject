namespace adosmelhoresproject.src.Models
{
    public class Coordenador : Funcionario
    {
        public int FormadorId { get; set; }

        public List<Formador> FormadoresAssociados { get; set; } = new List<Formador>();
    }
}