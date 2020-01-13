using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class DataBattle
	{
		public static void LoadFormation(int formation, bool alternate)
		{
			using (var reader = Data.Reader())
			{
				reader.BaseStream.Position = Data.Address(0x0B, 0x8400 + (formation * 16));

				var data = reader.ReadBytes(16);

				var enemy1 = data[2];
				var enemy2 = data[3];
				var enemy3 = data[4];
				var enemy4 = data[5];

				var minimum1 = data[6] >> 4;
				var maximum1 = data[6] & 0x0f;
				var minimum2 = data[7] >> 4;
				var maximum2 = data[7] & 0x0f;
				var minimum3 = data[8] >> 4;
				var maximum3 = data[8] & 0x0f;
				var minimum4 = data[9] >> 4;
				var maximum4 = data[9] & 0x0f;

				var alternateMinimum1 = data[14] >> 4;
				var alternateMaximum1 = data[14] & 0x0f;
				var alternateMinimum2 = data[15] >> 4;
				var alternateMaximum2 = data[15] & 0x0f;

				var enemies = new List<Battle.Enemy>();

				var random = new Random();

				if (!alternate)
				{
					var count1 = random.Next(minimum1, maximum1 + 1);
					var count2 = random.Next(minimum2, maximum2 + 1);
					var count3 = random.Next(minimum3, maximum3 + 1);
					var count4 = random.Next(minimum4, maximum4 + 1);

					for (var x = 0; x < count1; x++)
						enemies.Add(new Battle.Enemy { Type = enemy1 });

					for (var x = 0; x < count2; x++)
						enemies.Add(new Battle.Enemy { Type = enemy2 });

					for (var x = 0; x < count3; x++)
						enemies.Add(new Battle.Enemy { Type = enemy3 });

					for (var x = 0; x < count4; x++)
						enemies.Add(new Battle.Enemy { Type = enemy4 });
				}
				else
				{
					var alternateCount1 = random.Next(alternateMinimum1, alternateMaximum1 + 1);
					var alternateCount2 = random.Next(alternateMinimum2, alternateMaximum2 + 1);

					for (var x = 0; x < alternateCount1; x++)
						enemies.Add(new Battle.Enemy { Type = enemy1 });

					for (var x = 0; x < alternateCount2; x++)
						enemies.Add(new Battle.Enemy { Type = enemy2 });
				}

				Battle.Enemies = enemies.ToArray();
			}
		}
	}
}
