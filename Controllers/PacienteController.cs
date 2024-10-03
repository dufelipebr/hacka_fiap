using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using apibronco.bronco.com.br.Repository.Mongodb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fiap_hacka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoRepository _medicoRepository;
        public PacienteController(IMedicoRepository medicoRepository, IUsuarioRepository usuarioRepository, IPacienteRepository pacienteRepository)
        {

            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
            _pacienteRepository = pacienteRepository;
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
        /// permite a criação de usuarios - segurados no corretor online
        /// </summary>
        /// <param name="info">informações do usuario segurado que será criado</param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet("listar_medico")]
        public IActionResult listar_medico()
        {
            try
            {

                var medicos = _medicoRepository.ObterTodos();
                return Ok(medicos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
