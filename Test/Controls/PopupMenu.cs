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
        public static PopupMenu ActivePopupMenu { get; set; }

        public List<MenuItem> Items = new List<MenuItem>();
        public HTMLDivElement Base { get; set; } // Need to create base Control...

        public const string BeginGroupColor = "#919191"; // Also Disabled Forecolor 
        public const string BorderColor = "#A0A0A0";
        public const string BackgroundColor = "#F2F2F2";
        public const string MenuHoverColor = "#D9D9D9";
        public const string Forecolor = "black";

        public void Show(Point location)
        {
            if(ActivePopupMenu != this)
            {
                if(ActivePopupMenu != null)
                {
                    ActivePopupMenu.Close();
                }
                ActivePopupMenu = this;
            }

            if (Base != null)
            {
                Base.Remove();
                Base = null;
            }

            Base = new HTMLDivElement();
            Base.Style.Position = Position.Absolute;

            Base.Style.BorderColor = BorderColor;
            Base.Style.BorderStyle = BorderStyle.Solid;
            Base.Style.BorderWidth = BorderWidth.Thin;
            Base.Style.Padding = "2px";
            Base.Style.Margin = "0";

            Base.OnMouseDown = (ev) => { ev.StopPropagation(); };

            Base.Style.BackgroundColor = BackgroundColor;
            int ubound = Items.Count - 1;

            int y = 3;

            for (int i = 0; i < Items.Count; i++)
            {
                // Add Menu Item / Generate Div :D
                var inObj = Items[i];
                var item = inObj.GenerateDiv();

                if (inObj.BeginGroup)
                {
                    var div = new HTMLDivElement();
                    div.Style.Position = Position.Absolute;
                    y += 1;

                    jQuery.Select(div).
                    Css("left", 8).
                    Css("width", 202). // static as set now, need to change
                    Css("height", 1).
                    Css("top", y);
                    
                    div.Style.BackgroundColor = BeginGroupColor;

                    Base.AppendChild(div);

                    y += 3;
                }

                jQuery.Select(item).Css("top", y);
                Base.AppendChild(item);

                if (inObj.OnClick != null)
                {
                    item.OnClick = inObj.OnClick;
                }

                y += 24;
            }

            jQuery.Select(Base).
                Css("left", location.X).
                Css("top", location.Y).
                Css("width", 214).
                Css("height", y);

            Base.Style.Overflow = Overflow.Visible;
            Base.Style.ZIndex = int.MaxValue.ToString();
            // need to make the width min-width 100%
            // Fill Content :D
            //Base.Style.MinWidth = "100%";
            //Base.Style.MinHeight = "100%";                        

            Form.WindowHolder.AppendChild(Base);
        }

        public void Close()
        {
            ActivePopupMenu = null;
            if (Base != null)
            {
                Base.Remove();
                Base = null;
            }
        }
    }

    public class MenuItem
    {
        // 1px between each menu item
        // beginGroup 2px on top, 1px height, 3px bottom
        // Height is 22px
        public string Image { get; set; } // Height is 16x16
        public string Caption { get; set; }
        public bool Enabled { get; set; }
        public string Color { get; set; }
        public Action<MouseEvent<HTMLDivElement>> OnClick { get; set; }
        public bool BeginGroup { get; set; }

        public virtual HTMLDivElement GenerateDiv() // Allow other menu items to add stuff to them :D
        {
            var div = new HTMLDivElement();
            div.Style.Position = Position.Absolute;

            jQuery.Select(div).
                Css("left", 8).
                Css("width", 202). // static as set now, need to change
                Css("height", 22);

            div.Style.BackgroundColor = PopupMenu.BackgroundColor;

            div.OnMouseEnter = (ev) => { div.Style.BackgroundColor = PopupMenu.MenuHoverColor; };
            div.OnMouseLeave = (ev) => { div.Style.BackgroundColor = PopupMenu.BackgroundColor; };

            if (!string.IsNullOrWhiteSpace(Image))
            {
                // Add Image
            }

            if(!string.IsNullOrWhiteSpace(Caption))
            {
                var span = new HTMLSpanElement();
                span.InnerHTML = Caption;
                span.Style.Position = Position.Absolute;
                jQuery.Select(span).
                    Css("left", 34).
                    Css("top", 0).                    
                    Css("height", 22);
                span.Style.Width = StyleController.Calc(100, 34);
                span.Style.FontFamily = "Segoe UI";
                span.Style.FontSize = "9.5pt";
                span.Style.Cursor = Cursor.Default;
                div.AppendChild(span);
            }
            // add text            
            return div;
        }
    }

    public class SubMenuItem : MenuItem
    {
        public PopupMenu Menu { get; set; }
    }
}
