using System;

namespace ConsoleGame
{
	internal static class InputIntro
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
			Intro.Playing = false;
		}
	}
}