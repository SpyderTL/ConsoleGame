using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	public static class Input
	{
		public enum Button
		{
			Up,
			Down,
			Left,
			Right,
			Select,
			Back
		}

		public static event Action<Button> ButtonPressed;
		public static event Action<char> KeyPressed;

		public static void Pressed(Button button)
		{
			ButtonPressed?.Invoke(button);
		}

		public static void Pressed(char key)
		{
			KeyPressed?.Invoke(key);
		}
	}
}
