using Arua_Camera_Editor.Common.FileHandler;
using Arua_Camera_Editor.Common.GraphicsHandler.TerrainVertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common
{
	public class MapBlock
	{
		private HIM him;

		private TIL til;

		private ZON zon;

		private Vector2 mapPosition;

		private VertexBuffer vertBuffer;

		private GraphicsDevice graphics;

		private TerrainVertex[] vertices;

		private BoundingBox boundingBox;

		public MapBlock(GraphicsDevice gDevice)
		{
			this.graphics = gDevice;
		}

		public void Load(ZON zon, string mapFolder, string himName, string tilName, Vector2 mapPosition)
		{
			this.zon = zon;
			this.mapPosition = mapPosition;
			this.him = ContentManager.Instance().GetHIM(himName, mapFolder);
			this.til = ContentManager.Instance().GetTIL(tilName, mapFolder);
			this.GenerateVertices();
		}

		public void GenerateVertices()
		{
			Vector3[] array = new Vector3[6400];
			this.vertices = new TerrainVertex[6400];
			int num = 0;
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					for (int k = 0; k < 5; k++)
					{
						for (int l = 0; l < 5; l++)
						{
							array[num] = new Vector3((float)(j * 4 + k) * 2.5f + 160f * this.mapPosition.X, 10400f - ((float)(i * 4 + l) * 2.5f + 160f * this.mapPosition.Y), this.him.GetAltitude(i * 4 + l, j * 4 + k));
							this.vertices[num].Position = new Vector3((float)(j * 4 + k) * 2.5f + 160f * this.mapPosition.X, 10400f - ((float)(i * 4 + l) * 2.5f + 160f * this.mapPosition.Y), this.him.GetAltitude(i * 4 + l, j * 4 + k));
							this.vertices[num].Texture1 = new Vector2((float)l / 4f, (float)k / 4f);
							switch (this.zon.GetOrientation(this.til.GetTile(i, j).tileID))
							{
							case 2:
								this.vertices[num].Texture2 = new Vector2(1f - (float)k / 4f, (float)l / 4f);
								break;
							case 3:
								this.vertices[num].Texture2 = new Vector2((float)k / 4f, 1f - (float)l / 4f);
								break;
							case 4:
								this.vertices[num].Texture2 = new Vector2(1f - (float)k / 4f, 1f - (float)l / 4f);
								break;
							case 5:
								this.vertices[num].Texture2 = new Vector2((float)l / 4f, 1f - (float)k / 4f);
								break;
							case 6:
								this.vertices[num].Texture2 = new Vector2((float)l / 4f, (float)k / 4f);
								break;
							default:
								this.vertices[num].Texture2 = new Vector2((float)k / 4f, (float)l / 4f);
								break;
							}
							num++;
						}
					}
				}
				this.GenerateBoundingBox(array);
			}
			this.vertBuffer = new VertexBuffer(this.graphics, 179200, BufferUsage.WriteOnly);
			this.vertBuffer.SetData<TerrainVertex>(this.vertices);
		}

		public void GenerateBoundingBox(Vector3[] vertPosition)
		{
			this.boundingBox = BoundingBox.CreateFromPoints(vertPosition);
		}

		public bool IsView(BoundingFrustum frustrum)
		{
			return frustrum.Contains(this.boundingBox) == ContainmentType.Contains || frustrum.Contains(this.boundingBox) == ContainmentType.Intersects;
		}

		public void Draw(Effect effect)
		{
			this.graphics.Vertices[0].SetSource(this.vertBuffer, 0, 28);
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					effect.Parameters["Texture1"].SetValue(this.zon.GetBottomTexture(this.til.GetTile(i, j).tileID));
					effect.Parameters["Texture2"].SetValue(this.zon.GetTopTexture(this.til.GetTile(i, j).tileID));
					effect.CommitChanges();
					for (int k = 0; k < effect.CurrentTechnique.Passes.Count; k++)
					{
						effect.CurrentTechnique.Passes[k].Begin();
						this.graphics.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, i * 400 + j * 25, 0, 25, 0, 44);
						effect.CurrentTechnique.Passes[k].End();
					}
				}
			}
		}
	}
}
