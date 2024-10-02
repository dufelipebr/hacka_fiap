using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using Amazon.Auth.AccessControlPolicy;

namespace apibronco.bronco.com.br.Repository.Mongodb
{
    public class LogInfoRepository : Repository.MongodbBaseRepository<LogInfo>, ILogRepository
    {
        public LogInfoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        //public MDLog(IConfiguration configuration) : base(configuration) 
        //{ 

        //}
        public override void Alterar(LogInfo entidade)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<LogInfo> _collection = client.GetDatabase(DbName).GetCollection<LogInfo>("LogInfo");
            var filter = Builders<LogInfo>.Filter.Eq(e => e.Id, entidade.Id);

            var old = _collection.Find(filter).First();
            var oldId = old.Id;
            _collection.ReplaceOne(filter, entidade);
        }

        public override void Cadastrar(LogInfo entidade)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<LogInfo> _collection = client.GetDatabase(DbName).GetCollection<LogInfo>("LogInfo"); 
            _collection.InsertOne(entidade);
        }

        public override void Deletar(LogInfo entidade)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<LogInfo> _collection = client.GetDatabase(DbName).GetCollection<LogInfo>("LogInfo"); 
            var filter = Builders<LogInfo>.Filter.Eq(e => e.Id, entidade.Id);
            _collection.DeleteOne(filter);
        }

        public override IList<LogInfo> ObterTodos()
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<LogInfo> _collection = client.GetDatabase(DbName).GetCollection<LogInfo>("LogInfo"); ;
            var allDocs = _collection.Find(Builders<LogInfo>.Filter.Empty).ToList();
            return allDocs;
        }

        public override LogInfo ObterPorId(string id)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<LogInfo> _collection = client.GetDatabase(DbName).GetCollection<LogInfo>("LogInfo");
            var filter = Builders<LogInfo>.Filter.Eq(e => e.Id, id);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<LogInfo>();
        }

        public override LogInfo ObterPorCodigo(string codigo)
        {
            throw new NotImplementedException();
        }


        public IList<LogInfo> ObterTodosByFilter(LogFilter filter)
        {
            throw new NotImplementedException();
        }

        public override bool IsUnique(LogInfo entidade)
        {
            throw new NotImplementedException();
        }
    }
}
