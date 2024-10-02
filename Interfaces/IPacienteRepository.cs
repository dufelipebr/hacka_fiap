using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.Interfaces
{
    public interface IPacienteRepository : IRepository<Paciente>    
    {
        public Paciente ObterPorUsuarioID(string usuarioID);
    }
}
