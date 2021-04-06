using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GameModels
{
    class GreenPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; set; } = 11;
        public int EndPosition { get; set; } = 55;
    }
}
