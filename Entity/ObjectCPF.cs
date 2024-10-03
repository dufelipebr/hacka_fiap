using System.Text.RegularExpressions;

namespace apibronco.bronco.com.br.Entity
{
    public class ObjectCPF 
    {
        private string _cpf;
        public ObjectCPF(string cpf) 
        { 

            _cpf = cpf;
            isValid();
        }

        public void isValid() 
        {
            Regex regex = new Regex(@"^\d{3}.\d{3}.\d{3}-\d{2}$");
            AssertionConcern.AssertArgumentFalse(!regex.IsMatch(_cpf), "CPF invalido.");
        }
    }
}