using apibronco.bronco.com.br.Interfaces;
using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.Interfaces
{
    public interface IAgendaMedicoRepository : IRepository<AgendaMedico>
    {
        public IList<AgendaMedico> ObterPorMedicoID(string id);
    }
}
