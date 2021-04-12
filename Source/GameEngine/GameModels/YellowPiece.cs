namespace GameEngine.GameModels
{
    public class YellowPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; } = 31;
        public int EndPosition { get; } = 75;
        public int Offset { get; } = 30;
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
        public string Color { get; } = "Yellow";
    }
}
