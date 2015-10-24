using Arua_Camera_Editor.Common.GraphicsHandler.ObjectVertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class ZMS : IReadable
	{
		public struct Weight
		{
			public float Weight1;

			public float Weight2;

			public float Weight3;

			public float Weight4;
		}

		public struct BoneID
		{
			public short BoneId1;

			public short BoneId2;

			public short BoneId3;

			public short BoneId4;
		}

		private static Matrix roseCoordinate = Matrix.CreateRotationX(1.57079637f) * Matrix.CreateRotationZ(3.14159274f);

		private string formatCode;

		public Vector3 minBounds;

		public Vector3 maxBounds;

		public short boneCount;

		public short[] boneLookUp;

		public short vertCount;

		private short faceCount;

		private short stripCount;

		private Texture2D texture;

		private ZSC.Materiel Materiel;

		private int? bindBoneIndex = null;

		private int? bindDummyIndex = null;

		private VertexBuffer vertexBuffer;

		private IndexBuffer indexBuffer;

		private GraphicsDevice graphics;

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

		public ObjectVertex[] vertex
		{
			get;
			set;
		}

		public short[] indices
		{
			get;
			set;
		}

		public ZMS.Weight[] vertexWeight
		{
			get;
			set;
		}

		public ZMS.BoneID[] vertexBoneID
		{
			get;
			set;
		}

		public ZMS(GraphicsDevice graphics)
		{
			this.graphics = graphics;
		}

		public void Load(string Path, ClientType myClientType)
		{
			this.path = Path;
			this.clientType = myClientType;
			BinaryReader binaryReader = new BinaryReader(File.Open(this.path, FileMode.Open));
			binaryReader.BaseStream.Seek(8L, SeekOrigin.Begin);
			int num = binaryReader.ReadInt32();
			binaryReader.BaseStream.Seek(24L, SeekOrigin.Current);
			this.boneCount = binaryReader.ReadInt16();
			this.boneLookUp = new short[(int)this.boneCount];
			for (int i = 0; i < (int)this.boneCount; i++)
			{
				this.boneLookUp[i] = binaryReader.ReadInt16();
			}
			this.vertCount = binaryReader.ReadInt16();
			this.vertex = new ObjectVertex[(int)this.vertCount];
			if ((num & 2) > 0)
			{
				for (int i = 0; i < (int)this.vertCount; i++)
				{
					this.vertex[i].Position = RoseFile.ReadVector3(ref binaryReader);
				}
			}
			if ((num & 4) > 0)
			{
				binaryReader.BaseStream.Seek((long)(12 * this.vertCount), SeekOrigin.Current);
			}
			if ((num & 8) > 0)
			{
				binaryReader.BaseStream.Seek((long)(4 * this.vertCount), SeekOrigin.Current);
			}
			if ((num & 16) > 0 && (num & 32) > 0)
			{
				this.vertexWeight = new ZMS.Weight[(int)this.vertCount];
				this.vertexBoneID = new ZMS.BoneID[(int)this.vertCount];
				for (int i = 0; i < (int)this.vertCount; i++)
				{
					this.vertexWeight[i].Weight1 = binaryReader.ReadSingle();
					this.vertexWeight[i].Weight2 = binaryReader.ReadSingle();
					this.vertexWeight[i].Weight3 = binaryReader.ReadSingle();
					this.vertexWeight[i].Weight4 = binaryReader.ReadSingle();
					this.vertexBoneID[i].BoneId1 = binaryReader.ReadInt16();
					this.vertexBoneID[i].BoneId2 = binaryReader.ReadInt16();
					this.vertexBoneID[i].BoneId3 = binaryReader.ReadInt16();
					this.vertexBoneID[i].BoneId4 = binaryReader.ReadInt16();
				}
			}
			if ((num & 64) > 0)
			{
				binaryReader.BaseStream.Seek((long)(12 * this.vertCount), SeekOrigin.Current);
			}
			if ((num & 128) > 0)
			{
				for (int i = 0; i < (int)this.vertCount; i++)
				{
					this.vertex[i].TextureCoordinate = RoseFile.ReadVector2(ref binaryReader);
				}
			}
			if ((num & 256) > 0)
			{
				binaryReader.BaseStream.Seek((long)(8 * this.vertCount), SeekOrigin.Current);
			}
			if ((num & 512) > 0)
			{
			}
			if ((num & 1024) > 0)
			{
			}
			this.faceCount = binaryReader.ReadInt16();
			this.indices = new short[(int)(this.faceCount * 3)];
			for (int i = 0; i < (int)this.faceCount; i++)
			{
				this.indices[i * 3] = binaryReader.ReadInt16();
				this.indices[i * 3 + 1] = binaryReader.ReadInt16();
				this.indices[i * 3 + 2] = binaryReader.ReadInt16();
			}
			this.vertexBuffer = new VertexBuffer(this.graphics, (int)(20 * this.vertCount), BufferUsage.WriteOnly);
			this.vertexBuffer.SetData<ObjectVertex>(this.vertex);
			this.indexBuffer = new IndexBuffer(this.graphics, typeof(short), this.indices.Length, BufferUsage.None);
			this.indexBuffer.SetData<short>(this.indices);
			binaryReader.Close();
		}

		public void LoadTexture(string clientPath, GraphicsDevice graphics, ZSC.Materiel Materiel)
		{
			this.texture = ContentManager.Instance().GetTexture(Materiel.path);
			this.Materiel = Materiel;
		}

		public void ApplyZSCData(ZSC.Object objectData, int index)
		{
			objectData.list_mesh[index].world = Matrix.Identity;
			if (objectData.list_mesh[index].rotation_enabled)
			{
				objectData.list_mesh[index].world *= Matrix.CreateFromQuaternion(objectData.list_mesh[index].rotation);
			}
			if (objectData.list_mesh[index].scale_enabled)
			{
				objectData.list_mesh[index].world *= Matrix.CreateScale(objectData.list_mesh[index].scale);
			}
			if (objectData.list_mesh[index].position_enabled)
			{
				Vector3 value = Vector3.Transform(objectData.list_mesh[index].position, ZMS.roseCoordinate);
				value.X *= -1f;
				objectData.list_mesh[index].world *= Matrix.CreateTranslation(value / 100f);
			}
			int parent = (int)objectData.list_mesh[index].parent;
			if (objectData.list_mesh[index].parent_enabled && parent >= 0)
			{
			}
			for (int i = 0; i < this.vertex.Length; i++)
			{
				this.vertex[i].Position = Vector3.Transform(this.vertex[i].Position, objectData.list_mesh[index].world);
			}
			if (objectData.list_mesh[index].bone_index_enabled)
			{
				this.bindBoneIndex = new int?((int)objectData.list_mesh[index].bone_index);
			}
			if (objectData.list_mesh[index].dummy_index_enabled)
			{
				this.bindDummyIndex = new int?((int)objectData.list_mesh[index].dummy_index);
			}
			this.vertexBuffer.SetData<ObjectVertex>(this.vertex);
		}

		public void SetBindBoneIndex(int index)
		{
			this.bindBoneIndex = new int?(index);
		}

		public int? GetBindBoneIndex()
		{
			return this.bindBoneIndex;
		}

		public void SetBindDummyIndex(int index)
		{
			this.bindDummyIndex = new int?(index);
		}

		public int? GetBindDummyIndex()
		{
			return this.bindDummyIndex;
		}

		public void ApplyTransformation(ref Matrix matrixTransformation)
		{
			for (int i = 0; i < this.vertex.Length; i++)
			{
				this.vertex[i].Position = Vector3.Transform(this.vertex[i].Position, matrixTransformation);
			}
			this.vertexBuffer.SetData<ObjectVertex>(this.vertex);
		}

		public void Draw(GraphicsDevice graphics, Effect effect)
		{
			graphics.Vertices[0].SetSource(this.vertexBuffer, 0, 20);
			graphics.Indices = this.indexBuffer;
			effect.Parameters["zmsTexture"].SetValue(this.texture);
			for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
			{
				effect.CurrentTechnique.Passes[i].Begin();
				graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this.vertex.Length, 0, this.indices.Length / 3);
				effect.CurrentTechnique.Passes[i].End();
			}
		}
	}
}
