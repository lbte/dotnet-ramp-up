using Microsoft.EntityFrameworkCore;
using PRFTLatam.EmploymentInfo.Domain.Models;

namespace PRFTLatam.EmploymentInfo.Domain;

public class EmploymentInfoContext : DbContext
{
    public DbSet<Developer> Developers { get; set; }
    public EmploymentInfoContext (DbContextOptions<EmploymentInfoContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder) 
    {
        if (builder == null) {
            return;
        }

        builder.Entity<Developer>().ToTable("Developer").HasKey(k => k.Email);
        builder.Entity<Developer>().Property(p => p.FirstName).IsRequired().HasMaxLength(20);
        builder.Entity<Developer>().Property(p => p.LastName).IsRequired().HasMaxLength(30);
        builder.Entity<Developer>().Property(p => p.Email).IsRequired();
        
        base.OnModelCreating(builder);
    }
    
}