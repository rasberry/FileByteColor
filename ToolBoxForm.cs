using System;
using System.Windows.Forms;

namespace FileByteColor
{
	public class ToolBoxForm : Form
	{
		public ToolBoxForm()
		{
			Init();
		}
		
		private void Init()
		{
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Width = 200;
			this.Height = 200;
			int halfheight = this.Height / 2 - 30;
			this.FormClosing += HandleClosing;

			GroupBox gbzoom = new GroupBox();
			gbzoom.Text = "Zoom";
			gbzoom.Dock = DockStyle.Top;
			gbzoom.Height = halfheight;

			FlowLayoutPanel pzoom = new FlowLayoutPanel();
			pzoom.Dock = DockStyle.Fill;
			gbzoom.Controls.Add(pzoom);
			
			_x1 = new RadioButton();
			_x1.Text = "1x"; _x1.Width = 40; _x1.CheckedChanged += HandleCheckedChanged;
			_x2 = new RadioButton();
			_x2.Text = "2x"; _x2.Width = 40; _x2.CheckedChanged += HandleCheckedChanged;
			_x4 = new RadioButton();
			_x4.Text = "4x"; _x4.Width = 40; _x4.CheckedChanged += HandleCheckedChanged;
			_x8 = new RadioButton();
			_x8.Text = "8x"; _x8.Width = 40; _x8.CheckedChanged += HandleCheckedChanged;

			pzoom.Controls.Add(_x1);
			pzoom.Controls.Add(_x2);
			pzoom.Controls.Add(_x4);
			pzoom.Controls.Add(_x8);
			
			GroupBox gbwidth = new GroupBox();
			gbwidth.Text = "Width";
			gbwidth.Dock = DockStyle.Bottom;
			gbzoom.Height = halfheight;

			Spinner = new NumericUpDown();
			Spinner.Dock = DockStyle.Fill;
			Spinner.Minimum = 0;
			Spinner.Maximum = decimal.MaxValue;

			gbwidth.Controls.Add(Spinner);
		
			this.Controls.Add(gbzoom);
			this.Controls.Add(gbwidth);
		}

		private void HandleCheckedChanged(object sender, EventArgs e)
		{
			if (_x2.Checked) { _multiplier = 2; }
			else if (_x4.Checked) { _multiplier = 4; }
			else if (_x8.Checked) { _multiplier = 8; }
			else { _multiplier = 1; }
			if (MultiplierChanged != null) {
				MultiplierChanged(this,EventArgs.Empty);
			}
		}

		private void HandleClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}

		public NumericUpDown Spinner;
		private int _multiplier = 1;
		public int Multiplier { get { return _multiplier; }}
		public event EventHandler MultiplierChanged;
		private RadioButton _x1;
		private RadioButton _x2;
		private RadioButton _x4;
		private RadioButton _x8;


	}
}
