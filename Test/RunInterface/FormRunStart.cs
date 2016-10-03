using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;

namespace WinFormjs
{
	public class FormRunStart : Form
	{
		protected override void Initialise()
		{
			base.Initialise();

			this.Text = "Run Interface";
			this.Width = "400px";
			this.Height = "100px";

			var Input = new HTMLInputElement();
			Input.Style.Position = Position.Absolute;
			Input.Style.Left = "5px";
			Input.Style.Top = "5px";

			FillHorizontalControlWithParent(Input, 5);			

			Input.OnKeyUp = (ev) => {
				var kev = ev.As<KeyboardEvent>();
				if(kev.KeyCode == 13)
				{
					if(string.IsNullOrWhiteSpace(Input.Value))
					{
						ServerApplication.StartFromURI(Input.Value);
					}
					Input.Value = "";
				}
			};


			//FillHorizontalControlWithParent()



			//this.Body.AppendChild()

		}

		protected override void OnShowing()
		{
			
			base.OnShowing();
		}
	}
}
