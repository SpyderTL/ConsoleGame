using System;
using System.Threading;

namespace ConsoleGame
{
	internal static class MenuScreen
	{
		internal static string Title;
		internal static string[] Descriptions;

		internal static ManualResetEvent Close = new ManualResetEvent(false);

		internal static void Show()
		{
			Draw();

			Menu.CurrentItemChanged += Menu_CurrentItemChanged;
			Menu.ItemSelected += Menu_ItemSelected;

			Close.Reset();

			Close.WaitOne();

			Menu.CurrentItemChanged -= Menu_CurrentItemChanged;
			Menu.ItemSelected -= Menu_ItemSelected;
		}

		private static void Menu_ItemSelected()
		{
			Close.Set();
		}

		private static void Menu_CurrentItemChanged()
		{
			Draw();
		}

		internal static void Draw()
		{
			Screen.Clear();

			Screen.DrawString(Title, 0, 0);

			Screen.DrawHorizontalLine('-', 1, 0, 39);
			Screen.DrawVerticalLine('|', 10, 2, 22);
			Screen.DrawHorizontalLine('-', 23, 0, 39);

			var y = 8;

			for (var item = 0; item < Menu.Items.Length; item++)
			{
				Screen.DrawString(Menu.Items[item], 12, y);

				if (Menu.Current == item)
				{
					Screen.DrawString("-->", 6, y);

					Screen.DrawString(Descriptions[item], 0, 24);
				}

				y += 2;
			}

			Screen.Update();
		}
	}
}