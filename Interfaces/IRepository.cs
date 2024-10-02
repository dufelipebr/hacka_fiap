using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.Interfaces
{
    public interface IRepository<T> where T: Entidade
    {
        IList<T> ObterTodos();
        T ObterPorId(string id);
        
        T ObterPorCodigo(string codigo);
        void Cadastrar(T entidade);
        void Alterar(T entidade);
        void Deletar(T entidade);

        bool IsUnique(T entidade);
    }
}
