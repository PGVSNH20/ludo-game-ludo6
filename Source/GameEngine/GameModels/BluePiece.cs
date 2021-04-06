using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GameModels
{
    class BluePiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; set; } = 1;
        public int EndPosition { get; set; } = 45;
    }
}
