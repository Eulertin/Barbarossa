using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarbarossaShared;
using System.Windows.Forms;
using System.Xml;

namespace BarbarossaEditor
{
    class ObjectConnector
    {
        private static Dictionary<string, int> _nameCounter = new Dictionary<string, int>();

        public static string NextName(string type)
        {
            if (!_nameCounter.ContainsKey(type))
                _nameCounter.Add(type, 0);

            return type + (_nameCounter[type]++);
        }

        object _logicObject;
        public object LogicObject { get { return _logicObject; } }

        IDrawable _drawableObject;
        public IDrawable DrawableObject { get { return _drawableObject; } }

        string _name;
        public string Name { get { return _name; } }

        ListViewItem _listItem;
        public ListViewItem ListItem { get { return _listItem; } }

        public ObjectConnector(object logicObject, IDrawable drawableObject, string name)
        {
            _logicObject = logicObject;
            _drawableObject = drawableObject;
            _name = name;
            _listItem = new ListViewItem(_name);
        }

        public XmlNode GetSaveNode(XmlDocument doc)
        {
            XmlNode root = doc.CreateElement(_name);
            if (_logicObject is ISaveable)
            {
                root.AppendChild((_logicObject as ISaveable).GetSaveNode(doc));
                root.AppendChild((_drawableObject as ISaveable).GetSaveNode(doc));
            }
            return root;
        }
    }
}
