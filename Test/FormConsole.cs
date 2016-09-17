using Bridge.Html5;
using Bridge.jQuery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class FormConsole : Form
    {
        private HTMLDivElement CommandPanel;
        private HTMLInputElement CommandInput;
        private List<HTMLSpanElement> CommandLines;

        private int Line = -1;

        private void SetCommandLineElement(HTMLElement element)
        {
            if(element.TagName.ToLower() == "span")
            {
                element.SetAttribute("IL", "1");
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

        protected override void OnShowed()
        {
            CommandInput.Focus();
        }

        private void IncrementLine()
        {
            string cmd = CommandInput.Value;
            if (cmd.Length > 0)
            {
                CommandInput.Value = "";

                var SpanText = new HTMLSpanElement();
                FillHorizontalControlWithParent(SpanText, 2);
                
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
                Clear();
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
