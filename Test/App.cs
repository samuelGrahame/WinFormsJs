using System;
using Bridge;
using Bridge.Html5;

namespace Test
{
	public class App
	{
		public static void Main()
		{
			Form.Setup();

			var butBing = new HTMLButtonElement
			{
				InnerHTML = "Bing",
				OnClick = (ev) =>
				{
					var frm = new FormBrowser();
					frm.Left = "50px";
					frm.Top = "50px";
					frm.Text = "Bing";
					frm.Navigate("https://bing.com");
					frm.Show();
				}
			};

			var butNote = new HTMLButtonElement
			{
				InnerHTML = "NotePad",
				OnClick = (ev) =>
				{
					var frm = new FormNotePad();
					frm.Left = "50px";
					frm.Top = "50px";
					frm.Text = "Note Pad";
					frm.Show();
				}
			};
			Form.WindowHolder.AppendChild(butBing);
			Form.WindowHolder.AppendChild(butNote);			
		}
	}
}