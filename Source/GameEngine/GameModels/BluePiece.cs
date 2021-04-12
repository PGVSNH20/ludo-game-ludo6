namespace GameEngine.GameModels
{
    public class BluePiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; } = 1;
        public int EndPosition { get; } = 45;
        public int Offset { get; } = 0;
        public int RelativePosition { get { return Position; } }
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
        public string Color { get; } = "Blue";
    }
}
