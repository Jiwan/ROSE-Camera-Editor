using System;
using System.Collections.Generic;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class LIT : IReadable
	{
		public struct Object
		{
			public struct Part
			{
				public string name;

				public int partID;

				public string ddsName;

				public int ddsID;

				public int ddsDivisionSize;

				public int ddsDivisionCount;

				public int ddsPartID;
			}

			public int objectID;

			public List<LIT.Object.Part> listParts;
		}

		public List<LIT.Object> listObject
		{
			get;
			set;
		}

		public List<string> listDDSName
		{
			get;
			set;
		}

		public LIT()
		{
			this.listDDSName = new List<string>();
			this.listObject = new List<LIT.Object>();
		}

		public void Load(string path, ClientType type)
		{
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open));
			int num = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				int num2 = binaryReader.ReadInt32();
				LIT.Object item = default(LIT.Object);
				item.objectID = binaryReader.ReadInt32();
				for (int j = 0; j < num2; j++)
				{
					LIT.Object.Part item2 = default(LIT.Object.Part);
					item2.name = RoseFile.ReadBString(ref binaryReader);
					item2.partID = binaryReader.ReadInt32();
					item2.ddsName = RoseFile.ReadBString(ref binaryReader);
					item2.ddsID = binaryReader.ReadInt32();
					item2.ddsDivisionSize = binaryReader.ReadInt32();
					item2.ddsDivisionCount = binaryReader.ReadInt32();
					item2.ddsPartID = binaryReader.ReadInt32();
					item.listParts.Add(item2);
				}
				this.listObject.Add(item);
			}
			int num3 = binaryReader.ReadInt32();
			for (int i = 0; i < num3; i++)
			{
				string item3 = RoseFile.ReadBString(ref binaryReader);
				this.listDDSName.Add(item3);
			}
			binaryReader.Close();
		}
	}
}
