using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class ClientService : IClientSevice
    {
        private readonly IClientRepo _clientRepo;

        public ClientService(IClientRepo clientRepo)
        {
            _clientRepo = clientRepo;
        }

        private static ClientViewModel ClientMapper(ClientModel client)
        {
            return new()
            {
                Id = client.Id,
                Name = client.Name,
                ProjectId = client.ProjectId,
            };
        }
        public async Task<ClientViewModel> AddAsync(AddClient addClient, Guid userId)
        {
            return ClientMapper(await _clientRepo.AddAsync(new ClientModel
            {
                Name = addClient.Name,
                ProjectId = addClient.projectId,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            }));
        }
        public async Task<ClientViewModel> UpdateAsync(EditClient editClient, Guid userId)
        {
            var client = await _clientRepo.GetByIdAsync(editClient.Id) ?? throw new InnerException("No CLient Found", StatusType.NOT_FOUND);
            client.Name = editClient.Name;
            client.UpdatedOn = DateTime.Now;
            client.UpdatedBy = userId;
            return ClientMapper(await _clientRepo.UpdateAsync(client));
        }
        public async Task<bool> DeleteAsync(Guid Id)
        {
            var client = await _clientRepo.GetByIdAsync(Id) ?? throw new InnerException("No Client Found", StatusType.NOT_FOUND);
            if (await _clientRepo.RemoveAsync(client) > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<ClientViewModel>> GetAllAsync(Guid ProjectId)
        {
            return (await _clientRepo.GetAllAsync(ProjectId))
                .Select(client => ClientMapper(client))
                .ToList();
        }
    }
}
