using Arua_Camera_Editor.Common.FileHandler;
using Arua_Camera_Editor.Common.GraphicsHandler.ObjectVertex;
using Arua_Camera_Editor.Common.GraphicsHandler.TerrainVertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common
{
	internal class Terrain
	{
		private short[] predefIndice = new short[]
		{
			0,
			5,
			1,
			6,
			2,
			7,
			3,
			8,
			4,
			9,
			9,
			5,
			5,
			10,
			6,
			11,
			7,
			12,
			8,
			13,
			9,
			14,
			14,
			10,
			10,
			15,
			11,
			16,
			12,
			17,
			13,
			18,
			14,
			19,
			19,
			15,
			15,
			20,
			16,
			21,
			17,
			22,
			18,
			23,
			19,
			24
		};

		private ZON zon;

		private ZSC buildingZSC;

		private ZSC decorationZSC;

		private DecorationBlock[,] decorationBlocks;

		private MapBlock[,] mapBlocks;

		private GraphicsDevice graphics;

		private IndexBuffer indices;

		private VertexDeclaration decorationVertexDeclaration;

		private VertexDeclaration terrainVertexDeclaration;

		private BoundingFrustum frustrum;

		private bool[,] isView;

		public Terrain(GraphicsDevice gDevice)
		{
			this.graphics = gDevice;
			this.decorationVertexDeclaration = new VertexDeclaration(this.graphics, ObjectVertex.vertex);
			this.terrainVertexDeclaration = new VertexDeclaration(this.graphics, TerrainVertex.vertex);
		}

		public void Load(string zonPath, string zscBuildingPath, string zscDecorationPath, string mapFolder, int minSizeX, int minSizeY, int maxSizeX, int maxSizeY)
		{
			ZON zON = ContentManager.Instance().GetZON(zonPath);
			ZSC zSC = ContentManager.Instance().GetZSC(zscBuildingPath);
			ZSC zSC2 = ContentManager.Instance().GetZSC(zscDecorationPath);
			this.decorationBlocks = new DecorationBlock[maxSizeX - minSizeX + 1, maxSizeY - minSizeY + 1];
			this.mapBlocks = new MapBlock[maxSizeX - minSizeX + 1, maxSizeY - minSizeY + 1];
			this.isView = new bool[maxSizeX - minSizeX + 1, maxSizeY - minSizeY + 1];
			for (int i = minSizeX; i <= maxSizeX; i++)
			{
				for (int j = minSizeY; j <= maxSizeY; j++)
				{
					string himName = string.Concat(new object[]
					{
						i,
						"_",
						j,
						".HIM"
					});
					string tilName = string.Concat(new object[]
					{
						i,
						"_",
						j,
						".TIL"
					});
					MapBlock mapBlock = new MapBlock(this.graphics);
					mapBlock.Load(zON, mapFolder, himName, tilName, new Vector2((float)i, (float)j));
					this.mapBlocks[i - minSizeX, j - minSizeY] = mapBlock;
					string ifoName = string.Concat(new object[]
					{
						i,
						"_",
						j,
						".IFO"
					});
					DecorationBlock decorationBlock = new DecorationBlock(this.graphics);
					decorationBlock.Load(ifoName, mapFolder, zSC2, zSC, new Vector2((float)i, (float)j));
					this.decorationBlocks[i - minSizeX, j - minSizeY] = decorationBlock;
				}
			}
			this.GenerateIndice();
		}

		public void GenerateIndice()
		{
			this.indices = new IndexBuffer(this.graphics, typeof(short), this.predefIndice.Length, BufferUsage.WriteOnly);
			this.indices.SetData<short>(this.predefIndice);
		}

		public void Draw(GraphicsDevice graphics, Effect effect, Effect terrainEffect)
		{
			this.frustrum = new BoundingFrustum(terrainEffect.Parameters["WorldViewProjection"].GetValueMatrix());
			graphics.VertexDeclaration = this.terrainVertexDeclaration;
			graphics.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
			graphics.RenderState.AlphaBlendEnable = true;
			graphics.Indices = this.indices;
			terrainEffect.CurrentTechnique = effect.Techniques["Default"];
			terrainEffect.Begin();
			for (int i = 0; i < this.mapBlocks.GetLength(0); i++)
			{
				for (int j = 0; j < this.mapBlocks.GetLength(1); j++)
				{
					if (this.mapBlocks[i, j].IsView(this.frustrum))
					{
						this.mapBlocks[i, j].Draw(terrainEffect);
						this.isView[i, j] = true;
					}
					else
					{
						this.isView[i, j] = false;
					}
				}
			}
			terrainEffect.End();
			graphics.RenderState.AlphaBlendEnable = false;
			graphics.RenderState.CullMode = CullMode.CullClockwiseFace;
			graphics.VertexDeclaration = this.decorationVertexDeclaration;
			effect.CurrentTechnique = effect.Techniques["Default"];
			effect.Begin();
			for (int i = 0; i < this.decorationBlocks.GetLength(0); i++)
			{
				for (int j = 0; j < this.decorationBlocks.GetLength(1); j++)
				{
					if (this.isView[i, j])
					{
						this.decorationBlocks[i, j].Draw(effect);
					}
				}
			}
			effect.End();
		}
	}
}
