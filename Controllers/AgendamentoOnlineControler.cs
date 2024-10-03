
using Microsoft.AspNetCore.Mvc;
using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using apibronco.bronco.com.br.DTOs;
namespace fiap_hacka.Controllers
{
    [ApiController]
    [Route("agendamento_online")]
    public class AgendamentoOnlineControler : ControllerBase
    {

        private readonly IPacienteRepository _pacienteRepository;
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaMedicoRepository _agendaMedicoRepository;
       
        //private readonly ILogger<AgendamentoOnlineControler> _logger;


        public AgendamentoOnlineControler(
            IPacienteRepository pacienteRepository,
            IConsultaRepository consultaRepository,
            IAgendaMedicoRepository agendaMedicoRepository,
            ITokenService tokenService
        //    ILogger<AgendamentoOnlineControler> logger
        )
        {
            _pacienteRepository = pacienteRepository;
            _consultaRepository = consultaRepository;
            _agendaMedicoRepository = agendaMedicoRepository;
        }

      

       

       
    }
}
