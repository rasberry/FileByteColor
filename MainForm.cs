using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FileByteColor
{
	public class MainForm : Form
	{
		public MainForm ()
		{
			Init();
		}
		
		private void Init()
		{
			_mmanager = new MenuManager();
			_dmanager = new DisplayManager(_mmanager);
			this.MainMenuStrip = _mmanager.Menu;
			this.Controls.Add(_dmanager);
			this.Controls.Add(this.MainMenuStrip); //weird.. this doesn't happen automatically?
			this.Resize += HandleResize;
			_mmanager.MenuAction += OnMenuAction;
		}

		private void OnMenuAction(object sender, MenuManager.MenuActionEventArgs e)
		{
			if (e.Action == MenuManager.MenuActionType.FileClose) {
				FileHandler.Close();
			}
			else if (e.Action == MenuManager.MenuActionType.FileExit) {
				Application.Exit();
			}
			else if (e.Action == MenuManager.MenuActionType.FileOpen) {
				LoadFile();
			}
			else if (e.Action == MenuManager.MenuActionType.HelpAbout) {
				//TODO show about
			}
		}

		private void LoadFile()
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
			if (DialogResult.OK == d.ShowDialog(this))
			{
				ErrorCode err = FileHandler.Open(d.FileName);
				if (err != ErrorCode.None) {
					Error.Show(err, d.FileName);
				} else {
					this.Refresh();
				}
			}
		}

		private void HandleResize(object sender, EventArgs e)
		{
			//Debug.WriteLine("MainForm HandleResize");
			//_mmanager.SpinnerValue = _dmanager.Width;
		}

		private DisplayManager _dmanager = null;
		private MenuManager _mmanager = null;
	}
}

