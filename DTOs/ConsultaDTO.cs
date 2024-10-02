using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.DTOs
{
    public class ConsultaDTO    
    {
        //public StatusProposta StatusProposta { get; set; }
        public string PacienteID { get; set; }

        public string MedicoID { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }




        public bool IsValid()
        {
            return true;
        }

    }
}
