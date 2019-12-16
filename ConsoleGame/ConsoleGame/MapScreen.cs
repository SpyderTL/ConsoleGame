﻿using System;
using System.Threading;

namespace ConsoleGame
{
	internal static class MapScreen
	{
		private static AutoResetEvent Close = new AutoResetEvent(false);

		internal static void Show()
		{
			Map.PositionChanged += Map_PositionChanged;

			Draw();

			Close.WaitOne();
		}

		internal static void Draw()
		{
			Screen.Clear();

			// Draw Map
			{
				for (var y = 0; y < 21; y++)
				{
					for (var x = 0; x < 40; x++)
					{
						var zone = 0;

						for (var z = 1; z < Map.Zones.Length; z++)
						{
							if (Map.Zones[z].Left <= x + Map.X - 20 &&
								Map.Zones[z].Right >= x + Map.X - 20 &&
								Map.Zones[z].Top <= y + Map.Y - 10 &&
								Map.Zones[z].Bottom >= y + Map.Y - 10)
								zone = z;
						}

						Screen.Characters[(y * Screen.Width) + x] = Map.Zones[zone].Character;
					}
				}

				Screen.Characters[(10 * Screen.Width) + 20] = ';';
			}

			// Draw Party Status
			{
				Screen.DrawHorizontalLine('-', 20, 0, 39);

				var y = 21;

				for (int character = 0; character < Party.Characters.Length; character++)
				{
					if (Party.Characters[character] == null)
						continue;

					Screen.DrawString(Party.Characters[character].Name, 0, y);
					Screen.DrawString(Party.Characters[character].Health.ToString(), 10, y);
					Screen.DrawString("/", 15, y);
					Screen.DrawString(Party.Characters[character].MaxHealth.ToString(), 17, y);

					y++;
				}
			}

			Screen.Update();
		}

		private static void Map_PositionChanged()
		{
			Draw();
		}
	}
}