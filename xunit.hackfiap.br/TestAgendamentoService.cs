using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Entity;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace xunit.hackfiap
{
    public class TestAgendamentoService
    {

        [Fact]
        [Trait("Agendamento", "Validando Agendamento CRM Vazio")]
        public void Validate_Agendamento_CRMVazio()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "";
            dto.DataHoraInicio = DateTime.Parse("2025-01-01 09:00");
            dto.DataHoraFim = DateTime.Parse("2025-01-01 19:00");


            var result = "";
            try
            {
                AgendaMedico u = new AgendaMedico(dto);
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }
            
            Assert.Equal("CRM não pode ser vazio", result);
        }

        [Fact]
        [Trait("Agendamento", "Validando Agendamento Data hora inicio")]
        public void Validate_Agendamento_DataHoraInicio()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "crm1111";
            dto.DataHoraInicio = DateTime.Now.AddMinutes(-15);
            dto.DataHoraFim = DateTime.Now.AddMinutes(30);


            var result = "";
            try
            {
                AgendaMedico u = new AgendaMedico(dto);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            Assert.Equal("Data invalida, precisa ser superior a data atual ou no mesmo dia.", result);
        }

        [Fact]
        [Trait("Agendamento", "Validando Inicio da agendamento")]
        public void Validate_Agendamento_InicioAgendamento()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "crm1111";
            dto.DataHoraFim = DateTime.Now.AddDays(1);
            dto.DataHoraInicio = DateTime.Now.AddDays(1).AddHours(1);


            var result = "";
            try
            {
                AgendaMedico u = new AgendaMedico(dto);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            Assert.Equal("Inicio da consulta deve ser menor que o final", result);
        }

        [Fact]
        [Trait("Agendamento", "Validando TempoConsulta")]
        public void Validate_Agendamento_TempoConsulta_Menor30()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "crm1111";
            dto.DataHoraInicio = DateTime.Now.AddDays(1);
            dto.DataHoraFim = dto.DataHoraInicio.AddMinutes(15);


            var result = "";
            try
            {
                AgendaMedico u = new AgendaMedico(dto);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            Assert.Equal("O tempo da consulta não pode ser menor que 30 min e maior que 60min.", result);
        }

        [Fact]
        [Trait("Agendamento", "Validando TempoConsulta Maior 60")]
        public void Validate_Agendamento_TempoConsulta_Maior60()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "crm1111";
            dto.DataHoraInicio = DateTime.Now.AddDays(1);
            dto.DataHoraFim = dto.DataHoraInicio.AddDays(1);


            var result = "";
            try
            {
                AgendaMedico u = new AgendaMedico(dto);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            Assert.Equal("O tempo da consulta não pode ser menor que 30 min e maior que 60min.", result);
        }

        [Fact]
        [Trait("Agendamento", "Validando TempoConsulta Correto")]
        public void Validate_Agendamento_TempoConsulta_Correto()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "crm1111";
            dto.DataHoraInicio = DateTime.Now.AddDays(1);
            dto.DataHoraFim = dto.DataHoraInicio.AddHours(1);


            var result = "";
            try
            {
                AgendaMedico u = new AgendaMedico(dto);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            Assert.Equal("", result);
        }

        [Fact]
        [Trait("Agendamento", "Validando CheckSobreposicao")]
        public void Validate_Agendamento_CheckSobreposicao()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "crm1111";
            dto.DataHoraInicio = DateTime.Parse("2025-01-01 09:30");
            dto.DataHoraFim = DateTime.Parse("2025-01-01 10:30");

            List<AgendaMedico> lst = new List<AgendaMedico>();
            lst.Add(new AgendaMedico(
                    dto.CRM, 
                    new AgendaTime(DateTime.Parse("2025-01-01 09:00")), 
                    new AgendaTime(DateTime.Parse("2025-01-01 09:59"))
            ));
            lst.Add(new AgendaMedico(
                    dto.CRM,
                    new AgendaTime(DateTime.Parse("2025-01-01 10:00")),
                    new AgendaTime(DateTime.Parse("2025-01-01 10:59"))
            ));
            lst.Add(new AgendaMedico(
                    dto.CRM,
                    new AgendaTime(DateTime.Parse("2025-01-01 11:00")),
                    new AgendaTime(DateTime.Parse("2025-01-01 11:59"))
            ));
            lst.Add(new AgendaMedico(
                    dto.CRM,
                    new AgendaTime(DateTime.Parse("2025-01-01 13:00")),
                    new AgendaTime(DateTime.Parse("2025-01-01 13:59"))
            ));


            AgendaMedico u = new AgendaMedico(dto);
            Assert.True(u.checkSobrePosicao_Agendamento(lst));
        }


        [Fact]
        [Trait("Agendamento", "Validando CheckSobreposicao Fim")]
        public void Validate_Agendamento_CheckSobreposicao_Fim()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "crm1111";
            dto.DataHoraInicio = DateTime.Parse("2025-01-01 08:30");
            dto.DataHoraFim = DateTime.Parse("2025-01-01 09:29");

            List<AgendaMedico> lst = new List<AgendaMedico>();
            lst.Add(new AgendaMedico(
                    dto.CRM,
                    new AgendaTime(DateTime.Parse("2025-01-01 09:00")),
                    new AgendaTime(DateTime.Parse("2025-01-01 09:59"))
            ));
            lst.Add(new AgendaMedico(
                    dto.CRM,
                    new AgendaTime(DateTime.Parse("2025-01-01 10:00")),
                    new AgendaTime(DateTime.Parse("2025-01-01 10:59"))
            ));
            lst.Add(new AgendaMedico(
                    dto.CRM,
                    new AgendaTime(DateTime.Parse("2025-01-01 11:00")),
                    new AgendaTime(DateTime.Parse("2025-01-01 11:59"))
            ));
            lst.Add(new AgendaMedico(
                    dto.CRM,
                    new AgendaTime(DateTime.Parse("2025-01-01 13:00")),
                    new AgendaTime(DateTime.Parse("2025-01-01 13:59"))
            ));


            AgendaMedico u = new AgendaMedico(dto);
            Assert.True(u.checkSobrePosicao_Agendamento(lst));
        }

        [Fact]
        [Trait("Agendamento", "Validando CheckSobreposicao Livre")]
        public void Validate_Agendamento_CheckSobreposicao_Ok()
        {
            // Arrange
            DisponibilidadeDTO dto = new DisponibilidadeDTO();
            dto.CRM = "crm1111";
            dto.DataHoraInicio = DateTime.Parse("2025-01-01 14:00");
            dto.DataHoraFim = DateTime.Parse("2025-01-01 14:45");

            List<AgendaMedico> lst = new List<AgendaMedico>();
            lst.Add(new AgendaMedico(
                   dto.CRM,
                   new AgendaTime(DateTime.Parse("2025-01-01 09:00")),
                   new AgendaTime(DateTime.Parse("2025-01-01 09:59"))
            ));
            lst.Add(new AgendaMedico(
                 dto.CRM,
                 new AgendaTime(DateTime.Parse("2025-01-01 10:00")),
                 new AgendaTime(DateTime.Parse("2025-01-01 10:59"))
          ));
            lst.Add(new AgendaMedico(
                 dto.CRM,
                 new AgendaTime(DateTime.Parse("2025-01-01 11:00")),
                 new AgendaTime(DateTime.Parse("2025-01-01 11:59"))
          ));
            lst.Add(new AgendaMedico(
                 dto.CRM,
                 new AgendaTime(DateTime.Parse("2025-01-01 13:00")),
                 new AgendaTime(DateTime.Parse("2025-01-01 13:59"))
          ));


            var result = "";
            //try
            //{
                AgendaMedico u = new AgendaMedico(dto);
                Assert.False(u.checkSobrePosicao_Agendamento(lst));
            //}
            //catch (Exception ex)
            //{
            //    result = ex.Message;
            //}


        }





    }
}
