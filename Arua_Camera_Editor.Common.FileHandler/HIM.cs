using System;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class HIM : IReadable
	{
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

		public int gridCount
		{
			get;
			set;
		}

		public float gridSize
		{
			get;
			set;
		}

		public float[,] altitude
		{
			get;
			set;
		}

		public HIM()
		{
			this.width = 0;
			this.height = 0;
			this.gridCount = 0;
			this.gridSize = 0f;
			this.altitude = new float[65, 65];
		}

		public void Load(string path, ClientType clientType)
		{
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open));
			this.width = binaryReader.ReadInt32();
			this.height = binaryReader.ReadInt32();
			this.gridCount = binaryReader.ReadInt32();
			this.gridSize = binaryReader.ReadSingle();
			for (int i = 0; i < 65; i++)
			{
				for (int j = 0; j < 65; j++)
				{
					this.altitude[i, j] = binaryReader.ReadSingle();
				}
			}
			binaryReader.Close();
		}

		public float GetAltitude(int indexX, int indexY)
		{
			return this.altitude[indexX, indexY] / 100f;
		}
	}
}
