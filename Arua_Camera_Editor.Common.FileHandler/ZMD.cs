using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class ZMD : IReadable
	{
		public class Bone
		{
			public int parentid;

			public bool Dummy;

			public string name;

			public Vector3 position;

			public Vector4 rotation;

			public Matrix matrix;
		}

		public byte version;

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

		public List<ZMD.Bone> listBone
		{
			get;
			set;
		}

		public int DummyOffset
		{
			get;
			set;
		}

		public ZMD()
		{
			this.listBone = new List<ZMD.Bone>();
		}

		public void Load(string Path, ClientType myClientType)
		{
			this.path = Path;
			this.clientType = myClientType;
			BinaryReader binaryReader = new BinaryReader(File.Open(this.path, FileMode.Open));
			string text = RoseFile.ReadFString(ref binaryReader, 7);
			if (text == "ZMD0003")
			{
				this.version = 3;
			}
			else
			{
				if (!(text == "ZMD0002"))
				{
					throw new Exception("Wrong file header " + text + " on " + this.path);
				}
				this.version = 2;
			}
			int num = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ZMD.Bone bone = new ZMD.Bone();
				bone.Dummy = false;
				bone.parentid = binaryReader.ReadInt32();
				bone.name = RoseFile.ReadZString(ref binaryReader);
				bone.position = RoseFile.ReadVector3(ref binaryReader);
				bone.position /= 100f;
				bone.rotation = RoseFile.ReadVector4(ref binaryReader);
				bone.matrix = Matrix.CreateFromQuaternion(new Quaternion(bone.rotation.X, bone.rotation.Y, bone.rotation.Z, bone.rotation.W));
				bone.matrix *= Matrix.CreateTranslation(bone.position);
				this.listBone.Add(bone);
			}
			int num2 = binaryReader.ReadInt32();
			this.DummyOffset = num;
			for (int i = 0; i < num2; i++)
			{
				ZMD.Bone bone2 = new ZMD.Bone();
				bone2.Dummy = true;
				bone2.name = RoseFile.ReadZString(ref binaryReader);
				bone2.parentid = binaryReader.ReadInt32();
				bone2.position = RoseFile.ReadVector3(ref binaryReader);
				bone2.position /= 100f;
				bone2.matrix = Matrix.Identity;
				if (this.version == 3)
				{
					bone2.rotation = RoseFile.ReadVector4(ref binaryReader);
					bone2.matrix = Matrix.CreateFromQuaternion(new Quaternion(bone2.rotation.X, bone2.rotation.Y, bone2.rotation.Z, bone2.rotation.W));
				}
				bone2.matrix *= Matrix.CreateTranslation(bone2.position);
				this.listBone.Add(bone2);
			}
			binaryReader.Close();
			this.TransformChildren(0);
		}

		private void TransformChildren(int parent)
		{
			for (int i = 0; i < this.listBone.Count; i++)
			{
				if (i != parent)
				{
					if (this.listBone[i].parentid == parent)
					{
						this.listBone[i].matrix *= this.listBone[this.listBone[i].parentid].matrix;
						if (!this.listBone[i].Dummy)
						{
							this.TransformChildren(i);
						}
					}
				}
			}
		}
	}
}
