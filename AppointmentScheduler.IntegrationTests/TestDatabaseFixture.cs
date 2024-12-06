using AppointmentScheduler.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.IntegrationTests
{
    public class TestDatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        private readonly string _connectionString =
            "Host=localhost;Database=AppointmentSchedulingTestDb;Username=testuser;Password=testpass";

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }
                    _databaseInitialized = true;
                }
            }
        }

        public SchedulerDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SchedulerDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            var context = new SchedulerDbContext(options);
            return context;
        }

        public void Dispose()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
            }
        }
    }

}
