using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class ZON : IReadable
	{
		private enum BlockType
		{
			BASIC_INFO,
			EVENT_POINTS,
			TEXTURE_LIST,
			TILE_LIST,
			OTHER_INFO
		}

		public class BasicInfoBlock
		{
			public struct Zone
			{
				public byte useMap;

				public float x;

				public float y;
			}

			public int zoneType;

			public int zoneWidth;

			public int zoneHeight;

			public int gridCount;

			public float gridSize;

			public int xCount;

			public int yCount;

			public ZON.BasicInfoBlock.Zone[,] zones;
		}

		public class EventPointBlock
		{
			public struct Entry
			{
				public Vector3 vect;

				public string name;
			}

			public List<ZON.EventPointBlock.Entry> listEntry;
		}

		public class TextureBlock
		{
			public string[] texturePath;
		}

		public class TileListBlock
		{
			public struct TileEntry
			{
				public int base1;

				public int base2;

				public int offest1;

				public int offest2;

				public int isBlending;

				public int orientation;

				public int tileType;
			}

			public List<ZON.TileListBlock.TileEntry> listTileEntry;
		}

		public class OtherInfoBlock
		{
			public string aeraName;

			public int isUnderground;

			public string buttonBGM;

			public string buttonBack;

			public int checkCount;

			public int standardPopulation;

			public int standardGrowthRate;

			public int metalConsumption;

			public int stoneConsumption;

			public int woodConsumption;

			public int leatherConsumption;

			public int clothConsumption;

			public int alchemyConsumption;

			public int chemicalConsumption;

			public int industrialConsumption;

			public int medicineConsumption;

			public int foodConsumption;
		}

		private List<ZON.BasicInfoBlock> listBasicBlock;

		private List<ZON.EventPointBlock> listEventBlock;

		private List<ZON.TextureBlock> listTextureBlock;

		private List<ZON.TileListBlock> listTileBlock;

		private List<ZON.OtherInfoBlock> listOtherBlock;

		public ZON()
		{
			this.listBasicBlock = new List<ZON.BasicInfoBlock>();
			this.listEventBlock = new List<ZON.EventPointBlock>();
			this.listTextureBlock = new List<ZON.TextureBlock>();
			this.listTileBlock = new List<ZON.TileListBlock>();
			this.listOtherBlock = new List<ZON.OtherInfoBlock>();
		}

		public void Load(string path, ClientType clientType)
		{
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open));
			int num = binaryReader.ReadInt32();
			int[] array = new int[num];
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = binaryReader.ReadInt32();
				array2[i] = binaryReader.ReadInt32();
			}
			for (int i = 0; i < num; i++)
			{
				binaryReader.BaseStream.Seek((long)array2[i], SeekOrigin.Begin);
				if (array[i] == 0)
				{
					ZON.BasicInfoBlock basicInfoBlock = new ZON.BasicInfoBlock();
					basicInfoBlock.zoneType = binaryReader.ReadInt32();
					basicInfoBlock.zoneWidth = binaryReader.ReadInt32();
					basicInfoBlock.zoneHeight = binaryReader.ReadInt32();
					basicInfoBlock.gridCount = binaryReader.ReadInt32();
					basicInfoBlock.gridSize = binaryReader.ReadSingle();
					basicInfoBlock.xCount = binaryReader.ReadInt32();
					basicInfoBlock.yCount = binaryReader.ReadInt32();
					basicInfoBlock.zones = new ZON.BasicInfoBlock.Zone[basicInfoBlock.zoneWidth, basicInfoBlock.zoneHeight];
					for (int j = 0; j < basicInfoBlock.zoneWidth; j++)
					{
						for (int k = 0; k < basicInfoBlock.zoneHeight; k++)
						{
							basicInfoBlock.zones[j, k].useMap = binaryReader.ReadByte();
							basicInfoBlock.zones[j, k].x = binaryReader.ReadSingle();
							basicInfoBlock.zones[j, k].y = binaryReader.ReadSingle();
						}
					}
					this.listBasicBlock.Add(basicInfoBlock);
				}
				else if (array[i] == 1)
				{
					ZON.EventPointBlock eventPointBlock = new ZON.EventPointBlock();
					eventPointBlock.listEntry = new List<ZON.EventPointBlock.Entry>();
					int num2 = binaryReader.ReadInt32();
					for (int j = 0; j < num2; j++)
					{
						ZON.EventPointBlock.Entry item = default(ZON.EventPointBlock.Entry);
						item.vect = RoseFile.ReadVector3(ref binaryReader);
						item.name = RoseFile.ReadBString(ref binaryReader);
						eventPointBlock.listEntry.Add(item);
					}
					this.listEventBlock.Add(eventPointBlock);
				}
				else if (array[i] == 2)
				{
					ZON.TextureBlock textureBlock = new ZON.TextureBlock();
					int num2 = binaryReader.ReadInt32();
					textureBlock.texturePath = new string[num2];
					for (int j = 0; j < num2; j++)
					{
						textureBlock.texturePath[j] = RoseFile.ReadBString(ref binaryReader);
					}
					this.listTextureBlock.Add(textureBlock);
				}
				else if (array[i] == 3)
				{
					ZON.TileListBlock tileListBlock = new ZON.TileListBlock();
					tileListBlock.listTileEntry = new List<ZON.TileListBlock.TileEntry>();
					int num2 = binaryReader.ReadInt32();
					for (int j = 0; j < num2; j++)
					{
						ZON.TileListBlock.TileEntry item2 = default(ZON.TileListBlock.TileEntry);
						item2.base1 = binaryReader.ReadInt32();
						item2.base2 = binaryReader.ReadInt32();
						item2.offest1 = binaryReader.ReadInt32();
						item2.offest2 = binaryReader.ReadInt32();
						item2.isBlending = binaryReader.ReadInt32();
						item2.orientation = binaryReader.ReadInt32();
						item2.tileType = binaryReader.ReadInt32();
						tileListBlock.listTileEntry.Add(item2);
					}
					this.listTileBlock.Add(tileListBlock);
				}
				else if (array[i] == 4)
				{
				}
			}
			binaryReader.Close();
		}

		public Texture2D GetBottomTexture(int index)
		{
			return ContentManager.Instance().GetTexture(this.listTextureBlock[0].texturePath[this.listTileBlock[0].listTileEntry[index].base1 + this.listTileBlock[0].listTileEntry[index].offest1]);
		}

		public Texture2D GetTopTexture(int index)
		{
			return ContentManager.Instance().GetTexture(this.listTextureBlock[0].texturePath[this.listTileBlock[0].listTileEntry[index].base2 + this.listTileBlock[0].listTileEntry[index].offest2]);
		}

		public int GetOrientation(int index)
		{
			return this.listTileBlock[0].listTileEntry[index].orientation;
		}
	}
}
