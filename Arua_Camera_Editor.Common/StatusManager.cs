using System;

namespace Arua_Camera_Editor.Common
{
	internal class StatusManager
	{
		private static StatusManager currentInstance;

		public void SetDelegate()
		{
		}

		public void UpdateStatus(string newStatus, int progress = -1)
		{
		}

		public void PerformStepProgress()
		{
		}

		public static StatusManager Instance()
		{
			if (StatusManager.currentInstance == null)
			{
				StatusManager.currentInstance = new StatusManager();
			}
			return StatusManager.currentInstance;
		}
	}
}
