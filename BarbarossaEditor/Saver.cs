using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BarbarossaShared;

namespace BarbarossaEditor
{

    class Saver
    {
        public static void Save(string path, List<ObjectConnector> conList)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root;

            root = doc.CreateElement("root");
            doc.AppendChild(root);

            foreach (ObjectConnector con in conList)
            {
                root.AppendChild(con.GetSaveNode(doc));
            }

            doc.Save(path);
        }
    }
}
