using apibronco.bronco.com.br.DTOs;
using System.ComponentModel.DataAnnotations;

namespace apibronco.bronco.com.br.Entity
{
    public class Consulta : Entidade
    {
        public Consulta(ConsultaDTO dto)
        {
            

            IsValid();
        }
        //public StatusProposta StatusProposta { get; set; }

        public string PacienteID { get; set; }

        public string MedicoID { get; set; }

        public string AgendaMedicoID { get; set; }


        public override bool IsValid()
        {
            AssertionConcern.AssertArgumentNotEmpty(MedicoID, "MedicoID is empty");
            AssertionConcern.AssertArgumentNotEmpty(PacienteID, "PacienteID is empty");

            return true;
            
        }

    }
}
