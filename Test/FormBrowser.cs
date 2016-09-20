using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
    public class FormBrowser : Form
    {
        //public HTMLInputElement Location { get; set; }
        public HTMLIFrameElement Content { get; set; }

        public string URL { get; set; }

        public void Navigate(string url)
        {
            if(!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("file:///"))
            {
                url = "http://" + url;
            }
            URL = url;
            if(IsVisible())
            {
                Content.Src = URL;
            }            
        }

        protected override void Initialise()
        {           
            Content = new HTMLIFrameElement();

            FillControlWithParent(Content, 6, 6);
            
            Text = "Quick Search";
            
            Body.AppendChild(Content);
        }

        protected override void OnShowed()
        {
            if(URL != Content.Src)
            {
                Content.Src = URL;                
            }                        
        }
    }
}
