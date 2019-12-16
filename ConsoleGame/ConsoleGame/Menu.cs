using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	public static class Menu
	{
		public static string[] Items;
		public static int Current;

		public static event Action CurrentItemChanged;
		public static event Action ItemSelected;
		public static event Action MenuCancelled;

		internal static void Previous()
		{
			if (Current > 0)
				Current--;

			CurrentItemChanged?.Invoke();
		}

		internal static void Next()
		{
			if (Current < Items.Length - 1)
				Current++;

			CurrentItemChanged?.Invoke();
		}

		internal static void Select()
		{
			ItemSelected?.Invoke();
		}

		internal static void Cancel()
		{
			MenuCancelled?.Invoke();
		}
	}
}
