using System;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace FileByteColor
{
	public class MenuManager
	{
		public MenuManager(MainForm parent, ToolBoxForm toolbox)
		{
			_p = parent;
			_tx = toolbox;
			Init();
		}
		private MainForm _p;
		private ToolBoxForm _tx;
		
		private void Init()
		{
			//File menu
			MenuItem FileOpen = new MenuItem("&Open");
			FileOpen.Click += FileOpenAction;
			MenuItem FileClose = new MenuItem("&Close");
			FileClose.Click += FileCloseAction;
			MenuItem FileExit = new MenuItem("E&xit");
			FileExit.Click += FileExitAction;
			MenuItem file = new MenuItem("&File",new MenuItem[] {
				FileOpen,FileClose,FileExit
			});
			
			//Tools Menu
			MenuItem ToolsToggleToolBox = new MenuItem("&Toggle ToolBox");
			ToolsToggleToolBox.Click += ToolsToggleToolBoxAction;
			MenuItem tools = new MenuItem("&Tools",new MenuItem[] {
				ToolsToggleToolBox
			});
			
			//Help Menu
			MenuItem HelpAbout = new MenuItem("&About");
			HelpAbout.Click += HelpAboutAction;
			MenuItem help = new MenuItem("&Help",new MenuItem[] {
				HelpAbout
			});
			
			Menu = new MainMenu(new MenuItem[] {
				file,tools,help
			});
		}
		
		public MainMenu Menu { get; private set; }

		private void FileOpenAction (object sender, EventArgs e)
		{
			OpenFileDialog d = new OpenFileDialog();
			d.CheckFileExists = true;
			d.Multiselect = false;
			d.Title = "Open File";
			d.Filter = "All files (*.*)|*.*";
			d.FilterIndex = 0;
			if (!String.IsNullOrEmpty(FileHandler.Name)) {
				d.InitialDirectory = Path.GetDirectoryName(FileHandler.Name);
			}
			if (DialogResult.OK == d.ShowDialog(_p))
			{
				ErrorCode err = FileHandler.Open(d.FileName);
				if (err != ErrorCode.None) {
					Error.Show(err,d.FileName);
				} else {
					_p.Refresh();
				}
			}
		}

		private void FileCloseAction (object sender, EventArgs e)
		{
			FileHandler.Close();
			_p.Refresh();
		}
		private void FileExitAction (object sender, EventArgs e)
		{
			Application.Exit();
		}
		private void ToolsToggleToolBoxAction (object sender, EventArgs e)
		{
			_tx.Show();
		}
		private void HelpAboutAction (object sender, EventArgs e)
		{
			
		}
	}
}

