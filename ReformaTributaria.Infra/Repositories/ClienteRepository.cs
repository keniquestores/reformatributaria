using ReformaTributaria.Domain.Entities;
using ReformaTributaria.Domain.Interfaces.Infra.Repositories;
using ReformaTributaria.Infra.Context;

namespace ReformaTributaria.Infra.Repositories
{
    public class ClienteRepository(ReformaTributariaDbContext context) : Repository<Cliente>(context), IClienteRepository
    {
        public async Task<Cliente> GetByQuestorIdAsync(string questorId)
        {
            return await FirstOrDefaultAsync(c => c.QuestorId == questorId);
        }
    }
}
