using GameEngine.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.DataAccess
{
    class LudoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Piece> Pieces { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePlayers> GamePlayers { get; set; }
        public DbSet<GamePosition> GamePositions { get; set; }
    }
}
