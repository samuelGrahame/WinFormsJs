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

            FillControlWithParent(NotePadContent);

            NotePadContent.Style.Resize = Resize.None;

            Body.AppendChild(NotePadContent);
        }
    }
}
