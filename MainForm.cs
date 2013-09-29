using System;
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
			_toolbox = new ToolBoxForm();
			this.Menu = new MenuManager(this,_toolbox).Menu;
			this.Controls.Add(new DisplayManager(_toolbox));
			this.Resize += HandleResize;
			_toolbox.Spinner.ValueChanged += ForceResize;
			this.Show();
		}

		private void HandleResize(object sender, EventArgs e)
		{
			_toolbox.Spinner.Value = (decimal)this.Width;
		}

		private void ForceResize(object sender, EventArgs e)
		{
			int newwidth = (int)_toolbox.Spinner.Value;
			if (newwidth != this.Width) { this.Width = newwidth; }
			//this.Refresh();
		}

		private ToolBoxForm _toolbox = null;
	}
}

