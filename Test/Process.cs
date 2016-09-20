using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
    public class Process
    {
        public static Process Start(string fileName)
        {            
            string DirectoryName = Path.GetDirectoryName(fileName);
            string FileName = Path.GetFileName(fileName);
            
            if(DirectoryName == FileExplorer.DesktopPath)
            {                
                switch (FileName)
                {
                    case "iexplore.exe":
                        var iexplore = new FormBrowser();
                        iexplore.Left = "50px";
                        iexplore.Top = "50px";
                        iexplore.Text = "Bing";
                        iexplore.Navigate("https://bing.com");
                        iexplore.Show();
                        break;
                    case "Notepad.exe":
                        var Notepad = new FormNotePad();
                        Notepad.Left = "50px";
                        Notepad.Top = "50px";
                        Notepad.Text = "Note Pad";
                        Notepad.Show();
                        break;
                    case "cmd.exe":
                        var cmd = new FormConsole();
                        cmd.Left = "50px";
                        cmd.Top = "50px";
                        cmd.Text = "Command Prompt";
                        cmd.Show();
                        break;
                }
            }else
            {

            }

            return null;
        }
    }
}
