using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.DTOs
{
    /// <summary>
    /// Informações trafegadas para registro de usuario 
    /// </summary>
    public class MedicoDTO
    {
        public string Nome { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }

        public string NumeroCrm { get; set; }

        public string CPF_CNPJ { get; set; }
    }
}
  