using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Financier.Domain.Portfolios;
using Financier.Domain.Stocks;
using Financier.Domain.Trades;

namespace Financier.Data
{
    public class FinancierDbContext : DbContext
    {
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<OpenTrade> OpenTrades { get; set; }
        public DbSet<CloseTrade> CloseTrades { get; set; }
        public DbSet<Quote> Quotes { get; set; }


        public FinancierDbContext(DbContextOptions<FinancierDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quote>()
                .HasKey(x => new { x.SecurityId, x.DateTime, x.Period });

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(28,12)");
            }
        }
    }
}
