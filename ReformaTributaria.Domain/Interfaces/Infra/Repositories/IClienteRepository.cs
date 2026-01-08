using ReformaTributaria.Domain.Entities;

namespace ReformaTributaria.Domain.Interfaces.Infra.Repositories
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente?> GetByQuestorIdAsync(string questorId);
    }
}
