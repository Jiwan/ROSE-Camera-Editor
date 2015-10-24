using Arua_Camera_Editor.Common.FileHandler;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Arua_Camera_Editor.Common
{
	public class Map
	{
		private string mapFolder;

		private Terrain terrain;

		private int minSizeX;

		private int minSizeY;

		private int maxSizeX;

		private int maxSizeY;

		private GraphicsDevice graphics;

		public Map(GraphicsDevice gDevice)
		{
			this.graphics = gDevice;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public void Load(int mapIndex)
		{
			ContentManager.Instance().ClearMapData();
			STB sTB = ContentManager.Instance().GetSTB("3DDATA\\STB\\LIST_ZONE.STB");
			string cellValue = sTB.GetCellValue<string>(mapIndex, 2);
			string cellValue2 = sTB.GetCellValue<string>(mapIndex, 13);
			string cellValue3 = sTB.GetCellValue<string>(mapIndex, 12);
			this.mapFolder = Directory.GetParent(ContentManager.GetRootPath() + cellValue).FullName + "\\";
			this.minSizeX = sTB.GetCellValue<int>(mapIndex, 10);
			this.minSizeY = sTB.GetCellValue<int>(mapIndex, 11);
			this.maxSizeY = this.minSizeY;
			this.maxSizeX = this.minSizeX;
			while (File.Exists(string.Concat(new object[]
			{
				this.mapFolder,
				this.maxSizeX,
				"_",
				this.minSizeY,
				".HIM"
			})))
			{
				this.maxSizeX++;
			}
			this.maxSizeX--;
			while (File.Exists(string.Concat(new object[]
			{
				this.mapFolder,
				this.minSizeX,
				"_",
				this.maxSizeY,
				".HIM"
			})))
			{
				this.maxSizeY++;
			}
			this.maxSizeY--;
			this.terrain = new Terrain(this.graphics);
			this.terrain.Load(cellValue, cellValue2, cellValue3, this.mapFolder, this.minSizeX, this.minSizeY, this.maxSizeX, this.maxSizeY);
		}

		public void Draw(GraphicsDevice graphics, Effect effect, Effect terrainEffect)
		{
			this.terrain.Draw(graphics, effect, terrainEffect);
		}
	}
}
