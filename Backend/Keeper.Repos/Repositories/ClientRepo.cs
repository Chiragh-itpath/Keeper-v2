using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class ClientRepo : IClientRepo
    {
        private readonly DbKeeperContext _context;

        public ClientRepo(DbKeeperContext context)
        {
            _context = context;
        }
        public async Task<ClientModel> AddAsync(ClientModel client)
        {
            await _context.AddAsync(client);
            await _context.SaveChangesAsync();
            return client;
        }
        public async Task<ClientModel> UpdateAsync(ClientModel client)
        {
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return client;
        }
        public async Task<int> RemoveAsync(ClientModel client)
        {
            _context.Remove(client);
            return await _context.SaveChangesAsync();
        }
        public async Task<List<ClientModel>> GetAllAsync(Guid ProjectId)
        {
            return await _context.Client
                .AsNoTracking()
                .Where(x => x.ProjectId == ProjectId)
                .ToListAsync();
        }
        public async Task<ClientModel?> GetByIdAsync(Guid Id)
        {
            return await _context.Client
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }
    }
}
