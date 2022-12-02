using System;
using System.Windows.Forms;

namespace FG2ICCComms
{
	// Token: 0x02000006 RID: 6
	internal static class Program
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000046D6 File Offset: 0x000028D6
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
