using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BarbarossaShared;
using SFML.System;
using System.Drawing;

namespace BarbarossaEditor
{
    class PathDrawable : IDrawable, ISaveable
    {
        EditorImage _image;
        Vector2f[] _movePath;
        Pen _pen;

        public Vector2f Size { get { return _image.Size; } }

        public PathDrawable(Pen pen, EditorImage image, Vector2f[] movePath)
        {
            _pen = pen;
            _image = image;
            _movePath = movePath;
        }

        public void Draw(IRenderTarget target)
        {
            foreach (Vector2f v in _movePath)
            {
                _image.UpdatePosition(v);
                _image.Draw(target);
            }

            Graphics g = (Graphics)target.GetDrawer();

            for (int i = 0; i < _movePath.Length - 1; i++)
            {
                g.DrawLine(_pen, _movePath[i].X, _movePath[i].Y, _movePath[i + 1].X, _movePath[i + 1].Y);
            }
            g.DrawLine(_pen, _movePath[_movePath.Length - 1].X, _movePath[_movePath.Length - 1].Y,
                _movePath[0].X, _movePath[0].Y);
        }

        public void UpdatePosition(Vector2f position)
        {

        }

        public XmlNode GetSaveNode(XmlDocument doc)
        {
            XmlNode root = doc.CreateElement("Draw");
            XmlNode node = _image.GetSaveNode(doc);

            root.AppendChild(node);
            node = doc.CreateElement("MovePath");
            XmlNode subNode;

            XmlAttribute attr;
            for (int i = 0; i < _movePath.Length; i++)
            {
                subNode = doc.CreateElement("Path-Vector" + i);
                attr = doc.CreateAttribute("x");
                attr.Value = _movePath[i].X.ToString();
                subNode.Attributes.Append(attr);
                attr = doc.CreateAttribute("y");
                attr.Value = _movePath[i].Y.ToString();
                subNode.Attributes.Append(attr);
                node.AppendChild(subNode);
            }

            root.AppendChild(node);
            return root;
        }
    }
}
