namespace GameEngine
{
    public interface IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
    }
}