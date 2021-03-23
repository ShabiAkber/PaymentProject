using Microsoft.EntityFrameworkCore;
using PaymentProcedureData.Entities;
using System;

namespace PaymentProcedureData.DatabaseContext
{
    public interface IAppDbContext : IDisposable
    {
        DbContext Instance { get; }

        public DbSet<PaymentProcess> PaymentProcesses { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
    }
}
