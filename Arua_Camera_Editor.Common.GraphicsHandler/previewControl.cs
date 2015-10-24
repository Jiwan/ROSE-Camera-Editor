using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Arua_Camera_Editor.Common.GraphicsHandler
{
	public class previewControl : GraphicsDeviceControl
	{
		private enum CameraType
		{
			FREE_ROAM,
			MOTION
		}

		private Microsoft.Xna.Framework.Content.ContentManager XNAContent;

		private FPS fps;

		public Effect zmsEffect;

		public Effect terrainEffect;

		public BasicEffect motionEffect;

		private Stopwatch timer;

		public int zoom;

		private FreeRoamCamera camera;

		private CameraMotion cameraMotion;

		private Map renderMap;

		private bool drawingEnable;

		private previewControl.CameraType cameraType;

		protected override void Initialize()
		{
			this.cameraType = previewControl.CameraType.FREE_ROAM;
			Arua_Camera_Editor.Common.ContentManager.SetGraphicsDevice(base.GraphicsDevice);
			this.XNAContent = new Microsoft.Xna.Framework.Content.ContentManager(base.Services, "Content");
			this.zmsEffect = Shaders.CreateObjectEffect(base.GraphicsDevice);
			this.terrainEffect = Shaders.CreateTerrainEffect(base.GraphicsDevice);
			this.motionEffect = new BasicEffect(base.GraphicsDevice, null);
			this.zmsEffect.Parameters["World"].SetValue(Matrix.Identity);
			this.previewResize();
			this.camera = new FreeRoamCamera(new Vector2((float)(base.Width / 2), (float)(base.Height / 2)));
			this.timer = Stopwatch.StartNew();
			Application.Idle += delegate
			{
				base.Invalidate();
			};
			this.LoadContent();
		}

		public void previewResize()
		{
			float aspectRatio = base.GraphicsDevice.Viewport.AspectRatio;
			this.zmsEffect.Parameters["Projection"].SetValue(Matrix.CreatePerspectiveFieldOfView(0.7853982f, aspectRatio, 1f, 500f));
		}

		public void LoadContent()
		{
			this.fps = new FPS(base.GraphicsDevice, this.XNAContent);
		}

		public void UnloadContent()
		{
			this.XNAContent.Unload();
		}

		public new void Update()
		{
			float time = (float)this.timer.Elapsed.TotalSeconds;
			this.camera.Update(time);
			this.fps.Update(time);
			if (this.cameraType == previewControl.CameraType.FREE_ROAM)
			{
				this.zmsEffect.Parameters["View"].SetValue(this.camera.GetView());
			}
			else if (this.cameraType == previewControl.CameraType.MOTION)
			{
				this.cameraMotion.Update(time);
				this.zmsEffect.Parameters["View"].SetValue(this.cameraMotion.GetView());
			}
			this.motionEffect.World = this.zmsEffect.Parameters["World"].GetValueMatrix();
			this.motionEffect.View = this.zmsEffect.Parameters["View"].GetValueMatrix();
			this.motionEffect.Projection = this.zmsEffect.Parameters["Projection"].GetValueMatrix();
			this.terrainEffect.Parameters["WorldViewProjection"].SetValue(this.zmsEffect.Parameters["View"].GetValueMatrix() * this.zmsEffect.Parameters["World"].GetValueMatrix() * this.zmsEffect.Parameters["Projection"].GetValueMatrix());
		}

		protected override void Draw()
		{
			Form form = (Form)base.Parent;
			Vector3 freeRoamPosition = this.GetFreeRoamPosition();
			Matrix freeRoamMatrix = this.GetFreeRoamMatrix();
			Vector3 vector = freeRoamPosition - new Vector3(freeRoamMatrix.M13, freeRoamMatrix.M23, freeRoamMatrix.M33);
			Vector3 vector2 = new Vector3(freeRoamMatrix.M12, freeRoamMatrix.M22, freeRoamMatrix.M32);
			form.Text = string.Concat(new object[]
			{
				"Jiwan's Camera Editor : ",
				this.fps.GetFPS(),
				" Position: ",
				freeRoamPosition,
				"Look at: ",
				vector,
				"Up: ",
				vector2
			});
			base.GraphicsDevice.Clear(Color.White);
			this.Update();
			base.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
			if (this.renderMap != null && this.drawingEnable)
			{
				this.renderMap.Draw(base.GraphicsDevice, this.zmsEffect, this.terrainEffect);
			}
			if (this.cameraMotion != null)
			{
				this.cameraMotion.Draw(this.motionEffect);
			}
			this.fps.incrementeCounter();
		}

		public void DrawMap(Map map)
		{
			this.renderMap = map;
		}

		public void StopDraw()
		{
			this.drawingEnable = false;
		}

		public void StartDraw()
		{
			this.drawingEnable = true;
		}

		public void SetCameraMotion(CameraMotion camMotion)
		{
			this.cameraMotion = camMotion;
		}

		public void StartMotion()
		{
			if (this.cameraMotion != null)
			{
				this.cameraType = previewControl.CameraType.MOTION;
				this.cameraMotion.Start();
			}
		}

		public void StopMotion()
		{
			if (this.cameraMotion != null)
			{
				this.cameraMotion.Stop();
			}
		}

		public void ResetMotion()
		{
			if (this.cameraMotion != null)
			{
				this.cameraMotion.Reset();
			}
		}

		public void FreeRoam()
		{
			this.cameraType = previewControl.CameraType.FREE_ROAM;
		}

		public Matrix GetFreeRoamMatrix()
		{
			return this.camera.GetView();
		}

		public Vector3 GetFreeRoamPosition()
		{
			return this.camera.position;
		}

		public void SetMotionFrameIndex(int index)
		{
			if (this.cameraMotion != null)
			{
				this.cameraMotion.SetCurrentFrame(index);
				this.cameraMotion.Stop();
				this.cameraType = previewControl.CameraType.MOTION;
			}
		}

		public int GetCurrentMotionFrameIndex()
		{
			return this.cameraMotion.GetCurrentFrameIndex();
		}

		public CameraMotion.Frame GetMotionFrame(int index)
		{
			return this.cameraMotion.GetFrame(index);
		}

		public CameraMotion.Frame GetCurrentMotionFrame()
		{
			return this.cameraMotion.GetFrame(this.cameraMotion.GetCurrentFrameIndex());
		}

		public void UpdateCameraMatrices()
		{
			this.cameraMotion.GenerateViewMatrices();
		}

		public void SaveMotion()
		{
			this.cameraMotion.Save();
		}
	}
}
