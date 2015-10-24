using System;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class STL : IReadable
	{
		public enum LangageType
		{
			unknow,
			english
		}

		public struct STLEntry
		{
			public string string_ID;

			public uint ID;

			public uint[] Offset;

			public string[] text;

			public string[] comment;

			public string[] quest1;

			public string[] quest2;
		}

		public STL.STLEntry[] entry;

		public uint[] languageOffset;

		public string path
		{
			get;
			set;
		}

		public ClientType clientType
		{
			get;
			set;
		}

		public string type
		{
			get;
			set;
		}

		public int entryCount
		{
			get;
			set;
		}

		public int languageCount
		{
			get;
			set;
		}

		public void Load(string mypath, ClientType myclientType)
		{
			this.path = mypath;
			this.clientType = myclientType;
			BinaryReader binaryReader = new BinaryReader(new FileStream(this.path, FileMode.Open));
			this.type = RoseFile.ReadBString(ref binaryReader);
			this.entryCount = binaryReader.ReadInt32();
			this.entry = new STL.STLEntry[this.entryCount];
			for (int i = 0; i < this.entryCount; i++)
			{
				this.entry[i].string_ID = RoseFile.ReadBString(ref binaryReader);
				this.entry[i].ID = binaryReader.ReadUInt32();
			}
			if (this.clientType == ClientType.JROSE)
			{
				this.languageCount = 1;
				for (int j = 0; j < this.entryCount; j++)
				{
					this.entry[j].text = new string[1];
					this.entry[j].comment = new string[1];
					this.entry[j].quest1 = new string[1];
					this.entry[j].quest2 = new string[1];
				}
				for (int j = 0; j < this.entryCount; j++)
				{
					this.entry[j].text[0] = RoseFile.ReadBString(ref binaryReader);
					if (this.type == "QEST01" || this.type == "ITST01")
					{
						this.entry[j].comment[0] = RoseFile.ReadBString(ref binaryReader);
						if (this.type == "QEST01")
						{
							this.entry[j].quest1[0] = RoseFile.ReadBString(ref binaryReader);
							this.entry[j].quest2[0] = RoseFile.ReadBString(ref binaryReader);
						}
					}
				}
			}
			else
			{
				this.languageCount = binaryReader.ReadInt32();
				this.languageOffset = new uint[this.languageCount];
				for (int i = 0; i < this.languageCount; i++)
				{
					this.languageOffset[i] = binaryReader.ReadUInt32();
				}
				for (int j = 0; j < this.entryCount; j++)
				{
					this.entry[j].Offset = new uint[this.languageCount];
					this.entry[j].text = new string[this.languageCount];
					this.entry[j].comment = new string[this.languageCount];
					this.entry[j].quest1 = new string[this.languageCount];
					this.entry[j].quest2 = new string[this.languageCount];
				}
				for (int i = 0; i < this.languageCount; i++)
				{
					binaryReader.BaseStream.Seek((long)((ulong)this.languageOffset[i]), SeekOrigin.Begin);
					for (int j = 0; j < this.entryCount; j++)
					{
						this.entry[j].Offset[i] = binaryReader.ReadUInt32();
					}
				}
				for (int i = 0; i < this.languageCount; i++)
				{
					for (int j = 0; j < this.entryCount; j++)
					{
						binaryReader.BaseStream.Seek((long)((ulong)this.entry[j].Offset[i]), SeekOrigin.Begin);
						this.entry[j].text[i] = RoseFile.ReadBString(ref binaryReader);
						if (this.type == "QEST01" || this.type == "ITST01")
						{
							this.entry[j].comment[i] = RoseFile.ReadBString(ref binaryReader);
							if (this.type == "QEST01")
							{
								this.entry[j].quest1[i] = RoseFile.ReadBString(ref binaryReader);
								this.entry[j].quest2[i] = RoseFile.ReadBString(ref binaryReader);
							}
						}
					}
				}
			}
			binaryReader.Close();
		}

		public void Save(string mypath)
		{
			BinaryWriter binaryWriter = new BinaryWriter(File.Open(mypath, FileMode.Create));
			RoseFile.WriteBString(ref binaryWriter, this.type);
			binaryWriter.Write(this.entryCount);
			for (int i = 0; i < this.entryCount; i++)
			{
				RoseFile.WriteBString(ref binaryWriter, this.entry[i].string_ID);
				binaryWriter.Write(this.entry[i].ID);
			}
			if (this.clientType == ClientType.JROSE)
			{
				for (int i = 0; i < this.languageCount; i++)
				{
					for (int j = 0; j < this.entryCount; j++)
					{
						RoseFile.WriteBString(ref binaryWriter, this.entry[j].text[i]);
						if (this.type == "QEST01" || this.type == "ITST01")
						{
							RoseFile.WriteBString(ref binaryWriter, this.entry[j].comment[i]);
							if (this.type == "QEST01")
							{
								RoseFile.WriteBString(ref binaryWriter, this.entry[j].quest1[i]);
								RoseFile.WriteBString(ref binaryWriter, this.entry[j].quest2[i]);
							}
						}
					}
				}
			}
			else
			{
				binaryWriter.Write(this.languageCount);
				long position = binaryWriter.BaseStream.Position;
				for (int i = 0; i < this.languageCount; i++)
				{
					binaryWriter.Write(1);
				}
				uint value = Convert.ToUInt32(binaryWriter.BaseStream.Position);
				for (int j = 0; j < this.entryCount; j++)
				{
					binaryWriter.Write(1);
				}
				for (int j = 0; j < this.entryCount; j++)
				{
					this.entry[j].Offset[1] = Convert.ToUInt32(binaryWriter.BaseStream.Position);
					RoseFile.WriteBString(ref binaryWriter, this.entry[j].text[1]);
					if (this.type == "QEST01" || this.type == "ITST01")
					{
						RoseFile.WriteBString(ref binaryWriter, this.entry[j].comment[1]);
						if (this.type == "QEST01")
						{
							RoseFile.WriteBString(ref binaryWriter, this.entry[j].quest1[1]);
							RoseFile.WriteBString(ref binaryWriter, this.entry[j].quest2[1]);
						}
					}
				}
				binaryWriter.BaseStream.Seek(position, SeekOrigin.Begin);
				for (int i = 0; i < this.languageCount; i++)
				{
					binaryWriter.Write(value);
				}
				for (int j = 0; j < this.entryCount; j++)
				{
					binaryWriter.Write(this.entry[j].Offset[1]);
				}
			}
			binaryWriter.Close();
		}

		public void Save()
		{
			this.Save(this.path);
		}

		public void AddEntry(int ID, string string_ID, string text)
		{
			STL.STLEntry sTLEntry = default(STL.STLEntry);
			sTLEntry.ID = (uint)ID;
			sTLEntry.string_ID = string_ID;
			sTLEntry.text = new string[this.languageCount];
			sTLEntry.comment = new string[this.languageCount];
			sTLEntry.quest1 = new string[this.languageCount];
			sTLEntry.quest2 = new string[this.languageCount];
			sTLEntry.Offset = new uint[this.languageCount];
			for (int i = 0; i < this.languageCount; i++)
			{
				sTLEntry.text[i] = text;
				sTLEntry.comment[i] = "";
				sTLEntry.quest1[i] = "";
				sTLEntry.quest2[i] = "";
			}
			this.entryCount++;
			STL.STLEntry[] array = this.entry;
			this.entry = new STL.STLEntry[this.entryCount];
			for (int i = 0; i < this.entryCount - 1; i++)
			{
				this.entry[i] = array[i];
			}
			this.entry[this.entryCount - 1] = sTLEntry;
		}

		public void AddEntry(int ID, string string_ID, string text, string comment)
		{
			STL.STLEntry sTLEntry = default(STL.STLEntry);
			sTLEntry.ID = (uint)ID;
			sTLEntry.string_ID = string_ID;
			sTLEntry.text = new string[this.languageCount];
			sTLEntry.comment = new string[this.languageCount];
			sTLEntry.quest1 = new string[this.languageCount];
			sTLEntry.quest2 = new string[this.languageCount];
			sTLEntry.Offset = new uint[this.languageCount];
			for (int i = 0; i < this.languageCount; i++)
			{
				sTLEntry.text[i] = text;
				sTLEntry.comment[i] = comment;
				sTLEntry.quest1[i] = "";
				sTLEntry.quest2[i] = "";
			}
			this.entryCount++;
			STL.STLEntry[] array = this.entry;
			this.entry = new STL.STLEntry[this.entryCount];
			for (int i = 0; i < this.entryCount - 1; i++)
			{
				this.entry[i] = array[i];
			}
			this.entry[this.entryCount - 1] = sTLEntry;
		}

		public void AddEntry(int ID, string string_ID, string text, string comment, string quest1, string quest2)
		{
			STL.STLEntry sTLEntry = default(STL.STLEntry);
			sTLEntry.ID = (uint)ID;
			sTLEntry.string_ID = string_ID;
			sTLEntry.text = new string[this.languageCount];
			sTLEntry.comment = new string[this.languageCount];
			sTLEntry.quest1 = new string[this.languageCount];
			sTLEntry.quest2 = new string[this.languageCount];
			sTLEntry.Offset = new uint[this.languageCount];
			for (int i = 0; i < this.languageCount; i++)
			{
				sTLEntry.text[i] = text;
				sTLEntry.comment[i] = comment;
				sTLEntry.quest1[i] = quest1;
				sTLEntry.quest2[i] = quest2;
			}
			this.entryCount++;
			STL.STLEntry[] array = this.entry;
			this.entry = new STL.STLEntry[this.entryCount];
			for (int i = 0; i < this.entryCount - 1; i++)
			{
				this.entry[i] = array[i];
			}
			this.entry[this.entryCount - 1] = sTLEntry;
		}

		public bool GetEntry(string stringID, STL.STLEntry entry)
		{
			for (int i = 0; i < this.entryCount; i++)
			{
			}
			return false;
		}
	}
}
