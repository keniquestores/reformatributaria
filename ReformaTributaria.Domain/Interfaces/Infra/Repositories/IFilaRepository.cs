using ReformaTributaria.Domain.Entities;

namespace ReformaTributaria.Domain.Interfaces.Infra.Repositories
{
    public interface IFilaRepository : IRepository<Fila>
    {

        Task<bool> AtivarOuInativar(bool ativo, string inscricaoFederal, string questorId);

        Task<Fila?> GetByInscricaoFederalContribuinte(string inscricaoFederalContribuinte);

        Task<Fila?> GetByClienteQuestorIdEInscFederalContribuinteAsync(string inscricaoFederalContribuinte, string questorId);

        Task<IEnumerable<Fila>> GetFilasAtivasAsync(int skip, int take);

    }
}
