using System;

namespace ConsoleGame
{
	internal class InputBattle
	{
		internal static void Enable()
		{
			Input.KeyPressed += Input_KeyPressed;
		}

		private static void Input_KeyPressed(char key)
		{
			var index = key - '0';

			BattleMenu.Select(index + 1);
		}

		internal static void Disable()
		{
			Input.KeyPressed -= Input_KeyPressed;
		}
	}
}