using GameEngine.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GameEngine.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        public int? GamesWon { get; set; }
        public int? GamesLost { get; set; }
        [NotMapped]
        public List<IPiece> Pieces { get; set; }
        public ICollection<GameMember> GameMembers { get; set; }
    }
}
