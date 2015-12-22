using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SFML.System;

namespace BarbarossaShared
{
    public interface ISaveable
    {
        XmlNode GetSaveNode(XmlDocument doc);
    }

    public class Loader
    {
        DrawableFactory _drawableFactory;

        public Loader(DrawableFactory drawableFactory)
        {
            _drawableFactory = drawableFactory;
        }

        public virtual void Load(string path, LogicManager logicManager, DrawManager drawManager)
        {
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
        }

        public object LoadObject(XmlNode node, IDrawable drawable)
        {
            object o = null;

            switch (node["Type"].Attributes["type"].Value)
            {
                case "Monster":
                    //o = new Monster(drawable,);
                    break;

                case "Platform":
                    {
                        Vector2f position = new Vector2f(
                            Convert.ToSingle(node["Position"].Attributes["x"].Value),
                            Convert.ToSingle(node["Position"].Attributes["y"].Value));

                        Vector2f size = new Vector2f(
                            Convert.ToSingle(node["Size"].Attributes["width"].Value),
                            Convert.ToSingle(node["Size"].Attributes["height"].Value));
                        o = new Platform(drawable, position, size);
                        break;
                    }

                case "Player":
                    {
                        Vector2f position = new Vector2f(
                            Convert.ToSingle(node["Position"].Attributes["x"].Value),
                            Convert.ToSingle(node["Position"].Attributes["y"].Value));

                        Vector2f size = new Vector2f(
                            Convert.ToSingle(node["Size"].Attributes["width"].Value),
                            Convert.ToSingle(node["Size"].Attributes["height"].Value));
                        o = new Player(drawable, position, size);
                        break;
                    }
            }

            return o;
        }

        public IDrawable LoadDrawableObject(XmlNode drawNode)
        {
            if (drawNode["Type"].Attributes["type"].Value == "Image")
            {
                return _drawableFactory.CreateImage(drawNode["Path"].Attributes["path"].Value);
            }
            else if (drawNode["Type"].Attributes["type"].Value == "Platform")
            {
                Vector2f position = new Vector2f(
                    Convert.ToSingle(drawNode["Position"].Attributes["x"].Value),
                    Convert.ToSingle(drawNode["Position"].Attributes["y"].Value));

                Vector2f size = new Vector2f(
                    Convert.ToSingle(drawNode["Size"].Attributes["width"].Value),
                    Convert.ToSingle(drawNode["Size"].Attributes["height"].Value));

                return _drawableFactory.CreateDrawablePlatform(position, size);
            }
            else
            {
                throw new Exception("Drawable Object not supported or not given.");
            }
        }
    }
}
