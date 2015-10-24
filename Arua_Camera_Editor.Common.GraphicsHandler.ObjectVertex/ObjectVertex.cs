using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arua_Camera_Editor.Common.GraphicsHandler.ObjectVertex
{
	public struct ObjectVertex
	{
		public const int SIZE_IN_BYTES = 20;

		public static VertexElement[] vertex;

		public Vector3 Position
		{
			get;
			set;
		}

		public Vector2 TextureCoordinate
		{
			get;
			set;
		}

		static ObjectVertex()
		{
			ObjectVertex.vertex = new VertexElement[]
			{
				new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
				new VertexElement(0, 12, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0)
			};
		}
	}
}
