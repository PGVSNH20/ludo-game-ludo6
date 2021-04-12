namespace GameEngine
{
    public interface IPiece
    {
        public int Position { get; set; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public int Offset { get; }
        public int RelativePosition { get; }
        public int AbsoluteBoardPosition { get; }
        public string Color { get; }
    }
}
