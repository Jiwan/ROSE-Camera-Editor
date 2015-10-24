using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Arua_Camera_Editor.Common.GraphicsHandler
{
	public abstract class GraphicsDeviceControl : Control
	{
		private GraphicsDeviceService graphicsDeviceService;

		private ServiceContainer services = new ServiceContainer();

		public GraphicsDevice GraphicsDevice
		{
			get
			{
				return this.graphicsDeviceService.GraphicsDevice;
			}
		}

		public ServiceContainer Services
		{
			get
			{
				return this.services;
			}
		}

		protected override void OnCreateControl()
		{
			if (!base.DesignMode)
			{
				this.graphicsDeviceService = GraphicsDeviceService.AddRef(base.Handle, base.ClientSize.Width, base.ClientSize.Height);
				this.services.AddService<IGraphicsDeviceService>(this.graphicsDeviceService);
				this.Initialize();
			}
			base.OnCreateControl();
		}

		protected override void Dispose(bool disposing)
		{
			if (this.graphicsDeviceService != null)
			{
				this.graphicsDeviceService.Release(disposing);
				this.graphicsDeviceService = null;
			}
			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			string text = this.BeginDraw();
			if (string.IsNullOrEmpty(text))
			{
				this.Draw();
				this.EndDraw();
			}
			else
			{
				this.PaintUsingSystemDrawing(e.Graphics, text);
			}
		}

		private string BeginDraw()
		{
			string result;
			if (this.graphicsDeviceService == null)
			{
				result = this.Text + "\n\n" + base.GetType();
			}
			else
			{
				string text = this.HandleDeviceReset();
				if (!string.IsNullOrEmpty(text))
				{
					result = text;
				}
				else
				{
					Viewport viewport = default(Viewport);
					viewport.X = 0;
					viewport.Y = 0;
					viewport.Width = base.ClientSize.Width;
					viewport.Height = base.ClientSize.Height;
					viewport.MinDepth = 0f;
					viewport.MaxDepth = 1f;
					this.GraphicsDevice.Viewport = viewport;
					result = null;
				}
			}
			return result;
		}

		private void EndDraw()
		{
			try
			{
				Microsoft.Xna.Framework.Rectangle value = new Microsoft.Xna.Framework.Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height);
				this.GraphicsDevice.Present(new Microsoft.Xna.Framework.Rectangle?(value), null, base.Handle);
			}
			catch
			{
			}
		}

		private string HandleDeviceReset()
		{
			string result;
			bool flag;
			switch (this.GraphicsDevice.GraphicsDeviceStatus)
			{
			case GraphicsDeviceStatus.Lost:
				result = "Graphics device lost";
				return result;
			case GraphicsDeviceStatus.NotReset:
				flag = true;
				break;
			default:
			{
				PresentationParameters presentationParameters = this.GraphicsDevice.PresentationParameters;
				flag = (base.ClientSize.Width > presentationParameters.BackBufferWidth || base.ClientSize.Height > presentationParameters.BackBufferHeight);
				break;
			}
			}
			if (flag)
			{
				try
				{
					this.graphicsDeviceService.ResetDevice(base.ClientSize.Width, base.ClientSize.Height);
				}
				catch (Exception arg)
				{
					result = "Graphics device reset failed\n\n" + arg;
					return result;
				}
			}
			result = null;
			return result;
		}

		protected virtual void PaintUsingSystemDrawing(Graphics graphics, string text)
		{
			graphics.Clear(System.Drawing.Color.CornflowerBlue);
			using (Brush brush = new SolidBrush(System.Drawing.Color.Black))
			{
				using (StringFormat stringFormat = new StringFormat())
				{
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Center;
					graphics.DrawString(text, this.Font, brush, base.ClientRectangle, stringFormat);
				}
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		protected abstract void Initialize();

		protected abstract void Draw();
	}
}
