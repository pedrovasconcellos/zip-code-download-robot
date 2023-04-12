using System;
using System.ComponentModel.DataAnnotations;

namespace ZipeCodeConsoleCore.Models
{
    public class ZipeCode : ZipeCodeBase
    {
        public string bairro { get; set; }
        public string logradouro { get; set; }
        public Cidade cidade { get; set; }
        public Estado estado { get; set; }
        public decimal? altitude { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public DateTime datetime { get; set; } = DateTime.UtcNow;
    }

    public class Cidade 
    {
        public short? ddd { get; set; }
        public int? ibge { get; set; }
        public string nome { get; set; }
    }

    public class Estado
    {
        public string sigla { get; set; }
    }
}
