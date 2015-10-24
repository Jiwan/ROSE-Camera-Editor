using Arua_Camera_Editor.Common.FileHandler;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace Arua_Camera_Editor.Common
{
	public class ContentManager
	{
		public enum ZoneSTBColumn
		{
			COLUMN_ZON_PATH = 2,
			COLUMN_START_X = 10,
			COLUMN_START_Y,
			DECORATION_ZSC,
			BUILDING_ZSC
		}

		public static string rootPath;

		private static ContentManager currentInstance;

		private static GraphicsDevice device;

		private Hashtable STBs;

		private Hashtable STLs;

		private Hashtable ZSCs;

		private Hashtable CHRs;

		private Hashtable HIMs;

		private Hashtable ZONs;

		private Hashtable LITs;

		private Hashtable TILs;

		private Hashtable Textures;

		private Hashtable IFOs;

		public ContentManager()
		{
			this.STBs = new Hashtable();
			this.STLs = new Hashtable();
			this.ZSCs = new Hashtable();
			this.CHRs = new Hashtable();
			this.HIMs = new Hashtable();
			this.ZONs = new Hashtable();
			this.LITs = new Hashtable();
			this.TILs = new Hashtable();
			this.Textures = new Hashtable();
			this.IFOs = new Hashtable();
		}

		public static ContentManager Instance()
		{
			if (ContentManager.currentInstance == null)
			{
				ContentManager.currentInstance = new ContentManager();
			}
			return ContentManager.currentInstance;
		}

		public STL GetSTL(string name)
		{
			STL result;
			if (!this.STLs.Contains(name))
			{
				STL sTL = new STL();
				sTL.Load(ContentManager.rootPath + name, ClientType.IROSE);
				this.STLs.Add(name, sTL);
				result = sTL;
			}
			else
			{
				result = (STL)this.STLs[name];
			}
			return result;
		}

		public STB GetSTB(string name)
		{
			STB result;
			if (!this.STBs.Contains(name))
			{
				STB sTB = new STB();
				sTB.Load(ContentManager.rootPath + name, ClientType.IROSE);
				this.STBs.Add(name, sTB);
				result = sTB;
			}
			else
			{
				result = (STB)this.STBs[name];
			}
			return result;
		}

		public ZSC GetZSC(string name)
		{
			ZSC result;
			if (!this.ZSCs.Contains(name))
			{
				ZSC zSC = new ZSC();
				zSC.Load(ContentManager.rootPath + name, ClientType.IROSE);
				this.ZSCs.Add(name, zSC);
				result = zSC;
			}
			else
			{
				result = (ZSC)this.ZSCs[name];
			}
			return result;
		}

		public HIM GetHIM(string name)
		{
			return this.GetHIM(name, ContentManager.rootPath);
		}

		public HIM GetHIM(string name, string folderPath)
		{
			HIM result;
			if (!this.HIMs.Contains(name))
			{
				HIM hIM = new HIM();
				hIM.Load(folderPath + name, ClientType.IROSE);
				this.HIMs.Add(name, hIM);
				result = hIM;
			}
			else
			{
				result = (HIM)this.HIMs[name];
			}
			return result;
		}

		public ZON GetZON(string name)
		{
			return this.GetZON(name, ContentManager.rootPath);
		}

		public ZON GetZON(string name, string folderPath)
		{
			ZON result;
			if (!this.ZONs.Contains(name))
			{
				ZON zON = new ZON();
				zON.Load(folderPath + name, ClientType.IROSE);
				this.ZONs.Add(name, zON);
				result = zON;
			}
			else
			{
				result = (ZON)this.ZONs[name];
			}
			return result;
		}

		public LIT GetLIT(string name)
		{
			return this.GetLIT(name, ContentManager.rootPath);
		}

		public LIT GetLIT(string name, string folderPath)
		{
			LIT result;
			if (!this.LITs.Contains(name))
			{
				LIT lIT = new LIT();
				lIT.Load(folderPath + name, ClientType.IROSE);
				this.LITs.Add(name, lIT);
				result = lIT;
			}
			else
			{
				result = (LIT)this.LITs[name];
			}
			return result;
		}

		public TIL GetTIL(string name)
		{
			return this.GetTIL(name, ContentManager.rootPath);
		}

		public TIL GetTIL(string name, string folderPath)
		{
			TIL result;
			if (!this.TILs.Contains(name))
			{
				TIL tIL = new TIL();
				tIL.Load(folderPath + name, ClientType.IROSE);
				this.TILs.Add(name, tIL);
				result = tIL;
			}
			else
			{
				result = (TIL)this.TILs[name];
			}
			return result;
		}

		public Texture2D GetTexture(string name)
		{
			Texture2D result;
			if (!this.Textures.Contains(name))
			{
				Texture2D texture2D = Texture2D.FromFile(ContentManager.device, ContentManager.rootPath + name);
				this.Textures.Add(name, texture2D);
				result = texture2D;
			}
			else
			{
				result = (Texture2D)this.Textures[name];
			}
			return result;
		}

		public IFO GetIFO(string name)
		{
			return this.GetIFO(name, ContentManager.rootPath);
		}

		public IFO GetIFO(string name, string folderPath)
		{
			IFO result;
			if (!this.IFOs.Contains(name))
			{
				IFO iFO = new IFO();
				iFO.Load(folderPath + name, ClientType.IROSE);
				this.IFOs.Add(name, iFO);
				result = iFO;
			}
			else
			{
				result = (IFO)this.IFOs[name];
			}
			return result;
		}

		public void ClearMapData()
		{
			this.LITs.Clear();
			this.TILs.Clear();
			this.Textures.Clear();
			this.ZONs.Clear();
			this.HIMs.Clear();
			this.ZSCs.Clear();
			this.IFOs.Clear();
		}

		public static void SetRootPath(string path)
		{
			ContentManager.rootPath = path;
		}

		public static string GetRootPath()
		{
			return ContentManager.rootPath;
		}

		public static void SetGraphicsDevice(GraphicsDevice gdevice)
		{
			ContentManager.device = gdevice;
		}
	}
}
