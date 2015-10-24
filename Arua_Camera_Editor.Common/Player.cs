using Arua_Camera_Editor.Common.FileHandler;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common
{
	public class Player : Entity
	{
		private ZSC LIST_BACK_ZSC = new ZSC();

		private ZSC LIST_FACEIEM_ZSC = new ZSC();

		private ZSC LIST_MARMS_ZSC = new ZSC();

		private ZSC LIST_MBODY_ZSC = new ZSC();

		private ZSC LIST_MCAP_ZSC = new ZSC();

		private ZSC LIST_MFACE_ZSC = new ZSC();

		private ZSC LIST_MFOOT_ZSC = new ZSC();

		private ZSC LIST_MHAIR_ZSC = new ZSC();

		private ZSC LIST_SUBWPN_ZSC = new ZSC();

		private ZSC LIST_WARMS_ZSC = new ZSC();

		private ZSC LIST_WBODY_ZSC = new ZSC();

		private ZSC LIST_WCAP_ZSC = new ZSC();

		private ZSC LIST_WEAPON_ZSC = new ZSC();

		private ZSC LIST_WFACE_ZSC = new ZSC();

		private ZSC LIST_WFOOT_ZSC = new ZSC();

		private ZSC LIST_WHAIR_ZSC = new ZSC();

		private STB LIST_CAP_STB = new STB();

		private int backIndex;

		private int faceitemIndex;

		private int armsIndex;

		private int bodyIndex;

		private int capIndex;

		private int faceIndex;

		private int footIndex;

		private int hairIndex;

		private int subwpnIndex;

		private int weaponIndex;

		public Player(GraphicsDevice graphicsDevice, string ClientPath, bool sex, int backIndex, int faceitemIndex, int armsIndex, int bodyIndex, int capIndex, int faceIndex, int footIndex, int hairIndex, int subwpnIndex, int weaponIndex)
		{
			this.LIST_BACK_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_BACK.ZSC");
			this.LIST_FACEIEM_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_FACEIEM.ZSC");
			this.LIST_SUBWPN_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_SUBWPN.ZSC");
			this.LIST_WEAPON_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_WEAPON.ZSC");
			if (sex)
			{
				this.LIST_MARMS_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_MARMS.ZSC");
				this.LIST_MBODY_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_MBODY.ZSC");
				this.LIST_MCAP_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_MCAP.ZSC");
				this.LIST_MFACE_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_MFACE.ZSC");
				this.LIST_MFOOT_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_MFOOT.ZSC");
				this.LIST_MHAIR_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_MHAIR.ZSC");
			}
			else
			{
				this.LIST_WARMS_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_WARMS.ZSC");
				this.LIST_WBODY_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_WBODY.ZSC");
				this.LIST_WCAP_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_WCAP.ZSC");
				this.LIST_WFACE_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_WFACE.ZSC");
				this.LIST_WFOOT_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_WFOOT.ZSC");
				this.LIST_WHAIR_ZSC.Load(ClientPath + "3DDATA\\AVATAR\\LIST_WHAIR.ZSC");
			}
			if (capIndex != -1)
			{
				this.LIST_CAP_STB.Load(ClientPath + "3DDATA\\STB\\LIST_CAP.STB", ClientType.IROSE);
			}
			if (backIndex != -1)
			{
				for (int i = 0; i < this.LIST_BACK_ZSC.listObject[backIndex].list_mesh.Count; i++)
				{
					ZMS zMS = new ZMS(graphicsDevice);
					zMS.Load(ClientPath + this.LIST_BACK_ZSC.listMesh[(int)this.LIST_BACK_ZSC.listObject[backIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
					zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_BACK_ZSC.listMateriel[(int)this.LIST_BACK_ZSC.listObject[backIndex].list_mesh[i].material_id]);
					zMS.ApplyZSCData(this.LIST_BACK_ZSC.listObject[backIndex], i);
					zMS.SetBindDummyIndex(3);
					this.listZMS.Add(zMS);
				}
			}
			if (faceitemIndex != -1)
			{
				for (int i = 0; i < this.LIST_FACEIEM_ZSC.listObject[faceitemIndex].list_mesh.Count; i++)
				{
					ZMS zMS = new ZMS(graphicsDevice);
					zMS.Load(ClientPath + this.LIST_FACEIEM_ZSC.listMesh[(int)this.LIST_FACEIEM_ZSC.listObject[faceitemIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
					zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_FACEIEM_ZSC.listMateriel[(int)this.LIST_FACEIEM_ZSC.listObject[faceitemIndex].list_mesh[i].material_id]);
					zMS.ApplyZSCData(this.LIST_FACEIEM_ZSC.listObject[faceitemIndex], i);
					zMS.SetBindDummyIndex(4);
					this.listZMS.Add(zMS);
				}
			}
			if (subwpnIndex != -1)
			{
				for (int i = 0; i < this.LIST_SUBWPN_ZSC.listObject[subwpnIndex].list_mesh.Count; i++)
				{
					ZMS zMS = new ZMS(graphicsDevice);
					zMS.Load(ClientPath + this.LIST_SUBWPN_ZSC.listMesh[(int)this.LIST_SUBWPN_ZSC.listObject[subwpnIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
					zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_SUBWPN_ZSC.listMateriel[(int)this.LIST_SUBWPN_ZSC.listObject[subwpnIndex].list_mesh[i].material_id]);
					zMS.ApplyZSCData(this.LIST_SUBWPN_ZSC.listObject[subwpnIndex], i);
					this.listZMS.Add(zMS);
				}
			}
			if (weaponIndex != -1)
			{
				for (int i = 0; i < this.LIST_WEAPON_ZSC.listObject[weaponIndex].list_mesh.Count; i++)
				{
					ZMS zMS = new ZMS(graphicsDevice);
					zMS.Load(ClientPath + this.LIST_WEAPON_ZSC.listMesh[(int)this.LIST_WEAPON_ZSC.listObject[weaponIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
					zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_WEAPON_ZSC.listMateriel[(int)this.LIST_WEAPON_ZSC.listObject[weaponIndex].list_mesh[i].material_id]);
					zMS.ApplyZSCData(this.LIST_WEAPON_ZSC.listObject[weaponIndex], i);
					this.listZMS.Add(zMS);
				}
			}
			if (sex)
			{
				if (armsIndex != -1)
				{
					for (int i = 0; i < this.LIST_MARMS_ZSC.listObject[armsIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_MARMS_ZSC.listMesh[(int)this.LIST_MARMS_ZSC.listObject[armsIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_MARMS_ZSC.listMateriel[(int)this.LIST_MARMS_ZSC.listObject[armsIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_MARMS_ZSC.listObject[armsIndex], i);
						this.listZMS.Add(zMS);
					}
				}
				if (bodyIndex != -1)
				{
					for (int i = 0; i < this.LIST_MBODY_ZSC.listObject[bodyIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_MBODY_ZSC.listMesh[(int)this.LIST_MBODY_ZSC.listObject[bodyIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_MBODY_ZSC.listMateriel[(int)this.LIST_MBODY_ZSC.listObject[bodyIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_MBODY_ZSC.listObject[bodyIndex], i);
						this.listZMS.Add(zMS);
					}
				}
				if (capIndex != -1)
				{
					for (int i = 0; i < this.LIST_MCAP_ZSC.listObject[capIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_MCAP_ZSC.listMesh[(int)this.LIST_MCAP_ZSC.listObject[capIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_MCAP_ZSC.listMateriel[(int)this.LIST_MCAP_ZSC.listObject[capIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_MCAP_ZSC.listObject[capIndex], i);
						zMS.SetBindDummyIndex(6);
						this.listZMS.Add(zMS);
					}
				}
				if (faceIndex != -1)
				{
					for (int i = 0; i < this.LIST_MFACE_ZSC.listObject[faceIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_MFACE_ZSC.listMesh[(int)this.LIST_MFACE_ZSC.listObject[faceIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_MFACE_ZSC.listMateriel[(int)this.LIST_MFACE_ZSC.listObject[faceIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_MFACE_ZSC.listObject[faceIndex], i);
						zMS.SetBindBoneIndex(4);
						this.listZMS.Add(zMS);
					}
				}
				if (footIndex != -1)
				{
					for (int i = 0; i < this.LIST_MFOOT_ZSC.listObject[footIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_MFOOT_ZSC.listMesh[(int)this.LIST_MFOOT_ZSC.listObject[footIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_MFOOT_ZSC.listMateriel[(int)this.LIST_MFOOT_ZSC.listObject[footIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_MFOOT_ZSC.listObject[footIndex], i);
						this.listZMS.Add(zMS);
					}
				}
				if (hairIndex != -1)
				{
					if (capIndex != -1 && this.LIST_CAP_STB.cell[capIndex, 34] != "" && this.LIST_CAP_STB.cell[capIndex, 34] != " ")
					{
						hairIndex += Convert.ToInt32(this.LIST_CAP_STB.cell[capIndex, 34]);
					}
					for (int i = 0; i < this.LIST_MHAIR_ZSC.listObject[hairIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_MHAIR_ZSC.listMesh[(int)this.LIST_MHAIR_ZSC.listObject[hairIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_MHAIR_ZSC.listMateriel[(int)this.LIST_MHAIR_ZSC.listObject[hairIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_MHAIR_ZSC.listObject[hairIndex], i);
						zMS.SetBindBoneIndex(4);
						this.listZMS.Add(zMS);
					}
				}
			}
			else if (!sex)
			{
				if (armsIndex != -1)
				{
					for (int i = 0; i < this.LIST_WARMS_ZSC.listObject[armsIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_WARMS_ZSC.listMesh[(int)this.LIST_WARMS_ZSC.listObject[armsIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_WARMS_ZSC.listMateriel[(int)this.LIST_WARMS_ZSC.listObject[armsIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_WARMS_ZSC.listObject[armsIndex], i);
						this.listZMS.Add(zMS);
					}
				}
				if (bodyIndex != -1)
				{
					for (int i = 0; i < this.LIST_WBODY_ZSC.listObject[bodyIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_WBODY_ZSC.listMesh[(int)this.LIST_WBODY_ZSC.listObject[bodyIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_WBODY_ZSC.listMateriel[(int)this.LIST_WBODY_ZSC.listObject[bodyIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_WBODY_ZSC.listObject[bodyIndex], i);
						this.listZMS.Add(zMS);
					}
				}
				if (capIndex != -1)
				{
					for (int i = 0; i < this.LIST_WCAP_ZSC.listObject[capIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_WCAP_ZSC.listMesh[(int)this.LIST_WCAP_ZSC.listObject[capIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_WCAP_ZSC.listMateriel[(int)this.LIST_WCAP_ZSC.listObject[capIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_WCAP_ZSC.listObject[capIndex], i);
						zMS.SetBindDummyIndex(6);
						this.listZMS.Add(zMS);
					}
				}
				if (faceIndex != -1)
				{
					for (int i = 0; i < this.LIST_WFACE_ZSC.listObject[faceIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_WFACE_ZSC.listMesh[(int)this.LIST_WFACE_ZSC.listObject[faceIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_WFACE_ZSC.listMateriel[(int)this.LIST_WFACE_ZSC.listObject[faceIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_WFACE_ZSC.listObject[faceIndex], i);
						zMS.SetBindBoneIndex(4);
						this.listZMS.Add(zMS);
					}
				}
				if (footIndex != -1)
				{
					for (int i = 0; i < this.LIST_WFOOT_ZSC.listObject[footIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_WFOOT_ZSC.listMesh[(int)this.LIST_WFOOT_ZSC.listObject[footIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_WFOOT_ZSC.listMateriel[(int)this.LIST_WFOOT_ZSC.listObject[footIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_WFOOT_ZSC.listObject[footIndex], i);
						this.listZMS.Add(zMS);
					}
				}
				if (hairIndex != -1)
				{
					if (capIndex != -1 && this.LIST_CAP_STB.cell[capIndex, 34] != "" && this.LIST_CAP_STB.cell[capIndex, 34] != " ")
					{
						hairIndex += Convert.ToInt32(this.LIST_CAP_STB.cell[capIndex, 34]);
					}
					for (int i = 0; i < this.LIST_WHAIR_ZSC.listObject[hairIndex].list_mesh.Count; i++)
					{
						ZMS zMS = new ZMS(graphicsDevice);
						zMS.Load(ClientPath + this.LIST_WHAIR_ZSC.listMesh[(int)this.LIST_WHAIR_ZSC.listObject[hairIndex].list_mesh[i].mesh_id].path, ClientType.IROSE);
						zMS.LoadTexture(ClientPath, graphicsDevice, this.LIST_WHAIR_ZSC.listMateriel[(int)this.LIST_WHAIR_ZSC.listObject[hairIndex].list_mesh[i].material_id]);
						zMS.ApplyZSCData(this.LIST_WHAIR_ZSC.listObject[hairIndex], i);
						zMS.SetBindBoneIndex(4);
						this.listZMS.Add(zMS);
					}
				}
			}
		}
	}
}
