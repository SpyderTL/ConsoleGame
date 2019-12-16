using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class Keyboard
	{
		internal static event Action<ConsoleKeyInfo> KeyPressed;

		internal static void Enable()
		{
			new System.Threading.Thread(Start).Start();
		}

		private static void Start()
		{
			while (true)
				Update();
		}

		internal static void Update()
		{
			var key = Console.ReadKey(true);

			KeyPressed?.Invoke(key);
		}
	}
}
