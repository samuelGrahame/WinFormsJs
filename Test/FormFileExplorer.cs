using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
    public class FormFileExplorer : Form
    {
        public FileExplorer FileExplorerRender;
        private string StartingLocation;

        public FormFileExplorer(string startingLocation)
        {
            StartingLocation = startingLocation;
        }

        protected override void Initialise()
        {
            base.Initialise();

            FileExplorerRender = new FileExplorer(this.Body, this) { NodeViewType = NodeViewType.Medium_Icons };
            BackColor = "cornflowerblue";
            Body.Style.Overflow = Bridge.Html5.Overflow.Auto;
        }

        protected override void OnShowed()
        {
            FileExplorerRender.Path = StartingLocation; // StartingLocation;
            FileExplorerRender.Refresh();
        }

        protected override void OnClosing()
        {
            FileExplorer.LoadedExplorers.Remove(FileExplorerRender);
            base.OnClosing();
        }
    }
}
