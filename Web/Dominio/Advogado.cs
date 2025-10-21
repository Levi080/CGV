using Dominio.Enums;
using System;

namespace Dominio
{
    [Serializable]
    public class Advogado
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public SenioridadeEnum Senioridade { get; set; }

        public string Logradouro { get; set; }

        public string Bairro { get; set; }

        public EstadoEnum Estado { get; set; }

        public string Cep { get; set; }

        public int Numero { get; set; } 

        public string Complemento { get; set; }

    }
}
