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
        
        public void Show()
        {

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
    }

    public class SubMenuItem : MenuItem
    {
        public PopupMenu Menu { get; set; }
    }
}
