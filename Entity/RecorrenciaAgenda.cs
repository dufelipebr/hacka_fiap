using apibronco.bronco.com.br.DTOs;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
    public class RecorrenciaAgenda 
    {
        public bool criarRecorrenciaAgendamento { get; set; }
        public bool mesmoDiaSemana { get; set; }

        public DateTime TerminaEm { get; set; }
    }
}
