using apibronco.bronco.com.br.DTOs;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
    public class DisponibilidadeDTO 
    {
        public string CRM { get; set; }

        public DateTime DataHoraInicio { get; set; }

        public DateTime DataHoraFim { get; set; }

        //public TimeSpan Intervalo { get; set; }

        //public RecorrenciaAgenda Recorrencia { get; set; }


    }
}
