using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GameModels
{
    public class RedPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; } = 21;
        public int EndPosition { get; } = 65;
        public string Color { get; } = "Red";
    }
}
