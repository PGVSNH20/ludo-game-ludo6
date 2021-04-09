namespace GameEngine.GameModels
{
    public class RedPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; } = 21;
        public int EndPosition { get; } = 65;
        public int Offset { get; } = 20;
        public int BoardPosition { get { return Position == 0 ? 0 : Position - Offset; } }
        public string Color { get; } = "Red";
    }
}
