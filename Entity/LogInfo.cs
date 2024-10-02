using apibronco.bronco.com.br.Interfaces;

namespace apibronco.bronco.com.br.Entity
{
    public class LogFilter
    {
        public DateTime ?Data_Log { get; set; }
        public String ?Tipo_Log { get; set; }
    }


    public class LogInfo : Entidade, IEntidade
    {
        public LogInfo()
        {
            //IsValid();
        }

        public LogInfo(string mensagem, DateTime data_Log, string tipo_Log, string? stack_Trace, string module_Name)
        {
            Mensagem = mensagem;
            Data_Log = data_Log;
            Tipo_Log = tipo_Log;
            Stack_Trace = stack_Trace;
            Module_Name = module_Name;

            IsValid();
        }

        public string Mensagem { get; set; }
        public DateTime Data_Log { get; set; }

        public String Tipo_Log { get; set; }

        public string Stack_Trace { get; set; }

        public string Module_Name { get; set; }

        public override bool IsValid() {
            if (Tipo_Log != "Error" && Tipo_Log != "Information" && Tipo_Log != "Warning")
                throw new ArgumentException("Tipo_Log aceita somente os seguintes critérios: Error, Information, Warning");


            AssertionConcern.AssertArgumentNotEmpty(Mensagem, "Mensagem precisa ser preenchido.");
            AssertionConcern.AssertArgumentLength(Mensagem, 500, "Mensagem precisa ter no maximo 50 caracteres.");

            AssertionConcern.AssertArgumentNotEmpty(Module_Name, "Module_Name precisa ser preenchido.");
            AssertionConcern.AssertArgumentLength(Module_Name, 100, "Module_Name precisa ter no maximo 100 caracteres.");

           AssertionConcern.AssertArgumentTrue(Data_Log > new DateTime(2020,1,1), "Data do Log precisa ser maior que 2020-01-01");


            return true;
        }

    }

   
}
