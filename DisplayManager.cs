using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FileByteColor
{
	public class DisplayManager : UserControl
	{
		public DisplayManager(MenuManager mmanager)
		{
			_mmanager = mmanager;
			Init();
		}

		private MenuManager _mmanager;
		
		private void Init()
		{
			this.DoubleBuffered = true; //this helps reduce flickring
			this.Resize += HandleResize;
			this.Paint += HandlePaint;
			this.Scroll += HandleScroll;
			_mmanager.MultiplierChanged += HandleMultiplierChanged;
			_mmanager.SpinnerChanged += UpdateWidth;
		}

		private void UpdateWidth(object sender, MenuManager.SpinnerChangedEventArgs e)
		{
			CanvasWidth = e.SpinnerValue;
			this.Refresh();
		}

		protected override void InitLayout()
		{
			this.Parent.Resize += HandleResize;
			base.InitLayout();
			HandleResize(null, null);
			_mmanager.SpinnerValue = CanvasWidth;
		}

		private void HandleMultiplierChanged(object sender, EventArgs e)
		{
			this.Refresh();
		}

		private void HandleScroll (object sender, ScrollEventArgs e)
		{
			//this.Refresh();
		}

		private void HandleResize(object sender, EventArgs e)
		{
			int mheight = _mmanager.Menu.Height;
			Size psize = this.Parent.ClientSize;
			this.Height = psize.Height - mheight;
			this.Left = (psize.Width - CanvasWidth)/2;
			this.Top = mheight;
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

		private int GetScrollWidth() {
			return this.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0;
		}
		private int CanvasWidth {
			get {
				int scrollw = GetScrollWidth();
				return Math.Max(1,this.Width - scrollw);
			}
			set {
				int scrollw = GetScrollWidth();
				this.Width = Math.Max(1,value + scrollw);
			}
		}

		private ColorPalette _pal = null;
		private void HandlePaint (object sender, PaintEventArgs e)
		{
			int mult = _mmanager.Multiplier;
			Graphics g = e.Graphics;
			int w = CanvasWidth / mult; //scale down the canvas so we can draw it back at the multiple size
			int h = this.Height / mult;
			long lines = FileHandler.Length / w + 1; //+1 to make it a ceil instead of a floor
			this.AutoScrollMinSize = new Size(0,(int)lines * mult);
			int offset = Math.Abs(this.VerticalScroll.Value / mult) * w;

			Debug.WriteLine("w="+w+" h="+h+" lines="+lines+" offset="+offset);

			byte[] data = FileHandler.GetChunk(offset,w*h);
			if (data == null) { return; }

			using (Bitmap b = new Bitmap(w, h, PixelFormat.Format8bppIndexed))
			{
				if (_pal == null) {
					FillPalette(b.Palette); _pal = b.Palette;
				} else {
					b.Palette = _pal;
				}
				BitmapData bits = b.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, b.PixelFormat);
				WriteBmpData(bits, data, w, h);
				b.UnlockBits(bits);

				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.DrawImage(b, 0, 0, w * mult, h * mult); //redraw the image scaled up
			}
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
