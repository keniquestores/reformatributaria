using Microsoft.EntityFrameworkCore;
using ReformaTributaria.CrossCutting.ExecutionContext.Abstractions;

namespace ReformaTributaria.CrossCutting.EFCore
{
    public sealed class EFTransactionalContext<TContext>(TContext context)
    : ITransactionalContext
    where TContext : DbContext
    {
        private readonly TContext _context = context;

        public Task BeginAsync()
            => _context.Database.BeginTransactionAsync();

        public Task SaveAsync()
            => _context.SaveChangesAsync();

        public async Task CommitAsync()
        {
            if (_context.Database.CurrentTransaction != null)
                await _context.Database.CommitTransactionAsync();
        }

        public Task RollbackAsync()
        {
            if (_context.Database.CurrentTransaction == null)
                return Task.CompletedTask;

            return _context.Database.RollbackTransactionAsync();
        }
    }

}
