using apibronco.bronco.com.br.DTOs;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
    public class AgendaMedico : Entidade
    {
        public AgendaMedico(DisponibilidadeDTO dto)
        {
            this.CRM_BeforeLoad = dto.CRM;
            this.Data = dto.Data;
            this.Hora_Inicio = dto.Hora_Inicio;
            this.Hora_Fim = dto.Hora_Fim;

            IsValid();
        }

        public string CRM_BeforeLoad { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Hora_Inicio { get; set; }

        public TimeSpan Hora_Fim { get; set; }

      
        public bool flagReservado { get; set; }

        public override bool IsValid()
        {
            AssertionConcern.AssertArgumentNotEmpty(CRM_BeforeLoad, "CRM não pode ser vazio");
            AssertionConcern.AssertStateFalse(Data < DateTime.Now, "Data invalida, precisa ser superior a data atual ou no mesmo dia.");

            return true;
        }

    }
}
