using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using BarbarossaShared;

namespace Barbarossa
{
    class Game
    {
        RenderWindow _window;
        IGraphicsDevice _graphicsDevice;
        LogicManager _logicManager;
        DrawManager _drawManager;
        ControlInfo _controlInfo;

        System.Diagnostics.Stopwatch _watch;
        float _timeDelta;

        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        public Game()
        {
            _window = new RenderWindow(new VideoMode(800, 600), "SFML window");
            //_window.SetMouseCursorVisible(false);
            _window.Closed += new EventHandler(OnClose);
            _window.SetFramerateLimit(60);

            _logicManager = new LogicManager();

            _graphicsDevice = new WindowWrapper(_window);
            _drawManager = new DrawManager(_graphicsDevice);
            _controlInfo = new ControlInfo();

            Loader loader = new Loader(new GameDrawableFactory());
            loader.Load("..\\..\\..\\Levels\\Test.xml",_logicManager, _drawManager);

            _logicManager.PassControlInfoObject(_controlInfo);

            _watch = new System.Diagnostics.Stopwatch();
            run();
        }

        private void run()
        {
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                processKeyInput();

                timeStamp();

                _logicManager.Update(_timeDelta);
                _drawManager.Draw();

                _window.Display();
            }
        }

        private void processKeyInput()
        {
            _controlInfo.Left = Keyboard.IsKeyPressed(Keyboard.Key.Left);
            _controlInfo.Right = Keyboard.IsKeyPressed(Keyboard.Key.Right);
            _controlInfo.Up = Keyboard.IsKeyPressed(Keyboard.Key.Up);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                _window.Close();
        }

        private void timeStamp()
        {
            _timeDelta = (float)_watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            _watch.Restart();
        }
    }
}
