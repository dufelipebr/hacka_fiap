using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        public Usuario ObterAcesso(string email, string senha);
    }
}
