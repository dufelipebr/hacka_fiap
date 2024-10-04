using apibronco.bronco.com.br.DTOs;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
   

    public class AgendaMedico : Entidade
    {
        public AgendaMedico(DisponibilidadeDTO dto)
        {
            this.Id = dto.Id;
            this.CRM_BeforeLoad = dto.CRM;
            //this.DataHoraInicio = dto.DataHoraInicio;
            //this.DataHoraFim = dto.DataHoraFim;
            this.AgendaTime_ini = new AgendaTime(dto.DataHoraInicio);
            this.AgendaTime_fim = new AgendaTime(dto.DataHoraFim.AddSeconds(-1));

            //this.TempoConsulta = dto.DataHoraFim;

            IsValid();
        }

        public string MedicoID { get; set; }
        
        [BsonIgnore]
        public string CRM_BeforeLoad { get; set; }

        //public DateTime DataHoraInicio { get; set; }

        //public DateTime DataHoraFim { get; set; }

        public AgendaTime AgendaTime_ini { get; set; }
        public AgendaTime AgendaTime_fim { get; set; }

        [BsonIgnore]
        public double TempoConsulta { get {
                return (AgendaTime_ini.GetDate() - AgendaTime_fim.GetDate()).TotalMinutes;
            } 
        }
      
        public bool flagReservado { get; set; }

        public override bool IsValid()
        {
            AssertionConcern.AssertArgumentNotEmpty(CRM_BeforeLoad, "CRM não pode ser vazio");
            AssertionConcern.AssertStateFalse(AgendaTime_ini.GetDate() < DateTime.Now, "Data invalida, precisa ser superior a data atual ou no mesmo dia.");
            AssertionConcern.AssertStateFalse(AgendaTime_ini.GetDate() > AgendaTime_fim.GetDate(), "Inicio da consulta deve ser menor que o final");
            AssertionConcern.AssertArgumentRange(AgendaTime_fim.GetDate().Subtract(AgendaTime_ini.GetDate()).TotalMinutes, 30, 60, "O tempo da consulta não pode ser menor que 30 min e maior que 60min.");
            return true;
        }

    }
}
