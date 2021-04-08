using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GameModels
{
    public class BluePiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; set; } = 1;
        public int EndPosition { get; set; } = 45;
        public string Color { get; set; } = "Blue";
    }
}
