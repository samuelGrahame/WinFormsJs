using Bridge.Html5;
using Bridge.jQuery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
    public struct Rectange
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public static bool valueInRange(int value, int min, int max)
        {
            return (value >= min) && (value <= max);
        }

        public static bool rectOverlap(Rectange A, Rectange B)
        {
            bool xOverlap = valueInRange(A.X, B.X, B.X + B.Width) ||
                            valueInRange(B.X, A.X, A.X + A.Width);

            bool yOverlap = valueInRange(A.Y, B.Y, B.Y + B.Height) ||
                            valueInRange(B.Y, A.Y, A.Y + A.Height);

            return xOverlap && yOverlap;
        }

        public Rectange(int x, int y, int width, int height)
        {
            X = x; Y = y; Width = width; Height = height;
        }

        public static Rectange CreateFromHTMLElement(HTMLElement element)
        {
            if (element == null)
                return new Rectange();

            var obj = jQuery.Select(element);
            return new Rectange()
            {
                X = Global.ParseInt(obj.Css("left")),
                Y = Global.ParseInt(obj.Css("top")),
                Width = Global.ParseInt(obj.Css("width")),
                Height = Global.ParseInt(obj.Css("height"))
            };
        }        
    }
}
