using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class FormBrowser : Form
    {
        //public HTMLInputElement Location { get; set; }
        public HTMLIFrameElement Content { get; set; }

        public string URL { get; set; }

        public void Navigate(string url)
        {
            if(!url.ToLower().StartsWith("http"))
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
            //Location = new HTMLInputElement();
            //Location.Style.Position = Position.Absolute;
            //Location.Style.Top = "1px";
            //Location.Style.Left = "1px";
            //Location.Style.Width = "-webkit-calc(100% - 6px)";
            //Location.Style.Height = "28px";
         
            //Location.Disabled = true;
            //Location.Style.Padding = "0";            

            //Content = new HTMLIFrameElement();
            //Content.Style.Position = Position.Absolute;
            //Content.Style.Top = "36px";
            //Content.Style.Left = "1px";
            //Content.Style.Width = "-webkit-calc(100% - 6px)";
            //Content.Style.Height = "-webkit-calc(100% - 44px)";

            Content = new HTMLIFrameElement();
            Content.Style.Position = Position.Absolute;
            Content.Style.Top = "1px";
            Content.Style.Left = "1px";
            Content.Style.Width = "-webkit-calc(100% - 6px)";
            Content.Style.Height = "-webkit-calc(100% - 6px)";

            this.Text = "Quick Search";

            Content.Style.Padding = "0";             

            //Body.AppendChild(Location);
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
