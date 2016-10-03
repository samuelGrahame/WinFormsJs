using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using Bridge.jQuery2;

namespace WinFormjs
{
	public class ServerApplication
	{
		public string URI { get; set; }

		public static void StartFromURI(string source)
		{
			// Get Application from Source :D
			Network.Get(source, (data, str, obj) => {
				StartFromJson(data);
			});
		}

		public static void StartFromJson(object jsonData)
		{
			var saf = jsonData as ServerApplicationFile;
			if(saf == null)			
				return;			



		}
	}

	public class ServerApplicationFile
	{
		public string Title { get; set; }
			

	}

	public class ServerApplicationClassField
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public object Value { get; set; }
		public ServerApplicationClassControl SACC { get; set; } // Linked Control - Requires to be a EditType

		public ServerApplicationClassField()
		{
			SACC = null;
		}
	}

	public class ServerApplicationClassControl
	{
		public ServerControlTypes ServerControlTypeCode { get; set; }
		public string Name { get; set; }
		public object Tag { get; set; }
		public Size Size { get; set; }
		public Point Location { get; set; }
		
		public Rectange Bounds
		{
			get { return new Rectange(Location, Size); }
			set {
				Size = new Size(value.Width, value.Height);
				Location = new Point(value.X, value.Y);
			}
		}

		public string Text { get; set; }
		public Drawing.Color BackColor { get; set; }
		public Drawing.Color ForeColor { get; set; }


	}

	public enum ServerControlTypes
	{
		// Common Controls
		Button,
		CheckBox,
		CheckListBox,
		ComboBox,
		DateTimePicker,
		Label,
		LinkLabel,
		ListBox,
		ListView,
		MaskedTextBox,
		MonthCalendar,
		NotifyIcon,
		NumericUpDown,
		PictureBox,
		ProgressBox,
		RadioButton,
		RichTextBox,
		TextBox,
		ToolTip,
		TreeView,
		WebBrowser,
		// Containers
		FlowLayoutPanel,
		GroupBox,
		Panel,
		SplitContainer,
		TabControl,
		TableLayoutPanel,
		// Menus & Toolbars,
		ContextMenuStrip,
		MenuTrip,
		StatusStrip,
		ToolStrip,
		ToolStripContainer,
		// Data
		Chart,
		// Components
		BackgroundWoker,
		DirectoryEntry,
		DirectorySearcher,
		ErrorProvider,
		EventLog,
		FileSystemWatcher,
		HelpProvider,
		ImageList,
		MessageQueue,
		PerformanceCounter,
		Process,
		SerialPort,
		ServiceController,
		Timer,
		// Printing
		PageSetupDialog,
		PrintDialog,
		PrintDocument,
		// Dialogs
		ColorDialog,
		FolderBrowserDialog,
		FontDialog,
		OpenFileDialog,
		SafeFileDialog,
		Custom
	}
}
