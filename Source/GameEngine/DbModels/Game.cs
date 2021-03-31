using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GameEngine.Models
{
    public class Game
    {
        public int GameId { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }
        public User NextToRollDice { get; set; }

    }
}
