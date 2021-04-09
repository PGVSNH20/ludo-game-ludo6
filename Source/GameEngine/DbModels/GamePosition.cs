using System.ComponentModel.DataAnnotations;

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
        public int Position { get; set; } = 0;
    }
}
