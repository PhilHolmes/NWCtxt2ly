using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NWCTXT2Ly
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string FileName = "";
			if (args.Length > 0)
			{
				FileName = args[0];
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm(FileName));
		}
	}
}
