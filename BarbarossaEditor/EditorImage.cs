using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarbarossaShared;
using System.Drawing;
using SFML.System;
using System.Xml;

namespace BarbarossaEditor
{
    class EditorImage : IDrawable, ISaveable
    {
        Image _image;
        Vector2f _position;
        string _imagePath;

        public Vector2f Size { get { return new Vector2f(_image.Width,_image.Height); } }

        public EditorImage(Image image, string imagePath)
        {
            _image = image;
            _imagePath = imagePath;
        }

        public void Draw(IRenderTarget target)
        {
            Graphics g = (Graphics)target.GetDrawer();
            g.DrawImage(_image, _position.X, _position.Y);
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
            attr.Value = "Image";
            node.Attributes.Append(attr);
            root.AppendChild(node);

            node = doc.CreateElement("Path");
            attr = doc.CreateAttribute("path");
            attr.Value = _imagePath;
            node.Attributes.Append(attr);
            root.AppendChild(node);

            return root;
        }
    }
}
