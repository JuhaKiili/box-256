namespace Box256
{
	public class CPU
	{
		public const byte k_MemorySize = 16; // Must be multiple of 4
		
		private byte[] m_Memory;
		private byte m_ProgramCounter;
		private bool m_ExecutionJumpedDuringTick;

		public byte[] memory
		{
			get { return m_Memory; }
			set { m_Memory = value; }
		}

		public byte programCounter
		{
			get { return m_ProgramCounter; }
			set { m_ProgramCounter = value; }
		}

		public CPU()
		{
			m_Memory = new byte[k_MemorySize];
			m_ExecutionJumpedDuringTick = false;
			m_ProgramCounter = 0;
		}

		public void Tick()
		{
			m_ExecutionJumpedDuringTick = false;

			OpCodes.OpCode opCode = (OpCodes.OpCode) m_Memory[m_ProgramCounter];
			byte p1 = m_Memory[(byte) (m_ProgramCounter + 1)];
			byte p2 = m_Memory[(byte) (m_ProgramCounter + 2)];
			byte p3 = m_Memory[(byte) (m_ProgramCounter + 3)];

			unchecked
			{
				switch (opCode)
				{
					case OpCodes.OpCode.MOV010:
						SetMemory(ConstantToArray(p1, p3), p2);
						break;
					case OpCodes.OpCode.MOV110:
						SetMemory(GetMemory(p1, p3), p2);
						break;
					case OpCodes.OpCode.MOV210:
						SetMemory(GetMemory2(p1, p3), p2);
						break;
					case OpCodes.OpCode.MOV020:
						SetMemory(ConstantToArray(p1, p3), GetMemory(p2));
						break;
					case OpCodes.OpCode.MOV120:
						SetMemory(GetMemory(p1, p3), GetMemory(p2));
						break;
					case OpCodes.OpCode.MOV220:
						SetMemory(GetMemory2(p1, p3), GetMemory(p2));
						break;
					case OpCodes.OpCode.MOV011:
						SetMemory(ConstantToArray(p1, GetMemory(p3)), p2);
						break;
					case OpCodes.OpCode.MOV111:
						SetMemory(GetMemory(p1, GetMemory(p3)), p2);
						break;
					case OpCodes.OpCode.MOV211:
						SetMemory(GetMemory2(p1, GetMemory(p3)), p2);
						break;
					case OpCodes.OpCode.MOV021:
						SetMemory(ConstantToArray(p1, GetMemory(p3)), GetMemory(p2));
						break;
					case OpCodes.OpCode.MOV121:
						SetMemory(GetMemory(p1, GetMemory(p3)), GetMemory(p2));
						break;
					case OpCodes.OpCode.MOV221:
						SetMemory(GetMemory2(p1, GetMemory(p3)), GetMemory(p2));
						break;
					case OpCodes.OpCode.MOV012:
						SetMemory(ConstantToArray(p1, GetMemory(GetMemory(p3))), p2);
						break;
					case OpCodes.OpCode.MOV112:
						SetMemory(GetMemory(p1, GetMemory(GetMemory(p3))), p2);
						break;
					case OpCodes.OpCode.MOV212:
						SetMemory(GetMemory2(p1, GetMemory(GetMemory(p3))), p2);
						break;
					case OpCodes.OpCode.MOV022:
						SetMemory(ConstantToArray(p1, GetMemory(GetMemory(p3))), GetMemory(p2));
						break;
					case OpCodes.OpCode.MOV122:
						SetMemory(GetMemory(p1, GetMemory(GetMemory(p3))), GetMemory(p2));
						break;
					case OpCodes.OpCode.MOV222:
						SetMemory(GetMemory2(p1, GetMemory(GetMemory(p3))), GetMemory(p2));
						break;

					case OpCodes.OpCode.JMP0:
						Jump(p1);
						break;
					case OpCodes.OpCode.JMP1:
						JumpTo(p1);
						break;
					case OpCodes.OpCode.JMP2:
						JumpTo2(p1);
						break;

					case OpCodes.OpCode.JEQ100:
						if (GetMemory(p1) == p2) Jump(p3);
						break;
					case OpCodes.OpCode.JEQ110:
						if (GetMemory(p1) == GetMemory(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JEQ200:
						if (GetMemory2(p1) == p2) Jump(p3);
						break;
					case OpCodes.OpCode.JEQ210:
						if (GetMemory2(p1) == GetMemory(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JEQ220:
						if (GetMemory2(p1) == GetMemory2(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JEQ101:
						if (GetMemory(p1) == p2) JumpTo(p3);
						break;
					case OpCodes.OpCode.JEQ111:
						if (GetMemory(p1) == GetMemory(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JEQ201:
						if (GetMemory2(p1) == p2) JumpTo(p3);
						break;
					case OpCodes.OpCode.JEQ211:
						if (GetMemory2(p1) == GetMemory(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JEQ221:
						if (GetMemory2(p1) == GetMemory2(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JEQ102:
						if (GetMemory(p1) == p2) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JEQ112:
						if (GetMemory(p1) == GetMemory(p2)) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JEQ202:
						if (GetMemory2(p1) == p2) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JEQ212:
						if (GetMemory2(p1) == GetMemory(p2)) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JEQ222:
						if (GetMemory2(p1) == GetMemory2(p2)) JumpTo2(p3);
						break;

					case OpCodes.OpCode.JNE100:
						if (GetMemory(p1) != p2) Jump(p3);
						break;
					case OpCodes.OpCode.JNE110:
						if (GetMemory(p1) != GetMemory(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JNE200:
						if (GetMemory2(p1) != p2) Jump(p3);
						break;
					case OpCodes.OpCode.JNE210:
						if (GetMemory2(p1) != GetMemory(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JNE220:
						if (GetMemory2(p1) != GetMemory2(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JNE101:
						if (GetMemory(p1) != p2) JumpTo(p3);
						break;
					case OpCodes.OpCode.JNE111:
						if (GetMemory(p1) != GetMemory(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JNE201:
						if (GetMemory2(p1) != p2) JumpTo(p3);
						break;
					case OpCodes.OpCode.JNE211:
						if (GetMemory2(p1) != GetMemory(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JNE221:
						if (GetMemory2(p1) != GetMemory2(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JNE102:
						if (GetMemory(p1) != p2) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JNE112:
						if (GetMemory(p1) != GetMemory(p2)) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JNE202:
						if (GetMemory2(p1) != p2) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JNE212:
						if (GetMemory2(p1) != GetMemory(p2)) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JNE222:
						if (GetMemory2(p1) != GetMemory2(p2)) JumpTo2(p3);
						break;

					case OpCodes.OpCode.JGR010:
						if (p1 > GetMemory(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JGR100:
						if (GetMemory(p1) > p2) Jump(p3);
						break;
					case OpCodes.OpCode.JGR110:
						if (GetMemory(p1) > GetMemory(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JGR120:
						if (GetMemory(p1) > GetMemory2(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JGR200:
						if (GetMemory2(p1) > p2) Jump(p3);
						break;
					case OpCodes.OpCode.JGR210:
						if (GetMemory2(p1) > GetMemory(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JGR220:
						if (GetMemory2(p1) > GetMemory2(p2)) Jump(p3);
						break;
					case OpCodes.OpCode.JGR011:
						if (p1 > GetMemory(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JGR101:
						if (GetMemory(p1) > p2) JumpTo(p3);
						break;
					case OpCodes.OpCode.JGR111:
						if (GetMemory(p1) > GetMemory(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JGR121:
						if (GetMemory(p1) > GetMemory2(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JGR201:
						if (GetMemory2(p1) > p2) JumpTo(p3);
						break;
					case OpCodes.OpCode.JGR211:
						if (GetMemory2(p1) > GetMemory(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JGR221:
						if (GetMemory2(p1) > GetMemory2(p2)) JumpTo(p3);
						break;
					case OpCodes.OpCode.JGR012:
						if (p1 > GetMemory(p2)) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JGR102:
						if (GetMemory(p1) > p2) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JGR112:
						if (GetMemory(p1) > GetMemory(p2)) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JGR122:
						if (GetMemory(p1) > GetMemory2(p2)) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JGR202:
						if (GetMemory2(p1) > p2) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JGR212:
						if (GetMemory2(p1) > GetMemory(p2)) JumpTo2(p3);
						break;
					case OpCodes.OpCode.JGR222:
						if (GetMemory2(p1) > GetMemory2(p2)) JumpTo2(p3);
						break;

					case OpCodes.OpCode.ADD101:
						SetMemory((byte) (GetMemory(p1) + p2), p3);
						break;
					case OpCodes.OpCode.ADD111:
						SetMemory((byte) (GetMemory(p1) + GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.ADD201:
						SetMemory((byte) (GetMemory2(p1) + p2), p3);
						break;
					case OpCodes.OpCode.ADD211:
						SetMemory((byte) (GetMemory2(p1) + GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.ADD221:
						SetMemory((byte) (GetMemory2(p1) + GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.ADD102:
						SetMemory((byte) (GetMemory(p1) + p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.ADD112:
						SetMemory((byte) (GetMemory(p1) + GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.ADD202:
						SetMemory((byte) (GetMemory2(p1) + p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.ADD212:
						SetMemory((byte) (GetMemory2(p1) + GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.ADD222:
						SetMemory((byte) (GetMemory2(p1) + GetMemory2(p2)), GetMemory(p3));
						break;

					case OpCodes.OpCode.SUB011:
						SetMemory((byte) (p1 - GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.SUB101:
						SetMemory((byte) (GetMemory(p1) - p2), p3);
						break;
					case OpCodes.OpCode.SUB111:
						SetMemory((byte) (GetMemory(p1) - GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.SUB021:
						SetMemory((byte) (p1 - GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.SUB201:
						SetMemory((byte) (GetMemory2(p1) - p2), p3);
						break;
					case OpCodes.OpCode.SUB211:
						SetMemory((byte) (GetMemory2(p1) - GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.SUB121:
						SetMemory((byte) (GetMemory(p1) - GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.SUB221:
						SetMemory((byte) (GetMemory2(p1) - GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.SUB012:
						SetMemory((byte) (p1 - GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.SUB102:
						SetMemory((byte) (GetMemory(p1) - p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.SUB112:
						SetMemory((byte) (GetMemory(p1) - GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.SUB022:
						SetMemory((byte) (p1 - GetMemory2(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.SUB202:
						SetMemory((byte) (GetMemory2(p1) - p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.SUB212:
						SetMemory((byte) (GetMemory2(p1) - GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.SUB122:
						SetMemory((byte) (GetMemory(p1) - GetMemory2(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.SUB222:
						SetMemory((byte) (GetMemory2(p1) - GetMemory2(p2)), GetMemory(p3));
						break;

					case OpCodes.OpCode.MUL101:
						SetMemory((byte) (GetMemory(p1)*p2), p3);
						break;
					case OpCodes.OpCode.MUL111:
						SetMemory((byte) (GetMemory(p1)*GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.MUL201:
						SetMemory((byte) (GetMemory2(p1)*p2), p3);
						break;
					case OpCodes.OpCode.MUL211:
						SetMemory((byte) (GetMemory2(p1)*GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.MUL221:
						SetMemory((byte) (GetMemory2(p1)*GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.MUL102:
						SetMemory((byte) (GetMemory(p1)*p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.MUL112:
						SetMemory((byte) (GetMemory(p1)*GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.MUL202:
						SetMemory((byte) (GetMemory2(p1)*p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.MUL212:
						SetMemory((byte) (GetMemory2(p1)*GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.MUL222:
						SetMemory((byte) (GetMemory2(p1)*GetMemory2(p2)), GetMemory(p3));
						break;

					case OpCodes.OpCode.DIV011:
						SetMemory((byte) Divide(p1, GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.DIV021:
						SetMemory((byte) Divide(p1, GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.DIV101:
						SetMemory((byte) Divide(GetMemory(p1), p2), p3);
						break;
					case OpCodes.OpCode.DIV111:
						SetMemory((byte) Divide(GetMemory(p1), GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.DIV201:
						SetMemory((byte) Divide(GetMemory2(p1), p2), p3);
						break;
					case OpCodes.OpCode.DIV211:
						SetMemory((byte) Divide(GetMemory2(p1), GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.DIV121:
						SetMemory((byte) Divide(GetMemory(p1), GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.DIV221:
						SetMemory((byte) Divide(GetMemory2(p1), GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.DIV012:
						SetMemory((byte) Divide(p1, GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.DIV022:
						SetMemory((byte) Divide(p1, GetMemory2(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.DIV102:
						SetMemory((byte) Divide(GetMemory(p1), p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.DIV112:
						SetMemory((byte) Divide(GetMemory(p1), GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.DIV202:
						SetMemory((byte) Divide(GetMemory2(p1), p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.DIV212:
						SetMemory((byte) Divide(GetMemory2(p1), GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.DIV122:
						SetMemory((byte) Divide(GetMemory(p1), GetMemory2(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.DIV222:
						SetMemory((byte) Divide(GetMemory2(p1), GetMemory2(p2)), GetMemory(p3));
						break;

					case OpCodes.OpCode.MOD011:
						SetMemory((byte) Modulo(p1, GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.MOD021:
						SetMemory((byte) Modulo(p1, GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.MOD101:
						SetMemory((byte) Modulo(GetMemory(p1), p2), p3);
						break;
					case OpCodes.OpCode.MOD111:
						SetMemory((byte) Modulo(GetMemory(p1), GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.MOD201:
						SetMemory((byte) Modulo(GetMemory2(p1), p2), p3);
						break;
					case OpCodes.OpCode.MOD211:
						SetMemory((byte) Modulo(GetMemory2(p1), GetMemory(p2)), p3);
						break;
					case OpCodes.OpCode.MOD121:
						SetMemory((byte) Modulo(GetMemory(p1), GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.MOD221:
						SetMemory((byte) Modulo(GetMemory2(p1), GetMemory2(p2)), p3);
						break;
					case OpCodes.OpCode.MOD012:
						SetMemory((byte) Modulo(p1, GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.MOD022:
						SetMemory((byte) Modulo(p1, GetMemory2(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.MOD102:
						SetMemory((byte) Modulo(GetMemory(p1), p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.MOD112:
						SetMemory((byte) Modulo(GetMemory(p1), GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.MOD202:
						SetMemory((byte) Modulo(GetMemory2(p1), p2), GetMemory(p3));
						break;
					case OpCodes.OpCode.MOD212:
						SetMemory((byte) Modulo(GetMemory2(p1), GetMemory(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.MOD122:
						SetMemory((byte) Modulo(GetMemory(p1), GetMemory2(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.MOD222:
						SetMemory((byte) Modulo(GetMemory2(p1), GetMemory2(p2)), GetMemory(p3));
						break;
					case OpCodes.OpCode.INP01:
						SetMemory(Input(p1), p2);
						break;
					case OpCodes.OpCode.INP11:
						SetMemory(Input(GetMemory(p1)), p2);
						break;
					case OpCodes.OpCode.INP21:
						SetMemory(Input(GetMemory2(p1)), p2);
						break;
					case OpCodes.OpCode.INP02:
						SetMemory(Input(p1), GetMemory(p2));
						break;
					case OpCodes.OpCode.INP12:
						SetMemory(Input(GetMemory(p1)), GetMemory(p2));
						break;
					case OpCodes.OpCode.INP22:
						SetMemory(Input(GetMemory2(p1)), GetMemory(p2));
						break;
					case OpCodes.OpCode.OUT00:
						Output(p1, p2);
						break;
					case OpCodes.OpCode.OUT10:
						Output(GetMemory(p1), p2);
						break;
					case OpCodes.OpCode.OUT20:
						Output(GetMemory2(p1), p2);
						break;
					case OpCodes.OpCode.OUT01:
						Output(p1, GetMemory(p2));
						break;
					case OpCodes.OpCode.OUT11:
						Output(GetMemory(p1), GetMemory(p2));
						break;
					case OpCodes.OpCode.OUT21:
						Output(GetMemory2(p1), GetMemory(p2));
						break;
					case OpCodes.OpCode.OUT02:
						Output(p1, GetMemory2(p2));
						break;
					case OpCodes.OpCode.OUT12:
						Output(GetMemory(p1), GetMemory2(p2));
						break;
					case OpCodes.OpCode.OUT22:
						Output(GetMemory2(p1), GetMemory2(p2));
						break;
				}
			}

			if (!m_ExecutionJumpedDuringTick)
				Jump(4);
		}

		private int Divide(int a, int b)
		{
			if (b == 0 || a == 0)
				return 0;
			return a / b;
		}

		private int Modulo(int a, int b)
		{
			if (b == 0 || a == 0)
				return 0;
			return a % b;
		}

		byte[] ConstantToArray(byte value, byte length)
		{
			if (length == 1)
				return new byte[] {value};

			byte[] result = new byte[length];
			for (int loop = 0; loop < length; loop++)
			{
				result[loop] = value;
			}
			return result;
		}

		void SetMemory(byte value, byte address)
		{
			m_Memory[address] = value;
		}

		void SetMemory(byte[] value, byte address)
		{
			for (int loop = 0; loop < value.Length; loop++)
			{
				SetMemory(value[loop], (byte) (address + loop));
			}
		}

		byte GetMemory(byte address)
		{
			byte result = m_Memory[address];
			return result;
		}

		byte GetMemory2(byte address)
		{
			return GetMemory(GetMemory(address));
		}

		byte[] GetMemory2(byte address, byte length)
		{
			return GetMemory(GetMemory(address), length);
		}

		byte[] GetMemory(byte address, byte length)
		{
			byte[] result = new byte[length];
			for (int loop = 0; loop < length; loop++)
			{
				result[loop] = GetMemory((byte) (address + loop));
			}
			return result;
		}

		void Jump(byte offset)
		{
			m_ProgramCounter = (byte) ((m_ProgramCounter + offset)%m_Memory.Length);
			m_ExecutionJumpedDuringTick = true;
		}

		void JumpTo(byte address)
		{
			m_ProgramCounter = (byte) (address%m_Memory.Length);
			m_ExecutionJumpedDuringTick = true;
		}

		void JumpTo2(byte address)
		{
			m_ProgramCounter = (byte) (GetMemory(address)%m_Memory.Length);
			m_ExecutionJumpedDuringTick = true;
		}
		
		// Implement your I/O here
		// Example code:

		// INP 002 @34 000 ... Input from port 2 and store the byte to memory location 34
		// OUT 01C 0AB 000 ... Output to port 1C value AB
		// OUT 01D @56 000 ... Output to port 1D the value in memory location 56 

		// Handle input from port
		byte Input(byte port)
		{
			// Your implementation here
			return 0;
		}

		// Handle output to port
		void Output(byte port, byte data)
		{
			// Your implementation here
		}
	}
}
