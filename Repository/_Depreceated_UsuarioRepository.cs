//using apibronco.bronco.com.br.Repository;
//using apibronco.bronco.com.br.Entity;
//using apibronco.bronco.com.br.Interfaces;
//using System.Collections.Generic;
//using System.Data.SqlClient;
////using apibronco.bronco.com.br.Repository.Azuredb;
//using apibronco.bronco.com.br.Repository.Mongodb;
//using System.Security.Cryptography.Xml;

//namespace apibronco.bronco.com.br.Repository
//{
//    public class UsuarioRepository : DapperRepository<Usuario>, IUsuarioRepository
//    {
//        IConfiguration _config;
//        IUsuarioRepository _repository;
//        public UsuarioRepository(IConfiguration configuration) : base(configuration)
//        {
//            _config = configuration;
//            if (TypeConnection == ConnectionType.Mongodb)
//                _repository = new MDUsuario(_config);
//            else
//                throw new NotImplementedException();
//            // _repository = new AZUsuario(_config);
//        }

//        public override void Alterar(Usuario entidade)
//        {
//            _repository.Alterar(entidade);
//        }

//        public override void Cadastrar(Usuario entidade)
//        {
//            _repository.Cadastrar(entidade);
//        }

//        public override void Deletar(Usuario entidade)
//        {
//            _repository.Deletar(entidade);
//        }

//        public override Usuario ObterPorId(string id)
//        {
//            return _repository.ObterPorId(id);
//        }

//        public override Usuario ObterPorCodigo(string codigo)
//        {
//            return _repository.ObterPorCodigo(codigo);
//        }

//        public override IList<Usuario> ObterTodos()
//        {
//            return _repository.ObterTodos();
//        }

//        public Usuario ObterPorNomeUsuarioESenha(
//            string email,
//            string senha)
//        {

//            return _repository.ObterPorNomeUsuarioESenha(email, senha);

//        }

//        public override bool IsUnique(Usuario entidade)
//        {
//            IEnumerable<Usuario> listFind = ObterTodos().Where(a => a.Email == entidade.Email);
//            if (listFind.Count() > 0)
//                throw new ArgumentException("UNIQUEKEY: Email do usuario já existente.");
            
//            return true;
//        }
//    }
//}
