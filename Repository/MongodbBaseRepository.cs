
using apibronco.bronco.com.br.Interfaces;
using apibronco.bronco.com.br.Entity;
using MongoDB.Driver;
using Amazon.Auth.AccessControlPolicy;
using MongoDB.Driver.Core.Configuration;
using System.Xml.Linq;

namespace apibronco.bronco.com.br.Repository
{
    public abstract class MongodbBaseRepository<T> : IRepository<T> where T : Entidade
    {
        private readonly string? _connectionString;
        private readonly string? _dbName;
        protected string ConnectionString => _connectionString;
        protected string DbName => _dbName;


        public MongodbBaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:MongoDB");
            _dbName = configuration.GetValue<string>("ConnectionStrings:MongoDbName");
        }
        public abstract void Alterar(T entidade);

        public abstract void Cadastrar(T entidade);
        public abstract void Deletar(T entidade);
        public abstract T ObterPorId(string  id);
        public abstract T ObterPorCodigo(string codigo);
        public abstract IList<T> ObterTodos();
        
        public abstract bool IsUnique(T entidade);

    }
}
