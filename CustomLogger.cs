
using apibronco.bronco.com.br.Interfaces;
using System.Diagnostics;

namespace apibronco.bronco.com.br
{
    public class CustomLogger : ILogger
    {
        private readonly string _loggerName;
        private readonly CustomLoggerProviderConfiguration _cfg;
        private ILogRepository _logrepository;
        //private readonly ILogRepository logRepository;

        public CustomLogger(string nome, CustomLoggerProviderConfiguration cfg, ILogRepository logRepository) 
        {
            this._cfg = cfg;
            this._loggerName = nome;
            this._logrepository = logRepository;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel,  EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var mensagem = string.Format($"{DateTime.Now}-{logLevel}:{eventId} - {formatter(state, exception)}");

            if (logLevel == LogLevel.Error) 
            { 
                _logrepository.Cadastrar(new Entity.LogInfo()
                {
                    Mensagem = mensagem,
                    Data_Log = DateTime.Now,
                    Tipo_Log = logLevel.ToString(),
                    Module_Name = "API",
                    Stack_Trace = (exception != null ? exception.StackTrace : "")
                });
                Debug.WriteLine(mensagem);
            }
            ////EscreverArquivo(mensagem); // utilizado para escrever no arquivo ao inves da base de dados
        }

        //private void EscreverArquivo(string mensagem)
        //{
        //    var caminhoArquivo = @$"C:\temp\Log-{DateTime.Now:yyyy-MM-dd}.txt";
        //    if (!File.Exists(caminhoArquivo))
        //    {
        //        Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
        //        File.Create(caminhoArquivo).Dispose();
        //    }

        //    using StreamWriter streamWriter = new StreamWriter(caminhoArquivo, true);
        //    streamWriter.WriteLine(mensagem);
        //    streamWriter.Close();
        //}
    }
}
