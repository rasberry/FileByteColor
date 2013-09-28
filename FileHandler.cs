using System;
using System.IO;

namespace FileByteColor
{
	public class FileHandler
	{
		private static FileHandler _inst = null;
		private static FileHandler Self { get {
			if (_inst == null) {
				_inst = new FileHandler();
			}
			return _inst;
		}}
		
		private string _fileName = null;
		private FileStream _stream = null;

		public static ErrorCode Open(string filename)
		{
			if (!File.Exists(filename)) {
				return ErrorCode.FileDoesNotExist;
			}
			Self._fileName = filename;
			Self._stream = File.Open(filename,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
			
			if (!Self._stream.CanSeek || !Self._stream.CanRead) {
				return ErrorCode.CantReadFile;
			}
			
			return ErrorCode.None;
		}
		
		public static ErrorCode Close()
		{
			if (Self._stream != null)
			{
				Self._stream.Close();
			}
			return ErrorCode.None;
		}
		
		public static string Name { get {
			return Self._fileName;
		}}
		public static long Length { get {
			if (Self._stream != null) {
				return Self._stream.Length;
			}
			return 0;
		}}
		
		public static byte[] GetChunk(long start,long len)
		{
			if (Self._stream == null) { return null; }
			
			byte[] buff = new byte[len];
			if (start >= Self._stream.Length) { return buff; }
			if (start + len >Self. _stream.Length) {
				len = Self._stream.Length - start;
			}
			Self._stream.Seek(start,SeekOrigin.Begin);
			Self._stream.Read(buff,0,(int)len);
			return buff;
		}
	}
}

