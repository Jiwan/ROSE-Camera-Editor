using System;

namespace Arua_Camera_Editor.Common.FileHandler
{
	internal interface IReadable
	{
		void Load(string mypath, ClientType myclientType);
	}
}
