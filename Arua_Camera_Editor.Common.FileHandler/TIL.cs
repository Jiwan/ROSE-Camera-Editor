using System;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class TIL : IReadable
	{
		public struct Tile
		{
			public byte brushID;

			public byte tileIndex;

			public byte tileNumber;

			public int tileID;
		}

		public int width
		{
			get;
			set;
		}

		public int height
		{
			get;
			set;
		}

		public TIL.Tile[,] Tiles
		{
			get;
			set;
		}

		public TIL()
		{
			this.width = 0;
			this.height = 0;
			this.Tiles = new TIL.Tile[16, 16];
		}

		public void Load(string path, ClientType clientType)
		{
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open));
			this.width = binaryReader.ReadInt32();
			this.height = binaryReader.ReadInt32();
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.height; j++)
				{
					this.Tiles[i, j].brushID = binaryReader.ReadByte();
					this.Tiles[i, j].tileIndex = binaryReader.ReadByte();
					this.Tiles[i, j].tileNumber = binaryReader.ReadByte();
					this.Tiles[i, j].tileID = binaryReader.ReadInt32();
				}
			}
			binaryReader.Close();
		}

		public TIL.Tile GetTile(int indexX, int indexY)
		{
			return this.Tiles[indexX, indexY];
		}
	}
}
