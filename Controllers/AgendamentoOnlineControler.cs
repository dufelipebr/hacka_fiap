
using Microsoft.AspNetCore.Mvc;
using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using apibronco.bronco.com.br.DTOs;
namespace apibronco.bronco.com.br.Controllers
{
    [ApiController]
    [Route("agendamento_online")]
    public class AgendamentoOnlineControler : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaMedicoRepository _agendaMedicoRepository;
        private readonly ITokenService _tokenService;
        //private readonly ILogger<AgendamentoOnlineControler> _logger;


        public AgendamentoOnlineControler(
            IUsuarioRepository usuarioRepository, 
            IMedicoRepository medicoRepository,
            IPacienteRepository pacienteRepository,
            IConsultaRepository consultaRepository,
            IAgendaMedicoRepository agendaMedicoRepository,
            ITokenService tokenService
        //    ILogger<AgendamentoOnlineControler> logger
        )
        {
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
            _consultaRepository = consultaRepository;
            _usuarioRepository = usuarioRepository;
            _agendaMedicoRepository = agendaMedicoRepository;
            _tokenService = tokenService;
        }

        /// <summary>
        /// permite a criação de usuarios - segurados no corretor online
        /// </summary>
        /// <param name="RegisterInfo">informações do usuario segurado que será criado</param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost("cadastrar_medico")]
        public IActionResult cadastrar_medico([FromBody] MedicoDTO info)
        {

            try
            {
                Usuario usr = new Usuario(info);
                _usuarioRepository.Cadastrar(usr);

                // criar usuario na base de dados
                Medico medico = new Medico(info);
                medico.UsuarioID = usr.Id;
                _medicoRepository.Cadastrar(medico);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }

        /// <summary>
        /// permite a criação de usuarios - segurados no corretor online
        /// </summary>
        /// <param name="RegisterInfo">informações do usuario segurado que será criado</param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost("cadastrar_paciente")]
        public IActionResult cadastrar_paciente([FromBody] PacienteDTO info)
        {

            try
            {
                Usuario usr = new Usuario(info);
                _usuarioRepository.Cadastrar(usr);

                // criar usuario na base de dados
                Paciente paciente = new Paciente(info);
                paciente.UsuarioID = usr.Id;
                _pacienteRepository.Cadastrar(paciente);

                // Persistindo usuario com referencia do Id gerado para obter depois
                
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }

        /// <summary>
        /// Permite realizar login do cliente segurado no portal corretor online
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult login([FromBody] LoginDTO login)
        {
            var usuario = _usuarioRepository.ObterAcesso(login.Email, login.Senha);
            Medico medInfo = null;
            Paciente pacienteInfo = null;

            if (usuario == null)
                return NotFound(new { mensagem = "Usuario e ou Senha invalidos" });

            var token = _tokenService.GerarToken(usuario);

            usuario.Senha = null;

            if (usuario.TipoLogin == EnumTipoAcesso.Medico)
                medInfo = _medicoRepository.ObterPorUsuarioID(usuario.Id);
            else
                pacienteInfo = _pacienteRepository.ObterPorUsuarioID(usuario.Id);

            return Ok(new
            {
                Usuario = usuario,
                Medico = medInfo,
                Paciente = pacienteInfo,
                Token = token
            });
        }

        /// <summary>
        /// cria a agenda de disponibilidade do medico
        /// </summary>
        /// <returns>Ok para sucesso ou string com o erro</returns>
        [HttpPost("criar_agenda_medico")]
        public IActionResult criar_agenda_medico(DisponibilidadeDTO disponilidade)
        {
            try
            {
                // cadastrar disponibilidade do médico
                AgendaMedico disp = new AgendaMedico(disponilidade);
                _agendaMedicoRepository.Cadastrar(disp);
                
                //_usuarioRepository.Cadastrar(usr);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }

        /// <summary>
        /// lista de produtos disponiveis
        /// </summary>
        /// <returns>array de ProdutoInfo[]</returns>
        [HttpGet("obter_agendamentos")]
        public IActionResult obter_agendamentos()
        {
            var lista = _consultaRepository.ObterTodos();

            return Ok(lista);
        }
    }
}
