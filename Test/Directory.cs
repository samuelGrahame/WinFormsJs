using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Directory : FileExplorerNode
    {
        public static string[] GetDirectories(string path)
        {
            if(path == FileExplorer.DesktopPath)
            {
                return new string[] { string.Format("{0}/New Folder", path) };
            }

            return null;
        }

        public static string[] GetFiles(string path)
        {
            if (path == FileExplorer.DesktopPath)
            {
                return new string[] {
                string.Format("{0}/iexplore.exe", path),
                string.Format("{0}/Notepad.exe", path),
                string.Format("{0}/cmd.exe", path)};
            }

            return null;
        }
    }
}
