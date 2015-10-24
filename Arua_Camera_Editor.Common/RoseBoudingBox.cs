using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common
{
	internal class RoseBoudingBox
	{
		private BoundingBox box;

		public void CreateFromVertex(VertexPositionNormalTexture[] vertex)
		{
			Vector3[] array = new Vector3[vertex.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = vertex[i].Position;
			}
			this.box = BoundingBox.CreateFromPoints(array);
		}

		public Vector3 GetMiddle()
		{
			Vector3[] corners = this.box.GetCorners();
			return corners[0];
		}
	}
}
