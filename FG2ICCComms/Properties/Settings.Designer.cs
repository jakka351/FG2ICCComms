using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace FG2ICCComms.Properties
{
	// Token: 0x02000007 RID: 7
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
	[CompilerGenerated]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000046ED File Offset: 0x000028ED
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000027 RID: 39
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
