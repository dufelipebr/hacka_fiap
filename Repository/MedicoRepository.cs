using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using Amazon.Auth.AccessControlPolicy;

namespace apibronco.bronco.com.br.Repository.Mongodb
{
    public class MedicoRepository : MongodbBaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(IConfiguration configuration) : base(configuration) { 
            
        }
        public override void Alterar(Medico entidade)
        {
            if (!entidade.IsValid()) return;

            entidade.LastUpdateOn = DateTime.Now;

            var client = new MongoClient(ConnectionString);
            IMongoCollection<Medico> _collection = client.GetDatabase(DbName).GetCollection<Medico>("Medico");
            var filter = Builders<Medico>.Filter.Eq(e => e.Id, entidade.Id);

            var old = _collection.Find(filter).First();
            var oldId = old.Id;
            _collection.ReplaceOne(filter, entidade);
        }

        public override void Cadastrar(Medico entidade)
        {
            if (!entidade.IsValid() || !IsUnique(entidade)) return;


            entidade.CreatedOn = DateTime.Now;

            var client = new MongoClient(ConnectionString);
            IMongoCollection<Medico> _collection = client.GetDatabase(DbName).GetCollection<Medico>("Medico"); 
            _collection.InsertOne(entidade);
        }

        public override void Deletar(Medico entidade)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Medico> _collection = client.GetDatabase(DbName).GetCollection<Medico>("Medico"); 
            var filter = Builders<Medico>.Filter.Eq(e => e.Id, entidade.Id);
            _collection.DeleteOne(filter);
        }

        public override bool IsUnique(Medico entidade)
        {
            //IList<Medico> usuarios = ObterTodos();
            //var findProd = usuarios.Where(x => x.CRM == entidade.CRM).FirstOrDefault();
            //if (findProd != null)
            //    throw new ArgumentException("IsNotUnique - CRM já utilizado");

            return true;
        }

        public override Medico ObterPorId(string  id)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Medico> _collection = client.GetDatabase(DbName).GetCollection<Medico>("Medico"); 
            var filter = Builders<Medico>.Filter.Eq(e => e.Id, id);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<Medico>();
        }

        public override Medico ObterPorCodigo(string codigo)
        {
            //var client = new MongoClient(ConnectionString);
            //IMongoCollection<Medico> _collection = client.GetDatabase(DbName).GetCollection<Medico>("Medico");
            //var filter = Builders<Medico>.Filter.Eq(e => e.Email, codigo);
            //var allDocs = _collection.Find(filter).ToList();
            //return allDocs.FirstOrDefault<Medico>();

            throw new NotImplementedException();
        }

        public override IList<Medico> ObterTodos()
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Medico> _collection = client.GetDatabase(DbName).GetCollection<Medico>("Medico");
            var allDocs = _collection.Find(Builders<Medico>.Filter.Empty).ToList();
            return allDocs;
        }

        public Medico ObterPorUsuarioID(string usuarioID)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Medico> _collection = client.GetDatabase(DbName).GetCollection<Medico>("Medico");
            var filter = Builders<Medico>.Filter.Eq(e => e.UsuarioID, usuarioID);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<Medico>();
        }


        //public Medico ObterPorNomeMedicoESenha(string email, string senha)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
