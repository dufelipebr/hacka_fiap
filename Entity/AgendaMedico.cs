using apibronco.bronco.com.br.DTOs;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
   

    public class AgendaMedico : Entidade
    {
        public AgendaMedico(DisponibilidadeDTO dto)
        {
            //this.Id = dto.Id;
            this.CRM_BeforeLoad = dto.CRM;
            //this.DataHoraInicio = dto.DataHoraInicio;
            //this.DataHoraFim = dto.DataHoraFim;
            this.AgendaTime_ini = new AgendaTime(dto.DataHoraInicio);
            this.AgendaTime_fim = new AgendaTime(dto.DataHoraFim.AddSeconds(-1));

            //this.TempoConsulta = dto.DataHoraFim;

            IsValid();
        }

        public AgendaMedico(AlterarDisponibilidadeDTO dto)
        {
            //this.Id = dto.Id;
            this.CRM_BeforeLoad = dto.CRM;
            //this.DataHoraInicio = dto.DataHoraInicio;
            //this.DataHoraFim = dto.DataHoraFim;
            this.AgendaTime_ini = new AgendaTime(dto.DataHoraInicio);
            this.AgendaTime_fim = new AgendaTime(dto.DataHoraFim.AddSeconds(-1));

            //this.TempoConsulta = dto.DataHoraFim;

            IsValid();
        }

        public AgendaMedico(string crm_BeforeLoad, AgendaTime agendaTimeInicio, AgendaTime agendaTimeFim)
        { 
            //this.MedicoID = medicoID;
            this.CRM_BeforeLoad = crm_BeforeLoad;
            this.AgendaTime_ini = agendaTimeInicio;
            this.AgendaTime_fim = agendaTimeFim;

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

        public bool checkSobrePosicao_Agendamento(IList<AgendaMedico> _listaAgendamentos)
        {
            // pegar sobreposições data inicio
            var obterAgendasBetween = _listaAgendamentos.Where(x => this.AgendaTime_ini.GetDate() >= x.AgendaTime_ini.GetDate() && this.AgendaTime_ini.GetDate() <= x.AgendaTime_fim.GetDate()).ToList();
            if (obterAgendasBetween.Count > 0)
                return true;

            // pegar sobreposições data fim
            obterAgendasBetween = _listaAgendamentos.Where(x => this.AgendaTime_fim.GetDate() >= x.AgendaTime_ini.GetDate() && this.AgendaTime_fim.GetDate() <= x.AgendaTime_fim.GetDate()).ToList();
            if (obterAgendasBetween.Count > 0)
                return true;

            return false;
        }

    }
}
