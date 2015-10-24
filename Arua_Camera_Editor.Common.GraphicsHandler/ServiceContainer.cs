using System;
using System.Collections.Generic;

namespace Arua_Camera_Editor.Common.GraphicsHandler
{
	public class ServiceContainer : IServiceProvider
	{
		private Dictionary<Type, object> services = new Dictionary<Type, object>();

		public void AddService<T>(T service)
		{
			this.services.Add(typeof(T), service);
		}

		public object GetService(Type serviceType)
		{
			object result;
			this.services.TryGetValue(serviceType, out result);
			return result;
		}
	}
}
