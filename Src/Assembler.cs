using System;

namespace Box256
{
	public class Assembler
	{
		// Compiles entire program to bytecode.
		// Input: Source code splitted into 3 char parts. Size needs to be multiple of 4.
		// Output: Compiled program as executable bytecode.
		public static byte[] Assemble(string[] source)
		{
			byte[] byteCode = new byte[source.Length];
			for (int i = 0; i < byteCode.Length / 4; i++)
			{
				byte[] compiledInstruction = AssembleInstruction(new string[]
				{
					source[i * 4 + 0],
					source[i * 4 + 1],
					source[i * 4 + 2],
					source[i * 4 + 3]
				});

				byteCode[i * 4 + 0] = compiledInstruction[0];
				byteCode[i * 4 + 1] = compiledInstruction[1];
				byteCode[i * 4 + 2] = compiledInstruction[2];
				byteCode[i * 4 + 3] = compiledInstruction[3];
			}

			return byteCode;
		}

		// Compiles single instruction to bytecode.
		// INPUT: Four parts, each having 3 chars. Example: { "MOV", "001", "@0C", "000" }
		// OUTPUT: Compiled instruction as executable bytecode (byte[4])
		public static byte[] AssembleInstruction(string[] line)
		{
			if (line.Length == 0)
				return new byte[4] {0, 0, 0, 0};

			if (!IsInstruction(line))
				return new byte[4] {ParamToValue(line[0]), ParamToValue(line[1]), ParamToValue(line[2]), ParamToValue(line[3])};

			byte[] result = new byte[4];

			if (IsCommutativeOperation(line[0]) && PointerDepth(line[1]) < PointerDepth(line[2]))
			{
				string tmp = line[1];
				line[1] = line[2];
				line[2] = tmp;
			}

			if (IsMathOperation(line[0]))
			{
				if (PointerDepth(line[1]) == 0 && PointerDepth(line[2]) == 0)
				{
					string target = line[3];
					if (line[0] == "ADD")
						line[1] = (ParamToValue(line[1]) + ParamToValue(line[2])).ToString("X2");
					if (line[0] == "SUB")
						line[1] = (ParamToValue(line[1]) - ParamToValue(line[2])).ToString("X2");
					if (line[0] == "MUL")
						line[1] = (ParamToValue(line[1]) * ParamToValue(line[2])).ToString("X2");
					if (line[0] == "DIV")
						line[1] = ParamToValue(line[2]) > 0
							? (ParamToValue(line[1]) / ParamToValue(line[2])).ToString("X2")
							: (0).ToString("X2");
					if (line[0] == "MOD")
						line[1] = ParamToValue(line[2]) > 0
							? (ParamToValue(line[1]) % ParamToValue(line[2])).ToString("X2")
							: (0).ToString("X2");
					line[0] = "MOV";
					line[2] = target;
					line[3] = "001";
				}
			}

			if (IsLengthOperation(line[0]) && line[3] == "000")
				line[3] = "001";

			OpCodes.OpCode opCode = InstructionToOpCode(line[0], line[1], line[2], line[3]);
			result[0] = (byte) opCode;
			result[1] = ParamToValue(line[1]);
			result[2] = ParamToValue(line[2]);
			result[3] = ParamToValue(line[3]);
			return result;
		}

		private static bool IsInstruction(string[] line)
		{
			return
				line[0] == "MOV" ||
				line[0] == "JMP" ||
				line[0] == "JEQ" ||
				line[0] == "JNE" ||
				line[0] == "JGR" ||
				line[0] == "ADD" ||
				line[0] == "SUB" ||
				line[0] == "MUL" ||
				line[0] == "DIV" ||
				line[0] == "MOD"
				;
		}

		public static bool IsInstruction(string line)
		{
			return IsInstruction(new string[] {line});
		}

		static byte ParamToValue(string param)
		{
			string part = param.Replace("*", "").Replace("@", "");
			if (part.Length == 0)
				return 0;

			bool negative = part[0] == '-';
			part = part.Replace("-", "");

			int value = 0;
			try
			{
				value = Convert.ToInt32(part, 16);
			}
			catch
			{
				return 0;
			}
			if (negative)
				value *= -1;

			return (byte) value;
		}

		static OpCodes.OpCode InstructionToOpCode(string instruction, string param1, string param2, string param3)
		{
			int d1 = PointerDepth(param1);
			int d2 = PointerDepth(param2);
			int d3 = PointerDepth(param3);

			switch (instruction.ToUpper())
			{
				case ("MOV"):
					if (d1 == 0 && d2 == 1 && d3 == 0) return OpCodes.OpCode.MOV010;
					if (d1 == 1 && d2 == 1 && d3 == 0) return OpCodes.OpCode.MOV110;
					if (d1 == 2 && d2 == 1 && d3 == 0) return OpCodes.OpCode.MOV210;
					if (d1 == 0 && d2 == 2 && d3 == 0) return OpCodes.OpCode.MOV020;
					if (d1 == 1 && d2 == 2 && d3 == 0) return OpCodes.OpCode.MOV120;
					if (d1 == 2 && d2 == 2 && d3 == 0) return OpCodes.OpCode.MOV220;
					if (d1 == 0 && d2 == 1 && d3 == 1) return OpCodes.OpCode.MOV011;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.MOV111;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.MOV211;
					if (d1 == 0 && d2 == 2 && d3 == 1) return OpCodes.OpCode.MOV021;
					if (d1 == 1 && d2 == 2 && d3 == 1) return OpCodes.OpCode.MOV121;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.MOV221;
					if (d1 == 0 && d2 == 1 && d3 == 2) return OpCodes.OpCode.MOV012;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.MOV112;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.MOV212;
					if (d1 == 0 && d2 == 2 && d3 == 2) return OpCodes.OpCode.MOV022;
					if (d1 == 1 && d2 == 2 && d3 == 2) return OpCodes.OpCode.MOV122;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.MOV222;
					break;
				case ("JMP"):
					if (d1 == 0) return OpCodes.OpCode.JMP0;
					if (d1 == 1) return OpCodes.OpCode.JMP1;
					if (d1 == 2) return OpCodes.OpCode.JMP2;
					break;
				case ("JEQ"):
					if (d1 == 1 && d2 == 0 && d3 == 0) return OpCodes.OpCode.JEQ100;
					if (d1 == 1 && d2 == 1 && d3 == 0) return OpCodes.OpCode.JEQ110;
					if (d1 == 2 && d2 == 0 && d3 == 0) return OpCodes.OpCode.JEQ200;
					if (d1 == 2 && d2 == 1 && d3 == 0) return OpCodes.OpCode.JEQ210;
					if (d1 == 2 && d2 == 2 && d3 == 0) return OpCodes.OpCode.JEQ220;
					if (d1 == 1 && d2 == 0 && d3 == 1) return OpCodes.OpCode.JEQ101;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.JEQ111;
					if (d1 == 2 && d2 == 0 && d3 == 1) return OpCodes.OpCode.JEQ201;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.JEQ211;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.JEQ221;
					if (d1 == 1 && d2 == 0 && d3 == 2) return OpCodes.OpCode.JEQ102;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.JEQ112;
					if (d1 == 2 && d2 == 0 && d3 == 2) return OpCodes.OpCode.JEQ202;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.JEQ212;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.JEQ222;
					break;
				case ("JNE"):
					if (d1 == 1 && d2 == 0 && d3 == 0) return OpCodes.OpCode.JNE100;
					if (d1 == 1 && d2 == 1 && d3 == 0) return OpCodes.OpCode.JNE110;
					if (d1 == 2 && d2 == 0 && d3 == 0) return OpCodes.OpCode.JNE200;
					if (d1 == 2 && d2 == 1 && d3 == 0) return OpCodes.OpCode.JNE210;
					if (d1 == 2 && d2 == 2 && d3 == 0) return OpCodes.OpCode.JNE220;
					if (d1 == 1 && d2 == 0 && d3 == 1) return OpCodes.OpCode.JNE101;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.JNE111;
					if (d1 == 2 && d2 == 0 && d3 == 1) return OpCodes.OpCode.JNE201;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.JNE211;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.JNE221;
					if (d1 == 1 && d2 == 0 && d3 == 2) return OpCodes.OpCode.JNE102;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.JNE112;
					if (d1 == 2 && d2 == 0 && d3 == 2) return OpCodes.OpCode.JNE202;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.JNE212;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.JNE222;
					break;
				case ("JGR"):
					if (d1 == 0 && d2 == 1 && d3 == 0) return OpCodes.OpCode.JGR010;
					if (d1 == 1 && d2 == 0 && d3 == 0) return OpCodes.OpCode.JGR100;
					if (d1 == 1 && d2 == 1 && d3 == 0) return OpCodes.OpCode.JGR110;
					if (d1 == 1 && d2 == 2 && d3 == 0) return OpCodes.OpCode.JGR120;
					if (d1 == 2 && d2 == 0 && d3 == 0) return OpCodes.OpCode.JGR200;
					if (d1 == 2 && d2 == 1 && d3 == 0) return OpCodes.OpCode.JGR210;
					if (d1 == 2 && d2 == 2 && d3 == 0) return OpCodes.OpCode.JGR220;
					if (d1 == 0 && d2 == 1 && d3 == 1) return OpCodes.OpCode.JGR011;
					if (d1 == 1 && d2 == 0 && d3 == 1) return OpCodes.OpCode.JGR101;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.JGR111;
					if (d1 == 1 && d2 == 2 && d3 == 1) return OpCodes.OpCode.JGR121;
					if (d1 == 2 && d2 == 0 && d3 == 1) return OpCodes.OpCode.JGR201;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.JGR211;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.JGR221;
					if (d1 == 0 && d2 == 1 && d3 == 2) return OpCodes.OpCode.JGR012;
					if (d1 == 1 && d2 == 0 && d3 == 2) return OpCodes.OpCode.JGR102;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.JGR112;
					if (d1 == 1 && d2 == 2 && d3 == 2) return OpCodes.OpCode.JGR122;
					if (d1 == 2 && d2 == 0 && d3 == 2) return OpCodes.OpCode.JGR202;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.JGR212;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.JGR222;
					break;
				case ("ADD"):
					if (d1 == 1 && d2 == 0 && d3 == 1) return OpCodes.OpCode.ADD101;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.ADD111;
					if (d1 == 2 && d2 == 0 && d3 == 1) return OpCodes.OpCode.ADD201;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.ADD211;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.ADD221;
					if (d1 == 1 && d2 == 0 && d3 == 2) return OpCodes.OpCode.ADD102;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.ADD112;
					if (d1 == 2 && d2 == 0 && d3 == 2) return OpCodes.OpCode.ADD202;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.ADD212;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.ADD222;
					break;
				case ("SUB"):
					if (d1 == 0 && d2 == 1 && d3 == 1) return OpCodes.OpCode.SUB011;
					if (d1 == 0 && d2 == 2 && d3 == 1) return OpCodes.OpCode.SUB021;
					if (d1 == 1 && d2 == 0 && d3 == 1) return OpCodes.OpCode.SUB101;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.SUB111;
					if (d1 == 1 && d2 == 2 && d3 == 1) return OpCodes.OpCode.SUB121;
					if (d1 == 2 && d2 == 0 && d3 == 1) return OpCodes.OpCode.SUB201;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.SUB211;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.SUB221;
					if (d1 == 0 && d2 == 1 && d3 == 2) return OpCodes.OpCode.SUB012;
					if (d1 == 0 && d2 == 2 && d3 == 2) return OpCodes.OpCode.SUB022;
					if (d1 == 1 && d2 == 0 && d3 == 2) return OpCodes.OpCode.SUB102;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.SUB112;
					if (d1 == 1 && d2 == 2 && d3 == 2) return OpCodes.OpCode.SUB122;
					if (d1 == 2 && d2 == 0 && d3 == 2) return OpCodes.OpCode.SUB202;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.SUB212;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.SUB222;
					break;
				case ("MUL"):
					if (d1 == 1 && d2 == 0 && d3 == 1) return OpCodes.OpCode.MUL101;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.MUL111;
					if (d1 == 2 && d2 == 0 && d3 == 1) return OpCodes.OpCode.MUL201;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.MUL211;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.MUL221;
					if (d1 == 1 && d2 == 0 && d3 == 2) return OpCodes.OpCode.MUL102;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.MUL112;
					if (d1 == 2 && d2 == 0 && d3 == 2) return OpCodes.OpCode.MUL202;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.MUL212;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.MUL222;
					break;
				case ("DIV"):
					if (d1 == 0 && d2 == 1 && d3 == 1) return OpCodes.OpCode.DIV011;
					if (d1 == 0 && d2 == 2 && d3 == 1) return OpCodes.OpCode.DIV021;
					if (d1 == 1 && d2 == 0 && d3 == 1) return OpCodes.OpCode.DIV101;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.DIV111;
					if (d1 == 1 && d2 == 2 && d3 == 1) return OpCodes.OpCode.DIV121;
					if (d1 == 2 && d2 == 0 && d3 == 1) return OpCodes.OpCode.DIV201;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.DIV211;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.DIV221;
					if (d1 == 0 && d2 == 1 && d3 == 2) return OpCodes.OpCode.DIV012;
					if (d1 == 0 && d2 == 2 && d3 == 21) return OpCodes.OpCode.DIV022;
					if (d1 == 1 && d2 == 0 && d3 == 2) return OpCodes.OpCode.DIV102;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.DIV112;
					if (d1 == 1 && d2 == 2 && d3 == 2) return OpCodes.OpCode.DIV122;
					if (d1 == 2 && d2 == 0 && d3 == 2) return OpCodes.OpCode.DIV202;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.DIV212;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.DIV222;
					break;
				case ("MOD"):
					if (d1 == 0 && d2 == 1 && d3 == 1) return OpCodes.OpCode.MOD011;
					if (d1 == 1 && d2 == 0 && d3 == 1) return OpCodes.OpCode.MOD101;
					if (d1 == 1 && d2 == 1 && d3 == 1) return OpCodes.OpCode.MOD111;
					if (d1 == 1 && d2 == 2 && d3 == 1) return OpCodes.OpCode.MOD121;
					if (d1 == 2 && d2 == 0 && d3 == 1) return OpCodes.OpCode.MOD201;
					if (d1 == 2 && d2 == 1 && d3 == 1) return OpCodes.OpCode.MOD211;
					if (d1 == 2 && d2 == 2 && d3 == 1) return OpCodes.OpCode.MOD221;
					if (d1 == 0 && d2 == 1 && d3 == 2) return OpCodes.OpCode.MOD012;
					if (d1 == 1 && d2 == 0 && d3 == 2) return OpCodes.OpCode.MOD102;
					if (d1 == 1 && d2 == 1 && d3 == 2) return OpCodes.OpCode.MOD112;
					if (d1 == 1 && d2 == 2 && d3 == 2) return OpCodes.OpCode.MOD122;
					if (d1 == 2 && d2 == 0 && d3 == 2) return OpCodes.OpCode.MOD202;
					if (d1 == 2 && d2 == 1 && d3 == 2) return OpCodes.OpCode.MOD212;
					if (d1 == 2 && d2 == 2 && d3 == 2) return OpCodes.OpCode.MOD222;
					break;
			}

			return OpCodes.OpCode.NOP;
		}

		static int PointerDepth(string param)
		{
			if (string.IsNullOrEmpty(param) || param[0] != '*' && param[0] != '@')
				return 0;
			if (param[0] == '@')
				return 1;
			return 2;
		}

		static bool IsMathOperation(string instruction)
		{
			return instruction == "ADD" || instruction == "SUB" || instruction == "MUL" || instruction == "DIV" || instruction == "MOD";
		}

		static bool IsCommutativeOperation(string instruction)
		{
			return instruction == "ADD" || instruction == "MUL" || instruction == "JEQ" || instruction == "JNE";
		}

		static bool IsLengthOperation(string instruction)
		{
			return instruction == "MOV";
		}
	}
}
