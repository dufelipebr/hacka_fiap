using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using Amazon.Auth.AccessControlPolicy;

namespace apibronco.bronco.com.br.Repository.Mongodb
{
    public class AgendaMedicoRepository : Repository.MongodbBaseRepository<AgendaMedico>, IAgendaMedicoRepository
    {
        public AgendaMedicoRepository(IConfiguration configuration) : base(configuration) { 
            
        }

        public override void Alterar(AgendaMedico entidade)
        {
            //if (!entidade.IsValid()) return;

            entidade.LastUpdateOn = DateTime.Now;

            var client = new MongoClient(ConnectionString);
            IMongoCollection<AgendaMedico> _collection = client.GetDatabase(DbName).GetCollection<AgendaMedico>("agendaMedico");
            var filter = Builders<AgendaMedico>.Filter.Eq(e => e.Id, entidade.Id);

            var old = _collection.Find(filter).First();
            var oldId = old.Id;
            _collection.ReplaceOne(filter, entidade);
        }

        public override void Cadastrar(AgendaMedico entidade)
        {
            if (!entidade.IsValid() || !IsUnique(entidade)) return;


            var client = new MongoClient(ConnectionString);
            IMongoCollection<AgendaMedico> _collection = client.GetDatabase(DbName).GetCollection<AgendaMedico>("agendaMedico"); 
            _collection.InsertOne(entidade);
        }

        public override void Deletar(AgendaMedico entidade)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<AgendaMedico> _collection = client.GetDatabase(DbName).GetCollection<AgendaMedico>("agendaMedico"); 
            var filter = Builders<AgendaMedico>.Filter.Eq(e => e.Id, entidade.Id);
            _collection.DeleteOne(filter);
        }

        public override AgendaMedico ObterPorId(string  id)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<AgendaMedico> _collection = client.GetDatabase(DbName).GetCollection<AgendaMedico>("agendaMedico"); 
            var filter = Builders<AgendaMedico>.Filter.Eq(e => e.Id, id);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<AgendaMedico>();
        }
       
        public override IList<AgendaMedico> ObterTodos()
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<AgendaMedico> _collection = client.GetDatabase(DbName).GetCollection<AgendaMedico>("agendaMedico");
            var allDocs = _collection.Find(Builders<AgendaMedico>.Filter.Empty).ToList();
            return allDocs;
        }

        public IList<AgendaMedico> ObterPorMedicoID(string medicoID)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<AgendaMedico> _collection = client.GetDatabase(DbName).GetCollection<AgendaMedico>("agendaMedico");
            var filter = Builders<AgendaMedico>.Filter.Eq(e => e.MedicoID, medicoID);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs;
        }

        //public agendaMedico ObterPorCodigo(string codigo_interno)
        //{
        //    //ObterTodos.Where(x => x.flagReservado == 0);
        //}

        public override bool IsUnique(AgendaMedico entidade)
        {
            return true;
        }

        public override AgendaMedico ObterPorCodigo(string codigo)
        {
            throw new NotImplementedException();
        }
    }
}
