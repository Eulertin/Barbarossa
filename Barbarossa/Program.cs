using System;
using SFML.Audio;
using SFML.Window;
using SFML.Graphics;

namespace Example
{
    class Program
    {
        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        static void Main(string[] args)
        {
            // Create the main window
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "SFML window");
            window.Closed += new EventHandler(OnClose);

            // Start the game loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();

                // Clear screen
                window.Clear();

                // Update the window
                window.Display();
            }
        }
    }
}


