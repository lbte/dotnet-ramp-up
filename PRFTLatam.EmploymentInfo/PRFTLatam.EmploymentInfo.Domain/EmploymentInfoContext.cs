using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using PRFTLatam.EmploymentInfo.Domain.Models;

namespace PRFTLatam.EmploymentInfo.Domain;

public class EmploymentInfoContext : DbContext
{
    public EmploymentInfoContext () : base()
    {
    }
    public EmploymentInfoContext (DbContextOptions<EmploymentInfoContext> options) : base(options)
    {
    }
    public DbSet<Developer> Developers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) 
    {
        if (builder == null) {
            return;
        }

        builder.Entity<Developer>().ToTable("Developer").HasKey(k => k.Email);
        builder.Entity<Developer>().Property(p => p.FirstName).IsRequired().HasMaxLength(20);
        builder.Entity<Developer>().Property(p => p.LastName).IsRequired().HasMaxLength(30);
        builder.Entity<Developer>().Property(p => p.Email).IsRequired();
        builder.Entity<Developer>().Property(p => p.SalaryByHours).HasColumnType("decimal(18,4)");
        
        base.OnModelCreating(builder);
    }
    
}