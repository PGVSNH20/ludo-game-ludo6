﻿namespace GameEngine.GameModels
{
    public class GreenPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; } = 11;
        public int EndPosition { get; } = 55;
        public int Offset { get; } = 10;
        public int RelativePosition { get { return Position == 0 ? 0 : Position - Offset; } }
        public int AbsoluteBoardPosition
        {
            get
            {
                if (Position > 40)
                {
                    return Position - 40;
                }
                else
                    return Position;
            }
        }
        public string Color { get; } = "Green";
    }
}
