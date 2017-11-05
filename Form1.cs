using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Magic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.InteropServices;

namespace mbxScan2
{
	public partial class Form1 : Form
	{
		const int PROCESS_WM_READ = 0x0010;

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		public Form1()
		{
			InitializeComponent();
			wowMem = new BlackMagic();
			txtDebug.Text = "";
		}
		public HookVA MyVaHook { get; private set; }
		public BlackMagic wowMem { get; private set; }
		public int wowProc { get; private set; }
		private Process wowApp = null;
		private IntPtr wowProcHandle;
		private ProcessModule wowProcMod = null;
		private byte[] wrdPattern = new byte[] { 0x74, 0x02, 0xF3, 0xA5, 0xB1, 0x03, 0x23, 0xCA };
		//private char[] wrdMask = new char[] { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' };
		private uint curWrdBase = 0;
		private uint lastWrdBase = 0;
		private uint wrdPatternOffset = 0;
		private uint VirtualAllocReturnAddress = 0;
		//private bool wowAttached = false;
		private bool vaIsHooked = false;
		private byte LoginScreen = 0;



		private enum MiscOffsets
		{
			CanLogin = 0xEE7EB2,
			VirtualAlloc = 0xF12E,
		}


		private void cmdAttach_Click(object sender, EventArgs e)
		{
			
			try
			{
				//wowProc = SProcess.GetProcessFromWindowTitle("is1 Fuzixia-Ysera - toontest");
				wowProc = SProcess.GetProcessFromWindowTitle("World of Warcraft");
				wowApp = Process.GetProcessById(wowProc); /* notwendig für das Finden der Endscene */
				wowMem.OpenProcessAndThread(wowProc);
				wowProcHandle = OpenProcess(PROCESS_WM_READ, false, wowApp.Id);
				wowProcMod =  FindProcessModules.GetProcessModule(wowApp.Id,"wow.exe");

				VirtualAllocReturnAddress = FindProcessModules.GetProcessModuleBase(wowApp.Id, "KERNELBASE.dll");
				txtDebug.Text += "wowBase: " + wowMem.MainModule.BaseAddress.ToString("X") + "\r\n";
				
				//txtDebug.Text += "Login: " + LoginScreen.ToString("X") + "\r\n"; 
				txtDebug.Text += "AllocReturn: " + (VirtualAllocReturnAddress + (uint)MiscOffsets.VirtualAlloc).ToString("X") + "\r\n";
				txtDebug.Text += "AllocReturn: " + (VirtualAllocReturnAddress + (uint)MiscOffsets.VirtualAlloc).ToString("X") + "\r\n";
			}
			catch (IndexOutOfRangeException exc)
			{
				txtDebug.Text += exc.ToString() + "Kein WoW gefunden";
			}


			tmrCheckLogin.Interval = 2000;
			if (tmrCheckLogin.Enabled == false && wowMem.IsProcessOpen)
			{
				tmrCheckLogin.Start();
				cmdAttach.BackColor = Color.FromArgb(200, 200, 0);
				//txtDebug.Text += "\r\n" + "waiting for login sequence";
			}
			else if (!wowMem.IsProcessOpen)
			{
				txtDebug.Text += "\r\n" + "no process found!";
			}


		}
		private void tmrCheckLogin_Tick(object sender, EventArgs e)
		{
			if (wowMem.IsProcessOpen)
			{
				txtDebug.Text += "\r\n" + "try to hook VA" + "\r\n";
				// VirtualAlloc hooken
				if (MyVaHook == null || !MyVaHook.threadHooked || MyVaHook.eaxStore == 0)
				{
					MyVaHook = new HookVA(wowProc, VirtualAllocReturnAddress + (uint)MiscOffsets.VirtualAlloc, txtDebug);
				}

				// Wurde die WRD-Base durch den VA-Hook geschrieben?
				if (MyVaHook.threadHooked && MyVaHook.eaxStore != 0)
				{
					txtDebug.Text += "MyVaHook.eaxStore:" + MyVaHook.eaxStore.ToString("X") + "\r\n";
					curWrdBase = wowMem.ReadUInt((uint)MyVaHook.eaxStore);
					txtDebug.Text += "Wrd-Base:" + curWrdBase.ToString("X") + "\r\n";

					// Scanne nach WRD-Pattern
					if (curWrdBase != 0 && lastWrdBase != curWrdBase)
					{
						wrdPatternOffset = (uint)ScanMem.FindPattern(wowProcHandle, (int)curWrdBase, (int)curWrdBase + 32000, wrdPattern);
						lastWrdBase = curWrdBase;
					}
				}
				txtDebug.Text += "Wrd-Pattern-Offset: " + wrdPatternOffset.ToString("X") + "\r\n";

			}
			// !!!!!!!!!!!!!!!!! Prüfe ob das richtige Aloocate gehooked wurde, denn Allocate passieren ja ständig in der Kernelbase
			// !!!!!!!!!!!!!!!!! Prüfe ob das richtige Aloocate gehooked wurde, denn Allocate passieren ja ständig in der Kernelbase
			// !!!!!!!!!!!!!!!!! Prüfe ob das richtige Aloocate gehooked wurde, denn Allocate passieren ja ständig in der Kernelbase
		}
		private void cmdDisarm_Click(object sender, EventArgs e)
		{


			/*
			if (wowMem.IsProcessOpen)
			{
				txtDebug.Text = "";
				txtDebug.Text += "WoW-proc-id: " + wowProc.ToString() + "\r\n";
				MyHook = new HookVA(wowProc, FindVirtualAllocBase(wowApp) + (uint)MiscOffsets.VirtualAlloc, txtDebug);

			}
			*/
		}


		private void txtDebug_TextChanged(object sender, EventArgs e)
		{
			txtDebug.SelectionStart = txtDebug.Text.Length;
			txtDebug.ScrollToCaret();
		}

		private void cmdStop_Click(object sender, EventArgs e)
		{
			tmrCheckLogin.Stop();
		}
	}
}
