using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Entity;
using apibronco.bronco.com.br.Interfaces;
using fiap_hacka.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
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


        public MedicoController(IMedicoRepository medicoRepository, 
            IUsuarioRepository usuarioRepository,
            IConsultaRepository consultaRepository,
            IAgendaMedicoRepository agendaMedicoRepository,
            IPacienteRepository pacienteRepository)
        {
            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
            _agendaMedicoRepository = agendaMedicoRepository;
            _pacienteRepository = pacienteRepository;
            _consultaRepository = consultaRepository;
        }

        /// <summary>
        /// item 1. cadastro do usuario (medico) - o medico poderá se cadastar preennchedo os campos obrigatorios
        /// </summary>
        /// <param name="info">informações do usuario segurado que será criado</param>
        /// <returns>Ok - sucesso</returns>
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
        /// item 6. buscar por médicos - o sistema deve permitir que o paciente visualize a listagem de médico disponiveis. 
        /// </summary>
        /// <returns>array de ProdutoInfo[]</returns>
        //[Authorize(Roles = "Medico")]
        [HttpGet("listar_medicos")]
        public IActionResult listar_medicos()
        {
            var lista = _medicoRepository.ObterTodos();

            return Ok(lista);
        }



        /// <summary>
        /// item 3. cria a agenda de disponibilidade do medico - o sistema deve permitir que 
        /// o medico faça o cadastro e a edição de seus dias e horarios disponiveis para agendamento de consultas.
        /// </summary>
        /// <returns>Ok para sucesso ou string com o erro</returns>
        [Authorize(Roles = "Medico")]
        [HttpPost("criar_agenda_medico")]
        public IActionResult criar_agenda_medico(DisponibilidadeDTO disponilidade)
        {
            try
            {
                // cadastrar disponibilidade do médico
                AgendaMedico disp = new AgendaMedico(disponilidade);

                var validaMedico = (Medico) _medicoRepository.ObterTodos().Where(o => o.CRM == disp.CRM_BeforeLoad).FirstOrDefault();

                if (validaMedico == null)
                    return NotFound("Médico não encontrado pelo CRM.");

                disp.MedicoID = validaMedico.Id;
                var obterAgenda =  _agendaMedicoRepository.ObterPorMedicoID(disp.MedicoID);

                //var obterAgendasBetween = obterAgenda.Where(x=> disponilidade.DataHoraInicio >= x.AgendaTime_ini.GetDate() && disponilidade.DataHoraInicio <= x.AgendaTime_fim.GetDate()).ToList();
                //if (obterAgendasBetween.Count > 0)
                bool checkSobreposicao = disp.checkSobrePosicao_Agendamento(obterAgenda);
                if (checkSobreposicao)  return BadRequest("Agenda está sobrepondo existente.");
               
                _agendaMedicoRepository.Cadastrar(disp);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }

        [Authorize(Roles = "Medico")]
        [HttpPut("alterar_agendamentos_medico")]
        public IActionResult alterar_agendamentos_medicos(AlterarDisponibilidadeDTO disponilidade)
        {
            try
            {
                var agendamento = _agendaMedicoRepository.ObterPorId(disponilidade.IdAgenda);

                if (agendamento == null)
                    return NotFound("Agendamento não encontrado!");

                if (agendamento.flagReservado == true)
                    return BadRequest("Agendamento com consulta marcada não pode ser alterada!");

                AgendaMedico agendaMedico = new AgendaMedico(disponilidade);
                agendamento.AgendaTime_ini = agendaMedico.AgendaTime_ini;
                agendamento.AgendaTime_fim = agendaMedico.AgendaTime_fim;

                // check sobreposição
                var obterAgenda = _agendaMedicoRepository.ObterPorMedicoID(agendamento.MedicoID);
                bool checkSobreposicao = agendamento.checkSobrePosicao_Agendamento(obterAgenda);
                if (checkSobreposicao) return BadRequest("Agenda está sobrepondo existente.");

                _agendaMedicoRepository.Alterar(agendamento);
                return Ok("Ok");
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
        [Authorize(Roles = "Medico")]
        [HttpGet("obter_agendamentos_medico/{crmMedico}")]
        public IActionResult obter_agendamentos_medico(string crmMedico)
        {
            List<Medico> validaMedico = _medicoRepository.ObterTodos().Where(o => o.CRM == crmMedico).ToList();

            if (validaMedico == null)
                return NotFound("Médico não encontrado pelo CRM.");

            var lista = _agendaMedicoRepository.ObterPorMedicoID(validaMedico.FirstOrDefault().Id);

            return Ok(lista);
        }

      
    }
}
