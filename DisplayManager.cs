using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FileByteColor
{
	public class DisplayManager : UserControl
	{
		public DisplayManager (ToolBoxForm toolbox)
		{
			_toolbox = toolbox;
			Init();
		}

		private ToolBoxForm _toolbox;
		private int _multiplier = 1;
		
		private void Init()
		{
			this.Dock = DockStyle.Fill;
			this.Resize += HandleResize;
			this.Paint += HandlePaint;
			this.Scroll += HandleScroll;
			_toolbox.MultiplierChanged += HandleMultiplierChanged;
			this.Show();
		}

		private void HandleMultiplierChanged(object sender, EventArgs e)
		{
			_multiplier = _toolbox.Multiplier;
			this.Refresh();
		}

		private void HandleScroll (object sender, ScrollEventArgs e)
		{
			this.Refresh();
		}

		private void HandleResize (object sender, EventArgs e)
		{
			this.Refresh();
		}
		
		private void FillPalette(ColorPalette pal)
		{
			int len = pal.Entries.Length;
			for(int i=0; i<len; i++)
			{
				int c = i % 256;
				pal.Entries[i] = Color.FromArgb(c,c,c);
			}
		}

		private ColorPalette _pal = null;
		private void HandlePaint (object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			int w = this.Width - (this.VerticalScroll.Visible
				? SystemInformation.VerticalScrollBarWidth
				: 0 ) / _multiplier;
			int h = this.Height / _multiplier;
			long lines = FileHandler.Length / w + 1; //+1 to make it a ciel instead of a floor
			this.AutoScrollMinSize = new System.Drawing.Size(0,(int)lines*_multiplier);
			int top = Math.Abs(this.VerticalScroll.Value)*w;

			Console.WriteLine("w="+w+" h="+h+" lines="+lines+" top="+top);

			byte[] data = FileHandler.GetChunk(top,w*h);
			if (data == null) { return; }
			Bitmap b = new Bitmap(w,h,PixelFormat.Format8bppIndexed);
			if (_pal == null) {
				FillPalette(b.Palette); _pal = b.Palette;
			} else {
				b.Palette = _pal;
			}
			BitmapData bits = b.LockBits(new Rectangle(0,0,w,h),ImageLockMode.ReadWrite,b.PixelFormat);
			WriteBmpData(bits,data,w,h);
			b.UnlockBits(bits);

			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			g.DrawImage(b,0,0,w*_multiplier,h*_multiplier);
		}
		
		private static void WriteBmpData(BitmapData bmpDataDest,byte[] destBuffer,int imageWidth,int imageHeight)
		{
			// Get unmanaged data start address
			int addrStart = bmpDataDest.Scan0.ToInt32();
			
			for (int i = 0; i < imageHeight; i++)
			{
				// Get address of next row
				IntPtr realByteAddr = new IntPtr(addrStart +
					System.Convert.ToInt32(i * bmpDataDest.Stride)
				);

				// Perform copy from managed buffer
				// to unmanaged memory
				Marshal.Copy(destBuffer,i*imageWidth,realByteAddr,imageWidth);
			}
		}
	}
}
