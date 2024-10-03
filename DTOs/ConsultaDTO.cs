using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.DTOs
{
    public class ConsultaDTO    
    {
        //public StatusProposta StatusProposta { get; set; }
        public string cpf_Paciente { get; set; }

        public string AgendaID { get; set; }


        public bool IsValid()
        {
            return true;
        }

    }
}
