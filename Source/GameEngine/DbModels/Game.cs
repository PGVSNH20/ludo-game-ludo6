using GameEngine.DbModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameEngine.Models
{
    public class Game
    {
        public int GameId { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; } = true;
        public User NextToRollDice { get; set; }
        public ICollection<GameMember> GameMembers { get; set; }
        public User Winner { get; set; }

    }
}
