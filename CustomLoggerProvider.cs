using apibronco.bronco.com.br.Interfaces;
using apibronco.bronco.com.br.Repository;
using System.Collections.Concurrent;
using System.Data.SqlTypes;

namespace apibronco.bronco.com.br
{
    public class CustomLoggerProvider : ILoggerProvider 
    {
        private readonly CustomLoggerProviderConfiguration _loggerConfig;
        private readonly ConcurrentDictionary<string, CustomLogger> _loggers = new ConcurrentDictionary<string, CustomLogger>();
        private ILogRepository _logRepository;

        public CustomLoggerProvider(CustomLoggerProviderConfiguration cf, ILogRepository logRepository) 
        {
            this._loggerConfig = cf;
            this._logRepository = logRepository;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, nome => new CustomLogger(nome, _loggerConfig, _logRepository));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        
    }
}
