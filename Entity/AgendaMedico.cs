using apibronco.bronco.com.br.DTOs;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
    public class AgendaMedico : Entidade
    {
        public AgendaMedico(DisponibilidadeDTO dto)
        {
            this.CRM_BeforeLoad = dto.CRM;
            this.DataHoraInicio = dto.DataHoraInicio;
            this.DataHoraFim = dto.DataHoraFim;
            //this.TempoConsulta = dto.DataHoraFim;

            IsValid();
        }

        public string MedicoID { get; set; }
        
        [BsonIgnore]
        public string CRM_BeforeLoad { get; set; }

        public DateTime DataHoraInicio { get; set; }

        public DateTime DataHoraFim { get; set; }

        public string TempoConsulta { get; set; }
      
        public bool flagReservado { get; set; }

        public override bool IsValid()
        {
            AssertionConcern.AssertArgumentNotEmpty(CRM_BeforeLoad, "CRM não pode ser vazio");
            AssertionConcern.AssertStateFalse(DataHoraInicio < DateTime.Now, "Data invalida, precisa ser superior a data atual ou no mesmo dia.");
            AssertionConcern.AssertStateFalse(DataHoraInicio > DataHoraFim, "Inicio da consulta deve ser menor que o final");
            AssertionConcern.AssertArgumentRange(DataHoraFim.Subtract(DataHoraInicio).TotalMinutes, 30, 60, "O tempo da consulta não pode ser menor que 30 min e maior que 60min.");
            return true;
        }

    }
}
