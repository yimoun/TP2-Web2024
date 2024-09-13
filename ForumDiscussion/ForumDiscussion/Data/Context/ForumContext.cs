using Microsoft.EntityFrameworkCore;
using ForumDiscussion.Models;

namespace ForumDiscussion.Data.Context
{
    public class ForumContext : DbContext
    {
        public DbSet<Membre> Membre { get; set; }
        public DbSet<MessageModel> Message { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Sujet> Sujet { get; set; }
        public ForumContext(DbContextOptions<ForumContext>options) : base(options) { }
    }
}
