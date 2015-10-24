using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class ZSC : IReadable
	{
		public class Mesh
		{
			[Category("Mesh-ZMS"), DefaultValue(0), Description("Enter the path of the Mesh"), DisplayName("Mesh path :")]
			public string path
			{
				get;
				set;
			}

			public void read(ref BinaryReader br)
			{
				this.path = RoseFile.ReadZString(ref br);
			}

			public void save(ref BinaryWriter bw)
			{
				RoseFile.WriteZString(ref bw, this.path);
			}
		}

		public class Materiel
		{
			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter the path of the Materiel"), DisplayName("Materiel path :")]
			public string path
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter if it's Skin"), DisplayName("Is skin :")]
			public short is_skin
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter if the Alpha is enabled"), DisplayName("Alpha Enable :")]
			public short alpha_enabled
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter if there is two sided"), DisplayName("Two sided :")]
			public short two_sided
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter if there is an alpha test"), DisplayName("Alpha test:")]
			public short alpha_test_enabled
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter if there is an alpha ref"), DisplayName("Alpha ref:")]
			public short alpha_ref_enabled
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter if there is z write"), DisplayName("z write:")]
			public short z_write_enabled
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter if there is z test"), DisplayName("z test:")]
			public short z_test_enabled
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter the blending mode"), DisplayName("Blending mode:")]
			public short blending_mode
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter if the Specular is enabled"), DisplayName("Specular :")]
			public short specular_enabled
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter the alpha"), DisplayName("Alpha :")]
			public float alpha
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Enter the glow type"), DisplayName("Glow type :")]
			public short glow_type
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Red"), DisplayName("Red :")]
			public float red
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Green"), DisplayName("Green :")]
			public float green
			{
				get;
				set;
			}

			[Category("Materiel-DDS"), DefaultValue(0), Description("Blue"), DisplayName("Blue :")]
			public float blue
			{
				get;
				set;
			}

			public void read(ref BinaryReader br)
			{
				this.path = RoseFile.ReadZString(ref br);
				this.is_skin = br.ReadInt16();
				this.alpha_enabled = br.ReadInt16();
				this.two_sided = br.ReadInt16();
				this.alpha_test_enabled = br.ReadInt16();
				this.alpha_ref_enabled = br.ReadInt16();
				this.z_write_enabled = br.ReadInt16();
				this.z_test_enabled = br.ReadInt16();
				this.blending_mode = br.ReadInt16();
				this.specular_enabled = br.ReadInt16();
				this.alpha = br.ReadSingle();
				this.glow_type = br.ReadInt16();
				this.red = br.ReadSingle();
				this.green = br.ReadSingle();
				this.blue = br.ReadSingle();
			}

			public void save(ref BinaryWriter bw)
			{
				RoseFile.WriteZString(ref bw, this.path);
				bw.Write(this.is_skin);
				bw.Write(this.alpha_enabled);
				bw.Write(this.two_sided);
				bw.Write(this.alpha_test_enabled);
				bw.Write(this.alpha_ref_enabled);
				bw.Write(this.z_write_enabled);
				bw.Write(this.z_test_enabled);
				bw.Write(this.blending_mode);
				bw.Write(this.specular_enabled);
				bw.Write(this.alpha);
				bw.Write(this.glow_type);
				bw.Write(this.red);
				bw.Write(this.green);
				bw.Write(this.blue);
			}
		}

		public class Effect
		{
			[Category("Effect-eft"), DefaultValue(0), Description("Enter the path of the Effect"), DisplayName("Effect path :")]
			public string path
			{
				get;
				set;
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

		public class Object
		{
			public class Mesh
			{
				public enum bone_type
				{
					Pelvis,
					Head = 4
				}

				public enum dummy_type
				{
					RightHand,
					LeftHand,
					LeftShield,
					Back,
					Feet,
					Face,
					Head
				}

				public Vector3 position;

				public Quaternion rotation;

				public Vector3 scale;

				public Quaternion axisrot;

				public Matrix world;

				[Category(" Basic"), DefaultValue(0), Description("Enter the model id"), DisplayName("Model id :")]
				public short mesh_id
				{
					get;
					set;
				}

				[Category(" Basic"), DefaultValue(0), Description("Enter the texture id"), DisplayName("Texture id :")]
				public short material_id
				{
					get;
					set;
				}

				[Category("01 Position"), DefaultValue(0), Description("Enter the position"), DisplayName("X :")]
				public float Position_X
				{
					get
					{
						return this.position.X;
					}
					set
					{
						this.position.X = value;
					}
				}

				[Category("01 Position"), DefaultValue(0), Description("Enter the position"), DisplayName("Y :")]
				public float Position_Y
				{
					get
					{
						return this.position.Y;
					}
					set
					{
						this.position.Y = value;
					}
				}

				[Category("01 Position"), DefaultValue(0), Description("Enter the position"), DisplayName("Z :")]
				public float Position_Z
				{
					get
					{
						return this.position.Z;
					}
					set
					{
						this.position.Z = value;
					}
				}

				[Category("01 Position"), DefaultValue(0), Description("Enter the position"), DisplayName("Enabled :")]
				public bool position_enabled
				{
					get;
					set;
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("X :")]
				public float Rotation_X
				{
					get
					{
						return this.rotation.X;
					}
					set
					{
						this.rotation.X = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("Y :")]
				public float Rotation_Y
				{
					get
					{
						return this.rotation.Y;
					}
					set
					{
						this.rotation.Y = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("Z :")]
				public float Rotation_Z
				{
					get
					{
						return this.rotation.Z;
					}
					set
					{
						this.rotation.Z = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("W :")]
				public float Rotation_W
				{
					get
					{
						return this.rotation.W;
					}
					set
					{
						this.rotation.W = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("Enabled :")]
				public bool rotation_enabled
				{
					get;
					set;
				}

				[Category("03 Scale"), DefaultValue(0), Description("Enter the scale"), DisplayName("X :")]
				public float Scale_X
				{
					get
					{
						return this.scale.X;
					}
					set
					{
						this.scale.X = value;
					}
				}

				[Category("03 Scale"), DefaultValue(0), Description("Enter the scale"), DisplayName("Y :")]
				public float Scale_Y
				{
					get
					{
						return this.scale.Y;
					}
					set
					{
						this.scale.Y = value;
					}
				}

				[Category("03 Scale"), DefaultValue(0), Description("Enter the scale"), DisplayName("Z :")]
				public float Scale_Z
				{
					get
					{
						return this.scale.Z;
					}
					set
					{
						this.scale.Z = value;
					}
				}

				[Category("03 Scale"), DefaultValue(0), Description("Enter the scale"), DisplayName("Enabled :")]
				public bool scale_enabled
				{
					get;
					set;
				}

				[Category("04 Axisrot"), DefaultValue(0), Description("Enter axis rotation"), DisplayName("X :")]
				public float Axisrot_X
				{
					get
					{
						return this.axisrot.X;
					}
					set
					{
						this.axisrot.X = value;
					}
				}

				[Category("04 Axisrot"), DefaultValue(0), Description("Enter axis rotation"), DisplayName("Y :")]
				public float Axisrot_Y
				{
					get
					{
						return this.axisrot.Y;
					}
					set
					{
						this.axisrot.Y = value;
					}
				}

				[Category("04 Axisrot"), DefaultValue(0), Description("Enter axis rotation"), DisplayName("Z :")]
				public float Axisrot_Z
				{
					get
					{
						return this.axisrot.Z;
					}
					set
					{
						this.axisrot.Z = value;
					}
				}

				[Category("04 Axisrot"), DefaultValue(0), Description("Enter axis rotation"), DisplayName("W :")]
				public float Axisrot_W
				{
					get
					{
						return this.axisrot.W;
					}
					set
					{
						this.axisrot.W = value;
					}
				}

				[Category("04 Axisrot"), DefaultValue(0), Description("Enter axis rotation"), DisplayName("Enabled :")]
				public bool axisrot_enabled
				{
					get;
					set;
				}

				[Category("05 Bone index"), DefaultValue(0), Description("Enter the bone index"), DisplayName("Bone index :")]
				public short bone_index
				{
					get;
					set;
				}

				[Category("05 Bone index"), DefaultValue(0), Description("Enter the bone index"), DisplayName(" Enabled :")]
				public bool bone_index_enabled
				{
					get;
					set;
				}

				[Category("06 Dummy Index"), DefaultValue(0), Description("Enter the dummy index"), DisplayName("Dummy index :")]
				public short dummy_index
				{
					get;
					set;
				}

				[Category("06 Dummy Index"), DefaultValue(0), Description("Enter the dummy index"), DisplayName(" Enabled :")]
				public bool dummy_index_enabled
				{
					get;
					set;
				}

				[Category("07 Parent"), DefaultValue(0), Description("Enter the parent"), DisplayName("Parent :")]
				public short parent
				{
					get;
					set;
				}

				[Category("07 Parent"), DefaultValue(0), Description("Enter the parent"), DisplayName("Enabled :")]
				public bool parent_enabled
				{
					get;
					set;
				}

				[Category("08 Collision type"), DefaultValue(0), Description("Enter the collision type"), DisplayName("Collision type :")]
				public short collision_type
				{
					get;
					set;
				}

				[Category("08 Collision type"), DefaultValue(0), Description("Enter the collision type"), DisplayName(" Enabled :")]
				private bool collision_type_enabled
				{
					get;
					set;
				}

				[Category("09 Motion"), DefaultValue(0), Description("Enter the motion path"), DisplayName("Motion path :")]
				public string Motion_path
				{
					get;
					set;
				}

				[Category("09 Motion"), DefaultValue(0), Description("Enter the motion path"), DisplayName("Enabled :")]
				public bool Motion_path_enabled
				{
					get;
					set;
				}

				[Category("10 Range set"), DefaultValue(0), Description("Enter the range set"), DisplayName("Range set :")]
				public short range_set
				{
					get;
					set;
				}

				[Category("10 Range set"), DefaultValue(0), Description("Enter the range set"), DisplayName("Enabled :")]
				public bool range_set_enabled
				{
					get;
					set;
				}

				[Category("11 Lightmap"), DefaultValue(0), Description("Enter if it use the lightmap"), DisplayName("Use lightmap :")]
				public short use_lightmap
				{
					get;
					set;
				}

				[Category("12 Data"), DefaultValue(0), Description("Enter the Data"), DisplayName("Data :")]
				public byte[] data
				{
					get;
					set;
				}

				public void read(ref BinaryReader br)
				{
					this.mesh_id = br.ReadInt16();
					this.material_id = br.ReadInt16();
					while (true)
					{
						byte b = br.ReadByte();
						if (b == 0)
						{
							break;
						}
						byte count = br.ReadByte();
						if (b == 1)
						{
							this.position_enabled = true;
							this.position.X = br.ReadSingle();
							this.position.Z = br.ReadSingle();
							this.position.Y = br.ReadSingle();
						}
						else if (b == 2)
						{
							this.rotation_enabled = true;
							this.rotation.W = br.ReadSingle();
							this.rotation.X = br.ReadSingle();
							this.rotation.Y = br.ReadSingle();
							this.rotation.Z = br.ReadSingle();
						}
						else if (b == 3)
						{
							this.scale_enabled = true;
							this.scale.X = br.ReadSingle();
							this.scale.Y = br.ReadSingle();
							this.scale.Z = br.ReadSingle();
						}
						else if (b == 4)
						{
							this.axisrot_enabled = true;
							this.axisrot.X = br.ReadSingle();
							this.axisrot.Y = br.ReadSingle();
							this.axisrot.Z = br.ReadSingle();
							this.axisrot.W = br.ReadSingle();
						}
						else if (b == 5)
						{
							this.bone_index_enabled = true;
							this.bone_index = br.ReadInt16();
						}
						else if (b == 6)
						{
							this.dummy_index_enabled = true;
							this.dummy_index = br.ReadInt16();
						}
						else if (b == 7)
						{
							this.parent_enabled = true;
							this.parent = br.ReadInt16();
						}
						else if (b == 29)
						{
							this.collision_type_enabled = true;
							this.collision_type = br.ReadInt16();
						}
						else if (b == 30)
						{
							this.Motion_path_enabled = true;
							this.Motion_path = Encoding.UTF8.GetString(br.ReadBytes((int)count));
						}
						else if (b == 31)
						{
							this.range_set_enabled = true;
							this.range_set = br.ReadInt16();
						}
						else if (b == 32)
						{
							this.use_lightmap = br.ReadInt16();
						}
						else
						{
							this.data = br.ReadBytes((int)count);
						}
					}
				}

				public void save(ref BinaryWriter bw)
				{
					bw.Write(this.mesh_id);
					bw.Write(this.material_id);
					if (this.position_enabled)
					{
						bw.Write(1);
						bw.Write(12);
						bw.Write(this.position.X);
						bw.Write(this.position.Y);
						bw.Write(this.position.Z);
					}
					if (this.rotation_enabled)
					{
						bw.Write(2);
						bw.Write(16);
						bw.Write(this.rotation.W);
						bw.Write(this.rotation.X);
						bw.Write(this.rotation.Y);
						bw.Write(this.rotation.Z);
					}
					if (this.scale_enabled)
					{
						bw.Write(3);
						bw.Write(12);
						bw.Write(this.scale.X);
						bw.Write(this.scale.Y);
						bw.Write(this.scale.Z);
					}
					if (this.axisrot_enabled)
					{
						bw.Write(4);
						bw.Write(16);
						bw.Write(this.axisrot.W);
						bw.Write(this.axisrot.X);
						bw.Write(this.axisrot.Y);
						bw.Write(this.axisrot.Z);
					}
					if (this.bone_index_enabled)
					{
						bw.Write(5);
						bw.Write(2);
						bw.Write(this.bone_index);
					}
					if (this.dummy_index_enabled)
					{
						bw.Write(6);
						bw.Write(2);
						bw.Write(this.dummy_index);
					}
					if (this.parent_enabled)
					{
						bw.Write(7);
						bw.Write(2);
						bw.Write(this.parent);
					}
					if (this.collision_type_enabled)
					{
						bw.Write(29);
						bw.Write(2);
						bw.Write(this.collision_type);
					}
					if (this.Motion_path_enabled)
					{
						bw.Write(30);
						byte[] bytes = Encoding.UTF8.GetBytes(this.Motion_path);
						bw.Write((byte)bytes.Length);
						bw.Write(bytes, 0, bytes.Length);
					}
					if (this.range_set_enabled)
					{
						bw.Write(31);
						bw.Write(2);
						bw.Write(this.range_set);
					}
					if (Convert.ToBoolean(this.use_lightmap))
					{
						bw.Write(32);
						bw.Write(2);
						bw.Write(this.use_lightmap);
					}
					if (this.data != null)
					{
						bw.Write(16);
						bw.Write(this.data.Length);
						bw.Write(this.data, 0, this.data.Length);
					}
					bw.Write(0);
				}
			}

			public class Effect
			{
				public Vector3 position;

				private bool position_enabled;

				public Quaternion rotation;

				private bool rotation_enabled;

				public Vector3 scale;

				[Category(" Basic"), DefaultValue(0), Description("Enter the effect id"), DisplayName("effect id :")]
				public short effect_id
				{
					get;
					set;
				}

				[Category(" Basic"), DefaultValue(0), Description("Enter the effect type"), DisplayName("effect type :")]
				public short effect_type
				{
					get;
					set;
				}

				[Category("01 Position"), DefaultValue(0), Description("Enter the position"), DisplayName("X :")]
				public float Position_X
				{
					get
					{
						return this.position.X;
					}
					set
					{
						this.position.X = value;
					}
				}

				[Category("01 Position"), DefaultValue(0), Description("Enter the position"), DisplayName("Y :")]
				public float Position_Y
				{
					get
					{
						return this.position.Y;
					}
					set
					{
						this.position.Y = value;
					}
				}

				[Category("01 Position"), DefaultValue(0), Description("Enter the position"), DisplayName("Z :")]
				public float Position_Z
				{
					get
					{
						return this.position.Z;
					}
					set
					{
						this.position.Z = value;
					}
				}

				[Category("01 Position"), DefaultValue(0), Description("Enter the position"), DisplayName("Enabled :")]
				public bool Position_enabled
				{
					get
					{
						return this.position_enabled;
					}
					set
					{
						this.position_enabled = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("X :")]
				public float Rotation_X
				{
					get
					{
						return this.rotation.X;
					}
					set
					{
						this.rotation.X = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("Y :")]
				public float Rotation_Y
				{
					get
					{
						return this.rotation.Y;
					}
					set
					{
						this.rotation.Y = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("Z :")]
				public float Rotation_Z
				{
					get
					{
						return this.rotation.Z;
					}
					set
					{
						this.rotation.Z = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("W :")]
				public float Rotation_W
				{
					get
					{
						return this.rotation.W;
					}
					set
					{
						this.rotation.W = value;
					}
				}

				[Category("02 Rotation"), DefaultValue(0), Description("Enter the rotation"), DisplayName("Enabled :")]
				public bool Rotation_enable
				{
					get
					{
						return this.rotation_enabled;
					}
					set
					{
						this.rotation_enabled = value;
					}
				}

				[Category("03 Scale"), DefaultValue(0), Description("Enter the scale"), DisplayName("X :")]
				public float Scale_X
				{
					get
					{
						return this.scale.X;
					}
					set
					{
						this.scale.X = value;
					}
				}

				[Category("03 Scale"), DefaultValue(0), Description("Enter the scale"), DisplayName("Y :")]
				public float Scale_Y
				{
					get
					{
						return this.scale.Y;
					}
					set
					{
						this.scale.Y = value;
					}
				}

				[Category("03 Scale"), DefaultValue(0), Description("Enter the scale"), DisplayName("Z :")]
				public float Scale_Z
				{
					get
					{
						return this.scale.Z;
					}
					set
					{
						this.scale.Z = value;
					}
				}

				[Category("03 Scale"), DefaultValue(0), Description("Enter the scale"), DisplayName("Enabled :")]
				private bool scale_enabled
				{
					get;
					set;
				}

				[Category("07 Parent"), DefaultValue(0), Description("Enter the parent"), DisplayName("Parent :")]
				public short parent
				{
					get;
					set;
				}

				[Category("07 Parent"), DefaultValue(0), Description("Enter the parent"), DisplayName("Enabled :")]
				public bool parent_enabled
				{
					get;
					set;
				}

				[Category("12 Data"), DefaultValue(0), Description("Enter the Data"), DisplayName("Data :")]
				public byte[] data
				{
					get;
					set;
				}

				public void read(ref BinaryReader br)
				{
					this.effect_id = br.ReadInt16();
					this.effect_type = br.ReadInt16();
					while (true)
					{
						byte b = br.ReadByte();
						if (b == 0)
						{
							break;
						}
						byte count = br.ReadByte();
						if (b == 1)
						{
							this.position_enabled = true;
							this.position.X = br.ReadSingle();
							this.position.Y = br.ReadSingle();
							this.position.Z = br.ReadSingle();
						}
						else if (b == 2)
						{
							this.rotation_enabled = true;
							this.rotation.W = br.ReadSingle();
							this.rotation.X = br.ReadSingle();
							this.rotation.Y = br.ReadSingle();
							this.rotation.Z = br.ReadSingle();
						}
						else if (b == 3)
						{
							this.scale_enabled = true;
							this.scale.X = br.ReadSingle();
							this.scale.Y = br.ReadSingle();
							this.scale.Z = br.ReadSingle();
						}
						else if (b == 7)
						{
							this.parent_enabled = true;
							this.parent = br.ReadInt16();
						}
						else
						{
							this.data = br.ReadBytes((int)count);
						}
					}
				}

				public void save(ref BinaryWriter bw)
				{
					bw.Write(this.effect_id);
					bw.Write(this.effect_type);
					if (this.position_enabled)
					{
						bw.Write(1);
						bw.Write(12);
						bw.Write(this.position.X);
						bw.Write(this.position.Y);
						bw.Write(this.position.Z);
					}
					if (this.rotation_enabled)
					{
						bw.Write(2);
						bw.Write(16);
						bw.Write(this.rotation.W);
						bw.Write(this.rotation.X);
						bw.Write(this.rotation.Y);
						bw.Write(this.rotation.Z);
					}
					if (this.scale_enabled)
					{
						bw.Write(3);
						bw.Write(12);
						bw.Write(this.scale.X);
						bw.Write(this.scale.Y);
						bw.Write(this.scale.Z);
					}
					if (this.parent_enabled)
					{
						bw.Write(7);
						bw.Write(2);
						bw.Write(this.parent);
					}
					if (this.data != null)
					{
						bw.Write(16);
						bw.Write(this.data.Length);
						bw.Write(this.data, 0, this.data.Length);
					}
					bw.Write(0);
				}
			}

			private Vector3 minbounds;

			private Vector3 maxbounds;

			public List<ZSC.Object.Mesh> list_mesh = new List<ZSC.Object.Mesh>();

			public List<ZSC.Object.Effect> list_effect = new List<ZSC.Object.Effect>();

			[Category(" Object"), DefaultValue(0), Description("Enter bounding sphere radius"), DisplayName("Bounding sphere radius :")]
			private int boundingsphere_radius
			{
				get;
				set;
			}

			[Category(" Object"), DefaultValue(0), Description("Enter bounding sphere x"), DisplayName("Bounding sphere x :")]
			private int boundingsphere_x
			{
				get;
				set;
			}

			[Category(" Object"), DefaultValue(0), Description("Enter bounding sphere y"), DisplayName("Bounding sphere y :")]
			private int boundingsphere_y
			{
				get;
				set;
			}

			[Category("Minbounds"), DefaultValue(0), Description("Enter the minbound"), DisplayName("X :")]
			public float Minbounds_X
			{
				get
				{
					return this.minbounds.X;
				}
				set
				{
					this.minbounds.X = value;
				}
			}

			[Category("Minbounds"), DefaultValue(0), Description("Enter the minbound"), DisplayName("Y :")]
			public float Minbounds_Y
			{
				get
				{
					return this.minbounds.Y;
				}
				set
				{
					this.minbounds.Y = value;
				}
			}

			[Category("Minbounds"), DefaultValue(0), Description("Enter the minbound"), DisplayName("Z :")]
			public float Minbounds_Z
			{
				get
				{
					return this.minbounds.Z;
				}
				set
				{
					this.minbounds.Z = value;
				}
			}

			[Category("Maxbounds"), DefaultValue(0), Description("Enter the maxbound"), DisplayName("X :")]
			public float Maxbounds_X
			{
				get
				{
					return this.maxbounds.X;
				}
				set
				{
					this.maxbounds.X = value;
				}
			}

			[Category("Maxbounds"), DefaultValue(0), Description("Enter the maxbound"), DisplayName("Y :")]
			public float Maxbounds_Y
			{
				get
				{
					return this.maxbounds.Y;
				}
				set
				{
					this.maxbounds.Y = value;
				}
			}

			[Category("Maxbounds"), DefaultValue(0), Description("Enter the maxbound"), DisplayName("Z :")]
			public float Maxbounds_Z
			{
				get
				{
					return this.maxbounds.Z;
				}
				set
				{
					this.maxbounds.Z = value;
				}
			}

			public void read(ref BinaryReader br)
			{
				this.boundingsphere_radius = br.ReadInt32();
				this.boundingsphere_x = br.ReadInt32();
				this.boundingsphere_y = br.ReadInt32();
				short num = br.ReadInt16();
				if (num > 0)
				{
					for (int num2 = 0; num2 != (int)num; num2++)
					{
						ZSC.Object.Mesh mesh = new ZSC.Object.Mesh();
						mesh.read(ref br);
						this.list_mesh.Add(mesh);
					}
					short num3 = br.ReadInt16();
					for (int num2 = 0; num2 != (int)num3; num2++)
					{
						ZSC.Object.Effect effect = new ZSC.Object.Effect();
						effect.read(ref br);
						this.list_effect.Add(effect);
					}
					this.minbounds.X = br.ReadSingle();
					this.minbounds.Y = br.ReadSingle();
					this.minbounds.Z = br.ReadSingle();
					this.maxbounds.X = br.ReadSingle();
					this.maxbounds.Y = br.ReadSingle();
					this.maxbounds.Z = br.ReadSingle();
				}
			}

			public void save(ref BinaryWriter bw)
			{
				bw.Write(this.boundingsphere_radius);
				bw.Write(this.boundingsphere_x);
				bw.Write(this.boundingsphere_y);
				bw.Write((short)this.list_mesh.Count);
				if (this.list_mesh.Count > 0)
				{
					for (int num = 0; num != this.list_mesh.Count; num++)
					{
						this.list_mesh[num].save(ref bw);
					}
					bw.Write((short)this.list_effect.Count);
					for (int num = 0; num != this.list_effect.Count; num++)
					{
						this.list_effect[num].save(ref bw);
					}
					bw.Write(this.minbounds.X);
					bw.Write(this.minbounds.Y);
					bw.Write(this.minbounds.Z);
					bw.Write(this.maxbounds.X);
					bw.Write(this.maxbounds.Y);
					bw.Write(this.maxbounds.Z);
				}
			}
		}

		public string path
		{
			get;
			set;
		}

		public List<ZSC.Mesh> listMesh
		{
			get;
			set;
		}

		public List<ZSC.Materiel> listMateriel
		{
			get;
			set;
		}

		public List<ZSC.Effect> listEffect
		{
			get;
			set;
		}

		public List<ZSC.Object> listObject
		{
			get;
			set;
		}

		public ClientType clientType
		{
			get;
			set;
		}

		public ZSC()
		{
			this.listMesh = new List<ZSC.Mesh>();
			this.listMateriel = new List<ZSC.Materiel>();
			this.listEffect = new List<ZSC.Effect>();
			this.listObject = new List<ZSC.Object>();
		}

		public void Load(string filePath)
		{
			this.Load(filePath, ClientType.IROSE);
		}

		public void Load(string Path, ClientType clientType)
		{
			this.path = Path;
			this.clientType = clientType;
			BinaryReader binaryReader = new BinaryReader(File.Open(Path, FileMode.Open));
			short num = binaryReader.ReadInt16();
			for (int i = 0; i < (int)num; i++)
			{
				ZSC.Mesh mesh = new ZSC.Mesh();
				mesh.read(ref binaryReader);
				this.listMesh.Add(mesh);
			}
			short num2 = binaryReader.ReadInt16();
			for (int i = 0; i < (int)num2; i++)
			{
				ZSC.Materiel materiel = new ZSC.Materiel();
				materiel.read(ref binaryReader);
				this.listMateriel.Add(materiel);
			}
			short num3 = binaryReader.ReadInt16();
			for (int i = 0; i < (int)num3; i++)
			{
				ZSC.Effect effect = new ZSC.Effect();
				effect.read(ref binaryReader);
				this.listEffect.Add(effect);
			}
			short num4 = binaryReader.ReadInt16();
			for (int i = 0; i < (int)num4; i++)
			{
				ZSC.Object @object = new ZSC.Object();
				@object.read(ref binaryReader);
				this.listObject.Add(@object);
			}
			binaryReader.Close();
		}

		public void Save(string Path)
		{
			BinaryWriter binaryWriter = new BinaryWriter(File.Open(Path, FileMode.Create));
			binaryWriter.Write((short)this.listMesh.Count);
			for (int i = 0; i < this.listMesh.Count; i++)
			{
				this.listMesh[i].save(ref binaryWriter);
			}
			binaryWriter.Write((short)this.listMateriel.Count);
			for (int i = 0; i < this.listMateriel.Count; i++)
			{
				this.listMateriel[i].save(ref binaryWriter);
			}
			binaryWriter.Write((short)this.listEffect.Count);
			for (int i = 0; i < this.listEffect.Count; i++)
			{
				this.listEffect[i].save(ref binaryWriter);
			}
			binaryWriter.Write((short)this.listObject.Count);
			for (int i = 0; i < this.listObject.Count; i++)
			{
				this.listObject[i].save(ref binaryWriter);
			}
			binaryWriter.Close();
		}

		public void Save()
		{
			this.Save(this.path);
		}
	}
}
