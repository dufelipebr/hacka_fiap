using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.Interfaces
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
