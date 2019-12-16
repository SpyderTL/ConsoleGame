using System;

namespace ConsoleGame
{
	public static class KeyboardInput
	{
		public static void Enable()
		{
			Keyboard.KeyPressed += Keyboard_KeyPressed;
		}

		public static void Disable()
		{
			Keyboard.KeyPressed -= Keyboard_KeyPressed;
		}

		private static void Keyboard_KeyPressed(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
					Input.Pressed(Input.Button.Up);
					break;

				case ConsoleKey.DownArrow:
					Input.Pressed(Input.Button.Down);
					break;

				case ConsoleKey.LeftArrow:
					Input.Pressed(Input.Button.Left);
					break;

				case ConsoleKey.RightArrow:
					Input.Pressed(Input.Button.Right);
					break;

				case ConsoleKey.Enter:
					Input.Pressed(Input.Button.Select);
					break;

				case ConsoleKey.Escape:
					Input.Pressed(Input.Button.Back);
					break;
			}
		}
	}
}