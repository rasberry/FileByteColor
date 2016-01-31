using System;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace FileByteColor
{
	public class MenuManager
	{
		public MenuManager()
		{
			Init();
		}
		private NumericUpDown SpinnerUi { get; set; }
		private ToolStripDropDownButton MultiplierUi { get; set; }

		public MenuStrip Menu { get; private set; }

		//Events
		public class MultiplierChangedEventArgs : EventArgs {
			public int Multiplier { get; set; }
		}
		public delegate void MultiplierChangedEventHandler(object sender, MultiplierChangedEventArgs e);
		public event MultiplierChangedEventHandler MultiplierChanged;
		public int Multiplier { get; private set; }

		public class SpinnerChangedEventArgs : EventArgs {
			public int SpinnerValue { get; set; }
		}
		public delegate void SpinnerChangedEventHandler(object sender, SpinnerChangedEventArgs e);
		public event SpinnerChangedEventHandler SpinnerChanged;
		public int SpinnerValue { get { return (int)SpinnerUi.Value; } set { SpinnerUi.Value = value; } }

		public object Deubg { get; private set; }

		public enum MenuActionType { FileOpen, FileClose, FileExit, HelpAbout }
		public class MenuActionEventArgs : EventArgs {
			public MenuActionType Action { get; set; }
		}
		public delegate void MenuActionEventHandler(object sender, MenuActionEventArgs e);
		public event MenuActionEventHandler MenuAction;

		private void Init()
		{
			//File menu
			var FileOpen = new ToolStripMenuItem("&Open");
			FileOpen.Click += FileOpenAction;
			var FileClose = new ToolStripMenuItem("&Close");
			FileClose.Click += FileCloseAction;
			var FileExit = new ToolStripMenuItem("E&xit");
			FileExit.Click += FileExitAction;
			var file = new ToolStripMenuItem("&File");
			file.DropDownItems.AddRange(new ToolStripMenuItem[] {FileOpen,FileClose,FileExit });
			
			//Help Menu
			var HelpAbout = new ToolStripMenuItem("&About");
			HelpAbout.Click += HelpAboutAction;
			var help = new ToolStripMenuItem("&Help");
			help.DropDownItems.Add(HelpAbout);

			//Seperator
			var seperator = new ToolStripSeparator();

			//Spinner
			SpinnerUi = new NumericUpDown();
			SpinnerUi.Minimum = 1;
			SpinnerUi.Maximum = decimal.MaxValue;
			SpinnerUi.MaximumSize = new System.Drawing.Size(60,int.MaxValue);
			SpinnerUi.InterceptArrowKeys = false; //we're doing our own key capture
			SpinnerUi.KeyDown += OnSpinnerKeyDown; //keydown repeats if key is being held
			SpinnerUi.ValueChanged += OnSpinnerChange;

			var spinnerHost = new ToolStripControlHost(SpinnerUi,"Width");

			//Multiplier
			MultiplierUi = new ToolStripDropDownButton();
			MultiplierUi.DropDownItemClicked += OnMultiplierChanged;
			ToolStripMenuItem first;
			MultiplierUi.DropDownItems.AddRange( new ToolStripItem[] {
				first = new ToolStripMenuItem("&1x")
				,new ToolStripMenuItem("&2x")
				,new ToolStripMenuItem("&4x")
				,new ToolStripMenuItem("&8x")
			});
			first.Select();
			OnMultiplierChanged(null,new ToolStripItemClickedEventArgs(first));

			//Add everything to the menu
			Menu = new MenuStrip();
			Menu.Items.AddRange(new ToolStripItem[] { file,help,seperator,MultiplierUi,spinnerHost });
			Menu.Dock = DockStyle.Top;
		}

		private void OnSpinnerKeyDown(object sender, KeyEventArgs e)
		{
			int change = 0;
			if (e.KeyCode == Keys.Up) {
				change = + 01 + (e.Shift ? 04 : 0);
			} else if (e.KeyCode == Keys.Down) {
				change = - 01 - (e.Shift ? 04 : 0);
			} else if (e.KeyCode == Keys.PageUp) {
				change = + 10 + (e.Shift ? 10 : 0);
			} else if (e.KeyCode == Keys.PageDown) {
				change = - 10 - (e.Shift ? 10 : 0);
			}
			if (change != 0) {
				SpinnerUi.Value += change;
			}
		}

		private void OnSpinnerChange(object sender, EventArgs e)
		{
			if (SpinnerChanged != null) {
				SpinnerChanged.Invoke(this,new SpinnerChangedEventArgs { SpinnerValue = SpinnerValue });
			}
		}

		private void OnMultiplierChanged(object sender, ToolStripItemClickedEventArgs e)
		{
			string text = e.ClickedItem.Text;
			if (text == "&1x") { Multiplier = 1; }
			if (text == "&2x") { Multiplier = 2; }
			if (text == "&4x") { Multiplier = 4; }
			if (text == "&8x") { Multiplier = 8; }
			
			MultiplierUi.Text = text;
			if (MultiplierChanged != null) {
				MultiplierChanged.Invoke(this
					,new MultiplierChangedEventArgs { Multiplier = Multiplier }
				);
			}
		}

		private void FileOpenAction (object sender, EventArgs e)
		{
			FireMenuEvent(MenuActionType.FileOpen);
		}
		private void FileCloseAction (object sender, EventArgs e)
		{
			FireMenuEvent(MenuActionType.FileClose);
		}
		private void FileExitAction (object sender, EventArgs e)
		{
			FireMenuEvent(MenuActionType.FileExit);
		}
		private void HelpAboutAction (object sender, EventArgs e)
		{
			FireMenuEvent(MenuActionType.HelpAbout);
		}

		private void FireMenuEvent(MenuActionType type)
		{
			if (MenuAction != null) {
				MenuAction.Invoke(this,new MenuActionEventArgs {
					Action = type
				});
			}
		}
	}
}

