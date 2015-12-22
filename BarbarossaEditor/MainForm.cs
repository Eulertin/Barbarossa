using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarbarossaShared;
using SFML.System;
using System.Diagnostics;

namespace BarbarossaEditor
{
    public partial class MainForm : Form
    {
        DrawManager _drawManager;
        IGraphicsDevice _graphicsDevice;

        LogicManager _logicManager;

        EditorDrawableFactory _drawableFactory;
        ObjectFactory _objectFactory;

        Vector2f _origin;

        List<ObjectConnector> _conList;

        Stopwatch _scrollWatch;

        bool _wKeyState = false;
        bool _aKeyState = false;
        bool _sKeyState = false;
        bool _dKeyState = false;

        bool _movePathCreation = false;
        IDrawable _movePathDrawable;
        List<Vector2f> _movePath;

        public MainForm()
        {
            InitializeComponent();

            _drawableFactory = new EditorDrawableFactory();
            _graphicsDevice = new DrawPanelWrapper();

            _drawManager = new DrawManager(_graphicsDevice);
            _logicManager = new LogicManager();

            _objectFactory = new ObjectFactory(_drawableFactory);

            _conList = new List<ObjectConnector>();

            _scrollWatch = new Stopwatch();
            scrollTimer.Start();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            _graphicsDevice.SetUnderlying(e.Graphics);
            _graphicsDevice.SetOrigin(_origin);
            _drawManager.Draw();

            if (_movePathCreation)
            {
                Pen pathPen = new Pen(Color.Red);
                foreach (Vector2f v in _movePath)
                {
                    _movePathDrawable.UpdatePosition(v);
                    _movePathDrawable.Draw(_graphicsDevice);
                }

                _movePathDrawable.UpdatePosition(new Vector2f(
                    canvas.PointToClient(MousePosition).X, canvas.PointToClient(MousePosition).Y) - _origin);
                _movePathDrawable.Draw(_graphicsDevice);

                for (int i = 0; i < _movePath.Count - 1; i++)
                {
                    e.Graphics.DrawLine(pathPen, _movePath[i].X, _movePath[i].Y, _movePath[i+1].X, _movePath[i+1].Y);
                }

                e.Graphics.DrawLine(pathPen, new Point((int)(_movePath[_movePath.Count - 1].X - _origin.X),
                    (int)(_movePath[_movePath.Count - 1].Y - _origin.Y)),
                    PointToClient(MousePosition));

                e.Graphics.DrawLine(pathPen, PointToClient(MousePosition),
                    new Point((int)(_movePath[0].X - _origin.X),
                    (int)(_movePath[0].Y - _origin.Y)));

            }
        }

        private void addObjectFromTypeList(Vector2f mousePosition)
        {
            if (typeListBox.SelectedItem != null)
            {
                switch ((string)typeListBox.SelectedItem)
                {
                    case "Monster":
                        {
                            _movePathCreation = true;
                            _movePathDrawable = _drawableFactory.CreateImage("..\\..\\..\\Bilder\\monster.png");
                            _movePath = new List<Vector2f>();
                            _movePath.Add(mousePosition);
                            break;
                        }

                    case "Platform":
                        {
                            ObjectConnector con = _objectFactory.CreatePlatform(mousePosition);
                            _logicManager.AddObject(con.LogicObject);
                            _drawManager.AddObject(con.DrawableObject);
                            objectListView.Items.Add(con.ListItem);
                            _conList.Add(con);
                            break;
                        }

                    case "Player":
                        {
                            ObjectConnector con = _objectFactory.CreatePlayer(mousePosition);
                            _logicManager.AddObject(con.LogicObject);
                            _drawManager.AddObject(con.DrawableObject);
                            objectListView.Items.Add(con.ListItem);
                            _conList.Add(con);
                            break;
                        }
                }
            }
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (addRadioButton.Checked && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_movePathCreation)
                    _movePath.Add(new Vector2f(e.X, e.Y) - _origin);
                else
                    addObjectFromTypeList(new Vector2f(e.X, e.Y) - _origin);
            }
            canvas.Refresh();
        }

        private void speichernAlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                save(saveFileDialog.FileName);
        }

        private void save(string path)
        {
            Saver.Save(path, _conList);
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                open(openFileDialog.FileName);
            }
        }

        private void open(string path)
        {
            EditorLoader loader = new EditorLoader(_drawableFactory);

            _logicManager = new LogicManager();
            _drawManager = new DrawManager(_graphicsDevice);

            _conList = loader.Load(path, _logicManager, _drawManager);
        }

        private void scrollTimer_Tick(object sender, EventArgs e)
        {
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.W))
                _wKeyState = true;
            else if (System.Windows.Input.Keyboard.IsKeyUp(System.Windows.Input.Key.W))
                _wKeyState = false;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.A))
                _aKeyState = true;
            else if (System.Windows.Input.Keyboard.IsKeyUp(System.Windows.Input.Key.A))
                _aKeyState = false;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.S))
                _sKeyState = true;
            else if (System.Windows.Input.Keyboard.IsKeyUp(System.Windows.Input.Key.S))
                _sKeyState = false;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.D))
                _dKeyState = true;
            else if (System.Windows.Input.Keyboard.IsKeyUp(System.Windows.Input.Key.D))
                _dKeyState = false;

            if (_wKeyState || _aKeyState|| _sKeyState || _dKeyState)
            {
                if (_wKeyState)
                    _origin += new Vector2f(0, 1) * _scrollWatch.ElapsedMilliseconds / 5f;
                if (_aKeyState)
                    _origin += new Vector2f(1, 0) * _scrollWatch.ElapsedMilliseconds / 5f;
                if (_sKeyState)
                    _origin += new Vector2f(0, -1) * _scrollWatch.ElapsedMilliseconds / 5f;
                if (_dKeyState)
                    _origin += new Vector2f(-1, 0) * _scrollWatch.ElapsedMilliseconds / 5f;

                _scrollWatch.Restart();
                canvas.Refresh();
            }
            else if (MouseButtons == System.Windows.Forms.MouseButtons.Middle)
            {
                if (canvas.PointToClient(MousePosition).X < 200)
                    _origin += new Vector2f(1, 0) * _scrollWatch.ElapsedMilliseconds / 5f;
                if (canvas.PointToClient(MousePosition).X > canvas.ClientSize.Width - 200)
                    _origin += new Vector2f(-1, 0) * _scrollWatch.ElapsedMilliseconds / 5f;
                if (canvas.PointToClient(MousePosition).Y < 200)
                    _origin += new Vector2f(0, 1) * _scrollWatch.ElapsedMilliseconds / 5f;
                if (canvas.PointToClient(MousePosition).Y > canvas.ClientSize.Height - 200)
                    _origin += new Vector2f(0, -1) * _scrollWatch.ElapsedMilliseconds / 5f;

                _scrollWatch.Restart();
                canvas.Refresh();
            }
            else
            {
                _scrollWatch.Stop();
                _scrollWatch.Reset();
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_movePathCreation)
                canvas.Refresh();
        }
    }
}
