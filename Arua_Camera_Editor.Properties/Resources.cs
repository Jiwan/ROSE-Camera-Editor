using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Arua_Camera_Editor.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Arua_Camera_Editor.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		internal static Bitmap Back_small
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Back_small", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap Capture_small
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Capture_small", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap New_small
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("New_small", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap Open_small
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Open_small", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap Pause_small
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Pause_small", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap Play_smal
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Play_smal", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap Save_small
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Save_small", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap Walk_small
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Walk_small", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap Walk_small1
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Walk_small1", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal Resources()
		{
		}
	}
}
