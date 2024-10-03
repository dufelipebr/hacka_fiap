using apibronco.bronco.com.br.DTOs;
using System.Security.Cryptography.X509Certificates;

namespace apibronco.bronco.com.br.Entity
{
    public class Paciente : Entidade
    {
        #region constructors
        public Paciente(PacienteDTO info)
        {
            this.Nome = info.Nome;
            this.CPF = new ObjectCPF(info.CPF_CNPJ);
            IsValid();
        }
        #endregion
        public string Nome { get; set; }
        public ObjectCPF CPF { get; set; }
        public string UsuarioID { get; set; }



        #region methods
        public override bool IsValid()
        {
            AssertionConcern.AssertArgumentNotEmpty(Nome, "Nome precisa ser preenchido.");

            AssertionConcern.AssertArgumentNotEmpty(CPF.ToString(), "CPF precisa ser preenchido.");

            return true;
        }
        #endregion


    }
}
