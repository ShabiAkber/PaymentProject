using Microsoft.EntityFrameworkCore;
using PaymentProcedureData.Entities;

namespace PaymentProcedureData.DatabaseContext
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbContext Instance => this;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PaymentProcess>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Status>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<PaymentStatus>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<UserLogin>().HasQueryFilter(x => !x.IsDeleted);
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<PaymentProcess> PaymentProcesses { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
    }
}