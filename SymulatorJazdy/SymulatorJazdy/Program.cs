using System;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace SymulatorJazdy
{
    class Program
    {
        private static RenderWindow renderWindow;
        static void Main(string[] args)
        {
            renderWindow = new RenderWindow(new VideoMode(200, 200), "Hello world!");

            CircleShape circleShape = new CircleShape(100.0f);
            circleShape.FillColor = Color.Green;

            renderWindow.Closed += RenderWindow_Closed;

            while (renderWindow.IsOpen)
            {
                renderWindow.DispatchEvents();

                renderWindow.Clear();
                renderWindow.Draw(circleShape);
                renderWindow.Display();
            }
        }

        private static void RenderWindow_Closed(object sender, EventArgs e)
        {
            renderWindow.Close();
        }
    }
}
