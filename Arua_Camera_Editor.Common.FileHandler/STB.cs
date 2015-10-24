using System;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class STB : IReadable
	{
		public struct Column
		{
			public short width;

			public string title;
		}

		public STB.Column[] column;

		public ClientType clientType
		{
			get;
			set;
		}

		public string path
		{
			get;
			set;
		}

		public string formatCode
		{
			get;
			set;
		}

		public int rowCount
		{
			get;
			set;
		}

		public int columnCount
		{
			get;
			set;
		}

		public string[,] cell
		{
			get;
			set;
		}

		public int RowHeight
		{
			get;
			set;
		}

		public void Load(string mypath, ClientType myclientType)
		{
			this.path = mypath;
			this.clientType = myclientType;
			BinaryReader binaryReader = new BinaryReader(File.Open(mypath, FileMode.Open));
			this.formatCode = new string(binaryReader.ReadChars(4));
			int num = binaryReader.ReadInt32();
			this.rowCount = binaryReader.ReadInt32();
			this.columnCount = binaryReader.ReadInt32();
			this.column = new STB.Column[this.columnCount + 1];
			this.cell = new string[this.rowCount, this.columnCount];
			this.RowHeight = binaryReader.ReadInt32();
			for (int i = 0; i < this.columnCount + 1; i++)
			{
				this.column[i].width = binaryReader.ReadInt16();
			}
			for (int i = 0; i < this.columnCount + 1; i++)
			{
				this.column[i].title = RoseFile.ReadSString(ref binaryReader);
			}
			for (int i = 0; i < this.rowCount - 1; i++)
			{
				this.cell[i, 0] = RoseFile.ReadSString(ref binaryReader);
			}
			binaryReader.BaseStream.Seek((long)num, SeekOrigin.Begin);
			for (int i = 0; i < this.rowCount - 1; i++)
			{
				for (int j = 0; j < this.columnCount - 1; j++)
				{
					this.cell[i, j + 1] = RoseFile.ReadSString(ref binaryReader);
				}
			}
			binaryReader.Close();
		}

		public void Save(string mypath)
		{
			BinaryWriter binaryWriter = new BinaryWriter(File.Open(mypath, FileMode.Create));
			RoseFile.WriteFString(ref binaryWriter, this.formatCode, 4);
			binaryWriter.Write(0);
			binaryWriter.Write(this.rowCount);
			binaryWriter.Write(this.columnCount);
			binaryWriter.Write(this.RowHeight);
			for (int i = 0; i < this.columnCount + 1; i++)
			{
				binaryWriter.Write(this.column[i].width);
			}
			for (int i = 0; i < this.columnCount + 1; i++)
			{
				RoseFile.WriteSString(ref binaryWriter, this.column[i].title);
			}
			for (int i = 0; i < this.rowCount - 1; i++)
			{
				RoseFile.WriteSString(ref binaryWriter, this.cell[i, 0]);
			}
			long position = binaryWriter.BaseStream.Position;
			binaryWriter.BaseStream.Seek(4L, SeekOrigin.Begin);
			binaryWriter.Write((int)position);
			binaryWriter.Seek((int)position, SeekOrigin.Begin);
			for (int i = 0; i < this.rowCount - 1; i++)
			{
				for (int j = 0; j < this.columnCount - 1; j++)
				{
					RoseFile.WriteSString(ref binaryWriter, this.cell[i, j + 1]);
				}
			}
			binaryWriter.Close();
		}

		public void Save()
		{
			this.Save(this.path);
		}

		public void AddRow(string[] newRow)
		{
			if (newRow.Length == this.columnCount)
			{
				this.rowCount++;
				string[,] array = new string[this.rowCount, this.columnCount];
				Array.Copy(this.cell, array, this.rowCount - 1);
				for (int i = 0; i < this.columnCount; i++)
				{
					array[this.rowCount - 1, i] = newRow[i];
				}
				this.cell = array;
			}
		}

		public void ReplaceRow(int position, string[] newRow)
		{
			if (newRow.Length == this.columnCount)
			{
				for (int i = 0; i < this.columnCount; i++)
				{
					this.cell[position, i] = newRow[i];
				}
			}
		}

		public bool EmptyRow(int rowid)
		{
			bool result = true;
			for (int i = 0; i < this.columnCount; i++)
			{
				if (this.cell[rowid, i] != "")
				{
					result = false;
				}
			}
			return result;
		}

		public T GetCellValue<T>(int row, int column)
		{
			T result;
			if (typeof(T) == typeof(int))
			{
				result = (T)((object)Convert.ToInt32(this.cell[row, column]));
			}
			else
			{
				result = (T)((object)this.cell[row, column]);
			}
			return result;
		}
	}
}
