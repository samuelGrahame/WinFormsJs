using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Path
    {
        public static string GetDirectoryName(string path)
        {
            string FileName = GetFileName(path);
            return path.Substring(0, path.Length - FileName.Length - 1);
        }

        public static string GetFileName(string path)
        {
            if (path != null)
            {
//                CheckInvalidPathChars(path, false);
                int length = path.Length;
                int num2 = length;
                while (--num2 >= 0)
                {
                    char ch = path[num2];
                    if (((ch == '\\') || (ch == '/')) || (ch == ':'))
                    {
                        return path.Substring(num2 + 1, (length - num2) - 1);
                    }
                }
            }
            return path;
        }        
    }
}
