using System;
using System.Configuration;
using JazzOpsApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JazzOpsApp.DataAccess
{
    public class JazzDbContextFactory : IDesignTimeDbContextFactory<JazzDbContext>
    {
        public JazzDbContext CreateDbContext(string[] args)
        {

            var builder = new DbContextOptionsBuilder<JazzDbContext>();

            return new JazzDbContext("Server=.;Database=EFCoreDemo;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }

    public partial class JazzDbContext : DbContext
    {
        private string connectionString;
        public JazzDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public JazzDbContext(DbContextOptions<JazzDbContext> options)
            : base(options)
        {
        }

        public DbSet<IotNet> IoTNets { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Measurements> Measurements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                try
                {
                    optionsBuilder.UseSqlServer(connectionString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);                    
                }
                
            }
        }

    }
}
