using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Arua_Camera_Editor.Common.FileHandler
{
	public class ZMO : IReadable, ISavable
	{
		public enum TrackType
		{
			TRACK_TYPE_POSITION = 2,
			TRACK_TYPE_ROTATION = 4,
			TRACK_TYPE_NORMAL = 8,
			TRACK_TYPE_ALPHA = 16,
			TRACK_TYPE_UV1 = 32,
			TRACK_TYPE_UV2 = 64,
			TRACK_TYPE_UV3 = 128,
			TRACK_TYPE_UV4 = 256,
			TRACK_TYPE_TEXTUREANIM = 512,
			TRACK_TYPE_SCALE = 1024
		}

		public class Channel
		{
			public int trackID;

			public List<Vector3> position;

			public List<Vector4> rotation;

			public List<Vector3> normal;

			public List<float> alpha;

			public List<Vector2> uv1;

			public List<Vector2> uv2;

			public List<Vector2> uv3;

			public List<Vector2> uv4;

			public List<float> textureAnim;

			public List<float> scale;

			public ZMO.TrackType trackType
			{
				get;
				set;
			}
		}

		private static byte[] unknowData = new byte[]
		{
			74,
			49,
			87,
			52,
			110
		};

		private string path;

		public string Path
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

		public int FPS
		{
			get;
			set;
		}

		public int frameCount
		{
			get;
			set;
		}

		public int channelCount
		{
			get;
			set;
		}

		public List<ZMO.Channel> listChannel
		{
			get;
			set;
		}

		public ZMO()
		{
			this.FPS = 0;
			this.frameCount = 0;
			this.channelCount = 0;
			this.listChannel = new List<ZMO.Channel>();
		}

		public void Load(string mypath, ClientType myclientType)
		{
			this.path = mypath;
			BinaryReader binaryReader = new BinaryReader(File.Open(mypath, FileMode.Open));
			string text = RoseFile.ReadFString(ref binaryReader, 8);
			if (!text.Equals("ZMO0002\0"))
			{
				throw new Exception("wrong file header");
			}
			this.FPS = binaryReader.ReadInt32();
			this.frameCount = binaryReader.ReadInt32();
			this.channelCount = binaryReader.ReadInt32();
			for (int i = 0; i < this.channelCount; i++)
			{
				ZMO.Channel channel = new ZMO.Channel();
				channel.trackType = (ZMO.TrackType)binaryReader.ReadInt32();
				channel.trackID = binaryReader.ReadInt32();
				if (channel.trackType == ZMO.TrackType.TRACK_TYPE_NORMAL)
				{
					channel.normal = new List<Vector3>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_ROTATION)
				{
					channel.rotation = new List<Vector4>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_POSITION)
				{
					channel.position = new List<Vector3>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_ALPHA)
				{
					channel.alpha = new List<float>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_UV1)
				{
					channel.uv1 = new List<Vector2>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_UV2)
				{
					channel.uv2 = new List<Vector2>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_UV3)
				{
					channel.uv3 = new List<Vector2>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_UV4)
				{
					channel.uv4 = new List<Vector2>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_TEXTUREANIM)
				{
					channel.textureAnim = new List<float>();
				}
				else if (channel.trackType == ZMO.TrackType.TRACK_TYPE_SCALE)
				{
					channel.scale = new List<float>();
				}
				this.listChannel.Add(channel);
			}
			for (int i = 0; i < this.frameCount; i++)
			{
				for (int j = 0; j < this.channelCount; j++)
				{
					if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_NORMAL)
					{
						this.listChannel[j].normal[i] = RoseFile.ReadVector3(ref binaryReader);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_ROTATION)
					{
						this.listChannel[j].rotation[i] = RoseFile.ReadVector4(ref binaryReader);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_POSITION)
					{
						this.listChannel[j].position.Add(RoseFile.ReadVector3(ref binaryReader));
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_ALPHA)
					{
						this.listChannel[j].alpha[i] = binaryReader.ReadSingle();
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_UV1)
					{
						this.listChannel[j].uv1[i] = RoseFile.ReadVector2(ref binaryReader);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_UV2)
					{
						this.listChannel[j].uv2[i] = RoseFile.ReadVector2(ref binaryReader);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_UV3)
					{
						this.listChannel[j].uv3[i] = RoseFile.ReadVector2(ref binaryReader);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_UV4)
					{
						this.listChannel[j].uv4[i] = RoseFile.ReadVector2(ref binaryReader);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_TEXTUREANIM)
					{
						this.listChannel[j].textureAnim[i] = binaryReader.ReadSingle();
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_SCALE)
					{
						this.listChannel[j].scale[i] = binaryReader.ReadSingle();
					}
				}
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
			RoseFile.WriteFString(ref binaryWriter, "ZMO0002", 8);
			binaryWriter.Write(this.FPS);
			binaryWriter.Write(this.frameCount);
			binaryWriter.Write(this.channelCount);
			for (int i = 0; i < this.channelCount; i++)
			{
				binaryWriter.Write((int)this.listChannel[i].trackType);
				binaryWriter.Write(this.listChannel[i].trackID);
			}
			for (int i = 0; i < this.frameCount; i++)
			{
				for (int j = 0; j < this.channelCount; j++)
				{
					if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_NORMAL)
					{
						RoseFile.WriteVector3(ref binaryWriter, this.listChannel[j].normal[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_ROTATION)
					{
						RoseFile.WriteVector4(ref binaryWriter, this.listChannel[j].rotation[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_POSITION)
					{
						RoseFile.WriteVector3(ref binaryWriter, this.listChannel[j].position[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_ALPHA)
					{
						binaryWriter.Write(this.listChannel[j].alpha[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_UV1)
					{
						RoseFile.WriteVector2(ref binaryWriter, this.listChannel[j].uv1[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_UV2)
					{
						RoseFile.WriteVector2(ref binaryWriter, this.listChannel[j].uv1[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_UV3)
					{
						RoseFile.WriteVector2(ref binaryWriter, this.listChannel[j].uv1[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_UV4)
					{
						RoseFile.WriteVector2(ref binaryWriter, this.listChannel[j].uv1[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_TEXTUREANIM)
					{
						binaryWriter.Write(this.listChannel[j].textureAnim[i]);
					}
					else if (this.listChannel[j].trackType == ZMO.TrackType.TRACK_TYPE_SCALE)
					{
						binaryWriter.Write(this.listChannel[j].scale[i]);
					}
				}
			}
			binaryWriter.Write(ZMO.unknowData);
			binaryWriter.Close();
		}

		public bool IsCameraMotion()
		{
			return this.channelCount == 4 && (this.listChannel[0].trackType == ZMO.TrackType.TRACK_TYPE_POSITION && this.listChannel[1].trackType == ZMO.TrackType.TRACK_TYPE_POSITION && this.listChannel[2].trackType == ZMO.TrackType.TRACK_TYPE_POSITION) && this.listChannel[3].trackType == ZMO.TrackType.TRACK_TYPE_POSITION;
		}
	}
}
