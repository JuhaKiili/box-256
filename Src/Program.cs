using System;

namespace Box256
{
	class Program
	{
		static void Main(string[] args)
		{
			// NOTE: Source needs to be zero filled to exact length of CPU.k_MemorySize!
			string rawSourceCode = 
				"MOV 001 @0C 000 " +
				"MOV 002 @0D 000 " +
				"ADD *0F @0D @0E " +
				"000 000 000 00C";

			string[] sourceCodeSplitted = rawSourceCode.Split(' ');
			
			CPU cpu = new CPU();
			
			cpu.memory = Assembler.Assemble(sourceCodeSplitted);
			
			Log("Assembled:");
			PrintMemoryState(cpu);

			cpu.programCounter = 0;
			for (int tick = 0; tick < 3; tick++)
			{
				cpu.Tick();
				Log("Executed (Tick " + tick + ")");
				PrintMemoryState(cpu);
			}

			Log("Press Any Key to Quit...");
			Console.ReadLine();
		}

		private static void PrintMemoryState(CPU cpu)
		{
			byte[] mem = cpu.memory;
			for (int line = 0; line < mem.Length/4; line++)
			{
				string str = "";
				for (int i = 0; i < 4; i++)
				{
					str += mem[line*4 + i].ToString("X2");
				}
				Log(str);
			}
			Linefeed();
		}

		private static void Linefeed()
		{
			Console.Write(Environment.NewLine);
		}

		private static void Log(string line)
		{
			Console.WriteLine(line);
		}
	}
}
