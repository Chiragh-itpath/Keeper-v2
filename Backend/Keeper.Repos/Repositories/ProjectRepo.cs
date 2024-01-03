using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class ProjectRepo : IProjectRepo
    {
        private readonly DbKeeperContext _db;

        public ProjectRepo(DbKeeperContext dbKeeperContext)
        {
            _db = dbKeeperContext;
        }

        public async Task<List<ProjectModel>> GetAllAsync(Guid UserId)
        {
            List<ProjectModel> projects = await (
                    from p in _db.Projects
                    .Include(p => p.Tag)
                    .Include(p => p.CreatedBy)
                    .Include(p => p.UpdatedBy)
                    .AsNoTracking()
                    join sp in _db.SharedProjects on
                        new { ProjectId = p.Id, UserId, IsAccepted = true } equals
                        new { sp.ProjectId, sp.UserId, sp.IsAccepted } into sharedProject
                    from sp in sharedProject.DefaultIfEmpty()
                    join sk in _db.SharedKeeps on
                        new { ProjectId = p.Id, UserId, IsAccepted = true } equals
                        new { sk.ProjectId, sk.UserId, sk.IsAccepted } into shareKeepProjects
                    from sk in shareKeepProjects.DefaultIfEmpty()
                    where p.CreatedById == UserId || sp != null || sk != null
                    where !p.IsDeleted
                    select p
                )
                .Distinct()
                .ToListAsync();
            return projects;
        }

        public async Task<ProjectModel?> GetByIdAsync(Guid Id)
        {
            return await _db.Projects
                .Include(p => p.Tag)
                .Include(p => p.CreatedBy)
                .Include(p => p.UpdatedBy)
                .Where(p => p.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> SaveAsync(ProjectModel project)
        {
            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();
            return project.Id;
        }

        public async Task<Guid> UpdateAsync(ProjectModel project)
        {
            _db.Entry(project).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return project.Id;
        }

        public async Task DeleteAsync(ProjectModel project)
        {
            project.IsDeleted = true;
            await UpdateAsync(project);
        }
    }
}
