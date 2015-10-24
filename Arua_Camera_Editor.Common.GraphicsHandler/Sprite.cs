using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common.GraphicsHandler
{
	public class Sprite
	{
		private bool using_sourceRectangle;

		public Texture2D texture
		{
			get;
			set;
		}

		public Vector2 position
		{
			get;
			set;
		}

		public GraphicsDevice graphicsDevice
		{
			get;
			set;
		}

		public Rectangle sourceRectangle
		{
			get;
			set;
		}

		public Sprite(GraphicsDevice gDevice)
		{
			this.graphicsDevice = gDevice;
			this.using_sourceRectangle = false;
		}

		public void LoadTextureFromFile(string Path, Vector2 pos)
		{
			this.texture = Texture2D.FromFile(this.graphicsDevice, Path);
			this.position = pos;
		}

		public void LoadTextureFromFile(string Path, Vector2 pos, Rectangle sRectangle)
		{
			this.texture = Texture2D.FromFile(this.graphicsDevice, Path);
			this.position = pos;
			this.sourceRectangle = sRectangle;
			this.using_sourceRectangle = true;
		}

		public void Move(Vector2 newpostion)
		{
			this.position = newpostion;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!this.using_sourceRectangle)
			{
				spriteBatch.Draw(this.texture, this.position, Color.White);
			}
			else
			{
				spriteBatch.Draw(this.texture, this.position, new Rectangle?(this.sourceRectangle), Color.White);
			}
		}
	}
}
