
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using Magic;
using System.Windows.Forms;

namespace mbxScan2
{
	class ScanMem
	{
		const int PROCESS_WM_READ = 0x0010;

		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern int memcmp(byte[] b1, byte[] b2, long count);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(
			int hProcess,
			int lpBaseAddress,
			byte[] lpBuffer,
			int dwSize,
			ref int lpNumberOfBytesRead
		);


		static bool ByteArrayCompare(byte[] b1, byte[] b2)
		{
			// Validate buffers are the same length.
			// This also ensures that the count does not exceed the length of either buffer.  
			return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
		}

		public static int FindPattern(IntPtr curProcessHandle, int scanAdressFrom, int scanAdressTo, byte[] pattern)
		{
			byte[] bytesToRead = new byte[pattern.Length];
			int bytesRead = 0;
			bool findPattern;

			for (int i = scanAdressFrom; i < scanAdressTo; i++)
			{
				findPattern = ReadProcessMemory((int)curProcessHandle, i, bytesToRead, pattern.Length, ref bytesRead);  //ref = works in 2 ways; kann schon vor der Übergabe initialisiert werden

				if (ByteArrayCompare(bytesToRead, pattern))
				{
					string sBytesToRead = "";
					foreach (byte temp in bytesToRead)
					{
						sBytesToRead = sBytesToRead + temp.ToString("X") + " ";
					}
					//txtDebug.Text += "Adresse: " + i + " TRUE! " + sBytesToRead + "  -  " + bytesRead + "\r\n";
					//Console.WriteLine("Adresse: wow.exe + " + i + " TRUE! " + sBytesToRead + "  -  " + bytesRead);
					return i;
				}
			}
			return 0;

		}

	}
}

