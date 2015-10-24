using Arua_Camera_Editor.Common.FileHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Arua_Camera_Editor.Common
{
	public class Entity
	{
		public List<ZMS> listZMS;

		private ZMD Skeleton = new ZMD();

		public Entity()
		{
			this.listZMS = new List<ZMS>();
		}

		public void AddPart(ZMS part)
		{
			this.listZMS.Add(part);
		}

		public void Draw(GraphicsDevice graphicsDevice, Effect effect)
		{
			for (int i = 0; i < this.listZMS.Count; i++)
			{
				this.listZMS[i].Draw(graphicsDevice, effect);
			}
		}

		public void ApplyTransformation(Matrix transformation)
		{
			for (int i = 0; i < this.listZMS.Count; i++)
			{
				this.listZMS[i].ApplyTransformation(ref transformation);
			}
		}

		public void SetSkeleton(string path)
		{
			this.Skeleton = new ZMD();
			this.Skeleton.Load(path, ClientType.IROSE);
			for (int i = 0; i < this.listZMS.Count; i++)
			{
				if (this.listZMS[i].GetBindBoneIndex().HasValue)
				{
					for (int j = 0; j < (int)this.listZMS[i].vertCount; j++)
					{
						this.listZMS[i].vertex[j].Position = Vector3.Transform(this.listZMS[i].vertex[j].Position, this.Skeleton.listBone[this.listZMS[i].GetBindBoneIndex().Value].matrix);
					}
				}
				if (this.listZMS[i].GetBindDummyIndex().HasValue)
				{
					for (int j = 0; j < (int)this.listZMS[i].vertCount; j++)
					{
						this.listZMS[i].vertex[j].Position = Vector3.Transform(this.listZMS[i].vertex[j].Position, this.Skeleton.listBone[this.listZMS[i].GetBindDummyIndex().Value + this.Skeleton.DummyOffset].matrix);
					}
				}
			}
		}
	}
}
