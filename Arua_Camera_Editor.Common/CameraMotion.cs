using Arua_Camera_Editor.Common.FileHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Arua_Camera_Editor.Common
{
	public class CameraMotion : IReadable, ISavable, ICamera
	{
		public class Frame
		{
			[Category("Camera position :"), Description("Camera position coordinates")]
			public Vector3 cameraPosition
			{
				get;
				set;
			}

			[Category("Look at :"), Description("Coordinates that represent where the camera is looking at")]
			public Vector3 LookAt
			{
				get;
				set;
			}

			[Category("Vector up :"), Description("Vector up")]
			public Vector3 Up
			{
				get;
				set;
			}
		}

		public class Curve3D
		{
			public Curve curveX = new Curve();

			public Curve curveY = new Curve();

			public Curve curveZ = new Curve();

			public Curve3D()
			{
				this.curveX.PostLoop = CurveLoopType.Oscillate;
				this.curveY.PostLoop = CurveLoopType.Oscillate;
				this.curveZ.PostLoop = CurveLoopType.Oscillate;
				this.curveX.PreLoop = CurveLoopType.Oscillate;
				this.curveY.PreLoop = CurveLoopType.Oscillate;
				this.curveZ.PreLoop = CurveLoopType.Oscillate;
			}

			public void SetTangents()
			{
				for (int i = 0; i < this.curveX.Keys.Count; i++)
				{
					int num = i - 1;
					if (num < 0)
					{
						num = i;
					}
					int num2 = i + 1;
					if (num2 == this.curveX.Keys.Count)
					{
						num2 = i;
					}
					CurveKey curveKey = this.curveX.Keys[num];
					CurveKey curveKey2 = this.curveX.Keys[num2];
					CurveKey value = this.curveX.Keys[i];
					CameraMotion.Curve3D.SetCurveKeyTangent(ref curveKey, ref value, ref curveKey2);
					this.curveX.Keys[i] = value;
					curveKey = this.curveY.Keys[num];
					curveKey2 = this.curveY.Keys[num2];
					value = this.curveY.Keys[i];
					CameraMotion.Curve3D.SetCurveKeyTangent(ref curveKey, ref value, ref curveKey2);
					this.curveY.Keys[i] = value;
					curveKey = this.curveZ.Keys[num];
					curveKey2 = this.curveZ.Keys[num2];
					value = this.curveZ.Keys[i];
					CameraMotion.Curve3D.SetCurveKeyTangent(ref curveKey, ref value, ref curveKey2);
					this.curveZ.Keys[i] = value;
				}
			}

			private static void SetCurveKeyTangent(ref CurveKey prev, ref CurveKey cur, ref CurveKey next)
			{
				float num = next.Position - prev.Position;
				float num2 = next.Value - prev.Value;
				if (Math.Abs(num2) < 1.401298E-45f)
				{
					cur.TangentIn = 0f;
					cur.TangentOut = 0f;
				}
				else
				{
					cur.TangentIn = num2 * (cur.Position - prev.Position) / num;
					cur.TangentOut = num2 * (next.Position - cur.Position) / num;
				}
			}

			public void AddKey(Vector3 key, float time)
			{
				this.curveX.Keys.Add(new CurveKey(time, key.X));
				this.curveY.Keys.Add(new CurveKey(time, key.Y));
				this.curveZ.Keys.Add(new CurveKey(time, key.Z));
			}

			public Vector3 GetPointOnCurve(float time)
			{
				return new Vector3
				{
					X = this.curveX.Evaluate(time),
					Y = this.curveY.Evaluate(time),
					Z = this.curveZ.Evaluate(time)
				};
			}
		}

		private static Vector3 realPosition = new Vector3(5200f, 5200f, 0f);

		private static Matrix roseCoordinate = Matrix.CreateRotationX(1.57079637f) * Matrix.CreateRotationZ(3.14159274f);

		private GraphicsDevice graphics;

		private int currentFrameIndex;

		private bool pause;

		private float time;

		private VertexPositionColor[] vertices;

		private short[] indices;

		private List<CameraMotion.Frame> frameList;

		private Matrix[] viewMatrices;

		public ZMO motionFile
		{
			get;
			set;
		}

		public CameraMotion(GraphicsDevice graphics)
		{
			this.frameList = new List<CameraMotion.Frame>();
			this.currentFrameIndex = 0;
			this.graphics = graphics;
			this.pause = true;
		}

		public void Load(string mypath, ClientType myclientType)
		{
			this.motionFile = new ZMO();
			this.motionFile.Load(mypath, myclientType);
			if (!this.motionFile.IsCameraMotion())
			{
				throw new Exception("This ZMO isn't a cameraMotion");
			}
			this.GenerateFrames();
			this.GenerateViewMatrices();
			this.GenerateVertices();
		}

		public void GenerateVertices()
		{
			this.vertices = new VertexPositionColor[this.frameList.Count];
			this.indices = new short[this.frameList.Count];
			for (int i = 0; i < this.frameList.Count; i++)
			{
				this.vertices[i] = new VertexPositionColor(this.frameList[i].cameraPosition, Color.Red);
				this.indices[i] = (short)i;
			}
		}

		public void GenerateFrames()
		{
			this.frameList = new List<CameraMotion.Frame>();
			for (int i = 0; i < this.motionFile.frameCount; i++)
			{
				CameraMotion.Frame item = new CameraMotion.Frame
				{
					cameraPosition = this.motionFile.listChannel[0].position[i] / 100f + CameraMotion.realPosition,
					LookAt = this.motionFile.listChannel[1].position[i] / 100f + CameraMotion.realPosition,
					Up = this.motionFile.listChannel[2].position[i] / 100f
				};
				this.frameList.Add(item);
			}
		}

		public void Save()
		{
			string path = this.motionFile.Path;
			this.motionFile = new ZMO();
			this.motionFile.channelCount = 4;
			this.motionFile.listChannel = new List<ZMO.Channel>(4);
			this.motionFile.FPS = 30;
			this.motionFile.frameCount = this.frameList.Count;
			for (int i = 0; i < 4; i++)
			{
				this.motionFile.listChannel.Add(new ZMO.Channel
				{
					trackType = ZMO.TrackType.TRACK_TYPE_POSITION,
					trackID = i,
					position = new List<Vector3>(this.frameList.Count)
				});
			}
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < this.frameList.Count; j++)
				{
					if (i == 0)
					{
						this.motionFile.listChannel[i].position.Add((this.frameList[j].cameraPosition - CameraMotion.realPosition) * 100f);
					}
					else if (i == 1)
					{
						this.motionFile.listChannel[i].position.Add((this.frameList[j].LookAt - CameraMotion.realPosition) * 100f);
					}
					else if (i == 2)
					{
						this.motionFile.listChannel[i].position.Add(this.frameList[j].Up * 100f);
					}
					else if (i == 3)
					{
						this.motionFile.listChannel[i].position.Add(new Vector3(45f, 100f, 130000f));
					}
				}
			}
			this.motionFile.Save(path);
		}

		public void AddFrame(CameraMotion.Frame frame)
		{
			if (this.motionFile != null)
			{
				this.motionFile.listChannel[0].normal.Add(frame.cameraPosition);
				this.motionFile.listChannel[1].normal.Add(frame.LookAt);
				this.motionFile.listChannel[2].normal.Add(frame.Up);
				this.motionFile.listChannel[3].normal.Add(new Vector3(0f, 0f, 0f));
				this.motionFile.frameCount++;
			}
		}

		public void DeleteFrame(int indice)
		{
			this.motionFile.listChannel[0].normal.RemoveAt(indice);
			this.motionFile.listChannel[1].normal.RemoveAt(indice);
			this.motionFile.listChannel[2].normal.RemoveAt(indice);
			this.motionFile.listChannel[3].normal.RemoveAt(indice);
			this.motionFile.frameCount--;
		}

		public CameraMotion.Frame GetFrame(int indice)
		{
			return this.frameList[indice];
		}

		public void ChangeFrame(CameraMotion.Frame newFrame, int indice)
		{
			this.motionFile.listChannel[0].normal[indice] = newFrame.cameraPosition;
			this.motionFile.listChannel[1].normal[indice] = newFrame.LookAt;
			this.motionFile.listChannel[2].normal[indice] = newFrame.Up;
		}

		public List<CameraMotion.Frame> GetFrames()
		{
			return this.frameList;
		}

		public void Reset()
		{
			this.Stop();
			this.currentFrameIndex = 0;
		}

		public void Start()
		{
			this.pause = false;
		}

		public void Stop()
		{
			this.pause = true;
		}

		public int GetFPS()
		{
			return this.motionFile.FPS;
		}

		public void SetFPS(int fps)
		{
			this.motionFile.FPS = fps;
		}

		public void SetCurrentFrame(int frameIndex)
		{
			this.currentFrameIndex = frameIndex;
		}

		public int GetCurrentFrameIndex()
		{
			return this.currentFrameIndex;
		}

		public void GenerateViewMatrices()
		{
			this.viewMatrices = new Matrix[this.frameList.Count];
			int index = 0;
			this.frameList.ForEach(delegate(CameraMotion.Frame f)
			{
				this.viewMatrices[index] = Matrix.CreateLookAt(f.cameraPosition, f.LookAt, f.Up);
				index++;
			});
		}

		public Matrix GetView()
		{
			return this.viewMatrices[this.currentFrameIndex];
		}

		public void Update(float time)
		{
			if (!this.pause)
			{
				if (this.currentFrameIndex != this.frameList.Count - 1)
				{
					if (time - this.time > 1f / (float)this.motionFile.FPS)
					{
						this.currentFrameIndex++;
						this.time = time;
					}
				}
			}
		}

		public void Draw(BasicEffect effect)
		{
			effect.Begin();
			foreach (EffectPass current in effect.CurrentTechnique.Passes)
			{
				current.Begin();
				this.graphics.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, this.vertices, 0, this.vertices.Length, this.indices, 0, this.vertices.Length - 1);
				current.End();
			}
			effect.End();
		}

		public void Interpolate(int min, int max, int step)
		{
			if (min >= 0 && max < this.frameList.Count && min <= max)
			{
				float num = 0f;
				CameraMotion.Curve3D curve3D = new CameraMotion.Curve3D();
				CameraMotion.Curve3D curve3D2 = new CameraMotion.Curve3D();
				CameraMotion.Curve3D curve3D3 = new CameraMotion.Curve3D();
				int i = min;
				while (i < max + 1)
				{
					curve3D.AddKey(this.frameList[i].cameraPosition, num);
					curve3D2.AddKey(this.frameList[i].LookAt, num);
					curve3D3.AddKey(this.frameList[i].Up, num);
					i++;
					num += (float)step;
				}
				curve3D.SetTangents();
				curve3D2.SetTangents();
				curve3D3.SetTangents();
				List<CameraMotion.Frame> list = new List<CameraMotion.Frame>();
				for (i = 0; i < (max - min) * step; i++)
				{
					CameraMotion.Frame item = new CameraMotion.Frame
					{
						cameraPosition = curve3D.GetPointOnCurve((float)i),
						LookAt = curve3D2.GetPointOnCurve((float)i),
						Up = curve3D3.GetPointOnCurve((float)i)
					};
					list.Add(item);
				}
				this.frameList.RemoveRange(min, max - min);
				this.frameList.InsertRange(min, list);
				this.GenerateViewMatrices();
				this.GenerateVertices();
			}
		}
	}
}
