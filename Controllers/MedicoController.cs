using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using apibronco.bronco.com.br.Repository.Mongodb;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Operations;

namespace fiap_hacka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaMedicoRepository _agendaMedicoRepository;

        public MedicoController(IMedicoRepository medicoRepository, IUsuarioRepository usuarioRepository, IConsultaRepository consultaRepository, IAgendaMedicoRepository agendaMedicoRepository)
        {
            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
            _agendaMedicoRepository = agendaMedicoRepository;
        }

        /// <summary>
        /// permite a criação de usuarios - segurados no corretor online
        /// </summary>
        /// <param name="info">informações do usuario segurado que será criado</param>
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

                var validaMedico = _medicoRepository.ObterTodos().Where(o => o.CRM == disp.CRM_BeforeLoad);

                if (validaMedico == null)
                    return NotFound("Médico não encontrado pelo CRM.");
                        
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
            var lista = _agendaMedicoRepository.ObterTodos();

            return Ok(lista);
        }


    }
}
