using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Models
{
    class GamePosition
    {
        public int GamePositionId { get; set; }
        public Game Game { get; set; }
        public User User { get; set; }
        public string Color { get; set; }
        public double Position { get; set; }
    }
}
