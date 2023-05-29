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

            builder.Entity<Compagnie>().HasOne(e => e.employee).WithMany(x => x.compagnies).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Compagnie>().HasMany(e => e.compagnieUser).WithOne(x => x.compagnie);

            builder.Entity<CompagnieQuestion>().HasOne(c => c.compagnie).WithMany(q => q.compagnieQuestions);
            builder.Entity<CompagnieQuestion>().HasMany(x => x.reponses).WithOne(x => x.CompagnieQuestion);
            builder.Entity<CompagnieQuestion>().HasOne(x => x.question).WithMany(x => x.compagnieQuestions);

            builder.Entity<CompagnieReponse>().HasOne(x => x.user).WithMany(x => x.compagnieReponses);

            builder.Entity<CompagnieUser>().HasOne(x=>x.user).WithMany(x=>x.compagnieUser);

            base.OnModelCreating(builder);
        }
    }
}