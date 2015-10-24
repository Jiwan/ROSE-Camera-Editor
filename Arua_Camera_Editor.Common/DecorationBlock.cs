using Arua_Camera_Editor.Common.FileHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Arua_Camera_Editor.Common
{
	public class DecorationBlock
	{
		private IFO ifo;

		private List<Entity> listEntity;

		private ZSC decorationZSC;

		private ZSC buildingZSC;

		private GraphicsDevice graphics;

		public DecorationBlock(GraphicsDevice gdevice)
		{
			this.listEntity = new List<Entity>();
			this.graphics = gdevice;
		}

		public void Load(string ifoName, string mapFolder, ZSC dec, ZSC build, Vector2 mapPosition)
		{
			this.ifo = new IFO();
			this.ifo = ContentManager.Instance().GetIFO(ifoName, mapFolder);
			this.decorationZSC = dec;
			this.buildingZSC = build;
			for (int i = 0; i < this.ifo.listDecorationBlock.Count; i++)
			{
				for (int j = 0; j < this.ifo.listDecorationBlock[i].listDecoration.Count; j++)
				{
					int objectID = this.ifo.listDecorationBlock[i].listDecoration[j].objectID;
					Entity entity = new Entity();
					for (int k = 0; k < dec.listObject[objectID].list_mesh.Count; k++)
					{
						ZMS zMS = new ZMS(this.graphics);
						zMS.Load(ContentManager.GetRootPath() + dec.listMesh[(int)dec.listObject[objectID].list_mesh[k].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ContentManager.GetRootPath(), this.graphics, dec.listMateriel[(int)dec.listObject[objectID].list_mesh[k].material_id]);
						zMS.ApplyZSCData(dec.listObject[objectID], k);
						entity.AddPart(zMS);
					}
					Vector3 position = default(Vector3);
					position.X = this.ifo.listDecorationBlock[i].listDecoration[j].position.X / 100f + 5200f;
					position.Y = this.ifo.listDecorationBlock[i].listDecoration[j].position.Y / 100f + 5200f;
					position.Z = this.ifo.listDecorationBlock[i].listDecoration[j].position.Z / 100f;
					Matrix matrix = Matrix.Identity;
					matrix *= Matrix.CreateScale(this.ifo.listDecorationBlock[i].listDecoration[j].scale);
					matrix *= Matrix.CreateFromQuaternion(new Quaternion(this.ifo.listDecorationBlock[i].listDecoration[j].rotation.W, this.ifo.listDecorationBlock[i].listDecoration[j].rotation.X, this.ifo.listDecorationBlock[i].listDecoration[j].rotation.Y, this.ifo.listDecorationBlock[i].listDecoration[j].rotation.Z));
					matrix *= Matrix.CreateTranslation(position);
					entity.ApplyTransformation(matrix);
					this.listEntity.Add(entity);
				}
			}
			for (int j = 0; j < this.ifo.listBuildingBlock[0].listBuilding.Count; j++)
			{
				int objectID = this.ifo.listBuildingBlock[0].listBuilding[j].objectID;
				Entity entity = new Entity();
				for (int k = 0; k < build.listObject[objectID].list_mesh.Count; k++)
				{
					ZMS zMS = new ZMS(this.graphics);
					zMS.Load(ContentManager.GetRootPath() + build.listMesh[(int)build.listObject[objectID].list_mesh[k].mesh_id].path, ClientType.IROSE);
					zMS.LoadTexture(ContentManager.GetRootPath(), this.graphics, build.listMateriel[(int)build.listObject[objectID].list_mesh[k].material_id]);
					zMS.ApplyZSCData(build.listObject[objectID], k);
					entity.AddPart(zMS);
				}
				Vector3 position = default(Vector3);
				position.X = this.ifo.listBuildingBlock[0].listBuilding[j].position.X / 100f + 5200f;
				position.Y = this.ifo.listBuildingBlock[0].listBuilding[j].position.Y / 100f + 5200f;
				position.Z = this.ifo.listBuildingBlock[0].listBuilding[j].position.Z / 100f;
				Matrix matrix = Matrix.Identity;
				matrix *= Matrix.CreateScale(this.ifo.listBuildingBlock[0].listBuilding[j].scale);
				matrix *= Matrix.CreateFromQuaternion(new Quaternion(this.ifo.listBuildingBlock[0].listBuilding[j].rotation.W, this.ifo.listBuildingBlock[0].listBuilding[j].rotation.X, this.ifo.listBuildingBlock[0].listBuilding[j].rotation.Y, this.ifo.listBuildingBlock[0].listBuilding[j].rotation.Z));
				matrix *= Matrix.CreateTranslation(position);
				entity.ApplyTransformation(matrix);
				this.listEntity.Add(entity);
			}
		}

		public void Draw(Effect effect)
		{
			for (int i = 0; i < this.listEntity.Count; i++)
			{
				this.listEntity[i].Draw(this.graphics, effect);
			}
		}
	}
}
