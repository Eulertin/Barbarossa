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
        }

        private void addObjectFromTypeList(Vector2f mousePosition)
        {
            if (typeListBox.SelectedItem != null)
            {
                switch ((string)typeListBox.SelectedItem)
                {
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
                    _movePath.Add(new Vector2f(e.X, e.Y));
                else
                    addObjectFromTypeList(new Vector2f(e.X, e.Y)+_origin);
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
            if(MouseButtons == System.Windows.Forms.MouseButtons.Right)
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
    }
}
