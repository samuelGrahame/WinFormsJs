using Bridge.Html5;
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
        public static bool Exists(string path)
        {
            return Global.LocalStorage[CreateSaveToken(path)] != null;
        }
        
        public static string ReadAllText(string path)
        {
            return (string)Global.LocalStorage.GetItem(CreateSaveToken(path));
        }

        public static string[] ReadAllLines(string path)
        {
            return ReadAllText(path).ToString().Split("\r\n");            
        }

        public static byte[] ReadAllBytes(string path)
        {
            return GetBytes(ReadAllText(path));
        }

        private static byte[] GetBytes(string value)
        {
            byte[] data = new byte[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                data[i] = (byte)value[i];
            }
            return data;
        }

        private static char[] GetCharArray(byte[] value)
        {
            char[] data = new char[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                data[i] = (char)value[i];
            }
            return data;
        }

        private static string GetString(byte[] value)
        {
            char[] data = new char[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                data[i] = (char)value[i];
            }
            return new string(data);
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            WriteAllText(path, GetString(bytes));
        }

        public static void WriteAllLines(string path, string[] contents)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < contents.Length; i++)
            {
                builder.AppendLine(contents[i]);
            }
            WriteAllText(path, builder.ToString());
        }

        public static void WriteAllText(string path, string contents)
        {            
            Global.LocalStorage.SetItem(CreateSaveToken(path), contents);
        }

        public static void Delete(string path)
        {
            Global.LocalStorage.RemoveItem(CreateSaveToken(path));
        }
    }
}
