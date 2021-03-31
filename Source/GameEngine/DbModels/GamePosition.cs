using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GameEngine.Models
{
    public class GamePosition
    {
        public int GamePositionId { get; set; }

        [Required]
        public Game Game { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public double Position { get; set; } = 0;
    }
}
