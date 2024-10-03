using apibronco.bronco.com.br.DTOs;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
    public class Consulta : Entidade
    {
        public Consulta(string _pacienteID, string _agendaID, bool _flagEnviarNotificao)
        {
            this.PacienteID = _pacienteID;
            this.AgendaMedicoID = _agendaID;
            this.flagEnviarNotificao = _flagEnviarNotificao;

            IsValid();
        }

        public string PacienteID { get; set; }

        //public string MedicoID { get; set; }

        public string AgendaMedicoID { get; set; }

        public bool flagEnviarNotificao {get; set;}


        public override bool IsValid()
        {
            AssertionConcern.AssertArgumentNotEmpty(AgendaMedicoID, "AgendaMedicoID is empty");
            AssertionConcern.AssertArgumentNotEmpty(PacienteID, "PacienteID is empty");

            return true;
            
        }

    }
}
