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
    }
}
