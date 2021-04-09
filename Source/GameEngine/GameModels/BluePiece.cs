namespace GameEngine.GameModels
{
    public class BluePiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; } = 1;
        public int EndPosition { get; } = 45;
        public int Offset { get; } = 0;
        public int BoardPosition { get { return Position; } }
        public string Color { get; } = "Blue";
    }
}
