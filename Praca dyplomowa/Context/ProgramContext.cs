using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Praca_dyplomowa.Entities;
using System;

namespace Praca_dyplomowa.Context
{
    public class ProgramContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public ProgramContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;port=3306;database=organizer;uid=root;password=admin",
            builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Route> Routes { get; set; }
    }
}
