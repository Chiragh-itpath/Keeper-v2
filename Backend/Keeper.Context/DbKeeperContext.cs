using Keeper.Context.Model;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Context
{
    public class DbKeeperContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<KeepModel> Keeps { get; set; }
        public DbSet<ItemModel> Items { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<SharedProjectsModel> SharedProjects { get; set; }
        public DbSet<SharedKeepsModel> SharedKeeps { get; set; }
        public DbSet<ItemFileLinkerModel> ItemFileLinker { get; set; }
        public DbSet<CommentModel> Comment { get; set; }
        public DbSet<ContactModel> Contact { get; set; }
        public DbSet<GroupModel> Group { get; set; }
        public DbSet<ContactGroupLinkerModel> ContactGroupLinkers { get; set; }
        public DbKeeperContext(DbContextOptions options) : base(options) { }
    }
}
