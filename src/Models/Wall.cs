using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabiryntMatematyczny.Models
{
    public class Wall
    {
        public Rect Bounds { get; private set; }
        public Rectangle Shape { get; private set; }

        public Wall(int x, int y, int size)
        {
            Bounds = new Rect(x, y, size, size);
            Shape = new Rectangle
            {
                Width = size,
                Height = size,
                Fill = Brushes.DarkGray // Kolor ściany
            };
        }
    }
}