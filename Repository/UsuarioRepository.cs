using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using Amazon.Auth.AccessControlPolicy;
using System.Text.Json;

namespace apibronco.bronco.com.br.Repository.Mongodb
{
    public class UsuarioRepository : Repository.MongodbBaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IConfiguration configuration) : base(configuration) { 
            
        }
        public override void Alterar(Usuario entidade)
        {
            if (!entidade.IsValid()) return;

            entidade.LastUpdateOn = DateTime.Now;

            var client = new MongoClient(ConnectionString);
            IMongoCollection<Usuario> _collection = client.GetDatabase(DbName).GetCollection<Usuario>("usuario");
            var filter = Builders<Usuario>.Filter.Eq(e => e.Id, entidade.Id);

            var old = _collection.Find(filter).First();
            var oldId = old.Id;
            _collection.ReplaceOne(filter, entidade);
        }

        public override void Cadastrar(Usuario entidade)
        {
            if (!entidade.IsValid() || !IsUnique(entidade)) return;

            var client = new MongoClient(ConnectionString);
                IMongoCollection<Usuario> _collection = client.GetDatabase(DbName).GetCollection<Usuario>("usuario");
                _collection.InsertOne(entidade);
            
        }

        public override void Deletar(Usuario entidade)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Usuario> _collection = client.GetDatabase(DbName).GetCollection<Usuario>("usuario"); 
            var filter = Builders<Usuario>.Filter.Eq(e => e.Id, entidade.Id);
            _collection.DeleteOne(filter);
        }

        public override Usuario ObterPorId(string  id)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Usuario> _collection = client.GetDatabase(DbName).GetCollection<Usuario>("usuario"); 
            var filter = Builders<Usuario>.Filter.Eq(e => e.Id, id);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<Usuario>();
        }

        public override Usuario ObterPorCodigo(string email)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Usuario> _collection = client.GetDatabase(DbName).GetCollection<Usuario>("usuario");
            var filter = Builders<Usuario>.Filter.Eq(e => e.Email, email);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<Usuario>();
        }

        public override IList<Usuario> ObterTodos()
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Usuario> _collection = client.GetDatabase(DbName).GetCollection<Usuario>("usuario");
            var allDocs = _collection.Find(Builders<Usuario>.Filter.Empty).ToList();
            return allDocs;
        }

        public Usuario ObterAcesso(string email, string senha)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Usuario> _collection = client.GetDatabase(DbName).GetCollection<Usuario>("usuario");
            //var filter = Builders<Usuario>.Filter.Eq(e => e.Email, email);
            //var arrayFilter = Builders<BsonDocument>.Filter.Eq("Email", email)
            //                     & Builders<BsonDocument>.Filter.Eq("Senha", senha);
            string FilterJSON = "{'Email': '" + email + "','Senha':'" + senha+ "'}";
            //string JsonString = JsonSerializer.Serialize(FilterJSON);
            var allDocs = _collection.Find(FilterJSON);
            return allDocs.FirstOrDefault<Usuario>();
        }

        public override bool IsUnique(Usuario entidade)
        {
            IList<Usuario> usuarios = ObterTodos();
            var findProd = usuarios.Where(x => x.Email == entidade.Email).FirstOrDefault();
            if (findProd != null)
                throw new ArgumentException("IsNotUnique - Email já utilizado");

            return true;
        }
    }
}
