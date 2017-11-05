using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace mbxScan2
{
	public class FindProcessModules
	{
		/*
		private int processId;
		private string pModuleName;
		private Process curProcess;
		
		public FindProcessModules(int pId, string pMName)
		{
			this.processId = pId;
			this.pModuleName = pMName;
			curProcess = Process.GetProcessById(processId);
			GetProcessModule(curProcess);
		}
		*/
		public static ProcessModule GetProcessModule(int pId, string pMName)
		{
			Process curProcess = Process.GetProcessById(pId);
			ProcessModule curProcessModule;
			ProcessModuleCollection processModuleCollection = curProcess.Modules;
			for (int i = 0; i < processModuleCollection.Count; i++)
			{
				curProcessModule = processModuleCollection[i];
				if (curProcessModule.ModuleName.ToLower() == pMName.ToLower())
				{
					//Console.WriteLine("Found module" + curProcessModule.ModuleName + "    file: " + curProcessModule.FileName);
					return curProcessModule;
				}
			}
			return null;
		}
		public static uint GetProcessModuleBase(int pId, string pMName)
		{
			Process curProcess = Process.GetProcessById(pId);
			ProcessModule curProcessModule;
			ProcessModuleCollection processModuleCollection = curProcess.Modules;
			for (int i = 0; i < processModuleCollection.Count; i++)
			{
				curProcessModule = processModuleCollection[i];
				if (curProcessModule.ModuleName.ToLower() == pMName.ToLower())
				{
					//Console.WriteLine("Found module" + curProcessModule.ModuleName + "    file: " + curProcessModule.FileName);
					return (uint)curProcessModule.BaseAddress;
				}
			}
			return 0;
		}
	}
}
