using System;

namespace ConsoleGame
{
	internal static class InputMenu
	{
		internal static void Enable()
		{
			Input.ButtonPressed += Input_ButtonPressed;
		}

		internal static void Disable()
		{
			Input.ButtonPressed -= Input_ButtonPressed;
		}

		private static void Input_ButtonPressed(Input.Button button)
		{
			switch (button)
			{
				case Input.Button.Up:
					Menu.Previous();
					break;

				case Input.Button.Down:
					Menu.Next();
					break;

				case Input.Button.Select:
					Menu.Select();
					break;

				case Input.Button.Back:
					Menu.Cancel();
					break;
			}
		}
	}
}