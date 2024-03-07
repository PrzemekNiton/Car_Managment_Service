using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using SimpleCarManagment.Models.SimpleCarManagmentDb;

namespace SimpleCarManagment.Data
{
  public partial class SimpleCarManagmentDbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public SimpleCarManagmentDbContext(DbContextOptions<SimpleCarManagmentDbContext> options):base(options)
    {
    }

    public SimpleCarManagmentDbContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .HasOne(i => i.Service)
              .WithMany(i => i.CarServices)
              .HasForeignKey(i => i.ServiceId)
              .HasPrincipalKey(i => i.Id);
        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .HasOne(i => i.Car)
              .WithMany(i => i.CarServices)
              .HasForeignKey(i => i.CarId)
              .HasPrincipalKey(i => i.Id);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.Car>()
              .Property(p => p.UserId)
              .HasDefaultValueSql("(N'')");

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.Car>()
              .Property(p => p.Registration)
              .HasDefaultValueSql("(N'')");

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .Property(p => p.Document)
              .HasDefaultValueSql("(N'')");

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .Property(p => p.Mileage)
              .HasDefaultValueSql("((0.0000000000000000e+000))");


        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.Car>()
              .Property(p => p.Production)
              .HasColumnType("datetime2");

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .Property(p => p.Date)
              .HasColumnType("datetime2");

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.Car>()
              .Property(p => p.Id)
              .HasPrecision(10, 0);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.Car>()
              .Property(p => p.Mileage)
              .HasPrecision(53, 0);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .Property(p => p.Id)
              .HasPrecision(10, 0);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .Property(p => p.Cost)
              .HasPrecision(18, 2);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .Property(p => p.ServiceId)
              .HasPrecision(10, 0);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .Property(p => p.CarId)
              .HasPrecision(10, 0);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService>()
              .Property(p => p.Mileage)
              .HasPrecision(53, 0);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.Service>()
              .Property(p => p.Id)
              .HasPrecision(10, 0);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.Service>()
              .Property(p => p.Km)
              .HasPrecision(53, 0);

        builder.Entity<SimpleCarManagment.Models.SimpleCarManagmentDb.Service>()
              .Property(p => p.Years)
              .HasPrecision(10, 0);
        this.OnModelBuilding(builder);
    }


    public DbSet<SimpleCarManagment.Models.SimpleCarManagmentDb.Car> Cars
    {
      get;
      set;
    }

    public DbSet<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService> CarServices
    {
      get;
      set;
    }

    public DbSet<SimpleCarManagment.Models.SimpleCarManagmentDb.Service> Services
    {
      get;
      set;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
    }
  }
}
