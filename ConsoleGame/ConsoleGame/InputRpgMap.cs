using System;

namespace ConsoleGame
{
	internal static class InputRpgMap
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
					RpgGame.PartyMap.North();
					break;

				case Input.Button.Down:
					RpgGame.PartyMap.South();
					break;

				case Input.Button.Left:
					RpgGame.PartyMap.West();
					break;

				case Input.Button.Right:
					RpgGame.PartyMap.East();
					break;
			}
		}
	}
}