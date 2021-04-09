using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GameModels
{
    public class GreenPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; } = 11;
        public int EndPosition { get; } = 55;
        public string Color { get; } = "Green";
    }
}
