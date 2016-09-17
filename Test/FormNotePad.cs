using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class FormNotePad : Form
    {
        public HTMLTextAreaElement NotePadContent { get; set; }

        protected override void Initialise()
        {
            NotePadContent = new HTMLTextAreaElement();

            NotePadContent.Style.Position = Position.Absolute;
            NotePadContent.Style.Width = "-webkit-calc(100% - 8px)";
            NotePadContent.Style.Height = "-webkit-calc(100% - 9px)";

            NotePadContent.Style.Top = "1px";
            NotePadContent.Style.Left = "1px";
            NotePadContent.Style.Resize = Resize.None;

            Body.AppendChild(NotePadContent);
        }
    }
}
