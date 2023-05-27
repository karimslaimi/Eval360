using Eval360.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Poste> Poste { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<AxeEval> AxeEval { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Compagnie> Compagnie { get; set; }
        public DbSet<CompagnieQuestion> CompagnieQuestions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Poste>().HasOne(p=>p.Direction).WithMany(d=>d.postes).HasForeignKey(x=>x.IdDirection).OnDelete(DeleteBehavior.SetNull);
            base.OnModelCreating(builder);
        }
    }
}