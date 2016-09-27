using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using Bridge.jQuery2;
using Bridge;

namespace WinFormjs
{
	public class Form
	{
		public static HTMLDivElement TaskBar { get; set; }
		public static HTMLDivElement WindowHolder { get; set; }
		public static HTMLDivElement ButtonStart { get; set; }
		public static HTMLInputElement InputStartSearch { get; set; }
		public static int ResizeCorners { get; set; } = 2;
		public static Form MovingForm = null;
		public static HTMLElement Parent = null;
		public static bool Mouse_Down { get; set; } = false;
		public static int FadeLength { get; set; } = 100;
        public static string Window_BorderColorFocused { get; set; } = "#FBFBFB";
        public static string Window_BorderColor { get; set; } = "#FBFBFB"; // "#d8d8d8";
        public static string Window_HeadingBackgroundColor { get; set; } = "white";
        public static string Window_DefaultBackgroundColor { get; set; } = "#F0F0F0";
        public static List<Form> VisibleForm = new List<Form>();
        private static FileExplorer Window_Desktop;
        /// <summary>
        /// This is used for testing
        /// </summary>
        public static bool ShowBodyOverLay { get; set; } = false;

        public static int Window_DefaultHeight { get; set; } = 480;
        public static int Window_DefaultWidth { get; set; } = 640;

        private static Form _ActiveForm;
		private static Form _PrevActiveForm;
		private static MouseMoveAction MoveAction = MouseMoveAction.Move;
		public static HTMLDivElement WindowHolderSelectionBox { get; set; }

        public static FileExplorer ActiveFileExplorer { get; set; }


        public static int WindowHolderSelectionBoxX;
        public static int WindowHolderSelectionBoxY;

        public static int WindowHolderSelectionBoxXOff;
        public static int WindowHolderSelectionBoxYOff;
        
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

        public static bool MidleOfAction()
        {
            return WindowHolderSelectionBox != null || MovingForm != null;
        }
		
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

        public static void SetBodyOverLay()
        {
            for (int i = 0; i < VisibleForm.Count; i++)
            {
                var frm = VisibleForm[i];
                if (frm != null &&
                    frm.BodyOverLay != null &&
                    frm.BodyOverLay.Style.Visibility == Visibility.Collapse)
                {
                    frm.BodyOverLay.Style.Visibility = Visibility.Visible;
                }
            }         
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

                            _ActiveForm.Base.Style.BorderColor = Window_BorderColor;
						}						
					}
					_ActiveForm = value;
					if(_ActiveForm != null)
					{
						if(_ActiveForm.Base != null)
						{
                            _ActiveForm.BodyOverLay.Style.Visibility = Visibility.Collapse;
                            _ActiveForm.Base.Style.BorderColor = Window_BorderColorFocused;
							_ActiveForm.BringToFront();
						}
					}
				}

			}
		}		

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
                    if(input.Value.ToLower() == "notepad")
                    {
                        var Notepad = new FormNotePad();
                        Notepad.Left = "50px";
                        Notepad.Top = "50px";
                        Notepad.Text = "Note Pad";
                        Notepad.Show();
                    }
                    else if (input.Value.ToLower() == "cmd")
                    {
                        var cmd = new FormConsole();
                        cmd.Left = "50px";
                        cmd.Top = "50px";
                        cmd.Text = "Command Prompt";
                        cmd.Show();
                    }
                    else
                    {
                        var frm = new FormBrowser();
                        frm.Left = "100px";
                        frm.Top = "100px";
                        //https://www.bing.com/search?q=
                        //https://www.google.com/#q=
                        frm.Navigate(string.Format("https://www.bing.com/search?q={0}", input.Value));
                        frm.Show();
                    }                    

                    input.Blur();
                }
            });
            			

			input.OnMouseUp = (ev) =>
			{
				if(MidleOfAction())
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
				if(MidleOfAction())
					return;

				Mouse_Down = true;

				ev.StopPropagation();				
				
				ActiveForm = null;
			};

			input.OnMouseEnter = (ev) =>
			{
				if(MidleOfAction())
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
				if(MidleOfAction())
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

        public static void ChangeStateTextSelection(HTMLElement element, bool state)
        {            
            if (state)
            {
                jQuery.Select(element).Css("user-select", "text");
            }
            else
            {
                jQuery.Select(element).Css("user-select", "none");                
            }
        }

        public static void DisableStateDrag(HTMLElement element)
        {
            if(element is HTMLImageElement)
            {
                element.As<HTMLImageElement>().OnDragStart = (ev) =>
                {
                    ev.PreventDefault();
                };
            }
            else
            {
                jQuery.Select(element).Css("user-drag:", "none");
            }            
        }

        public static HTMLDivElement CreateStartButton()
		{
			var butt = new HTMLDivElement();

			butt.Style.Width = "48px";
			butt.Style.Height = "40px";
			butt.Style.Position = Position.Absolute;
			butt.Style.FontSize = "12pt";
			butt.Style.Background = IconRepository.WrapBase64(IconRepository.IMAGE_WinIcon);

			butt.OnMouseUp = (ev) =>
			{
				if(MidleOfAction())
					return;

				ev.StopPropagation();
				ev.PreventDefault();

				butt.Style.Background = IconRepository.WrapBase64(IconRepository.IMAGE_WinIcon);
			};

			butt.OnMouseDown = (ev) =>
			{
				if(MidleOfAction())
					return;

				Mouse_Down = true;

				ev.StopPropagation();
				ev.PreventDefault();

				butt.Style.Background = IconRepository.WrapBase64(IconRepository.IMAGE_WinIconDown);

				ActiveForm = null;
			};

			butt.OnMouseEnter = (ev) =>
			{
				if(MidleOfAction())
					return;

				if(Mouse_Down)
				{
					butt.Style.Background = IconRepository.WrapBase64(IconRepository.IMAGE_WinIconDown);
				}
				else
				{
					butt.Style.Background = IconRepository.WrapBase64(IconRepository.IMAGE_WinIconHover);
				}				
			};

			butt.OnMouseLeave = (ev) =>
			{
				if(MidleOfAction())
					return;

				butt.Style.Background = IconRepository.WrapBase64(IconRepository.IMAGE_WinIcon);
			};

			return butt;
		}

        protected static void HideFileSelection()
        {
            if (ActiveFileExplorer != null)
            {                
                if (ActiveFileExplorer.Owner != null)
                {
                    ActiveFileExplorer.Owner.BodyOverLay.Style.Visibility = Visibility.Collapse;
                }
                ActiveFileExplorer = null;
            }

            if (WindowHolderSelectionBox != null)
            {

                jQuery.Select(WindowHolderSelectionBox).FadeOut(FadeLength, () => { WindowHolderSelectionBox.Remove(); WindowHolderSelectionBox = null; });
            }
        }

		public static void Setup(HTMLElement parent = null)
		{
			StyleController.Setup();


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
			
			WindowHolder.Style.Width = "100%";
			WindowHolder.Style.Height = StyleController.Calc(100, 40);
			WindowHolder.Style.Top = "0";
			WindowHolder.Style.Left = "0";
			WindowHolder.Style.BackgroundColor = "cornflowerblue";
			WindowHolder.Style.ZIndex = "0";
			WindowHolder.Style.Overflow = Overflow.Auto;									            
    
            //SetBodyOverLay();
            
            ChangeStateTextSelection(WindowHolder, false);

			TaskBar = new HTMLDivElement();
			TaskBar.Style.Position = Position.Absolute;
			
			TaskBar.Style.Width = "100%";
			TaskBar.Style.Height = "40px";
			TaskBar.Style.Top = StyleController.Calc(100, 40);
			TaskBar.Style.Left = "0";
			TaskBar.Style.ZIndex = int.MaxValue.ToString();

            ChangeStateTextSelection(TaskBar, false);            

			TaskBar.Style.BackgroundColor = "#101010";

			ButtonStart = CreateStartButton();

			InputStartSearch = CreateStartSearchInput();
			
			Action<Event> mouseMove = (ev) =>
			{
				var mev = ev.As<MouseEvent>();

				if(MovingForm != null)
				{
                    if (MovingForm.BodyOverLay.Style.Visibility == Visibility.Collapse)
                    {
                        MovingForm.BodyOverLay.Style.Visibility = Visibility.Visible;
                        MovingForm.ChangeSelectionState(true);
                        MovingForm.Heading.Focus();
                    }
					
					var obj = jQuery.Select(MovingForm.Base);
                    
                    var Y = (mev.ClientY  + MovingForm.prev_py);
					var X = (mev.ClientX + MovingForm.prev_px);

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
							obj.Css("top", Y).
                                Css("left", X);							

							break;
						case MouseMoveAction.TopLeftResize:							
                            Rectange.SetBounds(out X1, out Y1, out W, out H, obj);

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

                            obj.Css("left", X).Css("top", Y).Css("width", W).Css("height", H);                            

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

							obj.Css("top", Y).
                                Css("height", H);

							break;
						case MouseMoveAction.TopRightResize:							
                            Rectange.SetBounds(out X1, out Y1, out W, out H, obj);

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

							obj.Css("top", Y).Css("height", H).Css("width", W);
														
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
                            Rectange.SetBounds(out X1, out Y1, out W, out H, obj);

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

							obj.Css("left", X).Css("width", W).Css("height", H);

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
                            Rectange.SetBounds(out X1, out Y1, out W, out H, obj);

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

							obj.Css("width", W).Css("height", H);

							break;
						default:
							break;
					}					
				}else if(WindowHolderSelectionBox != null && WindowHolderSelectionBox.Style.Visibility == Visibility.Visible)
				{
					if(Mouse_Down)
					{
                        if (ActiveFileExplorer == null)
                        {                            
                            return;
                        }                            

						WindowHolderSelectionBox.Style.Cursor = Cursor.Default;
                        ActiveFileExplorer.Element.Style.Cursor = Cursor.Default;

						int left;
						int top;
						int width;
						int height;

                        int ClientX = mev.ClientX - WindowHolderSelectionBoxXOff + ActiveFileExplorer.Element.ScrollLeft;
                        int ClientY = mev.ClientY - WindowHolderSelectionBoxYOff + ActiveFileExplorer.Element.ScrollTop;
                        
                        if (WindowHolderSelectionBoxX > ClientX)
						{
							left = ClientX;
							width = WindowHolderSelectionBoxX - ClientX;
						}
						else
						{
							left = WindowHolderSelectionBoxX;
							width = ClientX - WindowHolderSelectionBoxX;
						}

						if(WindowHolderSelectionBoxY > ClientY)
						{
							top = ClientY;
							height = WindowHolderSelectionBoxY - ClientY;
						}
						else
						{
							top = WindowHolderSelectionBoxY;
							height = ClientY - WindowHolderSelectionBoxY;
						}

						WindowHolderSelectionBox.Style.Left = left + "px";
						WindowHolderSelectionBox.Style.Top = top + "px";

						WindowHolderSelectionBox.Style.Width = width + "px";
						WindowHolderSelectionBox.Style.Height = height + "px";

                        Rectange SelectionRec = new Rectange(left, top, width, height);

                        for (int i = 0; i < ActiveFileExplorer.LoadedNodes.Count; i++)
                        {
                            if(ActiveFileExplorer.LoadedNodes[i] != null)
                            {
                                var htmlNode = ActiveFileExplorer.LoadedNodes[i].NodeBase;
                                if(htmlNode != null)
                                {
                                    if(Rectange.rectOverlap(Rectange.CreateFromHTMLElement(htmlNode), SelectionRec))
                                    {
                                        ActiveFileExplorer.LoadedNodes[i].NodeExplorerState = FileExplorerNode.FileExplorerState.Selected;
                                    }
                                    else
                                    {
                                        ActiveFileExplorer.LoadedNodes[i].NodeExplorerState = FileExplorerNode.FileExplorerState.None;
                                    }
                                }
                            }
                        }

						mev.StopImmediatePropagation();
						mev.PreventDefault();
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

                HideFileSelection();
            });

			Window.AddEventListener(EventType.MouseMove, mouseMove);

			Parent.AppendChild(WindowHolder);
			Parent.AppendChild(TaskBar);
			
			TaskBar.AppendChild(ButtonStart);
			TaskBar.AppendChild(InputStartSearch);

            Window_Desktop = new FileExplorer(WindowHolder) { NodeViewType = NodeViewType.Medium_Icons, Path = FileExplorer.DesktopPath };            
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

				Width = StyleController.Calc(100, 5);
				Height = StyleController.Calc(100, 5);

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

					butt.Style.Left = StyleController.Calc(100, 45);
					butt.Id = "Close";
					butt.InnerHTML = "&#10006";
					
					butt.OnMouseDown = (ev) =>
					{
                        if (MovingForm != null || WindowHolderSelectionBox != null)
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
                        if (MovingForm != null || WindowHolderSelectionBox != null)
                            return;

						ev.StopPropagation();
						ev.PreventDefault();

						Close();
					};

					butt.OnMouseEnter = (ev) =>
					{                        
						if(MovingForm != null || WindowHolderSelectionBox != null)
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
                        if (MovingForm != null || WindowHolderSelectionBox != null)
                            return;

						butt.Style.BackgroundColor = "white";
						butt.Style.Color = "black";
					};					

					break;
				case FormButtonType.Maximize:
					butt.Style.BackgroundColor = "white";
					butt.Style.Left = StyleController.Calc(100, 91);
					butt.Style.Color = "black";
					butt.Id = "Maximize";
					butt.InnerHTML = "&#9633;";
					butt.Style.FontSize = "14pt";

					butt.OnMouseUp = (ev) =>
					{
                        if (MovingForm != null || WindowHolderSelectionBox != null)
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
					butt.Style.Left = StyleController.Calc(100, 137);
					butt.Style.Color = "black";
					butt.Id = "Minimize";
					butt.InnerHTML = "&#8213;";

					butt.OnMouseUp = (ev) =>
					{
                        if (MovingForm != null || WindowHolderSelectionBox != null)
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
                        if (MovingForm != null || WindowHolderSelectionBox != null)
                            return;

						ev.StopPropagation();
						ev.PreventDefault();

						Mouse_Down = false;
					};
					break;
			}

			butt.OnMouseMove = (ev) =>
			{
                if (MovingForm != null || WindowHolderSelectionBox != null)
                    return;

				ev.StopImmediatePropagation();
				ev.PreventDefault();				
			};

			if(Type != FormButtonType.Close)
			{
				butt.OnMouseDown = (ev) =>
				{
                    if (MovingForm != null || WindowHolderSelectionBox != null)
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
                    if (MovingForm != null || WindowHolderSelectionBox != null)
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
                    if (MovingForm != null || WindowHolderSelectionBox != null)
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
                if((item.TagName.ToLower() == "input" ||
					item.TagName.ToLower() == "span" ||
					item.TagName.ToLower() == "textarea") && item.GetAttribute("IL") != "1")
                {
                    ChangeStateTextSelection(item, !TurnOff);                    
                }
                if(item.ChildElementCount > 0)
                {
                    ChangeSelectionState(item.Children, TurnOff);
                }
            }
        }

        public static void SetInternalLabel(HTMLElement element)
        {
            element.SetAttribute("IL", "1"); // Internal Label
        }
		
		public Form()
		{			
			Base = new HTMLDivElement();
            Heading = new HTMLDivElement();
            HeadingTitle = new HTMLSpanElement();
            Body = new HTMLDivElement();
            BodyOverLay = new HTMLDivElement();

            Base.Id = "Base";
            Base.Style.BorderStyle = BorderStyle.Solid;
            Base.Style.BorderWidthString = "2px";            
            Base.Style.BackgroundColor = Window_HeadingBackgroundColor;                        

            Base.Style.BorderColor = Window_BorderColorFocused;
            jQuery.Select(Base).Css("box-shadow", "0px 0px 63px -17px rgba(0,0,0,0.75)");            

            BodyOverLay.Style.Visibility = Visibility.Collapse;

            Base.AddEventListener(EventType.MouseDown, (ev) => {
				var mev = ev.As<MouseEvent>();

				Mouse_Down = true;
                MovingForm = this;

                ActiveForm = this;
                                
                SetBodyOverLay();

                var obj = jQuery.Select(Base);

                prev_px = Global.ParseInt(obj.Css("left")) - mev.ClientX;
                prev_py = Global.ParseInt(obj.Css("top")) - mev.ClientY;

                if (windowState == WindowState.Maximized)
				{
					SetCursor(Cursor.Default);

					MoveAction = MouseMoveAction.Move;                
					return;
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
				}else if(WindowHolderSelectionBox != null)
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
			Heading.Style.BackgroundColor = Window_HeadingBackgroundColor;
			Heading.Style.MarginTop = "0";
			Heading.Style.MarginLeft = "0";
			Heading.Style.MarginRight = "0";
			Heading.Style.MarginBottom = "0";
			Heading.Style.PaddingBottom = "1px";
            Heading.Style.FontFamily = "Segoe UI";
            Heading.Style.TextAlign = TextAlign.Left | TextAlign.Center;

            Heading.AddEventListener(EventType.MouseDown, (ev) => {
                SetBodyOverLay();

                if (windowState == WindowState.Maximized)
                {
                    MovingForm = this;
                    SetCursor(Cursor.Default);

                    MoveAction = MouseMoveAction.Move;
                }
                else
                {
                    MovingForm = this;
                }

                ActiveForm = this;                
            });

            HeadingTitle.Style.TextIndent = "3px";
            SetInternalLabel(HeadingTitle); // Internal Label
            
			ButtonClose = CreateFormButton(FormButtonType.Close);
			ButtonExpand = CreateFormButton(FormButtonType.Maximize);
			ButtonMinimize = CreateFormButton(FormButtonType.Minimize);

            jQuery.Select(Heading)
				.Css("user-select", "none")
				.Css("user-drag:", "none");

			jQuery.Select(Base)
				.Css("user-select", "none")
				.Css("user-drag:", "none");

			jQuery.Select(HeadingTitle)
				.Css("user-select", "none")
				.Css("user-drag:", "none");
            
			Body.Id = "Body";
			Body.Style.Top = "30px";
			Body.Style.Height = StyleController.Calc(100, 30);
			Body.Style.Width = StyleController.Calc(100, 1);
            Body.Style.Position = Position.Absolute;
            Body.Style.BackgroundColor = Window_DefaultBackgroundColor;
            Body.Style.Overflow = Overflow.Hidden;

            Body.AddEventListener(EventType.MouseDown, (ev) => {
                ActiveForm = this;
                MovingForm = null;
                SetCursor(Cursor.Default);
                ev.StopPropagation();
            });

            Body.AddEventListener(EventType.MouseMove, (ev) => {
                if(MovingForm == null)
                {
                    SetCursor(Cursor.Default);
                    ev.StopPropagation();                    
                }                               
            });

            BodyOverLay.Style.Top = "31px";
			BodyOverLay.Style.Height = StyleController.Calc(100, 33);
			BodyOverLay.Style.Width = StyleController.Calc(100, 4);
            BodyOverLay.Style.Left = "2px";
            BodyOverLay.Style.Position = Position.Absolute;
            BodyOverLay.Style.ZIndex = int.MaxValue.ToString();
            BodyOverLay.Style.Opacity = ShowBodyOverLay ? "0.5" : "0";
            BodyOverLay.Style.BackgroundColor = "black";
          
            BodyOverLay.AddEventListener(EventType.MouseDown, (ev) =>
            {                
                BodyOverLay.Style.Visibility = Visibility.Collapse;
                ActiveForm = this;                                                
            });

            BodyOverLay.AddEventListener(EventType.MouseUp, (ev) =>
            {                
                if(MovingForm == null &&
                    ActiveFileExplorer != null)
                {                    
                    HideFileSelection();
                }                               
            });            

            Body.AddEventListener(EventType.MouseLeave, (ev) =>
            {
                if(MovingForm == null)
                {                    
                    SetBodyOverLay();
                }                
            });

            BodyOverLay.AddEventListener(EventType.MouseEnter, (ev) =>
            {
                if(WindowHolderSelectionBox == null && MovingForm == null)
                {
                    BodyOverLay.Style.Visibility = Visibility.Collapse;
                }else
                {
                    BodyOverLay.Style.Visibility = Visibility.Visible;
                }
            });

            jQuery.Select(Base)
                .Css("width", Window_DefaultWidth)
                .Css("height", Window_DefaultHeight);
            
            Base.AppendChild(Heading);
            Base.AppendChild(Body);
            Base.AppendChild(BodyOverLay);

            Heading.AppendChild(HeadingTitle);
			Heading.AppendChild(ButtonClose);
			Heading.AppendChild(ButtonExpand);
			Heading.AppendChild(ButtonMinimize);

            Initialise();
        }

        public int TitleBarHeight()
        {
            return Heading.ClientHeight;
        }

        public int TitleBarWidth()
        {
            return Heading.ClientWidth;
        }

        public int ClientX()
        {
            return Body.ClientLeft;
        }

        public int ClientY()
        {
            return Body.ClientTop;
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
				VisibleForm[VisibleForm.Count - 1] != this)
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
				jQuery.Select(Base).FadeOut(FadeLength, () => { jQuery.Select(Base).Empty(); Base.Remove(); Base = null; });				
			}

			CalculateZOrder();			
		}

        public void FillControlWithParent(HTMLElement element, int widthOffset = 8, int heightOffset = 9)
        {
            element.Style.Position = Position.Absolute;
			element.Style.Width = StyleController.Calc(100, widthOffset);
            element.Style.Height = StyleController.Calc(100, heightOffset);

            element.Style.Top = "1px";
            element.Style.Left = "1px";
        }

        public void FillHorizontalControlWithParent(HTMLElement element, int widthOffset = 8)
        {
            element.Style.Position = Position.Absolute;
            element.Style.Width = StyleController.Calc(100, widthOffset);

			element.Style.Left = "1px";
        }

        public void FillVerticalControlWithParent(HTMLElement element, int heightOffset = 9)
        {
            element.Style.Position = Position.Absolute;            
            element.Style.Height = StyleController.Calc(100, heightOffset);

			element.Style.Top = "1px";            
        }
    }
}
