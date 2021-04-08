using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GameModels
{
    public class RedPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; set; } = 21;
        public int EndPosition { get; set; } = 65;
        public string Color { get; set; } = "Red";
    }
}
