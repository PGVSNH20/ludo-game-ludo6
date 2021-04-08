using GameEngine.DbModels;
using GameEngine.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.DataAccess
{
    public class LudoDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GamePosition> GamePositions { get; set; }
        public virtual DbSet<GameMember> GameMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=LudoDb;User Id=sa;Password=YourStrong@Passw0rd");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameMember>().HasKey(gm => new { gm.GameId, gm.UserId });
            modelBuilder.Entity<GameMember>().HasOne(gm => gm.Game).WithMany(g => g.GameMembers).HasForeignKey(gm => gm.GameId);
            modelBuilder.Entity<GameMember>().HasOne(gm => gm.User).WithMany(u => u.GameMembers).HasForeignKey(gm => gm.UserId);
        }
    }
}
