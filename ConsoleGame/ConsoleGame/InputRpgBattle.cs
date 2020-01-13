using System;

namespace ConsoleGame
{
	internal class InputRpgBattle
	{
		internal static void Enable()
		{
			Input.KeyPressed += Input_KeyPressed;
		}

		private static void Input_KeyPressed(char key)
		{
		}

		internal static void Disable()
		{
			Input.KeyPressed -= Input_KeyPressed;
		}
	}
}