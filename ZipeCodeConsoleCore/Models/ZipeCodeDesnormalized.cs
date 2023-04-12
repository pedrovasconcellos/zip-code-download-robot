using System;
using System.ComponentModel.DataAnnotations;

namespace ZipeCodeConsoleCore.Models
{
    public class ZipeCodeDesnormalized : ZipeCodeBase
    {
        public ZipeCodeDesnormalized() 
        {

        }
        public ZipeCodeDesnormalized(ZipeCode model) 
        {
            this.cep = model.cep;
            this.bairro = model.bairro;
            this.logradouro = model.logradouro;
            this.cidade = model.cidade?.nome;
            this.estado = model.estado?.sigla;
            this.ddd = model.cidade?.ddd;
            this.ibge = model.cidade?.ibge;
            this.altitude = model.altitude;
            this.latitude = model.latitude;
            this.longitude = model.longitude;
            this.datetime = model.datetime;
        }

        public string bairro { get; set; }
        public string logradouro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public short? ddd { get; set; }
        public int? ibge { get; set; }
        public decimal? altitude { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public DateTime datetime { get; set; } = DateTime.UtcNow;
    }
}