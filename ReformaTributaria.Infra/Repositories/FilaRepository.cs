using Microsoft.EntityFrameworkCore;
using ReformaTributaria.Domain.Entities;
using ReformaTributaria.Domain.Interfaces.Infra.Repositories;
using ReformaTributaria.Infra.Context;

namespace ReformaTributaria.Infra.Repositories
{
    public class FilaRepository(ReformaTributariaDbContext context) : Repository<Fila>(context), IFilaRepository
    {

        public async Task<Fila?> GetByInscricaoFederalContribuinte(string inscricaoFederalContribuinte)
        {
            return await FirstOrDefaultAsync(f => f.InscricaoFederalContribuinte == inscricaoFederalContribuinte);
        }

        public async Task<Fila?> GetByClienteQuestorIdEInscFederalContribuinteAsync(string inscricaoFederalContribuinte, string questorId)
        {
            return await _dbSet
                .Include(f => f.Cliente)
                .FirstOrDefaultAsync(f => f.Cliente.QuestorId == questorId && f.InscricaoFederalContribuinte == inscricaoFederalContribuinte);
        }

        public async Task<bool> AtivarOuInativar(bool ativo, string inscricaoFederalContribuinte, string questorId)
        {
            var fila = await GetByClienteQuestorIdEInscFederalContribuinteAsync(inscricaoFederalContribuinte, questorId);
            if (fila == null)
                return false;
            
            fila.Ativo = ativo;
            await UpdateAsync(fila);
            return fila.Ativo;
        }

        public async Task<IEnumerable<Fila>> GetFilasAtivasAsync(int skip, int take)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(f => f.Cliente)
                .Where(f => f.Ativo)
                .OrderBy(f => f.InscricaoFederalContribuinte)
                .ThenBy(f => f.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

    }
}