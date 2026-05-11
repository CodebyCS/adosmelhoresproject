using System.Text.Json.Serialization;

namespace adosmelhoresproject.src.Models

{
    [JsonDerivedType(typeof(Diretor), "Diretor")]
    [JsonDerivedType(typeof(Coordenador), "Coordenador")]
    [JsonDerivedType(typeof(Formador), "Formador")]
    [JsonDerivedType(typeof(Secretaria), "Secretaria")]
    public abstract class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Morada { get; set; }
        public string Contacto { get; set; }
        public DateTime DataRegistoCriminal { get; set; }
        public DateTime DataFimContrato { get; set; }
        public DateTime DataInicioContrato { get; set; }
        public decimal Salario { get; set; }
        public bool Ativo { get; set; }
    }

}