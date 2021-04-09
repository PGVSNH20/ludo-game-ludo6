using GameEngine.DbModels;
using GameEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace GameEngine.DataAccess
{
    public class LudoDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GamePosition> GamePositions { get; set; }
        public virtual DbSet<GameMember> GameMembers { get; set; }
        public virtual DbSet<Piece> Pieces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=LudoDb;User Id=sa;Password=YourStrong@Passw0rd");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameMember>().HasKey(gm => new { gm.GameId, gm.UserId });
            modelBuilder.Entity<GameMember>().HasOne(gm => gm.Game).WithMany(g => g.GameMembers).HasForeignKey(gm => gm.GameId);
            modelBuilder.Entity<GameMember>().HasOne(gm => gm.User).WithMany(u => u.GameMembers).HasForeignKey(gm => gm.UserId);

            modelBuilder.Entity<Piece>().HasData(
                new Piece { PieceId = 1, Color = "Blue" },
                new Piece { PieceId = 2, Color = "Green" },
                new Piece { PieceId = 3, Color = "Red" },
                new Piece { PieceId = 4, Color = "Yellow" }
            );
        }
    }
}
