using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class IFO : IReadable
	{
		private enum BlockType
		{
			ECONOMY_DATA,
			DECORATIONS,
			NPC_SPAWNS,
			BUILDINGS,
			SOUND_EFFECTS,
			EFFECTS,
			ANIMATABLES,
			WATERBIG,
			MONSTER_SPAWNS,
			WATER_PLANES,
			WARP_GATES,
			COLLISION_BLOCK,
			TRIGGERS
		}

		public class BasicEntry
		{
			public string data;

			public short warpID;

			public short eventID;

			public int objectType;

			public int objectID;

			public int mapPositionX;

			public int mapPositionY;

			public Vector4 rotation;

			public Vector3 position;

			public Vector3 scale;
		}

		public class EconomyDataBlock
		{
			public int width;

			public int height;

			public int mapCellX;

			public int mapCellY;

			public float[,] unused;

			public string name;
		}

		public struct DecorationBlock
		{
			public List<IFO.BasicEntry> listDecoration;
		}

		public class NpcBlock
		{
			public class NpcEntry : IFO.BasicEntry
			{
				public int aiPartenIndex;

				public string conFilePath;
			}

			public List<IFO.NpcBlock.NpcEntry> listNPC;
		}

		public class BuildingBlock
		{
			public List<IFO.BasicEntry> listBuilding;
		}

		public class SoundEffectBlock
		{
			public class SoundEffectEntry : IFO.BasicEntry
			{
				public string soundPath;

				public int range;

				public int interval;
			}

			public List<IFO.SoundEffectBlock.SoundEffectEntry> listSoundEffect;
		}

		public class EffectBlock
		{
			public class EffectEntry : IFO.BasicEntry
			{
				public string effectPath;
			}

			public List<IFO.EffectBlock.EffectEntry> listEffect;
		}

		public class AnimatablesBlock
		{
			public List<IFO.BasicEntry> listAnimatable;
		}

		public class WaterBigBlock
		{
			public struct WaterCell
			{
				public byte use;

				public float height;

				public int waterType;

				public int waterIndex;

				public int reserved;
			}

			public IFO.WaterBigBlock.WaterCell[,] waterCells;
		}

		public class MonstersSpawnBlock
		{
			public class MonstersEntry : IFO.BasicEntry
			{
				public struct Spawn
				{
					public struct Mob
					{
						public string name;

						public int ID;

						public int amount;
					}

					public string name;

					public int interval;

					public int limitCount;

					public int range;

					public int tacticPoints;

					public List<IFO.MonstersSpawnBlock.MonstersEntry.Spawn.Mob> listBasicMob;

					public List<IFO.MonstersSpawnBlock.MonstersEntry.Spawn.Mob> listTacticMob;
				}

				public IFO.MonstersSpawnBlock.MonstersEntry.Spawn spawn;
			}

			public List<IFO.MonstersSpawnBlock.MonstersEntry> listMonsterSpawn;
		}

		public class WaterPlaneBlock
		{
			public class WaterEntry
			{
				public Vector3 start;

				public Vector3 end;
			}

			public IFO.WaterPlaneBlock.WaterEntry[] watersEntries;

			public float basic;
		}

		public class WarpGateBlock
		{
			public List<IFO.BasicEntry> listWarpGate;
		}

		public class CollisionBlock
		{
			public List<IFO.BasicEntry> listCollision;
		}

		public class TriggerBlock
		{
			public class TriggerEntry : IFO.BasicEntry
			{
				public string qsdTrigger;

				public string luaTrigger;
			}

			public List<IFO.TriggerBlock.TriggerEntry> listTrigger;
		}

		public List<IFO.EconomyDataBlock> listEconomyBlock
		{
			get;
			set;
		}

		public List<IFO.DecorationBlock> listDecorationBlock
		{
			get;
			set;
		}

		public List<IFO.NpcBlock> listNpcBlock
		{
			get;
			set;
		}

		public List<IFO.BuildingBlock> listBuildingBlock
		{
			get;
			set;
		}

		public List<IFO.SoundEffectBlock> listSoundEffectBlock
		{
			get;
			set;
		}

		public List<IFO.EffectBlock> listEffectBlock
		{
			get;
			set;
		}

		public List<IFO.AnimatablesBlock> listAnimatablesBlock
		{
			get;
			set;
		}

		public List<IFO.WaterBigBlock> listWaterBigBlock
		{
			get;
			set;
		}

		public List<IFO.MonstersSpawnBlock> listMonstersSpawnBlock
		{
			get;
			set;
		}

		public List<IFO.WaterPlaneBlock> listWaterPlaneBlock
		{
			get;
			set;
		}

		public List<IFO.WarpGateBlock> listWarpGateBlock
		{
			get;
			set;
		}

		public List<IFO.CollisionBlock> listCollisionBlock
		{
			get;
			set;
		}

		public List<IFO.TriggerBlock> listTriggerBlock
		{
			get;
			set;
		}

		public IFO()
		{
			this.listEconomyBlock = new List<IFO.EconomyDataBlock>();
			this.listDecorationBlock = new List<IFO.DecorationBlock>();
			this.listNpcBlock = new List<IFO.NpcBlock>();
			this.listBuildingBlock = new List<IFO.BuildingBlock>();
			this.listSoundEffectBlock = new List<IFO.SoundEffectBlock>();
			this.listEffectBlock = new List<IFO.EffectBlock>();
			this.listAnimatablesBlock = new List<IFO.AnimatablesBlock>();
			this.listWaterBigBlock = new List<IFO.WaterBigBlock>();
			this.listMonstersSpawnBlock = new List<IFO.MonstersSpawnBlock>();
			this.listWaterPlaneBlock = new List<IFO.WaterPlaneBlock>();
			this.listWarpGateBlock = new List<IFO.WarpGateBlock>();
			this.listTriggerBlock = new List<IFO.TriggerBlock>();
			this.listCollisionBlock = new List<IFO.CollisionBlock>();
		}

		public void Load(string path, ClientType type)
		{
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open));
			int num = binaryReader.ReadInt32();
			IFO.BlockType[] array = new IFO.BlockType[num];
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (IFO.BlockType)binaryReader.ReadInt32();
				array2[i] = binaryReader.ReadInt32();
			}
			for (int i = 0; i < num; i++)
			{
				binaryReader.BaseStream.Seek((long)array2[i], SeekOrigin.Begin);
				if (array[i] == IFO.BlockType.ECONOMY_DATA)
				{
					IFO.EconomyDataBlock economyDataBlock = new IFO.EconomyDataBlock();
					economyDataBlock.width = binaryReader.ReadInt32();
					economyDataBlock.height = binaryReader.ReadInt32();
					economyDataBlock.mapCellX = binaryReader.ReadInt32();
					economyDataBlock.mapCellY = binaryReader.ReadInt32();
					economyDataBlock.unused = new float[4, 4];
					binaryReader.BaseStream.Seek(64L, SeekOrigin.Current);
					economyDataBlock.name = RoseFile.ReadBString(ref binaryReader);
					this.listEconomyBlock.Add(economyDataBlock);
				}
				else if (array[i] == IFO.BlockType.DECORATIONS)
				{
					IFO.DecorationBlock item = default(IFO.DecorationBlock);
					int num2 = binaryReader.ReadInt32();
					item.listDecoration = new List<IFO.BasicEntry>();
					for (int j = 0; j < num2; j++)
					{
						IFO.BasicEntry basicEntry = new IFO.BasicEntry();
						basicEntry.data = RoseFile.ReadBString(ref binaryReader);
						basicEntry.warpID = binaryReader.ReadInt16();
						basicEntry.eventID = binaryReader.ReadInt16();
						basicEntry.objectType = binaryReader.ReadInt32();
						basicEntry.objectID = binaryReader.ReadInt32();
						basicEntry.mapPositionX = binaryReader.ReadInt32();
						basicEntry.mapPositionY = binaryReader.ReadInt32();
						basicEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
						basicEntry.position = RoseFile.ReadVector3(ref binaryReader);
						basicEntry.scale = RoseFile.ReadVector3(ref binaryReader);
						item.listDecoration.Add(basicEntry);
					}
					this.listDecorationBlock.Add(item);
				}
				else if (array[i] == IFO.BlockType.NPC_SPAWNS)
				{
					IFO.NpcBlock npcBlock = new IFO.NpcBlock();
					int num2 = binaryReader.ReadInt32();
					npcBlock.listNPC = new List<IFO.NpcBlock.NpcEntry>();
					for (int j = 0; j < num2; j++)
					{
						IFO.NpcBlock.NpcEntry npcEntry = new IFO.NpcBlock.NpcEntry();
						npcEntry.data = RoseFile.ReadBString(ref binaryReader);
						npcEntry.warpID = binaryReader.ReadInt16();
						npcEntry.eventID = binaryReader.ReadInt16();
						npcEntry.objectType = binaryReader.ReadInt32();
						npcEntry.objectID = binaryReader.ReadInt32();
						npcEntry.mapPositionX = binaryReader.ReadInt32();
						npcEntry.mapPositionY = binaryReader.ReadInt32();
						npcEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
						npcEntry.position = RoseFile.ReadVector3(ref binaryReader);
						npcEntry.scale = RoseFile.ReadVector3(ref binaryReader);
						npcEntry.aiPartenIndex = binaryReader.ReadInt32();
						npcEntry.conFilePath = RoseFile.ReadBString(ref binaryReader);
						npcBlock.listNPC.Add(npcEntry);
					}
					this.listNpcBlock.Add(npcBlock);
				}
				else if (array[i] == IFO.BlockType.BUILDINGS)
				{
					IFO.BuildingBlock buildingBlock = new IFO.BuildingBlock();
					int num2 = binaryReader.ReadInt32();
					buildingBlock.listBuilding = new List<IFO.BasicEntry>();
					for (int j = 0; j < num2; j++)
					{
						IFO.BasicEntry basicEntry = new IFO.BasicEntry();
						basicEntry.data = RoseFile.ReadBString(ref binaryReader);
						basicEntry.warpID = binaryReader.ReadInt16();
						basicEntry.eventID = binaryReader.ReadInt16();
						basicEntry.objectType = binaryReader.ReadInt32();
						basicEntry.objectID = binaryReader.ReadInt32();
						basicEntry.mapPositionX = binaryReader.ReadInt32();
						basicEntry.mapPositionY = binaryReader.ReadInt32();
						basicEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
						basicEntry.position = RoseFile.ReadVector3(ref binaryReader);
						basicEntry.scale = RoseFile.ReadVector3(ref binaryReader);
						buildingBlock.listBuilding.Add(basicEntry);
					}
					this.listBuildingBlock.Add(buildingBlock);
				}
				else if (array[i] == IFO.BlockType.SOUND_EFFECTS)
				{
					IFO.SoundEffectBlock soundEffectBlock = new IFO.SoundEffectBlock();
					int num2 = binaryReader.ReadInt32();
					soundEffectBlock.listSoundEffect = new List<IFO.SoundEffectBlock.SoundEffectEntry>();
					for (int j = 0; j < num2; j++)
					{
						IFO.SoundEffectBlock.SoundEffectEntry soundEffectEntry = new IFO.SoundEffectBlock.SoundEffectEntry();
						soundEffectEntry.data = RoseFile.ReadBString(ref binaryReader);
						soundEffectEntry.warpID = binaryReader.ReadInt16();
						soundEffectEntry.eventID = binaryReader.ReadInt16();
						soundEffectEntry.objectType = binaryReader.ReadInt32();
						soundEffectEntry.objectID = binaryReader.ReadInt32();
						soundEffectEntry.mapPositionX = binaryReader.ReadInt32();
						soundEffectEntry.mapPositionY = binaryReader.ReadInt32();
						soundEffectEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
						soundEffectEntry.position = RoseFile.ReadVector3(ref binaryReader);
						soundEffectEntry.scale = RoseFile.ReadVector3(ref binaryReader);
						soundEffectEntry.soundPath = RoseFile.ReadBString(ref binaryReader);
						soundEffectEntry.range = binaryReader.ReadInt32();
						soundEffectEntry.interval = binaryReader.ReadInt32();
						soundEffectBlock.listSoundEffect.Add(soundEffectEntry);
					}
					this.listSoundEffectBlock.Add(soundEffectBlock);
				}
				else if (array[i] == IFO.BlockType.EFFECTS)
				{
					IFO.EffectBlock effectBlock = new IFO.EffectBlock();
					int num2 = binaryReader.ReadInt32();
					effectBlock.listEffect = new List<IFO.EffectBlock.EffectEntry>();
					for (int j = 0; j < num2; j++)
					{
						IFO.EffectBlock.EffectEntry effectEntry = new IFO.EffectBlock.EffectEntry();
						effectEntry.data = RoseFile.ReadBString(ref binaryReader);
						effectEntry.warpID = binaryReader.ReadInt16();
						effectEntry.eventID = binaryReader.ReadInt16();
						effectEntry.objectType = binaryReader.ReadInt32();
						effectEntry.objectID = binaryReader.ReadInt32();
						effectEntry.mapPositionX = binaryReader.ReadInt32();
						effectEntry.mapPositionY = binaryReader.ReadInt32();
						effectEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
						effectEntry.position = RoseFile.ReadVector3(ref binaryReader);
						effectEntry.scale = RoseFile.ReadVector3(ref binaryReader);
						effectEntry.effectPath = RoseFile.ReadBString(ref binaryReader);
						effectBlock.listEffect.Add(effectEntry);
					}
					this.listEffectBlock.Add(effectBlock);
				}
				else if (array[i] == IFO.BlockType.ANIMATABLES)
				{
					IFO.AnimatablesBlock animatablesBlock = new IFO.AnimatablesBlock();
					int num2 = binaryReader.ReadInt32();
					animatablesBlock.listAnimatable = new List<IFO.BasicEntry>();
					for (int j = 0; j < num2; j++)
					{
						IFO.BasicEntry basicEntry = new IFO.BasicEntry();
						basicEntry.data = RoseFile.ReadBString(ref binaryReader);
						basicEntry.warpID = binaryReader.ReadInt16();
						basicEntry.eventID = binaryReader.ReadInt16();
						basicEntry.objectType = binaryReader.ReadInt32();
						basicEntry.objectID = binaryReader.ReadInt32();
						basicEntry.mapPositionX = binaryReader.ReadInt32();
						basicEntry.mapPositionY = binaryReader.ReadInt32();
						basicEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
						basicEntry.position = RoseFile.ReadVector3(ref binaryReader);
						basicEntry.scale = RoseFile.ReadVector3(ref binaryReader);
						animatablesBlock.listAnimatable.Add(basicEntry);
					}
					this.listAnimatablesBlock.Add(animatablesBlock);
				}
				else if (array[i] == IFO.BlockType.WATERBIG)
				{
					IFO.WaterBigBlock waterBigBlock = new IFO.WaterBigBlock();
					int num3 = binaryReader.ReadInt32();
					int num4 = binaryReader.ReadInt32();
					waterBigBlock.waterCells = new IFO.WaterBigBlock.WaterCell[num3, num4];
					for (int j = 0; j < num3; j++)
					{
						for (int k = 0; k < num4; k++)
						{
							waterBigBlock.waterCells[j, k].use = binaryReader.ReadByte();
							waterBigBlock.waterCells[j, k].height = binaryReader.ReadSingle();
							waterBigBlock.waterCells[j, k].waterType = binaryReader.ReadInt32();
							waterBigBlock.waterCells[j, k].waterIndex = binaryReader.ReadInt32();
							waterBigBlock.waterCells[j, k].reserved = binaryReader.ReadInt32();
						}
					}
					this.listWaterBigBlock.Add(waterBigBlock);
				}
				else if (array[i] != IFO.BlockType.MONSTER_SPAWNS)
				{
					if (array[i] == IFO.BlockType.WATER_PLANES)
					{
						IFO.WaterPlaneBlock waterPlaneBlock = new IFO.WaterPlaneBlock();
						waterPlaneBlock.basic = binaryReader.ReadSingle();
						int num2 = binaryReader.ReadInt32();
						waterPlaneBlock.watersEntries = new IFO.WaterPlaneBlock.WaterEntry[num2];
						for (int j = 0; j < num2; j++)
						{
							waterPlaneBlock.watersEntries[j] = new IFO.WaterPlaneBlock.WaterEntry();
							waterPlaneBlock.watersEntries[j].start = RoseFile.ReadVector3(ref binaryReader);
							waterPlaneBlock.watersEntries[j].end = RoseFile.ReadVector3(ref binaryReader);
						}
						this.listWaterPlaneBlock.Add(waterPlaneBlock);
					}
					else if (array[i] == IFO.BlockType.WARP_GATES)
					{
						IFO.WarpGateBlock warpGateBlock = new IFO.WarpGateBlock();
						int num2 = binaryReader.ReadInt32();
						warpGateBlock.listWarpGate = new List<IFO.BasicEntry>();
						for (int j = 0; j < num2; j++)
						{
							IFO.BasicEntry basicEntry = new IFO.BasicEntry();
							basicEntry.data = RoseFile.ReadBString(ref binaryReader);
							basicEntry.warpID = binaryReader.ReadInt16();
							basicEntry.eventID = binaryReader.ReadInt16();
							basicEntry.objectType = binaryReader.ReadInt32();
							basicEntry.objectID = binaryReader.ReadInt32();
							basicEntry.mapPositionX = binaryReader.ReadInt32();
							basicEntry.mapPositionY = binaryReader.ReadInt32();
							basicEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
							basicEntry.position = RoseFile.ReadVector3(ref binaryReader);
							basicEntry.scale = RoseFile.ReadVector3(ref binaryReader);
							warpGateBlock.listWarpGate.Add(basicEntry);
						}
						this.listWarpGateBlock.Add(warpGateBlock);
					}
					else if (array[i] == IFO.BlockType.COLLISION_BLOCK)
					{
						IFO.CollisionBlock collisionBlock = new IFO.CollisionBlock();
						int num2 = binaryReader.ReadInt32();
						collisionBlock.listCollision = new List<IFO.BasicEntry>();
						for (int j = 0; j < num2; j++)
						{
							IFO.BasicEntry basicEntry = new IFO.BasicEntry();
							basicEntry.data = RoseFile.ReadBString(ref binaryReader);
							basicEntry.warpID = binaryReader.ReadInt16();
							basicEntry.eventID = binaryReader.ReadInt16();
							basicEntry.objectType = binaryReader.ReadInt32();
							basicEntry.objectID = binaryReader.ReadInt32();
							basicEntry.mapPositionX = binaryReader.ReadInt32();
							basicEntry.mapPositionY = binaryReader.ReadInt32();
							basicEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
							basicEntry.position = RoseFile.ReadVector3(ref binaryReader);
							basicEntry.scale = RoseFile.ReadVector3(ref binaryReader);
							collisionBlock.listCollision.Add(basicEntry);
						}
						this.listCollisionBlock.Add(collisionBlock);
					}
					else if (array[i] == IFO.BlockType.TRIGGERS)
					{
						IFO.TriggerBlock triggerBlock = new IFO.TriggerBlock();
						int num2 = binaryReader.ReadInt32();
						triggerBlock.listTrigger = new List<IFO.TriggerBlock.TriggerEntry>();
						for (int j = 0; j < num2; j++)
						{
							IFO.TriggerBlock.TriggerEntry triggerEntry = new IFO.TriggerBlock.TriggerEntry();
							triggerEntry.data = RoseFile.ReadBString(ref binaryReader);
							triggerEntry.warpID = binaryReader.ReadInt16();
							triggerEntry.eventID = binaryReader.ReadInt16();
							triggerEntry.objectType = binaryReader.ReadInt32();
							triggerEntry.objectID = binaryReader.ReadInt32();
							triggerEntry.mapPositionX = binaryReader.ReadInt32();
							triggerEntry.mapPositionY = binaryReader.ReadInt32();
							triggerEntry.rotation = RoseFile.ReadVector4(ref binaryReader);
							triggerEntry.position = RoseFile.ReadVector3(ref binaryReader);
							triggerEntry.scale = RoseFile.ReadVector3(ref binaryReader);
							triggerEntry.qsdTrigger = RoseFile.ReadBString(ref binaryReader);
							triggerEntry.luaTrigger = RoseFile.ReadBString(ref binaryReader);
							triggerBlock.listTrigger.Add(triggerEntry);
						}
						this.listTriggerBlock.Add(triggerBlock);
					}
				}
			}
			binaryReader.Close();
		}
	}
}
