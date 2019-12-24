using System;

namespace ConsoleGame
{
	internal static class InputRpgWorld
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
					RpgGame.PartyWorld.North();
					break;

				case Input.Button.Down:
					RpgGame.PartyWorld.South();
					break;

				case Input.Button.Left:
					RpgGame.PartyWorld.West();
					break;

				case Input.Button.Right:
					RpgGame.PartyWorld.East();
					break;
			}
		}
	}
}