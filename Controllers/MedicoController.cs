using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using fiap_hacka.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;

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
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ISendEmail _sendEmail;

        public MedicoController(IMedicoRepository medicoRepository, 
            IUsuarioRepository usuarioRepository,
            IConsultaRepository consultaRepository,
            IAgendaMedicoRepository agendaMedicoRepository,
            IPacienteRepository pacienteRepository,
            ISendEmail sendEmail)
        {
            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
            _agendaMedicoRepository = agendaMedicoRepository;
            _pacienteRepository = pacienteRepository;
            _consultaRepository = consultaRepository;
            _sendEmail = sendEmail;
        }

        /// <summary>
        /// item 1. cadastro do usuario (medico) - o medico poderá se cadastar preennchedo os campos obrigatorios
        /// </summary>
        /// <param name="info">informações do usuario segurado que será criado</param>
        /// <returns>Ok - sucesso</returns>
        //[Authorize]
        [HttpPost("cadastrar_medico")]
        public IActionResult cadastrar_medico([FromBody] MedicoDTO info)
        {

            try
            {
                Usuario usr = new Usuario(info); // Validações realizadas.
                Medico medico = new Medico(info); // Validações realizadas.

                List<Medico> validaCRM = _medicoRepository.ObterTodos().Where(o => o.CRM == info.NumeroCrm).ToList();
                if (validaCRM.Count > 0)
                    return BadRequest("Já existe medico cadastrado com esse CRM.");

                bool isValid = medico.IsValid() && usr.IsValid();

                if (!_usuarioRepository.IsUnique(usr))
                    return BadRequest("Email já cadastrado não pode ser utilizado");

                if (isValid) _usuarioRepository.Cadastrar(usr); // criar usuario na base de dados

                medico.UsuarioID = usr.Id;
                if (isValid) _medicoRepository.Cadastrar(medico);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }

        

        /// <summary>
        /// item 3. cria a agenda de disponibilidade do medico - o sistema deve permitir que 
        /// o medico faça o cadastro e a edição de seus dias e horarios disponiveis para agendamento de consultas.
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

                disp.MedicoID = validaMedico.FirstOrDefault().Id;
                var obterAgenda =  _agendaMedicoRepository.ObterPorMedicoID(disp.MedicoID);
                var obterAgendasBetween = obterAgenda.Where(x=> disponilidade.DataHoraInicio >= x.AgendaTime_ini.GetDate() && disponilidade.DataHoraInicio <= x.AgendaTime_fim.GetDate()).ToList();

                if (obterAgendasBetween.Count > 0)
                    return BadRequest("Agenda está sobrepondo existente.");


                
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
        /// item 6. buscar por médicos - o sistema deve permitir que o paciente visualize a listagem de médico disponiveis. 
        /// </summary>
        /// <returns>array de ProdutoInfo[]</returns>
        [HttpGet("obter_medicos")]
        public IActionResult obter_medicos()
        {
            var lista = _medicoRepository.ObterTodos();

            return Ok(lista);
        }

        /// <summary>
        /// item 7. apos selecionar o medico, o paciente deve visualizar os dias e horarios disponiveis do medico.
        /// </summary>
        /// <returns>array de ProdutoInfo[]</returns>
        [HttpGet("obter_agendamentos_medico/{crmMedico}")]
        public IActionResult obter_agendamentos_medico(string crmMedico)
        {
            List<Medico> validaMedico = _medicoRepository.ObterTodos().Where(o => o.CRM == crmMedico).ToList();

            if (validaMedico == null)
                return NotFound("Médico não encontrado pelo CRM.");

            var lista = _agendaMedicoRepository.ObterPorMedicoID(validaMedico.FirstOrDefault().Id);

            return Ok(lista);
        }

        /// <summary>
        /// item 7. O Paciente poderá selecionar o horario de preferencia e realizar o agendamento
        /// </summary>
        /// <returns>Ok para sucesso ou string com o erro</returns>
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

                var medico = _medicoRepository.ObterPorUsuarioID(agenda.MedicoID);

                _sendEmail.SendEmailAsync("ti.alexandre.costa@gmail.com", "teste de envio", "Olá teste");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }


        /// <summary>
        /// item 8. Apos o agendamento realizado pelo usuario paciente, o medico deverá receber um email.
        /// </summary>
        /// <returns>Ok para sucesso ou string com o erro</returns>
        [HttpPost("notificao_consulta")]
        public IActionResult notificao_consulta(ConsultaDTO consultaDTO)
        {
            try
            {
             
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }


    }
}
