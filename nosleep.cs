using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("NoSleep")]
[assembly: AssemblyDefaultAlias("NoSleep")]
[assembly: AssemblyProduct("NoSleep")]
[assembly: AssemblyDescription("NoSleep")]
[assembly: AssemblyCompany("Deathbaron")]
[assembly: AssemblyCopyright("Deathbaron - 2017")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyCulture("")]

namespace NoSleep
{
	public class NoSleep : Form
	{        
		[DllImport("kernel32.dll", CharSet = CharSet.Auto,SetLastError = true)]
		static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
		[FlagsAttribute]
		public enum EXECUTION_STATE :uint
		{
			 ES_AWAYMODE_REQUIRED = 0x00000040,
			 ES_CONTINUOUS = 0x80000000,
			 ES_DISPLAY_REQUIRED = 0x00000002,
			 ES_SYSTEM_REQUIRED = 0x00000001
			 // Legacy flag, should not be used.
			 // ES_USER_PRESENT = 0x00000004
		}
		
		private NotifyIcon  trayIcon;
        private ContextMenu trayMenu;
		private	Thread thread;
		
		public NoSleep(String[] args)
		{
			trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
 
            trayIcon = new NotifyIcon();
            trayIcon.Text = "NoSleep";
            trayIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
 
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
			
			thread = new Thread(new ThreadStart(Worker));
			thread.Start();
		}
		
		public void Worker()
		{
			SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
			while (true)
			{
				Thread.Sleep(60);
			}
		}

		protected override void OnLoad(EventArgs e)
        {
            Visible  = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

		private void OnExit(object sender, EventArgs e)
		{
            trayIcon.Visible = false;
			Application.Exit();
			thread.Abort();
		}
	
		[STAThread]
		public static void Main(String[] args)
		{
			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new NoSleep(args));
		}
	}
}
