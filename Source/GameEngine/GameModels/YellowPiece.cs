using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GameModels
{
    public class YellowPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; set; } = 31;
        public int EndPosition { get; set; } = 75;
    }
}
