using Microsoft.EntityFrameworkCore;

namespace UserAuthentication.Models
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure relationships
            modelBuilder.Entity<User>()
                .HasOne(e => e.UserLogin)
                .WithOne(e => e.User)
                .HasForeignKey<UserLogin>(e => e.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(e => e.UserLoginDataExternal)
                .WithOne(e => e.User)
                .HasForeignKey<UserLoginDataExternal>(e => e.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId)
                .IsRequired(true);

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

           

    


            // Insert role data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "admin"  },
                new Role { Id = 2, RoleName = "user"   },
                new Role { Id = 3, RoleName = "guest"  },
                new Role { Id = 4, RoleName = "viewer" }
                );

            modelBuilder.Entity<HashAlgorithm>().HasData(
                new HashAlgorithm { Id = 1, AlgorithmName = "SHA256" }
                );
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserLogin> UserLogins { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<HashAlgorithm> HashAlgorithms { get; set; } = null!;

        public DbSet<ExternalProvider> ExternalProviders { get; set; } = null!;

        public DbSet<UserState> UserStates { get; set; } = null!;

        
    }

    
}
