using MongoDB.Driver.Core.Operations;
using System.Text.RegularExpressions;

namespace apibronco.bronco.com.br.Entity
{
    public class ObjectEmail 
    {
        private string _email;
        public ObjectEmail(string email) 
        { 

            _email = email;
            isValid();
        }

        public string? Email_Value {
            get {
                return _email;
            }
        }

        public void isValid() 
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            AssertionConcern.AssertArgumentFalse(!regex.IsMatch(_email), "Email invalido.");
            //Email_Value = _email;
        }

        public override string ToString()
        {
            return _email.ToString();
        }
    }
}