using System;
using Microsoft.EntityFrameworkCore;
using MovieRental.Core.Account;
using MovieRental.Core.Movie;

namespace MovieRental.Persitence
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieLikesEntity>()
                .HasKey(pl => new { pl.MovieId, pl.AccountId });

            modelBuilder.Entity<MovieLikesEntity>()
                .HasOne(pl => pl.Movie)
                .WithMany(p => p.AccountLikes)
                .HasForeignKey(a => a.MovieId);

            modelBuilder.Entity<MovieLikesEntity>()
                .HasOne(pl => pl.Account)
                .WithMany(p => p.LikedProducts)
                .HasForeignKey(a => a.AccountId);

            modelBuilder.Entity<MovieEntity>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<AccountEntity>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PriceUpdateLogEntity>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<PurchaseLogEntity>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<RentalLogEntity>()
              .Property(p => p.Id)
              .ValueGeneratedOnAdd();
        }



        public DbSet<MovieEntity> Movie { get; set; }
        public DbSet<AccountEntity> Account { get; set; }
        public DbSet<MovieLikesEntity> MovieLikes { get; set; }

        public DbSet<PriceUpdateLogEntity> PriceUpdatesLog { get; set; }
        public DbSet<PurchaseLogEntity> PurchaseLog { get; set; }

        public DbSet<RentalLogEntity> RentalLog { get; set; }

    }
}
