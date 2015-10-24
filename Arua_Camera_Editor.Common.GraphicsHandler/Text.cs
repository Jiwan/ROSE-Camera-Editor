using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common.GraphicsHandler
{
	public class Text
	{
		private string text;

		private Vector2 position;

		private Color color;

		private SpriteFont font;

		public GraphicsDevice graphicsDevice
		{
			get;
			set;
		}

		public Text(GraphicsDevice gDevice)
		{
			this.graphicsDevice = gDevice;
		}

		public void Set(string txt, Vector2 pos, SpriteFont pfont, Color col)
		{
			this.text = txt;
			this.position = pos;
			this.font = pfont;
			this.color = col;
		}

		public void Draw(SpriteBatch spritebatch)
		{
			spritebatch.DrawString(this.font, this.text, this.position, this.color);
		}
	}
}
