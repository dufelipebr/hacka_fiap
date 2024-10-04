using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fiap_hacka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        public LoginController(ITokenService tokenService, IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository, IUsuarioRepository usuarioRepository)
        {
            _tokenService = tokenService;
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
            _usuarioRepository = usuarioRepository;

        }

        /// <summary>
        /// item 2 e item 5 Autenticação do usuario : permite login do medico e do paciente com e-mail senha.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>
        ///  return Ok(new
        ///        {
        ///            Usuario = usuario,
        ///            Medico = medInfo,
        ///            Paciente = pacienteInfo,
        ///            Token = token
        ///});
        /// </returns>
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

        [HttpDelete("delete_usuario")]
        public IActionResult delete_usuario(string usuarioID)
        {
            try
            { 
                var usuario = _usuarioRepository.ObterPorId(usuarioID);
            
                if (usuario == null)
                    return NotFound(new { mensagem = "Usuario não encontrado" });

                try 
                { 
                    if (usuario.TipoLogin == EnumTipoAcesso.Medico)
                    {
                        var obterMedico = _medicoRepository.ObterPorUsuarioID(usuarioID);
                        _medicoRepository.Deletar(obterMedico);
                    }
                    else
                    {
                        var obterPaciente = _pacienteRepository.ObterPorUsuarioID(usuarioID);
                        _pacienteRepository.Deletar(obterPaciente);
                    }
                }
                catch(Exception e)
                {

                }
                _usuarioRepository.Deletar(usuario);

                return Ok("Ok");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// listar pacientes
        /// </summary>
        /// informações dos pacientes cadastrados na base.
        /// <returns></returns>
        //[Authorize(Roles = "Paciente")]
        [HttpGet("listar_usuarios")]
        public IActionResult listar_usuarios()
        {
            try
            {
                List<Tuple<Usuario, Object>> tlist = new List<Tuple<Usuario, Object>>();
                var lista = _usuarioRepository.ObterTodos();

                foreach(var item in lista)
                {
                    if (item.TipoLogin == EnumTipoAcesso.Paciente)
                    {
                        var paciente = _pacienteRepository.ObterPorUsuarioID(item.Id);
                        //tlist.Add(item, paciente);
                        tlist.Add(new Tuple<Usuario, Object>(item, paciente));
                    }
                    else
                    {
                        var medico = _medicoRepository.ObterPorUsuarioID(item.Id);
                        tlist.Add(new Tuple<Usuario, Object>(item, medico));
                    }
                }
                return Ok(tlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
