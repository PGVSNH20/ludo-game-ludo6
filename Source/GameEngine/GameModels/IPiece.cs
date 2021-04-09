namespace GameEngine
{
    public interface IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public string Color { get; }
    }
}