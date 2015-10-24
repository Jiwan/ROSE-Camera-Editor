using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common
{
	public class FPS
	{
		private int frame;

		private int fps;

		private float elapsedTime;

		private SpriteFont font;

		private SpriteBatch batch;

		public FPS(GraphicsDevice gDevice, Microsoft.Xna.Framework.Content.ContentManager XNAContent)
		{
		}

		public void incrementeCounter()
		{
			this.frame++;
		}

		public void Update(float time)
		{
			if (time - this.elapsedTime > 1f)
			{
				this.fps = this.frame;
				this.frame = 0;
				this.elapsedTime = time;
			}
		}

		public string GetFPS()
		{
			return "FPS :" + this.fps;
		}

		public void Draw()
		{
		}
	}
}
