using System;
using Bridge;
using Bridge.Html5;

namespace WinFormjs
{
	public class App
	{
		public static void Main()
		{
			Form.Setup();

			//var butBing = new HTMLButtonElement
			//{
			//	InnerHTML = "Bing",
			//	OnClick = (ev) =>
			//	{
			//		var frm = new FormBrowser();
			//		frm.Left = "50px";
			//		frm.Top = "50px";
			//		frm.Text = "Bing";
			//		frm.Navigate("https://bing.com");
			//		frm.Show();
			//	}
			//};

   //         //var butLel = new HTMLButtonElement
   //         //{
   //         //    InnerHTML = "Lel",
   //         //    OnClick = (ev) =>
   //         //    {
   //         //        var frm = new FormBrowser();
   //         //        frm.Left = "50px";
   //         //        frm.Top = "50px";
   //         //        frm.Text = "Lel";
   //         //        frm.Navigate("file:///C:/Users/Samuel/Desktop/Test/Test/Bridge/www/demo.html");
   //         //        frm.Show();
   //         //    }
   //         //};

   //         var butNote = new HTMLButtonElement
			//{
			//	InnerHTML = "NotePad",
			//	OnClick = (ev) =>
			//	{
			//		var frm = new FormNotePad();
			//		frm.Left = "50px";
			//		frm.Top = "50px";
			//		frm.Text = "Note Pad";
			//		frm.Show();
			//	}
			//};

   //         var butCmd = new HTMLButtonElement
   //         {
   //             InnerHTML = "Command Prompt",
   //             OnClick = (ev) =>
   //             {
   //                 var frm = new FormConsole();
   //                 frm.Left = "50px";
   //                 frm.Top = "50px";
   //                 frm.Text = "Command Prompt";
   //                 frm.Show();
   //             }
   //         };

   //         Form.WindowHolder.AppendChild(butBing);
			//Form.WindowHolder.AppendChild(butNote);	//Form.WindowHolder.AppendChild(butLel);			
   //         Form.WindowHolder.AppendChild(butCmd);
        }
	}
}