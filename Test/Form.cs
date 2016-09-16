using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using Bridge.jQuery2;
using Bridge;

namespace Test
{
	public class Form
	{
		public static HTMLDivElement TaskBar { get; set; }
		public static HTMLDivElement WindowHolder { get; set; }
		public static HTMLDivElement ButtonStart { get; set; }
		public static HTMLInputElement InputStartSearch { get; set; }
		public static int ResizeCorners { get; set; } = 3;
		public static Form MovingForm = null;
		public static HTMLElement Parent = null;
		public static bool Mouse_Down { get; set; } = false;

		private static Form _ActiveForm;
		private static Form _PrevActiveForm;
		private static MouseMoveAction MoveAction = MouseMoveAction.Move;

		private const string WinIcon = "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAoCAIAAAA35e4mAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACSSURBVFhH7dbRCYAgFIXhRnASN3ADJ3GSu4gbuIGD1SUlejCOBpLE+R4NOT/0UJtZDIMQBiEMQhiEMAj5b5C11nsfQhCRlFLOeT/Vx93eBDnndFuHY4w6rCdlu6lc6TccVHdumoeXcqsfgxAGIcNBs/GVIQxCGIQMB6m1Pq5Pvvz9mIpBCIMQBiEMQhiELBZkzAGoRY/1a8YOvQAAAABJRU5ErkJggg==')";
		private const string WinIconHover = "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAoCAIAAAA35e4mAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACmSURBVFhH7dYxCoQwEIVhb5NasNBGZCstBUFkL7Dg9ttq6QG8gJ2FB/I2DkS2EOUlghjkfUwVCfODhXrKMQxCGIQwCGEQwiDkuUF+GEdp8arq7NOU7fDupu84y6yPjZ0JCpJMdsvi/NfLYjnRu3dHXzFnHbTZJ7N7+B99yxyDEAYh1kFX4ytDGIQwCLEOEm59XI/c+ftxKQYhDEIYhDAIYRDiWJBSC3edj/DGIv8/AAAAAElFTkSuQmCC')";
		private const string WinIconDown = "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAoCAIAAAA35e4mAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACnSURBVFhHY5AZZGDUQYTAqIMIgVEHEQKjDiIERh1ECAxfBynrGGvbehv6JFnGVrmUznWvXRE27zoQQaWJBuQ4SN3UHmg30GLHvIlAi4EiELuxIogW4gHJDkKzD4iwCsIRRBfxYNRBhMCogwgBkh1EazAaZYTAqIMIgVEHEQIkOwgIBlfligsMZPODpmDUQYTAqIMIgVEHEQKjDiIERh1ECAwyB8nIAADHEJbDMY47rQAAAABJRU5ErkJggg==')";

		private HTMLDivElement Base { get; set; }
		private HTMLDivElement Heading { get; set; }
		private HTMLDivElement ButtonClose { get; set; }
		private HTMLDivElement ButtonExpand { get; set; }
		private HTMLDivElement ButtonMinimize { get; set; }
		private HTMLSpanElement HeadingTitle { get; set; }

		public HTMLDivElement Body { get; set; }
        public HTMLDivElement BodyOverLay { get; set; }		
		public HTMLElement Owner { get; set; } = null;

		public int prev_px;
		public int prev_py;

		private int prev_width;
		private int prev_height;

		private int prev_top;
		private int prev_left;

		public int MinWidth { get; set; } = 200;
		public int MinHeight { get; set; } = 50;

		public WindowState windowState { get; set; }
		
        public bool IsVisible()
        {
            return Base != null && Base.Style.Visibility == Visibility.Visible;
        }

        protected virtual void Initialise()
        {

        }

        protected virtual void OnShowing()
        {

        }

        protected virtual void OnShowed()
        {

        }

        protected virtual void OnClosing()
        {

        }

		public enum WindowState
		{
			Normal,
			Minimized,
			Maximized
		}

		private enum MouseMoveAction
		{			
			Move,
			TopLeftResize,
			LeftResize,
			BottomLeftResize,
			BottomResize,
			BottomRightResize,
			RightResize,
			TopResize,
			TopRightResize
		}

		public static Form ActiveForm
		{
			get { return _ActiveForm; }
			set
			{
				if(_ActiveForm != value)
				{
					_PrevActiveForm = _ActiveForm;

					if(_ActiveForm != null)
					{			
						if(_ActiveForm.Base != null)
						{
                            _ActiveForm.BodyOverLay.Style.Visibility = Visibility.Visible;

                            _ActiveForm.Base.Style.BorderColor = "#AAAAAA";
						}						
					}
					_ActiveForm = value;
					if(_ActiveForm != null)
					{
						if(_ActiveForm.Base != null)
						{
                            _ActiveForm.BodyOverLay.Style.Visibility = Visibility.Collapse;
                            _ActiveForm.Base.Style.BorderColor = "#FBFBFB";
							_ActiveForm.BringToFront();
						}						
					}					
				}

			}
		}

		public static List<Form> VisibleForm = new List<Form>();

		public static HTMLInputElement CreateStartSearchInput()
		{
			var input = new HTMLInputElement() { Type = InputType.Text };
			bool InputFocused = false;

			input.Id = "StartSearchInput";
			input.Style.Position = Position.Absolute;
			input.Style.Width = "344px";
			input.Style.Height = "40px";
			input.Style.Left = "48px";
			input.Style.Top = "0";
			input.Style.BackgroundColor = "#3F3F3F";

			input.Style.BorderStyle = BorderStyle.None;
			input.Style.Padding = "0";
			input.Style.Margin = "0";

			input.Style.FontFamily = "Segoe UI";
			input.Style.FontSize = "11pt";
			input.Style.TextIndent = "13px";
			input.Style.Color = "black";

			input.Placeholder = "Search the web and Windows";

			//	Heading.Style.FontFamily = "Segoe UI";		
			input.AddEventListener(EventType.KeyUp, (ev) => {
                var kev = ev.As<KeyboardEvent>();

                if(kev.KeyCode == 13)
                {
                    // create a new Form

                    var frm = new FormBrowser();
                    frm.Left = "100px";
                    frm.Top = "100px";
                    //https://www.bing.com/search?q=
                    //https://www.google.com/#q=
                    frm.Navigate(string.Format("https://www.bing.com/search?q={0}", input.Value));
                    frm.Show();

                    input.Blur();
                }
            });
            			

			input.OnMouseUp = (ev) =>
			{
				if(MovingForm != null)
					return;

				ev.StopPropagation();								
			};
			jQuery.Select(input).On("focus", () => {
				input.Style.BackgroundColor = "#F3F3F3";
				input.Style.Outline = "0";								
			});
			jQuery.Select(input).On("focusout", () => {
				input.Style.BackgroundColor = "#3F3F3F";
				input.Value = "";
				InputFocused = false;
			});
			jQuery.Select(input).On("focusin", () => {
				input.Style.BackgroundColor = "#F3F3F3";
				InputFocused = true;
			});
			
			input.OnMouseDown = (ev) =>
			{
				if(MovingForm != null)
					return;

				Mouse_Down = true;

				ev.StopPropagation();				
				
				ActiveForm = null;
			};

			input.OnMouseEnter = (ev) =>
			{
				if(MovingForm != null)
					return;
				if(InputFocused)
				{
					input.Style.BackgroundColor = "#F3F3F3";
				}
				else
				{
					input.Style.BackgroundColor = "#575757";
				}				
			};

			input.OnMouseLeave = (ev) =>
			{
				if(MovingForm != null)
					return;
				
				if(InputFocused)
				{
					input.Style.BackgroundColor = "#F3F3F3";
				}
				else
				{
					input.Style.BackgroundColor = "#3F3F3F";					
				}
			};

			return input;
		}

		public static HTMLDivElement CreateStartButton()
		{
			var butt = new HTMLDivElement();

			butt.Style.Width = "48px";
			butt.Style.Height = "40px";
			butt.Style.Position = Position.Absolute;
			butt.Style.FontSize = "12pt";
			butt.Style.Background = WinIcon;

			butt.OnMouseUp = (ev) =>
			{
				if(MovingForm != null)
					return;

				ev.StopPropagation();
				ev.PreventDefault();

				butt.Style.Background = WinIcon;
			};

			butt.OnMouseDown = (ev) =>
			{
				if(MovingForm != null)
					return;

				Mouse_Down = true;

				ev.StopPropagation();
				ev.PreventDefault();

				butt.Style.Background = WinIconDown;

				ActiveForm = null;
			};

			butt.OnMouseEnter = (ev) =>
			{
				if(MovingForm != null)
					return;

				if(Mouse_Down)
				{
					butt.Style.Background = WinIconDown;
				}
				else
				{
					butt.Style.Background = WinIconHover;
				}				
			};

			butt.OnMouseLeave = (ev) =>
			{
				if(MovingForm != null)
					return;

				butt.Style.Background = WinIcon;
			};

			return butt;
		}

		public static void Setup(HTMLElement parent = null)
		{
			var keyCodes =  new List<int>(new int[] { 61, 107, 173, 109, 187, 189 });

			Document.AddEventListener(EventType.KeyDown, (ev) =>
			{
				var kev = ev.As<KeyboardEvent>();

				if(kev.CtrlKey && (keyCodes.Contains(kev.Which)))
				{
                    ev.PreventDefault();
				}
			});

            Script.Write("$(window).bind('mousewheel DOMMouseScroll', function (event) { if (event.ctrlKey == true) { event.preventDefault(); } });");            

            if (parent == null)			
				Parent = Document.Body;			
			else			
				Parent = parent;
			
			WindowHolder = new HTMLDivElement();
			WindowHolder.Style.Position = Position.Absolute;
			//TaskBar.Style.VerticalAlign = VerticalAlign.Bottom;
			WindowHolder.Style.Width = "100%";
			WindowHolder.Style.Height = "-webkit-calc(100% - 40px)";
			WindowHolder.Style.Top = "0";
			WindowHolder.Style.Left = "0";
			WindowHolder.Style.BackgroundColor = "cornflowerblue";
			WindowHolder.Style.ZIndex = "0";
			WindowHolder.Style.Overflow = Overflow.Auto;
			//user-select: none;

			jQuery.Select(WindowHolder).Css("user-select", "none");

			TaskBar = new HTMLDivElement();
			TaskBar.Style.Position = Position.Absolute;
			//TaskBar.Style.VerticalAlign = VerticalAlign.Bottom;
			TaskBar.Style.Width = "100%";
			TaskBar.Style.Height = "40px";
			TaskBar.Style.Top = "-webkit-calc(100% - 40px)";
			TaskBar.Style.Left = "0";
			TaskBar.Style.ZIndex = int.MaxValue.ToString();

			jQuery.Select(TaskBar).Css("user-select", "none");

			TaskBar.Style.BackgroundColor = "#101010";

			ButtonStart = CreateStartButton();

			InputStartSearch = CreateStartSearchInput();
			
			Action<Event> mouseMove = (ev) =>
			{
				if(MovingForm != null)
				{
                    if (MovingForm.BodyOverLay.Style.Visibility == Visibility.Collapse)
                    {
                        MovingForm.BodyOverLay.Style.Visibility = Visibility.Visible;
                        MovingForm.ChangeSelectionState(true);
                        MovingForm.Heading.Focus();
                    }

					var mev = ev.As<MouseEvent>();

					var obj = jQuery.Select(MovingForm.Base);

					var Y = (mev.ClientY + MovingForm.prev_py); // MovingForm.prev_py  Global.ParseInt(obj.Css("top")) + 
					var X = (mev.ClientX + MovingForm.prev_px); // - MovingForm.prev_px // Global.ParseInt(obj.Css("left")) + 

					if(MovingForm.windowState == WindowState.Maximized && MoveAction == MouseMoveAction.Move)
					{
						MovingForm.changeWindowState();
						X = mev.ClientX - (MovingForm.prev_width / 2);

						MovingForm.prev_px = X - mev.ClientX;
					}

					int X1;
					int Y1;

					int W;
					int H;

					if(Y < 0)
						Y = 1;
					if(X < 0)
						X = 1;

					ev.StopPropagation();

					switch(MoveAction)
					{
						case MouseMoveAction.Move:							
							obj.Css("top", Y);
							obj.Css("left", X);

							break;
						case MouseMoveAction.TopLeftResize:
							X1 = Global.ParseInt(obj.Css("left"));
							Y1 = Global.ParseInt(obj.Css("top"));

							W = Global.ParseInt(obj.Css("width"));
							H = Global.ParseInt(obj.Css("height"));							
							
							W -= X - X1;
							H -= Y - Y1;

							if(W < MovingForm.MinWidth)
							{
								X -= MovingForm.MinWidth - W;
								W = MovingForm.MinWidth;
							}

							if(H < MovingForm.MinHeight)
							{
								Y -= MovingForm.MinHeight - H;
								H = MovingForm.MinHeight;
							}

							obj.Css("top", Y);
							obj.Css("left", X);

							obj.Css("width", W);
							obj.Css("height", H);

							break;
						case MouseMoveAction.TopResize:							
							Y1 = Global.ParseInt(obj.Css("top"));
							
							H = Global.ParseInt(obj.Css("height"));
							
							H -= Y - Y1;

							if(H < MovingForm.MinHeight)
							{
								Y -= MovingForm.MinHeight - H;
								H = MovingForm.MinHeight;
							}

							obj.Css("top", Y);						
							
							obj.Css("height", H);

							break;
						case MouseMoveAction.TopRightResize:
							Y1 = Global.ParseInt(obj.Css("top"));
							X1 = Global.ParseInt(obj.Css("left"));

							W = Global.ParseInt(obj.Css("width"));
							H = Global.ParseInt(obj.Css("height"));

							H -= Y - Y1;
							W = mev.ClientX - X1;

							if(H < MovingForm.MinHeight)
							{
								Y -= MovingForm.MinHeight - H;
								H = MovingForm.MinHeight;
							}							

							if(W < MovingForm.MinWidth)
							{
								W = MovingForm.MinWidth;
							}

							obj.Css("top", Y);

							obj.Css("height", H);
							obj.Css("width", W);
														
							break;
						case MouseMoveAction.LeftResize:
							X1 = Global.ParseInt(obj.Css("left"));
							W = Global.ParseInt(obj.Css("width"));

							W -= X - X1;

							if(W < MovingForm.MinWidth)
							{
								X -= MovingForm.MinWidth - W;
								W = MovingForm.MinWidth;
							}

							obj.Css("left", X);
							obj.Css("width", W);

							break;
						case MouseMoveAction.BottomLeftResize:
							X1 = Global.ParseInt(obj.Css("left"));
							Y1 = Global.ParseInt(obj.Css("top"));

							W = Global.ParseInt(obj.Css("width"));
							H = Global.ParseInt(obj.Css("height"));

							W -= X - X1;
							H = mev.ClientY - Y1;					
							
							if(W < MovingForm.MinWidth)
							{
								X -= MovingForm.MinWidth - W;
								W = MovingForm.MinWidth;
							}

							if(H < MovingForm.MinHeight)
							{								
								H = MovingForm.MinHeight;
							}

							obj.Css("left", X);
							obj.Css("width", W);
							
							obj.Css("height", H);


							break;
						case MouseMoveAction.BottomResize:
							Y1 = Global.ParseInt(obj.Css("top"));
							H = Global.ParseInt(obj.Css("height"));

							H = mev.ClientY - Y1;

							if(H < MovingForm.MinHeight)
							{
								H = MovingForm.MinHeight;
							}

							obj.Css("height", H);

							break;
						case MouseMoveAction.RightResize:
							X1 = Global.ParseInt(obj.Css("left"));
							W = Global.ParseInt(obj.Css("width"));

							W = mev.ClientX - X1;

							if(W < MovingForm.MinWidth)
							{
								W = MovingForm.MinWidth;
							}

							obj.Css("width", W);

							break;
						case MouseMoveAction.BottomRightResize:
							Y1 = Global.ParseInt(obj.Css("top"));
							H = Global.ParseInt(obj.Css("height"));

							X1 = Global.ParseInt(obj.Css("left"));
							W = Global.ParseInt(obj.Css("width"));

							W = mev.ClientX - X1;

							H = mev.ClientY - Y1;

							if(H < MovingForm.MinHeight)
							{
								H = MovingForm.MinHeight;
							}							

							if(W < MovingForm.MinWidth)
							{
								W = MovingForm.MinWidth;
							}

							obj.Css("width", W);
							obj.Css("height", H);

							break;
						default:
							break;
					}					
				}				
			};

			Window.AddEventListener(EventType.MouseUp, (ev) =>
			{
                if(MovingForm != null)
                {
                    MovingForm.BodyOverLay.Style.Visibility = Visibility.Collapse;
                    MovingForm.ChangeSelectionState(false);
                }
                
                MovingForm = null;
				Mouse_Down = false;				
				MoveAction = MouseMoveAction.Move;
                
            });

			Window.AddEventListener(EventType.MouseMove, mouseMove);

			Parent.AppendChild(WindowHolder);
			Parent.AppendChild(TaskBar);
			
			TaskBar.AppendChild(ButtonStart);
			TaskBar.AppendChild(InputStartSearch);			
		}
		private enum FormButtonType
		{
			Close,
			Maximize,
			Minimize,
			Restore,
			Help
		}

		private void changeWindowState()
		{
			if(windowState == WindowState.Maximized)
			{
				Width = prev_width + "px";
				Height = prev_height + "px";

				Top = prev_top + "px";
				Left = prev_left + "px";

				windowState = WindowState.Normal;
			}
			else
			{
				prev_height = Global.ParseInt(Height);
				prev_width = Global.ParseInt(Width);

				prev_left = Global.ParseInt(Left);
				prev_top = Global.ParseInt(Top);

				windowState = WindowState.Maximized;

				Width = "-webkit-calc(100% - 4px)"; // "100%";
				Height = "-webkit-calc(100% - 4px)"; //"100%";

				Top = "0";
				Left = "0";
			}
		}

		private HTMLDivElement CreateFormButton(FormButtonType Type)
		{
			var butt = new HTMLDivElement();
			
			butt.Style.Width = "45px";
			butt.Style.Height = "29px";
			butt.Style.Position = Position.Absolute;
			butt.Style.FontSize = "12pt";

			switch(Type)
			{
				case FormButtonType.Close:
					butt.Style.BackgroundColor = "white";
					butt.Style.Color = "black";

					butt.Style.Left = "-webkit-calc(100% - 45px)";					
					butt.Id = "Close";
					butt.InnerHTML = "&#10006";
					
					butt.OnMouseDown = (ev) =>
					{
						if(MovingForm != null)
							return;
						Mouse_Down = true;

						ev.StopPropagation();
						ev.PreventDefault();

						butt.Style.BackgroundColor = "#F1707A";
						butt.Style.Color = "white";

						ActiveForm = this;
					};

					butt.OnMouseUp = (ev) =>
					{
						if(MovingForm != null)
							return;

						ev.StopPropagation();
						ev.PreventDefault();

						Close();
					};

					butt.OnMouseEnter = (ev) =>
					{                        
						if(MovingForm != null)
							return;

                        SetCursor(Cursor.Default);
                        
						if(Mouse_Down)
						{
							butt.Style.BackgroundColor = "#F1707A";							
						}
						else
						{
							butt.Style.BackgroundColor = "#E81123";							
						}
						butt.Style.Color = "white";
					};

					butt.OnMouseLeave = (ev) =>
					{
						if(MovingForm != null)
							return;

						butt.Style.BackgroundColor = "white";
						butt.Style.Color = "black";
					};					

					break;
				case FormButtonType.Maximize:
					butt.Style.BackgroundColor = "white";
					butt.Style.Left = "-webkit-calc(100% - 91px)";
					butt.Style.Color = "black";
					butt.Id = "Maximize";
					butt.InnerHTML = "&#9633;";
					butt.Style.FontSize = "14pt";

					butt.OnMouseUp = (ev) =>
					{
						if(MovingForm != null)
							return;

						ev.StopPropagation();
						ev.PreventDefault();

						Mouse_Down = false;

						butt.Style.BackgroundColor = "white";
						butt.Style.Color = "black";

						changeWindowState();					
					};

					break;
				case FormButtonType.Minimize:
					butt.Style.BackgroundColor = "white";
					butt.Style.Left = "-webkit-calc(100% - 137px)";
					butt.Style.Color = "black";
					butt.Id = "Minimize";
					butt.InnerHTML = "&#8213;";

					butt.OnMouseUp = (ev) =>
					{
						if(MovingForm != null)
							return;

						ev.StopPropagation();
						ev.PreventDefault();

						Mouse_Down = false;

						butt.Style.BackgroundColor = "white";
						butt.Style.Color = "black";

						windowState = WindowState.Minimized;
					};

					break;					
				case FormButtonType.Restore:
					break;
				case FormButtonType.Help:
					break;
				default:
					butt.OnMouseUp = (ev) =>
					{
						if(MovingForm != null)
							return;

						ev.StopPropagation();
						ev.PreventDefault();

						Mouse_Down = false;
					};
					break;
			}

			butt.OnMouseMove = (ev) =>
			{
				if(MovingForm != null)
					return;

				ev.StopImmediatePropagation();
				ev.PreventDefault();				
			};

			if(Type != FormButtonType.Close)
			{
				butt.OnMouseDown = (ev) =>
				{
					if(MovingForm != null)
						return;

					Mouse_Down = true;

					ev.StopPropagation();
					ev.PreventDefault();

					butt.Style.BackgroundColor = "#CACACA";
					butt.Style.Color = "black";

					ActiveForm = this;
				};

				butt.OnMouseEnter = (ev) =>
				{
					if(MovingForm != null)
						return;
                    SetCursor(Cursor.Default);

                    if (Mouse_Down)
					{
						butt.Style.BackgroundColor = "#CACACA";
					}
					else
					{
						butt.Style.BackgroundColor = "#E5E5E5";
					}
					butt.Style.Color = "black";
				};

				butt.OnMouseLeave = (ev) =>
				{
					if(MovingForm != null)
						return;

					butt.Style.BackgroundColor = "white";
					butt.Style.Color = "black";
				};
			}

			butt.Style.Top = "0";

			butt.Style.Padding = "0";
			butt.Style.Margin = "0";
			butt.Style.BorderWidthString = "0";

			butt.Style.FontFamily = "Lucida Sans Unicode";	
			butt.Style.TextAlign = TextAlign.Center;

			return butt;

		}

		public void SetCursor(Cursor cur)
		{
			Base.Style.Cursor = cur;
			Heading.Style.Cursor = cur;
			//ButtonClose.Style.Cursor = cur;
			//ButtonExpand.Style.Cursor = cur;
			//ButtonMinimize.Style.Cursor = cur;
		}        

        public void ChangeSelectionState(bool TurnOff = true)
        {
            ChangeSelectionState(Parent.Children, TurnOff);
        }

        public void ChangeSelectionState(HTMLCollection Children, bool TurnOff = true)
        {
            if (Children == null)
                return;
            
            foreach (var item in Children)
            {
                if(item.TagName.ToLower() == "input" || item.TagName.ToLower() == "span" || item.TagName.ToLower() == "textarea")
                {
                    if(TurnOff)
                    {                        
                        jQuery.Select(item).Css("user-select", "none");
                    }
                    else
                    {
                        jQuery.Select(item).Css("user-select", "text");
                    }
                }
                if(item.ChildElementCount > 0)
                {
                    ChangeSelectionState(item.Children, TurnOff);
                }
            }
        }
		
		public Form()
		{			
			Base = new HTMLDivElement();

			Base.Id = "Base";

			Heading = new HTMLDivElement();

			HeadingTitle = new HTMLSpanElement();

			Body = new HTMLDivElement();
            BodyOverLay = new HTMLDivElement();

            BodyOverLay.Style.Visibility = Visibility.Collapse;

            Base.AppendChild(Heading);
			Base.AppendChild(Body);
            Base.AppendChild(BodyOverLay);

            Base.AddEventListener(EventType.MouseDown, (ev) => {
				var mev = ev.As<MouseEvent>();
				Mouse_Down = true;

				var obj = jQuery.Select(Base);

				

				if(windowState == WindowState.Maximized)
				{
					SetCursor(Cursor.Default);

					MoveAction = MouseMoveAction.Move;

					//prev_px = prev_left - mev.ClientX;
					//prev_py = prev_top - mev.ClientY;

					prev_px = Global.ParseInt(obj.Css("left")) - mev.ClientX;
					prev_py = Global.ParseInt(obj.Css("top")) - mev.ClientY;

					return;
				}
				else
				{
					prev_px = Global.ParseInt(obj.Css("left")) - mev.ClientX;
					prev_py = Global.ParseInt(obj.Css("top")) - mev.ClientY;
				}				

				if(mev.LayerX <= ResizeCorners && mev.LayerY <= ResizeCorners)
				{					
					SetCursor(Cursor.NorthWestSouthEastResize);

					MoveAction  = MouseMoveAction.TopLeftResize;
				}
				else if(mev.LayerY <= ResizeCorners && mev.LayerX >= Global.ParseInt(this.Width) - ResizeCorners)
				{					
					SetCursor(Cursor.NorthEastSouthWestResize);

					MoveAction = MouseMoveAction.TopRightResize;					
				}
				else if(mev.LayerY <= ResizeCorners)
				{					
					SetCursor(Cursor.NorthResize);

					MoveAction = MouseMoveAction.TopResize;
				}
				else if(mev.LayerX <= ResizeCorners && mev.LayerY >= Global.ParseInt(this.Height) - ResizeCorners)
				{					
					SetCursor(Cursor.NorthEastSouthWestResize);

					MoveAction = MouseMoveAction.BottomLeftResize;
				}
				else if(mev.LayerY >= Global.ParseInt(this.Height) - ResizeCorners && mev.LayerX >= Global.ParseInt(this.Width) - ResizeCorners)
				{					
					SetCursor(Cursor.NorthWestSouthEastResize);
					
					MoveAction = MouseMoveAction.BottomRightResize;
				}
				else if(mev.LayerY >= Global.ParseInt(this.Height) - ResizeCorners)
				{
					SetCursor(Cursor.SouthResize);
					
					MoveAction = MouseMoveAction.BottomResize;
				}
				else if(mev.LayerX <= ResizeCorners)
				{
					SetCursor(Cursor.WestResize);
					
					MoveAction = MouseMoveAction.LeftResize;

				}
				else if(mev.LayerX >= Global.ParseInt(this.Width) - ResizeCorners)
				{
					SetCursor(Cursor.EastResize);
					
					MoveAction = MouseMoveAction.RightResize;					
				}
				else
				{
					SetCursor(Cursor.Default);
					
					MoveAction = MouseMoveAction.Move;
					return;
				}

                ChangeSelectionState();

                MovingForm = this;
				mev.StopPropagation();
			});

			Heading.AddEventListener(EventType.DblClick, (ev) => {
				changeWindowState();
				ev.PreventDefault();
				ev.StopPropagation();
			});

			Base.AddEventListener(EventType.MouseMove, (ev) => {
				var mev = ev.As<MouseEvent>();
				if(MovingForm != null && MoveAction == MouseMoveAction.Move)
				{					
					SetCursor(Cursor.Default);
					return;
				}else if(windowState == WindowState.Maximized)
				{
					SetCursor(Cursor.Default);
					return;
				}

				if(MoveAction == MouseMoveAction.TopLeftResize || mev.LayerX <= ResizeCorners && mev.LayerY <= ResizeCorners)
				{					
					SetCursor(Cursor.NorthWestSouthEastResize);
				}
				else if(MoveAction == MouseMoveAction.TopRightResize || mev.LayerY <= ResizeCorners && mev.LayerX >= Global.ParseInt(this.Width) - ResizeCorners)
				{					
					SetCursor(Cursor.NorthEastSouthWestResize);
				}
				else if(mev.LayerY <= ResizeCorners || MoveAction == MouseMoveAction.TopResize)
				{
					SetCursor(Cursor.NorthResize);					
				}
				else if(MoveAction == MouseMoveAction.BottomLeftResize || mev.LayerX <= ResizeCorners && mev.LayerY >= Global.ParseInt(this.Height) - ResizeCorners)
				{
					SetCursor(Cursor.NorthEastSouthWestResize);					
				}
				else if(MoveAction == MouseMoveAction.BottomRightResize || mev.LayerY >= Global.ParseInt(this.Height) - ResizeCorners && mev.LayerX >= Global.ParseInt(this.Width) - ResizeCorners)
				{
					SetCursor(Cursor.NorthWestSouthEastResize);					
				}
				else if(MoveAction == MouseMoveAction.BottomResize || mev.LayerY >= Global.ParseInt(this.Height) - ResizeCorners)
				{
					SetCursor(Cursor.SouthResize);					
				}
				else if(MoveAction == MouseMoveAction.LeftResize || mev.LayerX <= ResizeCorners)
				{
					SetCursor(Cursor.WestResize);									
				}
				else if(MoveAction == MouseMoveAction.RightResize || mev.LayerX >= Global.ParseInt(this.Width) - ResizeCorners)
				{
					SetCursor(Cursor.EastResize);	
				}else
				{
					SetCursor(Cursor.Default);					
				}								
			});

			Base.Style.Position = Position.Absolute;			

			Heading.Id = "Heading";
			Heading.Style.Height = "29px";
			Heading.Style.Width = "100%";
			Heading.Style.VerticalAlign = VerticalAlign.Top;
			Heading.Style.Cursor = Cursor.Default;
			Heading.Style.BackgroundColor = "white";// "#007ACC";
			Heading.Style.MarginTop = "0";
			Heading.Style.MarginLeft = "0";
			Heading.Style.MarginRight = "0";
			Heading.Style.MarginBottom = "0";
			Heading.Style.PaddingBottom = "1px";

            HeadingTitle.Style.TextIndent = "3px";


            Heading.AddEventListener(EventType.MouseDown, (ev) => {
				if(windowState == WindowState.Maximized)
				{					
					MovingForm = this;
					SetCursor(Cursor.Default);

					MoveAction = MouseMoveAction.Move;
				}else
				{
					MovingForm = this;
				}				

				ActiveForm = this;
			});

			Heading.Style.FontFamily = "Segoe UI";			
			Heading.Style.TextAlign = TextAlign.Left | TextAlign.Center;

			ButtonClose = CreateFormButton(FormButtonType.Close);
			ButtonExpand = CreateFormButton(FormButtonType.Maximize);
			ButtonMinimize = CreateFormButton(FormButtonType.Minimize);

			Body.AddEventListener(EventType.MouseDown, (ev) => {				
				ActiveForm = this;
			});

			jQuery.Select(Heading)
				.Css("user-select", "none")
				.Css("user-drag:", "none");

			jQuery.Select(Base)
				.Css("user-select", "none")
				.Css("user-drag:", "none");

			jQuery.Select(HeadingTitle)
				.Css("user-select", "none")
				.Css("user-drag:", "none");

			//user-select: none;

			Body.Id = "Body";
			Body.Style.Top = "30px";
			Body.Style.Height = "-webkit-calc(100% - 30px)"; // -webkit-calc(100% - 60px)
			Body.Style.Width = "100%";
            Body.Style.Position = Position.Absolute;            

            Height = "480px";
			Width = "640px";

			Body.Style.BackgroundColor = "#F0F0F0";

            BodyOverLay.Style.Top = "30px";
            BodyOverLay.Style.Height = "-webkit-calc(100% - 30px)"; // -webkit-calc(100% - 60px)
            BodyOverLay.Style.Width = "100%";
            BodyOverLay.Style.Position = Position.Absolute;
            BodyOverLay.Style.ZIndex = int.MaxValue.ToString();
            BodyOverLay.Style.Opacity = "0";            

            BodyOverLay.AddEventListener(EventType.MouseDown, (ev) =>
            {
                BodyOverLay.Style.Visibility = Visibility.Collapse;
                ActiveForm = this;				
            });

            Base.Style.BorderStyle = BorderStyle.Solid;
			Base.Style.BorderWidthString = "2px";
			Base.Style.BorderColor = "#FBFBFB";
			jQuery.Select(Base).Css("box-shadow", "0px 0px 63px -17px rgba(0,0,0,0.75)");
			
			Heading.AppendChild(HeadingTitle);
			Heading.AppendChild(ButtonClose);
			Heading.AppendChild(ButtonExpand);
			Heading.AppendChild(ButtonMinimize);

            Initialise();
        }

		public string Height
		{
			get { return Base.Style.Height; }
			set { Base.Style.Height = value; }
		}

		public string Width
		{
			get { return Base.Style.Width; }
			set { Base.Style.Width = value; }
		}

		public string Left
		{
			get { return Base.Style.Left; }
			set { Base.Style.Left = value; }
		}

		public string Top
		{
			get { return Base.Style.Top; }
			set { Base.Style.Top = value; }
		}

		public string Text
		{
			get { return HeadingTitle.InnerHTML; }
			set { HeadingTitle.InnerHTML = value; }
		}

		public string BackColor
		{
			get { return Body.Style.BackgroundColor; }
			set { Body.Style.BackgroundColor = value; }
		}

		public string ForeColor
		{
			get { return Body.Style.Color; }
			set { Body.Style.Color = value; }
		}

		public void Show(HTMLElement owner = null)
		{
			if(!VisibleForm.Contains(this))
			{
                OnShowing();

                if (owner == null)
				{
					WindowHolder.AppendChild(Base);					
					owner = WindowHolder;
				}
				else
					owner.AppendChild(Base);

				Owner = owner;

				Base.Style.Visibility = Visibility.Visible;

                Body.Focus();
                		
				VisibleForm.Add(this);

				CalculateZOrder();

                OnShowed();
            }
			ActiveForm = this;
		}

		public void BringToFront()
		{
			if(VisibleForm.Count > 1 &&
				VisibleForm.LastOrDefault() != this)
			{
				VisibleForm.Remove(this);
				VisibleForm.Add(this);				
			}

			CalculateZOrder();
		}

		public static void CalculateZOrder()
		{
			if(VisibleForm == null)
				return;
			for(int i = 0; i < VisibleForm.Count; i++)
			{
				if(VisibleForm[i] != null &&
					VisibleForm[i].Base != null)
				{
					jQuery.Select(VisibleForm[i].Base).Css("zIndex", i + 1);
				}
			}
		}		

		public void Close()
		{
            OnClosing();

            ActiveForm = _PrevActiveForm;

			VisibleForm.Remove(this);
                        
			if(Base != null)
			{
				jQuery.Select(Base).Empty();
				Base.Remove();
				Base = null;
			}

			CalculateZOrder();			
		}
	}
}
