using System;
using System.Windows.Forms;

namespace FileByteColor
{
	public enum ErrorCode
	{
		None = 0
		,FileDoesNotExist = 1
		,CantReadFile = 2
	}
	
	public static class Error
	{
		public static void Show(ErrorCode code)
		{
			Show(code,null);
		}
		public static void Show(ErrorCode code, string extra)
		{
			switch(code)
			{
			case ErrorCode.CantReadFile:
				MessageBox.Show("Can't read file: "+(extra??""),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				break;
			case ErrorCode.FileDoesNotExist:
				MessageBox.Show("File does not exist: "+(extra??""),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				break;
			}
		}
	}
}

