using apibronco.bronco.com.br.DTOs;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
    public class DisponibilidadeDTO 
    {
        public string CRM { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Hora_Inicio { get; set; }

        public TimeSpan Hora_Fim { get; set; }

        public RecorrenciaAgenda Recorrencia { get; set; }


       

    }
}
