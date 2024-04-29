using Keeper.Context.Model;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Context
{
    public class DbKeeperContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; } = default!;
        public DbSet<ProjectModel> Projects { get; set; } = default!;
        public DbSet<KeepModel> Keeps { get; set; } = default!;
        public DbSet<ItemModel> Items { get; set; } = default!;
        public DbSet<TagModel> Tags { get; set; } = default!;
        public DbSet<FileModel> Files { get; set; } = default!;
        public DbSet<SharedProjectsModel> SharedProjects { get; set; } = default!;
        public DbSet<SharedKeepsModel> SharedKeeps { get; set; } = default!;
        public DbSet<ItemFileLinkerModel> ItemFileLinker { get; set; } = default!;
        public DbSet<CommentModel> Comment { get; set; } = default!;
        public DbSet<ContactModel> Contact { get; set; } = default!;
        public DbSet<GroupModel> Group { get; set; } = default!;
        public DbSet<ContactGroupLinkerModel> ContactGroupLinkers { get; set; } = default!;
        public DbSet<ClientModel> Client { get; set; } = default!;
        public DbSet<ItemStatusModel> ItemStatus { get; set; } = default!;
        public DbSet<RuleBookModel> RuleBook { get; set; } = default!;
        public DbKeeperContext(DbContextOptions options) : base(options) { }
    }
}
