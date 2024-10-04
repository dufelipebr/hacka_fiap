using apibronco.bronco.com.br.DTOs;

namespace apibronco.bronco.com.br.Entity
{
    public class Usuario : Entidade
    {
        enum TipoAcesso { Medico, Paciente }

        public Usuario(PacienteDTO info)
        {
            this.Email= info.Email;
            this.Senha = info.Senha;
            this.TipoLogin = EnumTipoAcesso.Paciente;
            this.Email = new ObjectEmail(info.Email).Email_Value ;

            IsValid();
        }

        public Usuario(MedicoDTO info)
        {
            //this.Email = info.Email;
            this.Senha = info.Senha;
            this.TipoLogin = EnumTipoAcesso.Medico;
            this.Email = new ObjectEmail(info.Email).Email_Value;

            IsValid();
        }


        #region properties 
        public string Email { get; set; }

        //public ObjectEmail Email_Novo { get; set; }

        public string Senha { get; set; }

        //public string ReferenceID { get; set; }

        public EnumTipoAcesso TipoLogin { get; set; } 

        #endregion

        public override bool IsValid() 
        {
            AssertionConcern.AssertArgumentNotEmpty(Email, "Email precisa ser preenchido.");
            AssertionConcern.AssertArgumentNotEmpty(Senha, "Senha precisa ser preenchido.");
            //AssertionConcern.AssertArgumentNotEmpty(Email_Novo.ToString(), "Email invalido.");

            return true;
        }



    }
}
