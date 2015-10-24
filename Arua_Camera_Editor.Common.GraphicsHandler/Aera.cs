using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common.GraphicsHandler
{
	public class Aera
	{
		private Texture2D texture;

		public Rectangle rectangle
		{
			get;
			set;
		}

		public Color[] color
		{
			get;
			set;
		}

		public GraphicsDevice graphicsDevice
		{
			get;
			set;
		}

		public Aera(GraphicsDevice gDevice, Rectangle rect, Color col)
		{
			this.graphicsDevice = gDevice;
			this.rectangle = rect;
			this.texture = new Texture2D(gDevice, this.rectangle.Width, this.rectangle.Height, 1, TextureUsage.None, SurfaceFormat.Color);
			Color[] array = new Color[this.rectangle.Width * this.rectangle.Height];
			col.A = 50;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = col;
			}
			col.A = 255;
			for (int i = 0; i < this.rectangle.Width; i++)
			{
				array[i] = col;
			}
			for (int i = 0; i < this.rectangle.Width; i++)
			{
				array[array.Length - 1 - i] = col;
			}
			for (int i = 0; i < this.rectangle.Height; i++)
			{
				array[this.rectangle.Width * i] = col;
			}
			for (int i = 0; i < this.rectangle.Height; i++)
			{
				array[this.rectangle.Width * i + this.rectangle.Width - 1] = col;
			}
			this.texture.SetData<Color>(array);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.texture, new Vector2((float)this.rectangle.X, (float)this.rectangle.Y), Color.White);
		}
	}
}
