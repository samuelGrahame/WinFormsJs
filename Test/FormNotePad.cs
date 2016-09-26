using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
    public class FormNotePad : Form
    {
        public string Path { get; set; }
        public HTMLTextAreaElement NotePadContent { get; set; }

        protected override void Initialise()
        {
            NotePadContent = new HTMLTextAreaElement();

            FillControlWithParent(NotePadContent);

            NotePadContent.Style.Resize = Resize.None;

            Body.AppendChild(NotePadContent);            
        }

        protected override void OnShowing()
        {            
            if (Path != null)
            {
                NotePadContent.Value = File.ReadAllText(Path);
            }
        }

        protected override void OnClosing()
        {
            // Ask if you would like to save changes!!

            if (Path != null)
            {
                File.WriteAllText(Path, NotePadContent.Value);
            }
            base.OnClosing();
        }
    }
}
