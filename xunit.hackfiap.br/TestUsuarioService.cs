using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using apibronco.bronco.com.br.DTOs;
using apibronco.bronco.com.br.Entity;

namespace xunit.hackfiap
{
    public class TestUsuarioService
    {

        [Fact]
        [Trait("Paciente", "Validando Paciente")]
        public void Validate_CadastrarPaciente()
        {
            // Arrange
            PacienteDTO dto = new PacienteDTO();
            dto.Email = "du.felipe.br@gmail.com";
            dto.Senha = "123456";
            dto.Nome = "carlos oliveira";
            dto.Senha = "@123456cC";
            dto.CPF_CNPJ = "291.995.888-70";
            //act

            var result = "";
            try
            {
                Paciente u = new Paciente(dto);
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }
            
            Assert.Equal("", result);
        }

        [Fact]
        [Trait("Paciente", "Validando Paciente CPF INVALIDO")]
        public void Validate_CPF_Invalido()
        {
            // Arrange
            PacienteDTO dto = new PacienteDTO();
            dto.Email = "du.felipe.br@gmail.com";
            dto.Senha = "123456";
            dto.Nome = "carlos oliveira";
            dto.Senha = "@123456cC";
            dto.CPF_CNPJ = "291995888-71";
            //act

            var result = "";
            try
            {
                Paciente u = new Paciente(dto);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            Assert.Equal("CPF invalido.", result);
        }

        [Fact]
        [Trait("Medico", "Validando Medico")]
        public void Validate_CadastrarMedico()
        {
            // Arrange
            MedicoDTO dto = new MedicoDTO();
            dto.Email = "du.felipe.br@gmail.com";
            dto.Senha = "123456";
            dto.Nome = "carlos oliveira";
            dto.Senha = "@123456cC";
            dto.CPF_CNPJ = "553.111.121-12";
            dto.NumeroCrm = "crm1234";
            
            //act

            var result = "";
            try
            {
                Medico u = new Medico(dto);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            Assert.Equal("", result);
        }

        //[Fact]
        //public void Usuario_Testing_Valid_Email()
        //{
        //    // Arrange

        //    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        //    var email1 = "du343####$%¨&*gmail.com";
        //    var email2 = "du329420.hotmail.com";
        //    var email3 = "du343####$%@¨&*gmail.com";
        //    var email4 = "1234567asdjhaksjd@";
        //    var email5 = "carlos_oliveira@swissre.com";
        //    var email6 = "du.felipe.br@gmail.com";
        //    var email7 = "cadu@box.com";

        //    var expectations = new List<Tuple<object, object>>()
        //    {
        //        new(false, regex.IsMatch(email1)),
        //        new(false, regex.IsMatch(email2)),
        //        new(false, regex.IsMatch(email3)),
        //        new(false, regex.IsMatch(email4)),
        //        new(true, regex.IsMatch(email5)),
        //        new(true, regex.IsMatch(email6)),
        //        new(true, regex.IsMatch(email7)),
        //    };
        //    Assert.All(expectations, pair => Assert.Equal(pair.Item1, pair.Item2));





        //    //var emailRegex = /^[a-z0-9.]+@[a-z0-9]+\.[a-z]+\.([a-z]+)?$/i;

        //    //Assert
        //    ////Assert.Equal(false, regex.IsMatch(email1));
        //    ////Assert.Equal(true, regex.IsMatch(email2));
        //    //Assert.Equal(true, regex.IsMatch(email3));

        //}

        //[Fact]
        //public void Usuario_Testing_Valid_Password()
        //{
        //    // Arrange
        //    //1.Min 8 char and max 14 char
        //    //2.One upper case
        //    //3.Atleast one lower case
        //    //4.No white space
        //    //5.Check for one special character

        //    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        //    var option1 = "123456"; // fail 1 & multiple
        //    var option2 = "@carlos123456"; // fail 2
        //    var option3 = "Abcjdkjdjksdjks903290ls";// fail 1 - max 14
        //    var option4 = "asdjhkjasd"; // fail 1 - max 14
        //    var option5 = "@123 456C";// fail 4
        //    var option6 = "@123456cC";// OK 
            


        //    UsuarioService us = new UsuarioService();
        //    var expectations = new List<Tuple<object, object>>()
        //    {
        //        new(false, us.CheckPassword(option1)),
        //        new(false, us.CheckPassword(option2)),
        //        new(false, us.CheckPassword(option3)),
        //        new(false, us.CheckPassword(option4)),
        //        new(false, us.CheckPassword(option5)),
        //        new(true, us.CheckPassword(option6))

        //    };
        //    Assert.All(expectations, pair => Assert.Equal(pair.Item1, pair.Item2));





        //    //var emailRegex = /^[a-z0-9.]+@[a-z0-9]+\.[a-z]+\.([a-z]+)?$/i;

        //    //Assert
        //    ////Assert.Equal(false, regex.IsMatch(email1));
        //    ////Assert.Equal(true, regex.IsMatch(email2));
        //    //Assert.Equal(true, regex.IsMatch(email3));

        //}
    }
}
