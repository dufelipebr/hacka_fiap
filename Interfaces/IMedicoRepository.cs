using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.Interfaces
{
    public interface IMedicoRepository : IRepository<Medico>
    {
        public Medico ObterPorUsuarioID(string usuarioID);
    }
}
