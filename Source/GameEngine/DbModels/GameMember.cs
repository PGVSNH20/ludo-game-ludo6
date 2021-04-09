using GameEngine.Models;
using System.ComponentModel.DataAnnotations;

namespace GameEngine.DbModels
{
    public class GameMember
    {
        //public int GameMembersId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Piece Piece { get; set; }
    }
}
