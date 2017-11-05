using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic;
using System.Threading;
using System.Windows.Forms;

namespace mbxScan2
{
	public class HookVA
	{
		// Addresse Inection code:
		uint injected_code = 0;
		public uint eaxStore = 0;
		uint retnInjectionAsm = 0;
		public bool threadHooked = false;
		bool InjectionUsed = false;
		public BlackMagic Memory = new BlackMagic();
		public int processId = 0;
		//public uint DX_DEVICE { get; private set; }
		//public uint DX_DEVICE_IDX { get; private set; }
		public uint vaHookPtr { get; private set; }
		public TextBox txtDebug;

		public HookVA(int processId, uint vaHookPtr, TextBox txtDebug)
		{
			this.txtDebug = txtDebug;
			this.processId = processId;
			//this.DX_DEVICE = dx_device;
			this.vaHookPtr = vaHookPtr;
			Hooking();
		}


		public void Hooking()
		{
			// Process Connect:
			if (!Memory.IsProcessOpen)
			{
				Memory.OpenProcessAndThread(processId);
			}

			if (Memory.IsProcessOpen)
			{
				txtDebug.Text += "VirtualAlloc Hook: " + vaHookPtr.ToString("X") + " | " + Memory.ReadByte(vaHookPtr).ToString("X") + "\r\n";
				if (Memory.ReadByte(vaHookPtr) == 0xE9 && (injected_code == 0 || eaxStore == 0)) // check if wow is already hooked and dispose Hook
				{
					DisposeHooking();
				}

				if (Memory.ReadByte(vaHookPtr) != 0xE9) // check if wow is already hooked
				{
					try
					{
						threadHooked = false;
						// allocate memory to store injected code: (Das ist die codecave wohin die Endscene umgeleitet wird)
						injected_code = Memory.AllocateMemory(2048);
						txtDebug.Text += "injected_code: " + injected_code.ToString("X") + "\r\n";

						// die Adresse in welche EAX hingespeichert wird
						eaxStore = Memory.AllocateMemory(0x4);
						txtDebug.Text += "eaxStore: " + eaxStore.ToString("X") + "\r\n";

						// allocate memory the pointer return value:
						retnInjectionAsm = Memory.AllocateMemory(0x4);
						txtDebug.Text += "retnInjectionAsm: " + retnInjectionAsm.ToString("X") + "\r\n";
						Memory.WriteInt(retnInjectionAsm, 0);


						// Generate the STUB to be injected
						Memory.Asm.Clear(); // $Asm

						// save regs
						//Memory.Asm.AddLine("pushad");
						//Memory.Asm.AddLine("pushfd");

						Memory.Asm.AddLine("mov [" + eaxStore + "],eax");

						// Size asm jumpback
						int sizeJumpBack = 5;
						Memory.Asm.AddLine("jmp " + (vaHookPtr + sizeJumpBack));



						// inject code in codecave
						uint sizeAsm = (uint)(Memory.Asm.Assemble().Length);
						Memory.Asm.Inject(injected_code);
						txtDebug.Text += "Injecting ...\r\n";


						// create hook jump
						Memory.Asm.Clear(); // $jmpto
						Memory.Asm.AddLine("jmp " + (injected_code));
						Memory.Asm.AddLine("retn 10h");
						Memory.Asm.Inject(vaHookPtr);
					}
					catch { threadHooked = false; return; }
				}
				threadHooked = true;


			}
			
		}
		public void DisposeHooking()
		{
			try
			{
				txtDebug.Text += "VirtualAlloc Dispose: " + vaHookPtr + " | " + Memory.ReadByte(vaHookPtr).ToString("X") + "\r\n";
				if (Memory.ReadByte(vaHookPtr) == 0xE9) // check if wow is already hooked and dispose Hook
				{
					// Restore origine VirtualAlloc:
					
					Memory.Asm.Clear();
					Memory.Asm.AddLine("retn 10");
					Memory.Asm.AddLine("int3");
					Memory.Asm.AddLine("int3");
					Memory.Asm.AddLine("int3");
					Memory.Asm.AddLine("int3");
					Memory.Asm.AddLine("int3");
					Memory.Asm.Inject(vaHookPtr);
					
				}

				// free memory:
				Memory.FreeMemory(injected_code);
				Memory.FreeMemory(eaxStore);
				Memory.FreeMemory(retnInjectionAsm);

			}
			catch { }
		}
	}
}
