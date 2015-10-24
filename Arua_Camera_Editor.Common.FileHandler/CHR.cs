using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class CHR
	{
		public enum type_action
		{
			Warning,
			Walking,
			Attack,
			Hit,
			Dieing,
			Running,
			Unknow6,
			Unknow7,
			Unknow8,
			Unknow9,
			Unknow10,
			Unknow11,
			Unknow12,
			Unknow = -1
		}

		public class Mesh
		{
			public string path;

			[Category("Skeleton-ZMD"), DefaultValue(0), Description("Enter the path of the Mesh"), DisplayName("Mesh path :")]
			public string Mesh_path
			{
				get
				{
					return this.path;
				}
				set
				{
					this.path = value;
				}
			}

			public void read(ref BinaryReader br)
			{
				this.path = "";
				while (true)
				{
					byte b = br.ReadByte();
					if (b == 0)
					{
						break;
					}
					this.path += (char)b;
				}
			}

			public void save(ref BinaryWriter bw)
			{
				byte[] bytes = Encoding.Default.GetBytes(this.path);
				bw.Write(bytes, 0, bytes.Length);
				bw.Write(0);
			}
		}

		public class Motion
		{
			public string path;

			[Category("Motion-ZMO"), DefaultValue(0), Description("Enter the path of the Motion"), DisplayName("Motion path :")]
			public string Motion_path
			{
				get
				{
					return this.path;
				}
				set
				{
					this.path = value;
				}
			}

			public void read(ref BinaryReader br)
			{
				this.path = "";
				while (true)
				{
					byte b = br.ReadByte();
					if (b == 0)
					{
						break;
					}
					this.path += (char)b;
				}
			}

			public void save(ref BinaryWriter bw)
			{
				byte[] bytes = Encoding.Default.GetBytes(this.path);
				bw.Write(bytes, 0, bytes.Length);
				bw.Write(0);
			}
		}

		public class Effect
		{
			public string path;

			[Category("Effect-eft"), DefaultValue(0), Description("Enter the path of the Effect"), DisplayName("Effect path :")]
			public string Effect_path
			{
				get
				{
					return this.path;
				}
				set
				{
					this.path = value;
				}
			}

			public void read(ref BinaryReader br)
			{
				this.path = "";
				while (true)
				{
					byte b = br.ReadByte();
					if (b == 0)
					{
						break;
					}
					this.path += (char)b;
				}
			}

			public void save(ref BinaryWriter bw)
			{
				byte[] bytes = Encoding.Default.GetBytes(this.path);
				bw.Write(bytes, 0, bytes.Length);
				bw.Write(0);
			}
		}

		public class Character
		{
			public enum type_action
			{
				Warning,
				Walking,
				Attack,
				Hit,
				Dieing,
				Running,
				Unknow6,
				Unknow7,
				Unknow8,
				Unknow9,
				Unknow10,
				Unknow11,
				Unknow12,
				Unknow = -1
			}

			public enum npc_action
			{

			}

			public class Mesh
			{
				public short zscobjid;

				[Category("Part"), DefaultValue(0), Description("Enter the id of the part"), DisplayName("Part :")]
				public short zsc_obj_id
				{
					get
					{
						return this.zscobjid;
					}
					set
					{
						this.zscobjid = value;
					}
				}

				public void read(ref BinaryReader br)
				{
					this.zscobjid = br.ReadInt16();
				}

				public void save(ref BinaryWriter bw)
				{
					bw.Write(this.zscobjid);
				}
			}

			public class Motion
			{
				public short id;

				public short zscmotionid;

				[Category("Animation "), DefaultValue(0), Description("Select the type of action"), DisplayName("Action type :")]
				public CHR.Character.type_action ID
				{
					get
					{
						return (CHR.Character.type_action)this.id;
					}
					set
					{
						this.id = Convert.ToInt16(value);
					}
				}

				[Category("Animation "), DefaultValue(0), Description("Select the motion of the animation"), DisplayName("Motion id :")]
				public short Motion_id
				{
					get
					{
						return this.zscmotionid;
					}
					set
					{
						this.zscmotionid = value;
					}
				}

				public void read(ref BinaryReader br)
				{
					this.id = br.ReadInt16();
					this.zscmotionid = br.ReadInt16();
				}

				public void save(ref BinaryWriter bw)
				{
					bw.Write(this.id);
					bw.Write(this.zscmotionid);
				}
			}

			public class Effect
			{
				public short id;

				public short zsceffectid;

				[Category("Effect "), DefaultValue(0), Description("Select the type of action"), DisplayName("Action type :")]
				public CHR.Character.type_action ID
				{
					get
					{
						return (CHR.Character.type_action)this.id;
					}
					set
					{
						this.id = Convert.ToInt16(value);
					}
				}

				[Category("Effect "), DefaultValue(0), Description("Select the effect"), DisplayName("Effect id :")]
				public short Effect_id
				{
					get
					{
						return this.zsceffectid;
					}
					set
					{
						this.zsceffectid = value;
					}
				}

				public void read(ref BinaryReader br)
				{
					this.id = br.ReadInt16();
					this.zsceffectid = br.ReadInt16();
				}

				public void save(ref BinaryWriter bw)
				{
					bw.Write(this.id);
					bw.Write(this.zsceffectid);
				}
			}

			public byte is_active;

			public short bone_id;

			public string name;

			public List<CHR.Character.Mesh> List_Mesh = new List<CHR.Character.Mesh>();

			public List<CHR.Character.Motion> List_Motion = new List<CHR.Character.Motion>();

			public List<CHR.Character.Effect> List_Effect = new List<CHR.Character.Effect>();

			[Category("Mob"), DefaultValue(0), Description("Activate or not"), DisplayName("Activate")]
			public bool activate
			{
				get
				{
					return Convert.ToBoolean(this.is_active);
				}
				set
				{
					this.is_active = Convert.ToByte(value);
				}
			}

			[Category("Mob"), DefaultValue(0), Description("Enter the id of the bone"), DisplayName("Bone id (Skeleton) :")]
			public short Bone_id
			{
				get
				{
					return this.bone_id;
				}
				set
				{
					this.bone_id = value;
				}
			}

			[Category("Mob"), DefaultValue(0), Description("Entrer the name of the character"), DisplayName("Name :")]
			public string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					this.name = value;
				}
			}

			public void read(ref BinaryReader br)
			{
				this.is_active = br.ReadByte();
				if (this.is_active == 1)
				{
					this.bone_id = br.ReadInt16();
					this.name = "";
					while (true)
					{
						byte b = br.ReadByte();
						if (b == 0)
						{
							break;
						}
						this.name += (char)b;
					}
					int num = (int)br.ReadInt16();
					for (int num2 = 0; num2 != num; num2++)
					{
						CHR.Character.Mesh mesh = new CHR.Character.Mesh();
						mesh.read(ref br);
						this.List_Mesh.Add(mesh);
					}
					int num3 = (int)br.ReadInt16();
					for (int num2 = 0; num2 != num3; num2++)
					{
						CHR.Character.Motion motion = new CHR.Character.Motion();
						motion.read(ref br);
						this.List_Motion.Add(motion);
					}
					int num4 = (int)br.ReadInt16();
					for (int num2 = 0; num2 != num4; num2++)
					{
						CHR.Character.Effect effect = new CHR.Character.Effect();
						effect.read(ref br);
						this.List_Effect.Add(effect);
					}
				}
			}

			public void save(ref BinaryWriter bw)
			{
				bw.Write(this.is_active);
				if (this.is_active == 1)
				{
					bw.Write(this.bone_id);
					byte[] bytes = Encoding.Default.GetBytes(this.name);
					bw.Write(bytes, 0, bytes.Length);
					bw.Write(0);
					bw.Write((short)this.List_Mesh.Count);
					for (int num = 0; num != this.List_Mesh.Count; num++)
					{
						this.List_Mesh[num].save(ref bw);
					}
					bw.Write((short)this.List_Motion.Count);
					for (int num = 0; num != this.List_Motion.Count; num++)
					{
						this.List_Motion[num].save(ref bw);
					}
					bw.Write((short)this.List_Effect.Count);
					for (int num = 0; num != this.List_Effect.Count; num++)
					{
						this.List_Effect[num].save(ref bw);
					}
				}
			}
		}

		private ClientType type;

		public string path;

		public List<CHR.Mesh> list_mesh = new List<CHR.Mesh>();

		public List<CHR.Motion> list_motion = new List<CHR.Motion>();

		public List<CHR.Effect> list_effect = new List<CHR.Effect>();

		public List<CHR.Character> list_character = new List<CHR.Character>();

		public void Load(string path, ClientType myClientType)
		{
			this.type = myClientType;
			this.path = path;
			this.list_mesh.Clear();
			this.list_character.Clear();
			this.list_effect.Clear();
			this.list_motion.Clear();
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open), Encoding.GetEncoding("EUC-KR"));
			int num = (int)binaryReader.ReadInt16();
			for (int num2 = 0; num2 != num; num2++)
			{
				CHR.Mesh mesh = new CHR.Mesh();
				mesh.read(ref binaryReader);
				this.list_mesh.Add(mesh);
			}
			int num3 = (int)binaryReader.ReadInt16();
			for (int num2 = 0; num2 != num3; num2++)
			{
				CHR.Motion motion = new CHR.Motion();
				motion.read(ref binaryReader);
				this.list_motion.Add(motion);
			}
			int num4 = (int)binaryReader.ReadInt16();
			for (int num2 = 0; num2 != num4; num2++)
			{
				CHR.Effect effect = new CHR.Effect();
				effect.read(ref binaryReader);
				this.list_effect.Add(effect);
			}
			int num5 = (int)binaryReader.ReadInt16();
			for (int num2 = 0; num2 != num5; num2++)
			{
				CHR.Character character = new CHR.Character();
				character.read(ref binaryReader);
				this.list_character.Add(character);
			}
			binaryReader.Close();
		}

		public void Save()
		{
			this.Save(this.path);
		}

		public void Save(string path)
		{
			BinaryWriter binaryWriter = new BinaryWriter(File.Open(path, FileMode.Create));
			binaryWriter.Write((short)this.list_mesh.Count);
			for (int num = 0; num != this.list_mesh.Count; num++)
			{
				this.list_mesh[num].save(ref binaryWriter);
			}
			binaryWriter.Write((short)this.list_motion.Count);
			for (int num = 0; num != this.list_motion.Count; num++)
			{
				this.list_motion[num].save(ref binaryWriter);
			}
			binaryWriter.Write((short)this.list_effect.Count);
			for (int num = 0; num != this.list_effect.Count; num++)
			{
				this.list_effect[num].save(ref binaryWriter);
			}
			binaryWriter.Write((short)this.list_character.Count);
			for (int num = 0; num != this.list_character.Count; num++)
			{
				this.list_character[num].save(ref binaryWriter);
			}
			binaryWriter.Close();
		}
	}
}
