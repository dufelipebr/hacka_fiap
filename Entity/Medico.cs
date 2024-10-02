using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Interfaces;

namespace apibronco.bronco.com.br.Entity
{
    public class Medico : Entidade, IEntidade
    {
        #region constructors
        public Medico(MedicoDTO info) 
        {
            this.Nome = info.Nome;
            this.CPF = info.CPF_CNPJ;
            this.CRM = info.NumeroCrm;

            IsValid();
        }
        #endregion

        #region properties

        public string CRM { get; set; }
        public string Nome { get; set; }
      
        public string CPF{ get; set; }

        public string UsuarioID { get; set; }
        
        #endregion

        #region methods
        public override bool IsValid()
        {
            AssertionConcern.AssertArgumentNotEmpty(Nome, "Nome precisa ser preenchido.");
            
            AssertionConcern.AssertArgumentNotEmpty(CPF, "CPF precisa ser preenchido.");

            AssertionConcern.AssertArgumentNotEmpty(CRM, "CRM precisa ser preenchido.");
            
            return true;
        }
        #endregion
    }
}
