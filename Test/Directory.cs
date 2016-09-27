using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
    public class Directory : FileExplorerNode
    {
        public static void Create(string path)
        {
            Global.LocalStorage.SetItem(CreateSaveToken(path, false), "");
        }

        public static string[] GetDirectories(string path)
        {
            return GetTokens(path, "#");
        }

        public static string[] GetTokens(string path, string Token = "@")
        {
            if (!path.EndsWith("\\"))
                path += "\\";
            path = path.Replace(@"\ate", @"\");
            
            List<string> Tokens = new List<string>();

            string SearchToken = string.Format("{0}{1}", Token, path);

            for (int i = 0; i < Global.LocalStorage.Length; i++)
            {
                string key = Global.LocalStorage.Key(i);

                if (key.StartsWith(SearchToken))
                {
                    if (key.Substring(SearchToken.Length).Contains("\\"))
                    {
                        continue;
                    }

                    key = key.Replace(@"\ate", @"\");
                    Tokens.Add(key.Substring(1));
                }
            }
            return Tokens.ToArray();
        }

        public static string[] GetFiles(string path)
        {
            List<string> Tokens = new List<string>();
            if (path == FileExplorer.DesktopPath)
            {
                Tokens.AddRange(new string[] {
                string.Format("{0}/Recycle Bin", path),
                string.Format("{0}/iexplore.exe", path),
                string.Format("{0}/Notepad.exe", path),
                string.Format("{0}/cmd.exe", path)});                
            }

            Tokens.AddRange(GetTokens(path));

            return Tokens.ToArray();            
        }        
    }

    public static class Debugging
    {
        public static void Log(string Value)
        {
            Script.Call("console.log", Value);
        }
    }
}
