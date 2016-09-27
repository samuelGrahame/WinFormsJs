using Bridge.Html5;
using Bridge.jQuery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormjs
{
    public class FormConsole : Form
    {
        private HTMLDivElement CommandPanel;
        private HTMLInputElement CommandInput;
        private List<HTMLSpanElement> CommandLines;

        public string currentcd;

        private int Line = -1;

        private void SetCommandLineElement(HTMLElement element)
        {
            if(element.TagName.ToLower() == "span")
            {
                SetInternalLabel(element);
                jQuery.Select(element).Css("user-select", "text");

                element.AddEventListener(EventType.MouseMove, (ev) => {
                    element.Style.Cursor = Cursor.Text;
                    ev.StopPropagation();
                });
                element.AddEventListener(EventType.Click, (ev) => {                    
                    ev.StopPropagation();
                });
            }            

            element.Style.BackgroundColor = "black";
            element.Style.Height = "24px";
            element.Style.Padding = "0";
            element.Style.Color = "white";
            element.Style.Margin = "0";
            element.Style.BorderStyle = BorderStyle.None;
            element.Style.FontFamily = "monospace";
            element.Style.FontSize = "12pt";

            jQuery.Select(element).On("focus", () => {
                element.Style.Outline = "0";
            });
        }

        protected override void Initialise()
        {
            CommandPanel = new HTMLDivElement();
            CommandPanel.Style.BackgroundColor = "black";            
            CommandPanel.Style.Overflow = Overflow.Auto;

            CommandPanel.AddEventListener(EventType.MouseMove, (ev) => {
                CommandPanel.Style.Cursor = Cursor.Text;
                ev.StopPropagation();
            });

            CommandLines = new List<HTMLSpanElement>();

            FillControlWithParent(CommandPanel, 2, 2);

            CommandInput = new HTMLInputElement() { Type = InputType.Text };

            FillHorizontalControlWithParent(CommandInput, 2);
            
            SetCommandLineElement(CommandInput);

            CommandInput.AddEventListener(EventType.KeyDown, (ev) => {
                var kev = ev.As<KeyboardEvent>();

                if(kev.KeyCode == 13)
                {
                    IncrementLine();
                }
            });

            IncrementLine();

            CommandPanel.AppendChild(CommandInput);

            CommandPanel.AddEventListener(EventType.Click, (ev) => {
                CommandInput.Focus();
            });

            Body.AppendChild(CommandPanel);

            Width = "677px";
            Height = "392px";
        }

        public void Reset()
        {
            currentcd = FileExplorer.DesktopPath;
            Text = "Console - Path: " + currentcd;
            Clear();
            WriteLine("Commands: [clear, toggle bodyoverlay, cd, dir, createfile, deletefile, createdir]");

            CommandInput.Focus();
        }

        protected override void OnShowed()
        {
            
            Reset();
        }

        public void WriteLine(string line)
        {
            line = line.Replace('\n', '\r');
            foreach (var parse in line.Split('\r'))
            {
                CommandInput.Value = parse;
                IncrementLine();
            }            
        }

        private void IncrementLine()
        {
            string cmd = CommandInput.Value;
            if (cmd.Length > 0)
            {
                CommandInput.Value = "";

                var SpanText = new HTMLSpanElement();
                FillHorizontalControlWithParent(SpanText, 2);
                SpanText.Style.WhiteSpace = WhiteSpace.NoWrap;
                SetCommandLineElement(SpanText);
                SpanText.InnerHTML = cmd;
                SpanText.Style.Top = (Global.ParseInt(CommandInput.Style.Height) * Line) + 3 + "px";
                CommandPanel.AppendChild(SpanText);
                CommandLines.Add(SpanText);
            }
            Line++;
            CommandInput.Style.Top = (Global.ParseInt(CommandInput.Style.Height) * Line) + "px";
            CommandPanel.ScrollTop = CommandPanel.ScrollHeight;

            if (cmd.ToLower() == "clear")
                Reset();
            else if (cmd.ToLower() == "toggle bodyoverlay")
            {
                ShowBodyOverLay = !ShowBodyOverLay;
            }
            else if (cmd.ToLower().StartsWith("cd "))
            {
                currentcd += "\\" + cmd.Substring(3);
                Text = "Console - Path: " + currentcd;
            }
            else if (cmd.ToLower().StartsWith("dir"))
            {
                string location;

                if (cmd.ToLower().StartsWith("dir "))
                {
                    location = cmd.Substring("dir ".Length);
                }else
                {
                    location = currentcd;
                }

                foreach (string item in Directory.GetDirectories(location))
                {
                    WriteLine(item);
                }
                foreach (string item in Directory.GetFiles(location))
                {
                    WriteLine(item);
                }
            }
            else if (cmd.ToLower().StartsWith("createfile "))
            {
                string pathAndFile = currentcd + @"\" + cmd.Substring("createfile ".Length);
                
                File.WriteAllText(pathAndFile, "");
                
                FileExplorer.FileChangeAt(Path.GetDirectoryName(pathAndFile));
            }else if (cmd.ToLower().StartsWith("deletefile "))
            {
                string pathAndFile = currentcd + @"\" + cmd.Substring("deletefile ".Length);
                
                File.Delete(pathAndFile);

                FileExplorer.FileChangeAt(Path.GetDirectoryName(pathAndFile));
            }
            else if (cmd.ToLower().StartsWith("createdir "))
            {
                string pathAndFile = currentcd + @"\" + cmd.Substring("createdir ".Length);

                Directory.Create(pathAndFile);

                FileExplorer.FileChangeAt(Path.GetDirectoryName(pathAndFile));
            }
        }

        private void Clear()
        {
            Line = -1;
            CommandInput.Value = "";

            for (int i = 0; i < CommandLines.Count; i++)
            {
                if (CommandLines[i] != null)
                    CommandLines[i].Remove();
            }
            CommandLines = new List<HTMLSpanElement>();
            
            IncrementLine();
        }
    }
}
