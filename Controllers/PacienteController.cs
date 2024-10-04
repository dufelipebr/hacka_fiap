using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using apibronco.bronco.com.br.Repository.Mongodb;
using fiap_hacka.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ISendEmail _sendEmail;
        private readonly IAgendaMedicoRepository _agendaMedicoRepository;
        private readonly IConsultaRepository _consultaRepository;
        public PacienteController(
                IMedicoRepository medicoRepository, 
                IUsuarioRepository usuarioRepository, 
                IPacienteRepository pacienteRepository,
                ISendEmail sendEmail,
                IAgendaMedicoRepository agendaMedicoRepository,
                IConsultaRepository consultaRepository
            )
        {

            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
            _pacienteRepository = pacienteRepository;
            _sendEmail = sendEmail;
            _agendaMedicoRepository = agendaMedicoRepository;
            _consultaRepository = consultaRepository;
        }
        /// <summary>
        /// item 4. permite a criação de usuarios - segurados no corretor online
        /// </summary>
        /// <param name="info">informações do usuario segurado que será criado</param>
        /// <returns></returns>
        
        [HttpPost("cadastrar_paciente")]
        public IActionResult cadastrar_paciente([FromBody] PacienteDTO info)
        {

            try
            {
                Usuario usr = new Usuario(info);
                Paciente paciente = new Paciente(info);

                bool isValid = paciente.IsValid() && usr.IsValid();

                if (!_usuarioRepository.IsUnique(usr))
                    return BadRequest("O Email do usuario já está sendo utilizado.");

                if (isValid) _usuarioRepository.Cadastrar(usr); // criar usuario na base de dados
                
                paciente.UsuarioID = usr.Id;
                if (isValid) _pacienteRepository.Cadastrar(paciente);  // Persistindo usuario com referencia do Id gerado para obter depois
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }

        /// <summary>
        /// listar pacientes
        /// </summary>
        /// informações dos pacientes cadastrados na base.
        /// <returns></returns>
        //[Authorize(Roles = "Paciente")]
        [HttpGet("listar_pacientes")]
        public IActionResult listar_pacientes()
        {
            try
            {
                
                var lista = _pacienteRepository.ObterTodos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// item 7. apos selecionar o medico, o paciente deve visualizar os dias e horarios disponiveis do medico.
        /// </summary>
        /// <returns>array de ProdutoInfo[]</returns>
        [Authorize(Roles = "Paciente")]
        [HttpGet("obter_agendamentos_medico_disponiveis/{crmMedico}")]
        public IActionResult obter_agendamentos_medico_disponiveis(string crmMedico)
        {
            List<Medico> validaMedico = _medicoRepository.ObterTodos().Where(o => o.CRM == crmMedico).ToList();

            if (validaMedico == null)
                return NotFound("Médico não encontrado pelo CRM.");

            var lista = _agendaMedicoRepository.ObterPorMedicoID(validaMedico.FirstOrDefault().Id).Where(d => d.flagReservado == false);

            return Ok(lista);
        }

        /// <summary>
        /// item 7. O Paciente poderá selecionar o horario de preferencia e realizar o agendamento
        /// </summary>
        /// <returns>Ok para sucesso ou string com o erro</returns>
        [Authorize(Roles = "Paciente")]
        [HttpPost("agendar_consulta_medico")]
        public IActionResult agendar_consulta_medico(ConsultaDTO consultaDTO)
        {
            try
            {

                var paciente = _pacienteRepository.ObterPorCodigo(consultaDTO.cpf_Paciente);
                if (paciente == null)
                    return BadRequest("Paciente não encontrado");

                var agenda = _agendaMedicoRepository.ObterPorId(consultaDTO.AgendaID);
                if (agenda == null)
                    return BadRequest("Agenda não encontrado");

                if (agenda.flagReservado)
                    return BadRequest("Horario já reservado");


                Consulta cons = new Consulta(paciente.Id, agenda.Id, true);
                agenda.flagReservado = true;
                _agendaMedicoRepository.Alterar(agenda); // reservar a agenda do medico
                _consultaRepository.Cadastrar(cons);// criar a consulta.

                var medico = _medicoRepository.ObterPorId(agenda.MedicoID);

                var usuario = _usuarioRepository.ObterPorId(medico.UsuarioID);

                var bodyEmail = $"<p>Olá Dr. {medico.Nome}!</p>" +
                            $"<p>Você tem uma nova consulta marcada!</p>" +
                            $"<p>Paciente: {paciente.Nome}.</p>" +
                            $"<p>Data e horário: {agenda.AgendaTime_ini.Data.ToString("dd/MM/yyyy")} às {agenda.AgendaTime_ini.Hora} horas.</p>";


                _sendEmail.SendEmailAsync("ti.alexandre.costa@gmail.com", "Health&Med - Nova consulta agendada", bodyEmail);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }


    }
}
