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
			this.Menu = new MenuManager(this).Menu;
			this.Controls.Add(new DisplayManager());
			this.Show();
		}
	}
}

