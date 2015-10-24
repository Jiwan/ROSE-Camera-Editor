using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Arua_Camera_Editor.Common
{
	internal class FreeRoamCamera : ICamera
	{
		private static Matrix roseCoordinate = Matrix.CreateRotationX(1.57079637f) * Matrix.CreateRotationZ(3.14159274f);

		public Vector3 position;

		public Vector3 yawpitchroll;

		private Vector2? lastMousePosition;

		private Vector2 viewMiddle;

		private float speed = 1f;

		public Matrix view
		{
			get;
			set;
		}

		public FreeRoamCamera(Vector2 viewMiddle)
		{
			this.position = new Vector3(5200f, 5200f, 20f);
			this.lastMousePosition = null;
			this.view = Matrix.Invert(FreeRoamCamera.roseCoordinate * Matrix.CreateTranslation(this.position));
			this.yawpitchroll.X = 0f;
			this.yawpitchroll.Y = 0f;
			this.yawpitchroll.Z = 0f;
			this.viewMiddle = viewMiddle;
		}

		public void ActualizeMatrix()
		{
			this.view = Matrix.Invert(Matrix.CreateFromYawPitchRoll(this.yawpitchroll.X, this.yawpitchroll.Y, 0f) * FreeRoamCamera.roseCoordinate * Matrix.CreateTranslation(this.position));
		}

		public void MoveForward()
		{
			this.position += Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(this.yawpitchroll.X, this.yawpitchroll.Y, 0f) * FreeRoamCamera.roseCoordinate);
			this.ActualizeMatrix();
		}

		public void MoveBackward()
		{
			this.position += Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(this.yawpitchroll.X, this.yawpitchroll.Y, 0f) * FreeRoamCamera.roseCoordinate);
			this.ActualizeMatrix();
		}

		public void MoveLeft()
		{
			this.position += Vector3.Transform(Vector3.Left, Matrix.CreateFromYawPitchRoll(this.yawpitchroll.X, this.yawpitchroll.Y, 0f) * FreeRoamCamera.roseCoordinate);
			this.ActualizeMatrix();
		}

		public void MoveRight()
		{
			this.position += Vector3.Transform(Vector3.Right, Matrix.CreateFromYawPitchRoll(this.yawpitchroll.X, this.yawpitchroll.Y, 0f) * FreeRoamCamera.roseCoordinate);
			this.ActualizeMatrix();
		}

		public void ChangeView(Vector2 mouse)
		{
			if (!this.lastMousePosition.HasValue)
			{
				this.lastMousePosition = new Vector2?(mouse);
			}
			else
			{
				Vector2 vector = new Vector2(mouse.X - this.lastMousePosition.Value.X, mouse.Y - this.lastMousePosition.Value.Y);
				this.yawpitchroll.X = this.yawpitchroll.X + -vector.X * 0.01f;
				this.yawpitchroll.Y = this.yawpitchroll.Y + -vector.Y * 0.01f;
				this.lastMousePosition = new Vector2?(mouse);
				this.ActualizeMatrix();
			}
		}

		public void ResumeMousePosition()
		{
			this.lastMousePosition = null;
		}

		public void Update(float time)
		{
			KeyboardState state = Keyboard.GetState();
			MouseState state2 = Mouse.GetState();
			if (state.IsKeyDown(Keys.Z))
			{
				this.MoveForward();
			}
			else if (state.IsKeyDown(Keys.S))
			{
				this.MoveBackward();
			}
			else if (state.IsKeyDown(Keys.Q))
			{
				this.MoveLeft();
			}
			else if (state.IsKeyDown(Keys.D))
			{
				this.MoveRight();
			}
			if (state2.RightButton == ButtonState.Pressed)
			{
				this.ChangeView(new Vector2((float)state2.X, (float)state2.Y));
			}
			else
			{
				this.lastMousePosition = null;
			}
		}

		public Matrix GetView()
		{
			return this.view;
		}
	}
}
