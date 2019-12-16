using System;

namespace ConsoleGame
{
	internal static class InputWorld
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
					Map.North();
					break;

				case Input.Button.Down:
					Map.South();
					break;

				case Input.Button.Left:
					Map.West();
					break;

				case Input.Button.Right:
					Map.East();
					break;
			}
		}
	}
}