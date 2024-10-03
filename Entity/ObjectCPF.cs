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

        public string? CPF_Value { get; set; }
        public void isValid() 
        {
            CPF_Value = "";
            Regex regex = new Regex(@"^\d{3}.\d{3}.\d{3}-\d{2}$");
            AssertionConcern.AssertArgumentFalse(!regex.IsMatch(_cpf), "CPF invalido.");
            CPF_Value = _cpf;
        }

        public override string ToString()
        {
            if (_cpf != null)
                return _cpf.ToString();

            return "";
        }
    }
}