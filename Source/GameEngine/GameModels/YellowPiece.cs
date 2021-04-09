namespace GameEngine.GameModels
{
    public class YellowPiece : IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; } = 31;
        public int EndPosition { get; } = 75;
        public string Color { get; } = "Yellow";
    }
}
