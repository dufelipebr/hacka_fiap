using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using Amazon.Auth.AccessControlPolicy;

namespace apibronco.bronco.com.br.Repository.Mongodb
{
    public class ConsultaRepository : Repository.MongodbBaseRepository<Consulta>, IConsultaRepository
    {
        public ConsultaRepository(IConfiguration configuration) : base(configuration) { 
            
        }

        public override void Alterar(Consulta entidade)
        {
            if (!entidade.IsValid()) return;

            entidade.LastUpdateOn = DateTime.Now;

            var client = new MongoClient(ConnectionString);
            IMongoCollection<Consulta> _collection = client.GetDatabase(DbName).GetCollection<Consulta>("consulta");
            var filter = Builders<Consulta>.Filter.Eq(e => e.Id, entidade.Id);

            var old = _collection.Find(filter).First();
            var oldId = old.Id;
            _collection.ReplaceOne(filter, entidade);
        }

        public override void Cadastrar(Consulta entidade)
        {
            if (!entidade.IsValid() || !IsUnique(entidade)) return;


            var client = new MongoClient(ConnectionString);
            IMongoCollection<Consulta> _collection = client.GetDatabase(DbName).GetCollection<Consulta>("consulta"); 
            _collection.InsertOne(entidade);
        }

        public override void Deletar(Consulta entidade)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Consulta> _collection = client.GetDatabase(DbName).GetCollection<Consulta>("consulta"); 
            var filter = Builders<Consulta>.Filter.Eq(e => e.Id, entidade.Id);
            _collection.DeleteOne(filter);
        }

        public override Consulta ObterPorId(string  id)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Consulta> _collection = client.GetDatabase(DbName).GetCollection<Consulta>("consulta"); 
            var filter = Builders<Consulta>.Filter.Eq(e => e.Id, id);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<Consulta>();
        }
       
        public override IList<Consulta> ObterTodos()
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Consulta> _collection = client.GetDatabase(DbName).GetCollection<Consulta>("consulta"); ;
            var allDocs = _collection.Find(Builders<Consulta>.Filter.Empty).ToList();
            return allDocs;
        }

        ////public consulta ObterPorCodigo(string codigo_interno)
        ////{
        ////    var client = new MongoClient(ConnectionString);
        ////    IMongoCollection<consulta> _collection = client.GetDatabase(DbName).GetCollection<consulta>("consulta");
        ////    var filter = Builders<consulta>.Filter.Eq(e => e.Codigo_Interno, codigo_interno);
        ////    var allDocs = _collection.Find(filter).ToList();

        ////    if (allDocs.Count == 0)
        ////        throw new Exception("codigo interno não encontrado");

        ////    return allDocs.FirstOrDefault<consulta>();
        ////}

        public override bool IsUnique(Consulta entidade)
        {
            return true;
        }

        public override Consulta ObterPorCodigo(string codigo)
        {
            throw new NotImplementedException();
        }
    }
}
