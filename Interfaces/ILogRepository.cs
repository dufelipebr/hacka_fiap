
using apibronco.bronco.com.br.Entity;

namespace apibronco.bronco.com.br.Interfaces
{
    public interface ILogRepository : IRepository<LogInfo>
    {
        public IList<LogInfo> ObterTodosByFilter(LogFilter filter);
    }
}
