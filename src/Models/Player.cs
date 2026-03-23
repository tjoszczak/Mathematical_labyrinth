namespace LabiryntMatematyczny.Models
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; private set; } = 30; // Gracz jest nieco mniejszy niż kafelki (40), co ułatwia ruch
        public int Speed { get; private set; } = 5;

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
        }
    }
}