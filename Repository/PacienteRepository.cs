using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using Amazon.Auth.AccessControlPolicy;
using System.Text.Json;

namespace apibronco.bronco.com.br.Repository.Mongodb
{
    public class PacienteRepository : Repository.MongodbBaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(IConfiguration configuration) : base(configuration) { 
            
        }
        public override void Alterar(Paciente entidade)
        {

            if (!entidade.IsValid()) return;
            
            entidade.LastUpdateOn = DateTime.Now;

            var client = new MongoClient(ConnectionString);
            IMongoCollection<Paciente> _collection = client.GetDatabase(DbName).GetCollection<Paciente>("Paciente");
            var filter = Builders<Paciente>.Filter.Eq(e => e.Id, entidade.Id);

            var old = _collection.Find(filter).First();
            var oldId = old.Id;
            _collection.ReplaceOne(filter, entidade);
            
        }

        public override void Cadastrar(Paciente entidade)
        {
            if (!entidade.IsValid() || !IsUnique(entidade)) return;


            entidade.CreatedOn = DateTime.Now;
          
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Paciente> _collection = client.GetDatabase(DbName).GetCollection<Paciente>("Paciente"); 
            _collection.InsertOne(entidade);
          
        }

        public override void Deletar(Paciente entidade)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Paciente> _collection = client.GetDatabase(DbName).GetCollection<Paciente>("Paciente"); 
            var filter = Builders<Paciente>.Filter.Eq(e => e.Id, entidade.Id);
            _collection.DeleteOne(filter);
        }

        public override bool IsUnique(Paciente entidade)
        {
            //IList<Paciente> Pacientes = ObterTodos();
            //var findProd = Pacientes.Where(x => x.CPF == entidade.CPF).FirstOrDefault();
            //if (findProd != null)
            //    throw new ArgumentException("IsNotUnique - CPF já utilizado");

            return true;
        }

        public override Paciente ObterPorId(string  id)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Paciente> _collection = client.GetDatabase(DbName).GetCollection<Paciente>("Paciente");
            //var filter = Builders<Paciente>.Filter.Eq(e => e.Id, id) & Builders<Paciente>.Filter.Eq(e => e.Id_Status, 1);
            var filter = Builders<Paciente>.Filter.Eq(e => e.Id, id);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<Paciente>();
        }

        /// <summary>
        ///  ira realizar a busca por CPF
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Paciente ObterPorCodigo(string codigo)
        {
            
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Paciente> _collection = client.GetDatabase(DbName).GetCollection<Paciente>("Paciente");
            var filter = Builders<Paciente>.Filter.Eq(e => e.CPF.CPF_Value, codigo);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<Paciente>();
        }

        public override IList<Paciente> ObterTodos()
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Paciente> _collection = client.GetDatabase(DbName).GetCollection<Paciente>("Paciente");
            //var allDocs = _collection.Find(Builders<Paciente>.Filter.Eq(e => e.Id_Status, 1)).ToList();
            var allDocs = _collection.Find(Builders<Paciente>.Filter.Empty).ToList();
            return allDocs;
        }

        public Paciente ObterPorUsuarioID(string usuarioID)
        {
            var client = new MongoClient(ConnectionString);
            IMongoCollection<Paciente> _collection = client.GetDatabase(DbName).GetCollection<Paciente>("Paciente");
            var filter = Builders<Paciente>.Filter.Eq(e => e.UsuarioID, usuarioID);
            var allDocs = _collection.Find(filter).ToList();
            return allDocs.FirstOrDefault<Paciente>();
        }


    }
}
