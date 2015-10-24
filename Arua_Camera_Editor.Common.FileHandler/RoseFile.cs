using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Text;

namespace Arua_Camera_Editor.Common.FileHandler
{
	internal class RoseFile
	{
		public static string ReadZString(ref BinaryReader br)
		{
			string text = "";
			while (true)
			{
				byte b = br.ReadByte();
				if (b == 0)
				{
					break;
				}
				text += (char)b;
			}
			return text;
		}

		public static void WriteZString(ref BinaryWriter bw, string mystring)
		{
			byte[] bytes = Encoding.Default.GetBytes(mystring);
			bw.Write(bytes, 0, mystring.Length);
			bw.Write(0);
		}

		public static string ReadFString(ref BinaryReader br, int length)
		{
			return new string(br.ReadChars(length));
		}

		public static void WriteFString(ref BinaryWriter bw, string mystring, int length)
		{
			byte[] bytes = Encoding.Default.GetBytes(mystring);
			bw.Write(bytes, 0, bytes.Length);
			for (int i = bytes.Length; i < length; i++)
			{
				bw.Write(0);
			}
		}

		public static string ReadSString(ref BinaryReader br)
		{
			short count = br.ReadInt16();
			return Encoding.UTF7.GetString(br.ReadBytes((int)count));
		}

		public static void WriteSString(ref BinaryWriter bw, string mystring)
		{
			bw.Write((short)mystring.Length);
			byte[] bytes = Encoding.Default.GetBytes(mystring);
			bw.Write(bytes, 0, mystring.Length);
		}

		public static string ReadBString(ref BinaryReader br)
		{
			int num = (int)br.ReadByte();
			if (num > 128)
			{
				num |= (int)br.ReadByte() << 7;
			}
			return Encoding.UTF8.GetString(br.ReadBytes(num));
		}

		public static void WriteBString(ref BinaryWriter bw, string mystring)
		{
			byte[] bytes = Encoding.Default.GetBytes(mystring);
			if (mystring.Length < 128)
			{
				bw.Write((byte)mystring.Length);
			}
			else
			{
				if (mystring.Length < 256)
				{
					bw.Write(Convert.ToByte(mystring.Length / 128));
				}
				else
				{
					bw.Write((int)(Convert.ToByte(mystring.Length / 128 - 1) * 128));
				}
				bw.Write((byte)mystring.Length);
			}
			bw.Write(bytes, 0, bytes.Length);
		}

		public static Vector2 ReadVector2(ref BinaryReader br)
		{
			return new Vector2
			{
				X = br.ReadSingle(),
				Y = br.ReadSingle()
			};
		}

		public static Vector3 ReadVector3(ref BinaryReader br)
		{
			return new Vector3
			{
				X = br.ReadSingle(),
				Y = br.ReadSingle(),
				Z = br.ReadSingle()
			};
		}

		public static Vector4 ReadVector4(ref BinaryReader br)
		{
			return new Vector4
			{
				W = br.ReadSingle(),
				X = br.ReadSingle(),
				Y = br.ReadSingle(),
				Z = br.ReadSingle()
			};
		}

		public static void WriteVector2(ref BinaryWriter bw, Vector2 value)
		{
			bw.Write(value.X);
			bw.Write(value.Y);
		}

		public static void WriteVector3(ref BinaryWriter bw, Vector3 value)
		{
			bw.Write(value.X);
			bw.Write(value.Y);
			bw.Write(value.Z);
		}

		public static void WriteVector4(ref BinaryWriter bw, Vector4 value)
		{
			bw.Write(value.W);
			bw.Write(value.X);
			bw.Write(value.Y);
			bw.Write(value.Z);
		}
	}
}
