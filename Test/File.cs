using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
	/// <summary>
	/// Need to make File Interface. so that I can have DropBoxFile, LocalFile and FTPFile
	/// </summary>
    public class File : FileExplorerNode
    {
        public static string ReadAllText(string path)
        {
            return null;
        }

        public static string[] ReadAllLines(string path)
        {
            return null;
        }

        public static byte[] ReadAllBytes(string path)
        {
            return null;
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {

        }

        public static void WriteAllLines(string path, string[] contents)
        {

        }

        public static void WriteAllText(string path, string contents)
        {

        }
    }
}
