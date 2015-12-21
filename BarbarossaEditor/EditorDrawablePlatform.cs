using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using BarbarossaShared;
using System.Drawing;
using System.Xml;

namespace BarbarossaEditor
{
    class EditorDrawablePlatform : IDrawable, ISaveable
    {
        Vector2f _position, _size;
        float _grassThickness;
        Brush _dirtBrush, _grassBrush;

        public EditorDrawablePlatform(Vector2f position, Vector2f size, float grassThickness, Brush dirtPen, Brush grassPen)
        {
            _position = position;
            _size = size;
            _dirtBrush = dirtPen;
            _grassBrush = grassPen;
            _grassThickness = grassThickness;
        }

        public void Draw(IRenderTarget target)
        {
            Graphics g = (Graphics)target.GetDrawer();

            g.FillRectangle(_dirtBrush, _position.X, _position.Y, _size.X, _size.Y);
            g.FillRectangle(_grassBrush, _position.X, _position.Y, _size.X, Math.Min(_size.Y,_grassThickness));
        }

        public void UpdatePosition(Vector2f position)
        {
            _position = position;
        }

        public System.Xml.XmlNode GetSaveNode(System.Xml.XmlDocument doc)
        {
            XmlNode root = doc.CreateElement("Draw");

            XmlNode node = doc.CreateElement("Type");
            XmlAttribute attr = doc.CreateAttribute("type");
            attr.Value = "Platform";
            node.Attributes.Append(attr);
            root.AppendChild(node);

            node = doc.CreateElement("Position");
            attr = doc.CreateAttribute("x");
            attr.Value = _position.X.ToString();
            node.Attributes.Append(attr);
            attr = doc.CreateAttribute("y");
            attr.Value = _position.Y.ToString();
            node.Attributes.Append(attr);
            root.AppendChild(node);
            node = doc.CreateElement("Size");
            attr = doc.CreateAttribute("width");
            attr.Value = _size.X.ToString();
            node.Attributes.Append(attr);
            attr = doc.CreateAttribute("height");
            attr.Value = _size.Y.ToString();
            node.Attributes.Append(attr);
            root.AppendChild(node);

            return root;
        }

    }
}
