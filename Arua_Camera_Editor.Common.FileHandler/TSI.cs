using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	internal class TSI : IReadable
	{
		public class DDS
		{
			public class DDSElement
			{
				[Category("1) Element"), Description("Select owner id"), DisplayName("Owner id :")]
				public short OwnerId
				{
					get;
					set;
				}

				[Category("2) Coord"), Description("Enter X cord")]
				public int X
				{
					get;
					set;
				}

				[Category("2) Coord"), Description("Enter Y cord")]
				public int Y
				{
					get;
					set;
				}

				[Category("3) Dimension"), Description("Enter the widht")]
				public int Width
				{
					get;
					set;
				}

				[Category("3) Dimension"), Description("Enter the height")]
				public int Height
				{
					get;
					set;
				}

				[Category("1) Element"), Description("Entrer the color"), DisplayName("Color :")]
				public int Color
				{
					get;
					set;
				}

				[Category("1) Element"), Description("Enter the name (32 caractères maximum)"), DisplayName("Name :")]
				public string Name
				{
					get;
					set;
				}

				public DDSElement()
				{
				}

				public DDSElement(short OwnerId, int X, int Y, int Width, int Height, int Color, string Name)
				{
					this.OwnerId = OwnerId;
					this.X = X;
					this.Y = Y;
					this.Width = Width;
					this.Height = Height;
					this.Color = Color;
					this.Name = Name;
				}
			}

			private Texture2D texture;

			[Category("DDS"), Description("Enter the path of DDS"), DisplayName("Name of dds :")]
			public string Path
			{
				get;
				set;
			}

			[Category("DDS"), Description("Enter the color key"), DisplayName("Color Key :")]
			public int ColourKey
			{
				get;
				set;
			}

			public List<TSI.DDS.DDSElement> ListDDS_element
			{
				get;
				set;
			}

			public DDS()
			{
				this.ListDDS_element = new List<TSI.DDS.DDSElement>();
			}
		}

		private string Path;

		public List<TSI.DDS> listDDS
		{
			get;
			set;
		}

		public ClientType clientType
		{
			get;
			set;
		}

		public TSI()
		{
			this.listDDS = new List<TSI.DDS>();
		}

		public void Load(string filePath)
		{
			this.Load(filePath, ClientType.IROSE);
		}

		public void Load(string filePath, ClientType clientType)
		{
			this.Path = filePath;
			this.clientType = clientType;
			BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open));
			short num = binaryReader.ReadInt16();
			this.listDDS = new List<TSI.DDS>((int)num);
			for (int i = 0; i < (int)num; i++)
			{
				TSI.DDS dDS = new TSI.DDS();
				dDS.Path = RoseFile.ReadSString(ref binaryReader);
				dDS.ColourKey = binaryReader.ReadInt32();
				this.listDDS.Add(dDS);
			}
			short num2 = binaryReader.ReadInt16();
			for (int i = 0; i < (int)num; i++)
			{
				short num3 = binaryReader.ReadInt16();
				this.listDDS[i].ListDDS_element = new List<TSI.DDS.DDSElement>((int)num3);
				for (int j = 0; j < (int)num3; j++)
				{
					TSI.DDS.DDSElement dDSElement = new TSI.DDS.DDSElement();
					dDSElement.OwnerId = binaryReader.ReadInt16();
					dDSElement.X = binaryReader.ReadInt32();
					dDSElement.Y = binaryReader.ReadInt32();
					dDSElement.Width = binaryReader.ReadInt32() - dDSElement.X;
					dDSElement.Height = binaryReader.ReadInt32() - dDSElement.Y;
					dDSElement.Color = binaryReader.ReadInt32();
					dDSElement.Name = RoseFile.ReadFString(ref binaryReader, 32);
					this.listDDS[i].ListDDS_element.Add(dDSElement);
				}
			}
			binaryReader.Close();
		}

		public void Reload()
		{
			this.Load(this.Path);
		}

		public void Save(string filePath)
		{
			this.Path = filePath;
			BinaryWriter bw = new BinaryWriter(File.Open(filePath, FileMode.Create));
			bw.Write((short)this.listDDS.Count);
			int totalElementCount = 0;
			this.listDDS.ForEach(delegate(TSI.DDS dds)
			{
				RoseFile.WriteSString(ref bw, dds.Path);
				bw.Write(dds.ColourKey);
				totalElementCount += dds.ListDDS_element.Count;
			});
			bw.Write((short)totalElementCount);
			this.listDDS.ForEach(delegate(TSI.DDS dds)
			{
				bw.Write((short)dds.ListDDS_element.Count);
				dds.ListDDS_element.ForEach(delegate(TSI.DDS.DDSElement dds_element)
				{
					bw.Write(dds_element.OwnerId);
					bw.Write(dds_element.X);
					bw.Write(dds_element.Y);
					bw.Write(dds_element.Width + dds_element.X);
					bw.Write(dds_element.Height + dds_element.Y);
					bw.Write(dds_element.Color);
					RoseFile.WriteFString(ref bw, dds_element.Name, 32);
				});
			});
			bw.Close();
		}

		public void Save()
		{
			this.Save(this.Path);
		}
	}
}
