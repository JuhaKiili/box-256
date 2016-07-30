# BOX-256
BOX-256 compiler and virtual machine in C#.

Simplified version of compiler and virtual machine used in the assembler programming game BOX-256 (http://box-256.com).
There is no UI, playback controls, etc. included, just an example implementation that outputs text.

Differences with the game:
- THR and PIX instruction removed
- Program counter hidden as internal state and no longer accessible in BOX-256 memory
- New opcodes INP and OUT to implement I/O

The example solution/project is made with Visual Studio 2013.

