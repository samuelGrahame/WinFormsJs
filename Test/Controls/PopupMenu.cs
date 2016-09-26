using Bridge.Html5;
using Bridge.jQuery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
    public class PopupMenu
    {
        public List<MenuItem> Items = new List<MenuItem>();
        public HTMLDivElement Base { get; set; } // Need to create base Control...

        public void Show(Point location)
        {
            Base = new HTMLDivElement();

            jQuery.Select(Base).
                Css("left", location.X).
                Css("top", location.Y);

            // need to make the width min-width 100%
            // Fill Content :D
            Base.Style.MinWidth = "100%";
            Base.Style.MinHeight = "100%";


            for (int i = 0; i < Items.Count; i++)
            {
                // Add Menu Item / Generate Div :D
                Base.AppendChild(Items[i].GenerateDiv());
            }
        }

        public void Close()
        {

        }        
    }

    public class MenuItem
    {
        public string Caption { get; set; }
        public bool Enabled { get; set; }
        public string Color { get; set; }
        public Action OnClick { get; set; }

        public virtual HTMLDivElement GenerateDiv() // Allow other menu items to add stuff to them :D
        {
            return null;
        }
    }

    public class SubMenuItem : MenuItem
    {
        public PopupMenu Menu { get; set; }
    }
}
