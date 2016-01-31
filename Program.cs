using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FileByteColor
{
	public class Program
	{
		[STAThread]
		public static void Main(string[] argv)
		{
			#if DEBUG
			Debug.Listeners.Add(new ConsoleTraceListener());
			#endif

			Application.Run(new MainForm());
		}
	}
}

