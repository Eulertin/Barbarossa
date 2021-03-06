﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarbarossaShared;
using System.Xml;

namespace BarbarossaEditor
{
    class EditorLoader : Loader
    {
        public EditorLoader(DrawableFactory drawableFactory) : base(drawableFactory)
        { }

        public new List<ObjectConnector> Load(string path, LogicManager logicManager, DrawManager drawManager)
        {
            List<ObjectConnector> conList = new List<ObjectConnector>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            IDrawable drawable;
            object o;

            foreach (XmlNode node in doc.ChildNodes[0].ChildNodes)
            {
                XmlNode childNode = node["Draw"];
                if (childNode != null)
                {
                    drawable = LoadDrawableObject(childNode);
                    drawManager.AddObject(drawable);
                    o = LoadObject(node["Logic"], drawable);
                }
                else
                {
                    o = LoadObject(node["Logic"], null);
                }
                logicManager.AddObject(o);
            }

            return conList;
        }
    }
}
